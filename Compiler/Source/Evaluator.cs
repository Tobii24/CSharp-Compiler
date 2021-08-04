using Compiler.Source.Datatypes;
using Compiler.Source.Errors;
using Compiler.Source.Lib;
using Compiler.Source.Syntax;
using System;

namespace Compiler.Source
{
    internal sealed class Evaluator
    {
        public DiagnosticBag _diagnostics;
        public ExpressionSyntax[] _roots;
        private SyntaxTree tree;

        public Evaluator(string filename, string text)
        {
            var parser = new Parser(filename, text);
            tree = parser.Parse();

            _diagnostics = new DiagnosticBag();
            _diagnostics.Extend(tree.Diagnostics);

            _roots = tree.Roots;
        }

        public void Evaluate(Context context, bool PrintSyntaxTree = false)
        {
            if (PrintSyntaxTree)
                tree.Log();

            var err = _diagnostics.GetIfError();
            if (err != null)
            {
                err.Throw();
            }
            else
            {
                foreach (var root in _roots)
                {
                    var res = EvaluateExpression(root, context);
                    if (res.Err != null)
                    {
                        res.Err.Throw();
                        break;
                    }
                    else if (res.Value != null)
                    {
                        Console.WriteLine(res.Value);
                    }
                }
            }
        }

        private RuntimeResult EvaluateExpression(ExpressionSyntax node, Context context)
        {
            if (node is LiteralExpressionSyntax n)
            {
                return new RuntimeResult().Success(
                    new NumberType(n.LiteralToken)
                    );
            }

            if (node is UnaryExpressionSyntax ue)
            {
                var res = new RuntimeResult();
                var num = res.Register(EvaluateExpression(ue.PrimarySyntax, context));
                if (res.ShouldReturn()) { return res; }

                (Datatype, Error) result;

                switch (ue.UnaryToken.Type)
                {
                    case SyntaxType.MinusToken:
                        result = num.Notted(ue.Pos, context);
                        break;
                    case SyntaxType.PlusToken:
                        result = (num, null);
                        break;
                    default:
                        return res.Failure(
                            new RuntimeError(
                                ue.Pos,
                                $"Unknown unary operator <{ue.UnaryToken.Type}>",
                                context.ContextString
                            )
                        );
                }

                var retrieved = result.Item1;
                var err = result.Item2;

                if (err == null)
                    return res.Success(retrieved);
                else
                    return res.Failure(err);
            }

            if (node is BinaryExpressionSyntax be)
            {
                var res = new RuntimeResult();
                var left = res.Register(EvaluateExpression(be.Left, context));
                if (res.ShouldReturn()) { return res; }
                var right = res.Register(EvaluateExpression(be.Right, context));
                if (res.ShouldReturn()) { return res; }

                var check = Datatype.Check(be.OperatorToken.Text, be.Pos, context, left, right);
                if (check != null) 
                    return res.Failure(check);

                if (!left.CanArithmetic || !right.CanArithmetic)
                {
                    return res.Failure(
                        new RuntimeError(
                            be.Pos,
                            $"Types <{left.Type}> or/and <{right.Type}> don't support arithmetic operations",
                            context.ContextString
                        )
                    );
                }

                dynamic retrieved;
                dynamic resVal;

                switch (be.OperatorToken.Type)
                {
                    case SyntaxType.PlusToken:
                        resVal = left.Add(right);
                        break;
                    case SyntaxType.MinusToken:
                        resVal = left.Minus(right);
                        break;
                    case SyntaxType.StarToken:
                        resVal = left.Times(right);
                        break;
                    case SyntaxType.SlashToken:
                        retrieved = left.Divided(right, be.Pos, context);
                        if (retrieved.Item2 != null)
                            return res.Failure(retrieved.Item2);
                        resVal = retrieved.Item1;
                        break;
                    case SyntaxType.PowToken:
                        resVal = left.Pow(right);
                        break;
                    case SyntaxType.EqualsEqualsToken:
                        retrieved = left.EqualsTo(be.Pos, context, right);
                        if (retrieved.Item2 != null)
                            return res.Failure(retrieved.Item2);
                        resVal = retrieved.Item1;
                        break;
                    case SyntaxType.NotEquals:
                        retrieved = left.NotEqualsTo(be.Pos, context, right);
                        if (retrieved.Item2 != null)
                            return res.Failure(retrieved.Item2);
                        resVal = retrieved.Item1;
                        break;
                    case SyntaxType.GreaterThanToken:
                        retrieved = left.GreaterThan(be.Pos, context, right);
                        if (retrieved.Item2 != null)
                            return res.Failure(retrieved.Item2);
                        resVal = retrieved.Item1;
                        break;
                    case SyntaxType.LessThanToken:
                        retrieved = left.LessThan(be.Pos, context, right);
                        if (retrieved.Item2 != null)
                            return res.Failure(retrieved.Item2);
                        resVal = retrieved.Item1;
                        break;
                    case SyntaxType.GreaterThanEqualsToken:
                        retrieved = left.GreaterThanEquals(be.Pos, context, right);
                        if (retrieved.Item2 != null)
                            return res.Failure(retrieved.Item2);
                        resVal = retrieved.Item1;
                        break;
                    case SyntaxType.LessThanEqualsToken:
                        retrieved = left.LessThanEquals(be.Pos, context, right);
                        if (retrieved.Item2 != null)
                            return res.Failure(retrieved.Item2);
                        resVal = retrieved.Item1;
                        break;
                    default:
                        return res.Failure(
                        new RuntimeError(
                            be.Pos,
                            $"Unknown arithmetic operator '{be.OperatorToken.Text}'",
                            context.ContextString
                        )
                    );
                }
                return res.Success(resVal);
            }

            if (node is VarAccessExpressionSyntax va)
            {
                var res = new RuntimeResult();
                var varName = va.VarToken.Text;

                foreach (var existingVarName in context.Variables.Keys)
                {
                    if (varName == existingVarName)
                    {
                        if (context.Variables.TryGetValue(varName, out var varVal))
                            return res.Success(varVal);
                        else
                            return res.Failure(
                                new RuntimeError(
                                    va.Pos,
                                    $"An unexpected error ocurred trying to get the variable '{varName}' value",
                                    context.ContextString
                                )
                            );
                    }
                }

                return res.Failure(
                        new RuntimeError(
                            va.Pos,
                            $"Variable '{varName}' is not declared",
                            context.ContextString
                        )
                    );
            }

            /*if (node is VarAssignExpressionSyntax vas)
            {
                var res = new RuntimeResult();
                var varName = vas.VarToken.Text;
                var varVal = vas.ValueToken;
            }*/

            return new RuntimeResult().Failure(
                new RuntimeError(
                    node.Pos,
                    $"Unknown node <'{node.Type}'>",
                    context.ContextString
                ));
        }
    }
}

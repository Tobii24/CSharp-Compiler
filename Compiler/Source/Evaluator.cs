using System;
using System.Collections.Generic;
using Compiler.Source.Lib;
using Compiler.Source.Syntax;
using Compiler.Source.Datatypes;
using Compiler.Source.Errors;
using System.Linq;

namespace Compiler.Source
{
    internal sealed class Evaluator
    {
        public DiagnosticBag _diagnostics;
        public ExpressionSyntax[] _roots;
        public Context _context;

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true)
        {
            var marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Type);

            if (node is SyntaxToken st)
            {
                if (st.Value != null)
                {
                    Console.Write(" ");
                    Console.Write(st.Value);
                }
            }

            Console.WriteLine();

            indent += isLast ? "   " : "│   ";

            var lastChild = node.GetChildren().LastOrDefault();

            foreach (var child in node.GetChildren())
                PrettyPrint(child, indent, child == lastChild);
        }

        public Evaluator(string filename, string text)
        {
            var parser = new Parser(filename, text);
            var tree = parser.Parse();

            /* // Print syntax tree
            foreach(var root in tree.Roots)
            {
                PrettyPrint(root);
            } */

            _diagnostics = new DiagnosticBag();
            _diagnostics.Extend(tree.Diagnostics);

            _roots = tree.Roots;
        }

        public object[] Evaluate(Context context, ExpressionSyntax[] ROOTS=null)
        {
            if (ROOTS == null)
                ROOTS = _roots;

            _context = context;

            var results = new List<object>();
            foreach(var root in ROOTS)
            {
                results.Add(EvaluateExpression(root));
            }

            return results.ToArray();
        }

        private dynamic EvaluateExpression(ExpressionSyntax node)
        {
            if (node is LiteralExpressionSyntax n)
            { return new NumberType(n.LiteralToken); }

            if (node is UnaryExpressionSyntax un)
            {
                var parsd = EvaluateExpression(un.PrimarySyntax);
                if (un.UnaryToken.Type == SyntaxType.MinusToken || un.UnaryToken.Value == KeywordType.NotStatement)
                    parsd.Negate();

                return parsd;
            }

            if (node is BinaryExpressionSyntax b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                if (left.Equals(null) || right.Equals(null))
                {
                    _diagnostics.Append(
                        new RuntimeError(
                            node.Pos,
                            $"Cannot perform arithmetic expression with 'null'>",
                            _context.ContextString
                            )
                        );
                }
                else if (left.GetType() != right.GetType())
                {
                    switch (b.OperatorToken.Value)
                    {
                        case KeywordType.AndStatement:
                            return left & right;
                        case KeywordType.OrStatement:
                            return left | right;
                    }

                    _diagnostics.Append(
                        new RuntimeError(
                            node.Pos,
                            $"Cannot perform binary operation (operation token <'{b.OperatorToken.Type}'>) with types <'{left.Type}'> & <'{right.Type}'>",
                            _context.ContextString
                            )
                        );
                }
                else
                {
                    switch (b.OperatorToken.Value)
                    {
                        case KeywordType.AndStatement:
                            return left & right;
                        case KeywordType.OrStatement:
                            return left | right;
                    }

                    switch (b.OperatorToken.Type)
                    {
                        case SyntaxType.PlusToken:
                            return left + right;
                        case SyntaxType.MinusToken:
                            return left - right;
                        case SyntaxType.StarToken:
                            return left * right;
                        case SyntaxType.SlashToken:
                            return left / right;
                        case SyntaxType.PowToken:
                            return left ^ right;
                        case SyntaxType.EqualsEqualsToken:
                            return left == right;
                        case SyntaxType.NotEquals:
                            return left != right;
                        case SyntaxType.GreaterThanToken:
                            return left > right;
                        case SyntaxType.LessThanToken:
                            return left < right;
                        case SyntaxType.GreaterThanEqualsToken:
                            return left >= right;
                        case SyntaxType.LessThanEqualsToken:
                            return left <= right;
                        default:
                            _diagnostics.Append(new RuntimeError(
                                node.Pos,
                            $"Unkown operator '{b.OperatorToken.Text}'",
                            _context.ContextString
                            ));
                            return null;
                    }
                }
            }

            if (node is ParenthesizedExpressionSyntax pe)
            { return EvaluateExpression(pe.Expression); }

            if (node is VarAccessExpressionSyntax va)
            {
                foreach(var name in _context.Variables.Keys)
                {
                    if (name == va.VarToken.Text)
                    {
                        if (_context.Variables.TryGetValue(name, out var val)) { return val; }
                        else
                        {
                            _diagnostics.Append(new RuntimeError(
                                node.Pos,
                                $"An errored occured while trying to retrieve the varible '{va.VarToken.Text}' value",
                                _context.ContextString
                                ));
                            return null;
                        }
                    }
                }
                _diagnostics.Append(new RuntimeError(
                    node.Pos,
                    $"Variable '{va.VarToken.Text}' wasn't declared",
                    _context.ContextString
                    ));
            }

            if (node is VarAssignExpressionSyntax vas)
            {
                var varName = vas.VarToken.Text;
                var val = EvaluateExpression(vas.ValueToken);

                if (varName != null)
                {
                    foreach(var existingVarName in _context.Variables.Keys)
                    {
                        if (varName == existingVarName)
                            _context.Variables.Remove(varName);
                    } 
                    _context.Variables.Add(varName, val);
                    return null;
                }
            }

            if (node is IfExpressionSyntax ie)
            {
                var condition = EvaluateExpression(ie.Condition);

                if (!condition.Equals(null) && condition.IsTrue() && _diagnostics.GetIfError() == null)
                {
                    var newContext = _context.Clone("IfStatement");
                    var results = new List<dynamic>();
                    ie.Statements.ForEach(item => results.Add(EvaluateExpression(item)));

                    foreach(var res in results)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(res);
                        Console.ResetColor();
                    }
                }

                return null;
            }

            _diagnostics.Append(new RuntimeError(
                node.Pos,
                $"Unknown node <'{node.Type}'>",
                _context.ContextString
                ));

            return new NullType();
        }
    }
}

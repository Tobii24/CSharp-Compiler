using System;
using System.Collections.Generic;
using Compiler.Source.Lib;
using Compiler.Source.Syntax;
using Compiler.Source.Datatypes;
using Compiler.Source.Errors;

namespace Compiler.Source
{
    internal sealed class Evaluator
    {
        public DiagnosticBag _diagnostics;
        public ExpressionSyntax[] _roots;

        public Evaluator(string filename, string text)
        {
            var parser = new Parser(filename, text);
            var tree = parser.Parse();

            _diagnostics = new DiagnosticBag();

            _roots = tree.Roots;
            _diagnostics.Extend(tree.Diagnostics);
        }

        public object[] Evaluate()
        {
            var results = new List<object>();
            foreach(var root in _roots)
            {
                results.Add(EvaluateExpression(root));
            }

            return results.ToArray();
        }

        private dynamic EvaluateExpression(ExpressionSyntax node)
        {
            if (node is LiteralExpressionSyntax n)
                return new NumberType(n.LiteralToken);

            if (node is UnaryExpressionSyntax un)
            {
                var num = new NumberType(un.LiteralToken);
                if (un.UnaryToken.Type == SyntaxType.MinusToken)
                    num.Negate();

                return num;
            }     

            if (node is BinaryExpressionSyntax b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                if (b.OperatorToken.Type == SyntaxType.PlusToken)
                    return left + right;
                else if (b.OperatorToken.Type == SyntaxType.MinusToken)
                    return left - right;
                else if (b.OperatorToken.Type == SyntaxType.StarToken)
                    return left * right;
                else if (b.OperatorToken.Type == SyntaxType.SlashToken)
                    return left / right;
                else
                    _diagnostics.Append(new RuntimeError(
                        $"Unkown operator '{b.OperatorToken.Text}'"
                        ));
            }

            throw new Exception("Unknown node");
        }
    }
}

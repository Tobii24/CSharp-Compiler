using System;
using System.Collections.Generic;

using Compiler.Source.Syntax;
using Compiler.Source.Errors;
using Compiler.Source.Lib;

namespace Compiler.Source
{
    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;
        private Position _position;
        public DiagnosticBag _diagnostics;

        public Parser(string filename, string text)
        {
            var lexer = new Lexer(filename, text);

            var toks = lexer.Lex();

            _position = new Position(0, 0, 0, filename);
            _tokens = toks;
            _diagnostics = new DiagnosticBag();

            _diagnostics.Extend(lexer._diagnostics);
        }

        private SyntaxToken Peek(int offset)
        {
            var index = _position.Index + offset;
            if (index < _tokens.Length)
                return _tokens[index];

            return _tokens[_tokens.Length - 1];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            var current = Current;
            _position.Advance();
            return current;
        }

        private SyntaxToken MatchToken(SyntaxType type)
        {
            if (Current.Type == type)
                return NextToken();

            //Console.WriteLine($"{_position} {Current.PosStart}");
            
            var err = new ExpectedTokenError(_position, $"<{type}> not <{Current.Type}>");
            _diagnostics.Append(err);

            return new SyntaxToken(type, null, null);
        }

        public SyntaxTree Parse()
        {
            var expresions = ParseExpressions();
            var endOfFileToken = MatchToken(SyntaxType.EndOfFileToken);

            return new SyntaxTree(expresions, endOfFileToken, _diagnostics);
        }

        private ExpressionSyntax[] ParseExpressions()
        {
            var expressions = new List<ExpressionSyntax>();
            do
            {
                var expr = ParseExpression(true);
                expressions.Add(expr);

                if (Current.Type == SyntaxType.EndOfFileToken)
                    break;
            } while (true);
            return expressions.ToArray();
        }

        private ExpressionSyntax ParseExpression(bool mainExpression=false)
        {
            var term = ParseTerm();

            if (mainExpression)
                MatchToken(SyntaxType.SemicolonToken);

            return term;
        }

        private ExpressionSyntax ParseTerm()
        {
            var left = ParseFactor();

            while (Current.Type == SyntaxType.PlusToken ||
                   Current.Type == SyntaxType.MinusToken)
            {
                var operatorToken = NextToken();
                var right = ParseFactor();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParseFactor()
        {
            var left = ParseAtom();

            while (Current.Type == SyntaxType.StarToken ||
                   Current.Type == SyntaxType.SlashToken)
            {
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParseAtom()
        {
            var left = ParsePrimaryExpression();

            while (Current.Type == SyntaxType.PowToken)
            {
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.Type == SyntaxType.OpenParenthesisToken)
            {
                var left = NextToken();
                var expression = ParseExpression();
                var right = MatchToken(SyntaxType.CloseParenthesisToken);

                return new ParenthesizedExpressionSyntax(left, expression, right);
            }

            if (Current.IsUnaryOperator())
            {
                var opTok = NextToken();
                var numTok = MatchToken(SyntaxType.NumberToken);

                return new UnaryExpressionSyntax(opTok, numTok);
            }

            var numberToken = MatchToken(SyntaxType.NumberToken);

            return new LiteralExpressionSyntax(numberToken);
        }
    }
}

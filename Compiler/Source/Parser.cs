using System.Collections.Generic;
using Compiler.Source.Syntax;
using Compiler.Source.Errors;

namespace Compiler.Source
{
    internal sealed class Parser
    {
        private readonly SyntaxToken[] _tokens;
        private Position _position;

        public Parser(string filename, string text)
        {
            var lexer = new Lexer(filename, text);

            var lexerResults = lexer.GetTokens();
            var toks = lexerResults.Item1;
            var err = lexerResults.Item2;

            if (err != null)
                err.Throw();

            _position = new Position(0, 0, 0, filename, text);
            _tokens = toks;
        }

        #nullable enable
        private SyntaxToken? Peek(int offset)
        {
            var index = _position.Index + offset;
            if (index < _tokens.Length)
                return _tokens[index];

            return null;
        }
        #nullable disable

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            var current = Current;
            _position.Advance();
            return current;
        }

        private (SyntaxToken, Error) MatchToken(SyntaxType type)
        {
            if (Current.Type == type)
                return (NextToken(), null);

            return (
                new SyntaxToken(type, null, null, Current.PosStart, Current.PosEnd), 
                new ExpectedTokenError(_position, Current.PosStart, $"<{type}> not <{Current.Type}>")
                );
        }

        public SyntaxTree Parse()
        {
            if (Current == null) 
            { 
                return new SyntaxTree(
                    new ExpressionSyntax[] { new ErroredExpression() }, 
                    new SyntaxToken(SyntaxType.EndOfFileToken, null, null)
                );  
            }
            var expresions = ParseExpressions();
            var endOfFileToken = MatchToken(SyntaxType.EndOfFileToken);

            if (endOfFileToken.Item2 != null)
                endOfFileToken.Item2.Throw();

            return new SyntaxTree(expresions, endOfFileToken.Item1);
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
            {
                var res = MatchToken(SyntaxType.SemicolonToken);
                if (res.Item2 != null)
                {
                    res.Item2.Throw();
                    return new ErroredExpression();
                }
            }

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

                if (right.Item2 != null)
                {
                    right.Item2.Throw();
                    return new ErroredExpression();
                }
                return new ParenthesizedExpressionSyntax(left, expression, right.Item1);
            }

            var numberToken = MatchToken(SyntaxType.NumberToken);

            if (numberToken.Item2 != null)
            {
                numberToken.Item2.Throw();
                return new ErroredExpression();
            } 

            return new LiteralExpressionSyntax(numberToken.Item1);
        }
    }
}

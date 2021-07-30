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

            NextToken();

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
            if (Current.Match(SyntaxType.KeywordToken, KeywordType.DeclareVariable))
            {
                var declareTok = NextToken();
                var varNameTok = MatchToken(SyntaxType.IdentifierToken);
                var eqTok = MatchToken(SyntaxType.EqualsToken);
                var valExpr = ParseExpression(true);

                return new VarAssignExpressionSyntax(declareTok, varNameTok, eqTok, valExpr);
            }

            var term = BinaryOp(ParseComparison, 
            new dynamic[] {
                new dynamic[] { SyntaxType.KeywordToken, KeywordType.AndStatement },
                new dynamic[] { SyntaxType.KeywordToken, KeywordType.OrStatement }
            });

            if (mainExpression)
                MatchToken(SyntaxType.SemicolonToken);

            return term;
        }

        private ExpressionSyntax ParseComparison()
        {
            if (Current.Match(SyntaxType.KeywordToken, KeywordType.NotStatement))
            {
                var opTok = NextToken();

                var node = ParseComparison();
                return new UnaryExpressionSyntax(opTok, node);
            }

            var _node = BinaryOp(ParseTerm, new dynamic[]
                { SyntaxType.EqualsEqualsToken,
                SyntaxType.GreaterThanToken,
                SyntaxType.LessThanToken,
                SyntaxType.GreaterThanEqualsToken,
                SyntaxType.GreaterThanEqualsToken,
                SyntaxType.NotEquals }
                );

            return _node;
        }

        private ExpressionSyntax ParseTerm()
        {
            return BinaryOp(ParseFactor, new dynamic[] { SyntaxType.PlusToken, SyntaxType.MinusToken });
        }

        private ExpressionSyntax ParseFactor()
        {
            return BinaryOp(ParseAtom, new dynamic[] { SyntaxType.StarToken, SyntaxType.SlashToken });
        }

        private ExpressionSyntax ParseAtom()
        {
            return BinaryOp(ParsePrimaryExpression, new dynamic[] { SyntaxType.PowToken, SyntaxType.PowToken });
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            if (Current.Type == SyntaxType.OpenParenthesisToken)
            {
                var left = NextToken();
                var expression = ParseTerm();
                var right = MatchToken(SyntaxType.CloseParenthesisToken);

                return new ParenthesizedExpressionSyntax(left, expression, right);
            }

            if (Current.Type == SyntaxType.IdentifierToken)
            {
                var tok = NextToken();
                return new VarAccessExpressionSyntax(tok);
            }

            if (Current.IsUnaryOperator())
            {
                var opTok = NextToken();
                var numTok = ParseFactor();

                return new UnaryExpressionSyntax(opTok, numTok);
            }


            if (Current.Match(SyntaxType.KeywordToken, KeywordType.IfStatement))
            {
                return IfExpression();
            }

            var numberToken = MatchToken(SyntaxType.NumberToken);

            return new LiteralExpressionSyntax(numberToken);
        }

        private ExpressionSyntax IfExpression()
        {
            var ifNameTok = NextToken();
            var openPToken = MatchToken(SyntaxType.OpenParenthesisToken);
            var conditionExpr = ParseExpression();
            var closePToken = MatchToken(SyntaxType.CloseParenthesisToken);
            var openBToken = MatchToken(SyntaxType.OpenKeyToken);

            var statements = new List<ExpressionSyntax>();

            do
            {
                var statement = ParseExpression();
                if (statement == null)
                    break;
                statements.Add(statement);
            } while (Current.Type != SyntaxType.CloseKeyToken);
            var closeBToken = MatchToken(SyntaxType.CloseKeyToken);

            return new IfExpressionSyntax(
                ifNameTok, openPToken, conditionExpr, closePToken,
                openBToken, statements, closeBToken
                );
        }

        private bool In(dynamic what, dynamic[] inWhat)
        {
            var testArray = new dynamic[] { };
            foreach (var item in inWhat)
            {
                if (item.GetType() == testArray.GetType())
                {
                    if (what.Type == item[0] && what.Value == item[1])
                        return true;
                }
                else if (what.Type == item)
                { return true; }
            }

            return false;
        }

        private ExpressionSyntax BinaryOp(Func<ExpressionSyntax> left, dynamic[] operators, Func<ExpressionSyntax> right =null)
        {
            if (right == null)
                right = left;

            var lleft = left();
            
            while (In(Current, operators))
            {
                var operatorToken = NextToken();
                var rright = right();
                lleft = new BinaryExpressionSyntax(lleft, operatorToken, rright);
            }

            return lleft;
        }
    }
}

using System;
using System.Collections.Generic;
using Compiler.Source.Lib;

namespace Compiler.Source.Syntax
{
    public sealed class WhileExpressionSyntax : ExpressionSyntax
    {
        public WhileExpressionSyntax(Position pos, SyntaxToken whileDeclarationToken, SyntaxToken openParenthesisToken, ExpressionSyntax condition, SyntaxToken closeParenthesisToken, SyntaxToken openKeyToken, List<ExpressionSyntax> statements, SyntaxToken closeKeyToken)
        {
            Pos = pos;
            WhileDeclarationToken = whileDeclarationToken;
            OpenParenthesisToken = openParenthesisToken;
            Condition = condition;
            CloseParenthesisToken = closeParenthesisToken;
            OpenKeyToken = openKeyToken;
            Statements = statements;
            CloseKeyToken = closeKeyToken;
        }

        public override Position Pos { get; }
        public override SyntaxType Type => SyntaxType.IfExpression;
        public SyntaxToken WhileDeclarationToken { get; }
        public SyntaxToken OpenParenthesisToken { get; }
        public ExpressionSyntax Condition { get; }
        public SyntaxToken CloseParenthesisToken { get; }
        public SyntaxToken OpenKeyToken { get; }
        public List<ExpressionSyntax> Statements { get; }
        public SyntaxToken CloseKeyToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return WhileDeclarationToken;
            yield return OpenParenthesisToken;
            yield return Condition;
            yield return CloseParenthesisToken;
            yield return OpenKeyToken;
            foreach (var statement in Statements)
            {
                yield return statement;
            }
            yield return CloseKeyToken;
        }
    }
}

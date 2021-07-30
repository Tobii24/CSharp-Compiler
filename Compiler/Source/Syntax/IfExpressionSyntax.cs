using System;
using System.Collections.Generic;
using Compiler.Source.Lib;
namespace Compiler.Source.Syntax
{
    public sealed class IfExpressionSyntax : ExpressionSyntax
    {
        public IfExpressionSyntax(SyntaxToken ifDeclarationToken, SyntaxToken openParenthesisToken, ExpressionSyntax condition, SyntaxToken closeParenthesisToken, SyntaxToken openKeyToken, List<ExpressionSyntax> statements, SyntaxToken closeKeyToken)
        {
            IfDeclarationToken = ifDeclarationToken;
            OpenParenthesisToken = openParenthesisToken;
            Condition = condition;
            CloseParenthesisToken = closeParenthesisToken;
            OpenKeyToken = openKeyToken;
            Statements = statements;
            CloseKeyToken = closeKeyToken;
        }

        public override SyntaxType Type => SyntaxType.IfExpression;
        public SyntaxToken IfDeclarationToken { get; }
        public SyntaxToken OpenParenthesisToken { get; }
        public ExpressionSyntax Condition { get; }
        public SyntaxToken CloseParenthesisToken { get; }
        public SyntaxToken OpenKeyToken { get; }
        public List<ExpressionSyntax> Statements { get; }
        public SyntaxToken CloseKeyToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IfDeclarationToken;
            yield return OpenParenthesisToken;
            yield return Condition;
            yield return CloseParenthesisToken;
            yield return OpenKeyToken;
            foreach(var statement in Statements)
            {
                yield return statement;
            }
            yield return CloseKeyToken;
        }
    }
}

using System;
using System.Collections.Generic;
using Compiler.Source.Lib;

namespace Compiler.Source.Syntax
{
    public sealed class VarAssignExpressionSyntax : ExpressionSyntax
    {
        public VarAssignExpressionSyntax(SyntaxToken varDeclareToken, SyntaxToken varToken, SyntaxToken equalsToken, ExpressionSyntax valueToken)
        {
            VarDeclareToken = varDeclareToken;
            VarToken = varToken;
            EqualsToken = equalsToken;
            ValueToken = valueToken;
        }

        public override SyntaxType Type => SyntaxType.VarAssignExpression;
        public SyntaxToken VarDeclareToken { get; }
        public SyntaxToken VarToken { get; }
        public SyntaxToken EqualsToken { get; }
        public ExpressionSyntax ValueToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return VarDeclareToken;
            yield return VarToken;
            yield return EqualsToken;
            yield return ValueToken;
        }
    }
}

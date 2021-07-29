using System.Collections.Generic;
using Compiler.Source.Lib;

namespace Compiler.Source.Syntax
{
    public sealed class UnaryExpressionSyntax : ExpressionSyntax
    {
        public UnaryExpressionSyntax(SyntaxToken unaryToken, SyntaxToken literalToken) {
            UnaryToken = unaryToken;
            LiteralToken = literalToken;
        }

        public SyntaxToken UnaryToken { get; }
        public SyntaxToken LiteralToken { get; }

        public override SyntaxType Type => SyntaxType.UnaryExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return UnaryToken;
            yield return LiteralToken;
        }
    }
}

using System.Collections.Generic;
using Compiler.Source.Lib;

namespace Compiler.Source.Syntax
{
    public sealed class UnaryExpressionSyntax : ExpressionSyntax
    {
        public UnaryExpressionSyntax(SyntaxToken unaryToken, ExpressionSyntax literalToken) {
            UnaryToken = unaryToken;
            PrimarySyntax = literalToken;
        }

        public SyntaxToken UnaryToken { get; }
        public ExpressionSyntax PrimarySyntax { get; }

        public override SyntaxType Type => SyntaxType.UnaryExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return UnaryToken;
            yield return PrimarySyntax;
        }
    }
}

using System.Collections.Generic;
using Compiler.Source.Lib;

namespace Compiler.Source.Syntax
{
    public sealed class UnaryExpressionSyntax : ExpressionSyntax
    {
        public UnaryExpressionSyntax(Position pos, SyntaxToken unaryToken, ExpressionSyntax literalToken) {
            Pos = pos;
            UnaryToken = unaryToken;
            PrimarySyntax = literalToken;
        }

        public override Position Pos { get; }
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

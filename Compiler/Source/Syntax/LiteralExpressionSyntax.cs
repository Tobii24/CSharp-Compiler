using System.Collections.Generic;
using Compiler.Source.Lib;

namespace Compiler.Source.Syntax
{
    public sealed class LiteralExpressionSyntax : ExpressionSyntax
    {
        public LiteralExpressionSyntax(Position pos, SyntaxToken literalToken)
        {
            Pos = pos;
            LiteralToken = literalToken;
        }

        public override Position Pos { get; }
        public override SyntaxType Type => SyntaxType.LiteralExpression;
        public SyntaxToken LiteralToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LiteralToken;
        }
    }
}

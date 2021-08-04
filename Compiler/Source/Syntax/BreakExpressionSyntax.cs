using System.Collections.Generic;
using Compiler.Source.Lib;

namespace Compiler.Source.Syntax
{
    public sealed class BreakExpressionSyntax : ExpressionSyntax
    {
        public BreakExpressionSyntax(Position pos, SyntaxToken breakToken)
        {
            Pos = pos;
            BreakToken = breakToken;
        }

        public override Position Pos { get; }
        public override SyntaxType Type => SyntaxType.LiteralExpression;
        public SyntaxToken BreakToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return BreakToken;
        }
    }
}

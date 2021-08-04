using System.Collections.Generic;
using Compiler.Source.Lib;

namespace Compiler.Source.Syntax
{
    public sealed class ContinueExpressionSyntax : ExpressionSyntax
    {
        public ContinueExpressionSyntax(Position pos, SyntaxToken continueToken)
        {
            Pos = pos;
            ContinueToken = continueToken;
        }

        public override Position Pos { get; }
        public override SyntaxType Type => SyntaxType.LiteralExpression;
        public SyntaxToken ContinueToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return ContinueToken;
        }
    }
}

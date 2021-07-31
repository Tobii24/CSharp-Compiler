using System.Collections.Generic;
using Compiler.Source.Lib;

namespace Compiler.Source.Syntax
{
    public sealed class VarAccessExpressionSyntax : ExpressionSyntax
    {
        public VarAccessExpressionSyntax(Position pos, SyntaxToken varToken)
        {
            Pos = pos;
            VarToken = varToken;
        }

        public override Position Pos { get; }
        public override SyntaxType Type => SyntaxType.VarAccessExpression;
        public SyntaxToken VarToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return VarToken;
        }
    }
}

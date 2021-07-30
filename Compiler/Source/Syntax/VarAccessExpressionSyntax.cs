using System.Collections.Generic;
using Compiler.Source.Lib;

namespace Compiler.Source.Syntax
{
    public sealed class VarAccessExpressionSyntax : ExpressionSyntax
    {
        public VarAccessExpressionSyntax(SyntaxToken varToken)
        {
            VarToken = varToken;
        }

        public override SyntaxType Type => SyntaxType.VarAccessExpression;
        public SyntaxToken VarToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return VarToken;
        }
    }
}

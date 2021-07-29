using System.Collections.Generic;
using System.Linq;

namespace Compiler.Source.Syntax
{
    public sealed class ErroredExpression : ExpressionSyntax
    {
        public override SyntaxType Type => SyntaxType.ErroredExpression;

        public override IEnumerable<SyntaxNode> GetChildren() => Enumerable.Empty<SyntaxNode>();
    }
}

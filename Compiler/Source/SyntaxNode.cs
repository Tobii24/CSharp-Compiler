using System.Collections.Generic;

namespace Compiler.Source
{
    public abstract class SyntaxNode
    {
        public abstract SyntaxType Type { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();
    }

    public abstract class ExpressionSyntax : SyntaxNode { }
}

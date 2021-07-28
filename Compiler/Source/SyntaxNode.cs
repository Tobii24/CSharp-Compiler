using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Source
{
    abstract class SyntaxNode
    {
        public abstract SyntaxType Kind { get; }
    }

    abstract class ExpressionSyntax : SyntaxNode { }
}

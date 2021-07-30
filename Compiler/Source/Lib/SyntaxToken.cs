using System.Collections.Generic;
using System.Linq;

namespace Compiler.Source.Lib
{
    public class SyntaxToken : SyntaxNode
    {
        public override SyntaxType Type { get; }
        public string Text { get; }
        public dynamic Value { get; }

        public SyntaxToken(SyntaxType type, string text, object value)
        {
            Type = type;
            Text = text;
            Value = value;
        }

        public bool Match(SyntaxType type, dynamic value=null)
        {
            if (value != null)
                return Type == type && Value == value;
            else
                return Type == type;
        }

        public bool IsUnaryOperator()
        {
            switch (Type)
            {
                case SyntaxType.PlusToken:
                case SyntaxType.MinusToken:
                    return true;
                default:
                    return false;
            }
        }

        public override string ToString()
        {
            if (Value != null)
                return $"{Type}: {Text} => ({Value})";
            return $"{Type}: {Text}";
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler.Source
{
    class SyntaxToken
    {
        public SyntaxType Type { get; }
        public Position PosStart { get; }
        public Position PosEnd { get; }
        public string Text { get; }
        public object Value { get; }

        public SyntaxToken(SyntaxType type, string text, object value, Position posStart=null, Position posEnd = null)
        {
            Type = type;
            Text = text;
            Value = value;

            if (posStart != null)
            {
                PosStart = posStart.Copy();
                PosEnd = posStart.Copy();
                PosEnd.Advance();
            }

            if (posEnd != null)
                PosEnd = posEnd.Copy();
        }

        public bool Matches(SyntaxType type, object value)
        {
            return Type == type && Value == value;
        }

        public override string ToString()
        {
            if (Value != null)
                return $"{Type}: {Text} => ({Value})";
            return $"{Type}: {Text}";
        }
    }
}

using Compiler.Source.Lib;
using Compiler.Source.Syntax;

namespace Compiler.Source.Datatypes
{
    public enum Datatypes
    {
        Number,
        Boolean,
        Null
    }

    public abstract class Datatype
    {
        public abstract Datatypes Type { get; }
        public abstract dynamic Value { set; get; }

        #region Utils
        public bool IsTrue()
        {
            return Value != null && Value != false;
        }
        #endregion

        #region Comp Operations [==, !=, <, >, <=, >=, &, |]
        public static BooleanType operator ==(Datatype left, Datatype right)
        {
            if (left.GetType() != right.GetType()) { return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false)); }
            if (left.Value == right.Value)
                return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));
            else
                return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));
        }

        public static BooleanType operator !=(Datatype left, Datatype right)
        {
            if (left.GetType() != right.GetType()) { return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false)); }
            if (left.Value != right.Value)
                return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));
            else
                return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));

        }

        public static BooleanType operator >(Datatype left, Datatype right)
        {
            if (left.GetType() != right.GetType()) { return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false)); }
            if (left.Value > right.Value)
                return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));
            else
                return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));

        }

        public static BooleanType operator <(Datatype left, Datatype right)
        {
            if (left.GetType() != right.GetType()) { return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false)); }
            if (left.Value < right.Value)
                return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));
            else
                return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));

        }

        public static BooleanType operator >=(Datatype left, Datatype right)
        {
            if (left.GetType() != right.GetType()) { return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false)); }
            if (left.Value >= right.Value)
                return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));
            else
                return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));

        }

        public static BooleanType operator <=(Datatype left, Datatype right)
        {
            if (left.GetType() != right.GetType()) { return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false)); }
            if (left.Value <= right.Value)
                return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));
            else
                return new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));

        }

        public static BooleanType operator &(Datatype left, Datatype right)
        {
            var comp = left.Value != null && right.Value != null && left.Value != false && right.Value != false;
            return new BooleanType(
                new SyntaxToken(SyntaxType.IdentifierToken, $"{comp}", comp)
                );
        }

        public static BooleanType operator |(Datatype left, Datatype right)
        {
            var comp = left.Value != null || right.Value != null && left.Value != false || right.Value != false;
            return new BooleanType(
                new SyntaxToken(SyntaxType.IdentifierToken, $"{comp}", comp)
                );
        }
        #endregion
    }
}

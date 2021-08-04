using Compiler.Source.Lib;
using Compiler.Source.Errors;
using System;

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

        public abstract bool CanArithmetic { get; }

        public abstract (Datatype, Error) Notted(Position pos, Context currentContext);

        #region Utils
        public bool IsTrue()
        {
            return !Value.Equals(null) && !Value.Equals(false);
        }

        public static bool IsNull(dynamic left, dynamic right)
        {
            return left.Equals(null) || right.Equals(null);
        }

        public static bool IsSameType(dynamic left, dynamic right)
        {
            return left.GetType() == right.GetType();
        }

        #nullable enable
        public static Error? Check(string opr, Position pos, Context context, dynamic left, dynamic right)
        {
            if (IsNull(left, right))
                return new RuntimeError(
                    pos,
                    $"Cannot use '{opr}' with null",
                    context.ContextString
                );
            if (!IsSameType(left, right))
                return new RuntimeError(
                    pos,
                    $"Cannot use '{opr}' with types <'{left.Type}'> & <'{right.Type}'>",
                    context.ContextString
                );
            return null;
        }
        #nullable disable
        #endregion

        #region Comparisons 
        public (BooleanType, Error) EqualsTo(Position pos, Context context, dynamic right)
        {
            var left = this;
            var falseType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));
            var trueType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));

            var c = Check("==", pos, context, left, right);
            if (c != null)
                return (null, c);

            if (left.Value == right.Value)
                return (trueType, null);
            else
                return (falseType, null);
        }

        public (BooleanType, Error) NotEqualsTo(Position pos, Context context, dynamic right)
        {
            var left = this;
            var falseType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));
            var trueType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));

            var c = Check("!=", pos, context, left, right);
            if (c != null)
                return (null, c);

            if (left.Value != right.Value)
                return (trueType, null);
            else
                return (falseType, null);
        }

        public (BooleanType, Error) GreaterThan(Position pos, Context context, dynamic right)
        {
            var left = this;
            var falseType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));
            var trueType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));

            var c = Check(">", pos, context, left, right);
            if (c != null)
                return (null, c);

            if (left.Value > right.Value)
                return (trueType, null);
            else
                return (falseType, null);
        }

        public (BooleanType, Error) LessThan(Position pos, Context context, dynamic right)
        {
            var left = this;
            var falseType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));
            var trueType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));

            var c = Check("<", pos, context, left, right);
            if (c != null)
                return (null, c);

            if (left.Value < right.Value)
                return (trueType, null);
            else
                return (falseType, null);
        }

        public (BooleanType, Error) GreaterThanEquals(Position pos, Context context, dynamic right)
        {
            var left = this;
            var falseType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));
            var trueType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));

            var c = Check(">=", pos, context, left, right);
            if (c != null)
                return (null, c);

            if (left.Value >= right.Value)
                return (trueType, null);
            else
                return (falseType, null);
        }

        public (BooleanType, Error) LessThanEquals(Position pos, Context context, dynamic right)
        {
            var left = this;
            var falseType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));
            var trueType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));

            var c = Check("<=", pos, context, left, right);
            if (c != null)
                return (null, c);

            if (left.Value <= right.Value)
                return (trueType, null);
            else
                return (falseType, null);
        }

        public BooleanType AndedBy(Position pos, Context context, dynamic right)
        {
            var left = this;
            var falseType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));
            var trueType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));

            if (IsNull(left, right))
                return falseType;

            if (!IsSameType(left, right))
                return falseType;

            var chk = left.Value != null && right.Value != null;
            if (chk)
                return trueType;
            else
                return falseType;
        }

        public BooleanType OredBy(Position pos, Context context, dynamic right)
        {
            var left = this;
            var falseType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "false", false));
            var trueType = new BooleanType(new SyntaxToken(SyntaxType.IdentifierToken, "true", true));

            if (IsNull(left, right))
                return falseType;

            if (!IsSameType(left, right))
                return falseType;

            var chk = left.Value != null || right.Value != null;
            if (chk)
                return trueType;
            else
                return falseType;
        }
        #endregion
    }
}

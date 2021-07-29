namespace Compiler.Source
{
    public enum SyntaxType
    {
        //Tokens
        EndOfFileToken,
        BadToken,
        SemicolonToken,
        NumberToken,
        PlusToken,
        MinusToken,
        SlashToken,
        StarToken,
        PowToken,
        OpenParenthesisToken,
        CloseParenthesisToken,

        //Expressions
        ErroredExpression,
        NumberExpression,
        ParenthesizedExpression,
        BinaryExpression
    }
}

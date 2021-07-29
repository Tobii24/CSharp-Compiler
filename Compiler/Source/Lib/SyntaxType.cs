namespace Compiler.Source.Lib
{
    public enum SyntaxType
    {
        //Tokens
        EndOfFileToken,
        BadToken,
        SemicolonToken,
        NumberToken,
        KeywordToken,
        IdentifierToken,
        PlusToken,
        PlusPlusToken,
        MinusToken,
        MinusMinusToken,
        SlashToken,
        StarToken,
        PowToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        EqualsToken,

        //Expressions
        ErroredExpression,
        LiteralExpression,
        ParenthesizedExpression,
        BinaryExpression,
        UnaryExpression
    }
}

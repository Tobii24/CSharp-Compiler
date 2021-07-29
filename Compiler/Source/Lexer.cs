using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using Compiler.Source.Errors;

namespace Compiler.Source
{
    internal sealed class Lexer
    {
        private readonly string _text;
        private readonly string _filename;
        private Position _position;

        public Lexer(string filename, string text)
        {
            _text = text;
            _filename = filename;
            _position = new Position(0, 0, 0, filename, text);
        }

        private char? CurrentChar
        {
            get
            {
                if (_position.Index >= _text.Length)
                    return null;

                return _text[_position.Index];
            }
        }

        private void Next()
        {
            _position.Advance(CurrentChar);
        }

        public (SyntaxToken[], Error) GetTokens()
        {
            var tokens = new List<SyntaxToken>();

            while (CurrentChar != null)
            {
                #region Ignore indentation
                if (CurrentChar == '\t' || CurrentChar == ' ' || CurrentChar == '\n')
                {
                    Next();
                }
                #endregion

                #region Numbers
                else if (char.IsDigit((char)CurrentChar))
                {
                    var pos_start = _position.Copy();
                    string end_num = "";
                    int dot_count = 0;

                    while (CurrentChar != null && Regex.IsMatch(CurrentChar.ToString(), @"^[0-9]+$") || CurrentChar == '.')
                    {
                        if (CurrentChar == '.')
                        {
                            if (dot_count == 1)
                                break;
                            dot_count++;
                            end_num += '.';
                        }
                        else { end_num += CurrentChar; }

                        Next();
                    }

                    if (dot_count == 1)
                    {
                        float val = float.Parse(end_num, CultureInfo.InvariantCulture);
                        tokens.Add(new SyntaxToken(SyntaxType.NumberToken, end_num, val, pos_start, _position));
                    }
                    else
                    {
                        tokens.Add(new SyntaxToken(SyntaxType.NumberToken, end_num, int.Parse(end_num)));
                    }
                }
                #endregion

                #region Operation Symbols [-, +, *, /, ^] & Parenthesis
                else if (CurrentChar == '+')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.PlusToken, "+", null, _position));
                }
                else if (CurrentChar == '-')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.MinusToken, "-", null, _position));
                }
                else if (CurrentChar == '*')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.StarToken, "*", null, _position));
                }
                else if (CurrentChar == '/')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.SlashToken, "/", null, _position));
                }
                else if (CurrentChar == '^')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.PowToken, "^", null, _position));
                }
                else if (CurrentChar == '(')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.OpenParenthesisToken, "(", null, _position));
                }
                else if (CurrentChar == ')')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.CloseParenthesisToken, ")", null, _position));
                }
                #endregion

                #region Semicolon (newline ref)
                else if (CurrentChar == ';')
                {
                    Next();
                    tokens.Add(new SyntaxToken(SyntaxType.SemicolonToken, ";", null, _position));
                }
                #endregion

                #region Unknown Character
                else
                {
                    var _posStart = _position.Copy();
                    char? Char = CurrentChar;
                    Next();
                    return (Array.Empty<SyntaxToken>(), new IllegalCharError(_posStart, _position, $"'{Char}'"));
                }
                #endregion
            }

            Next();

            tokens.Add(new SyntaxToken(SyntaxType.EndOfFileToken, "\0", null, _position));
            return (tokens.ToArray(), null);
        }
    }
}

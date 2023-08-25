using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Yeet.Common.Assembler;

public sealed class Lexer
{
    public readonly bool IncludeCommas;
    public readonly bool IncludeWhitespaces;
    public readonly bool IncludeComments;

    private readonly string _text;

    private int _index;

    public Lexer(string? file, bool includeCommas = true, bool includeWhitespaces = true, bool includeComments = true)
    {
        if (!File.Exists(file)) throw new FileNotFoundException("LEXER HALTED: FileNotFound Exception");

        IncludeCommas = includeCommas;
        IncludeWhitespaces = includeWhitespaces;
        IncludeComments = includeComments;

        _text = File.ReadAllText(file);
        _index = 0;
    }

    ///<summary>Separate each word into tokens</summary>
    ///<returns>Returns file tokens</returns>
    ///<exception cref="FileNotFoundException">Could not find a file, make sure the path is correct.</exception>
    ///<exception cref="SyntaxErrorException">The character/string isn't recognised.</exception>
    public List<Token> Run()
    {
        var tokens = new List<Token>();

        while (_index < _text.Length)
        {
            var character = _text[_index];

            switch (character)
            {
                case ',':
                {
                    if (IncludeCommas) tokens.Add(new Token(TokenType.Comma, character.ToString()));
                    _index++;
                    break;
                }
                case ' ' or '\r' or '\n' or '\t':
                {
                    if (IncludeWhitespaces) tokens.Add(new Token(TokenType.Whitespace, character.ToString()));
                    _index++;
                    break;
                }
                default:
                {
                    if (char.IsDigit(character))
                    {
                        // Determine the start and end _index of the number
                        var start = _index;
                        var end = start;

                        while (_index < _text.Length && char.IsDigit(_text[_index]))
                        {
                            end++;
                            _index++;
                        }

                        tokens.Add(new Token(TokenType.Number, _text.Substring(start, end - start)));
                    }
                    else if (char.IsLetter(character))
                    {
                        var start = _index;
                        var end = start;

                        char c;
                        while (_index < _text.Length && (char.IsLetter(c = _text[_index]) || char.IsDigit(c)))
                        {
                            end++;
                            _index++;
                        }

                        var substring = _text.Substring(start, end - start).ToLowerInvariant();

                        // Check if substring is an opcode or a register
                        if (IsOpcode(substring))
                            tokens.Add(new Token(TokenType.Opcode, substring));
                        // Check if the string is a register
                        else if (substring[0] is 'r' && byte.TryParse(substring = substring[1..], out var num) &&
                                 num <= 15)
                            tokens.Add(new Token(TokenType.Register, substring));
                        // Checks if the string is a label
                        else if (_index < _text.Length && _text[_index++] == ':')
                            tokens.Add(new Token(TokenType.Label, substring));
                        else
                            throw new SyntaxErrorException($"LEXER HALTED: Invalid string \"{substring}\"");
                    }
                    else if (character == '#')
                    {
                        var start = ++_index;
                        var end = start;

                        while (_index < _text.Length && _text[_index] is not '\r' and not '\n')
                        {
                            end++;
                            _index++;
                        }

                        if (IncludeComments)
                            tokens.Add(new Token(TokenType.Comment, _text.Substring(start, end - start).TrimStart()));
                    }
                    else
                    {
                        throw new SyntaxErrorException($"LEXER HALTED: Invalid character '{character}'");
                    }

                    break;
                }
            }
        }

        return tokens;
    }

    private static bool IsOpcode(string text)
    {
        return text
            is "add" or "sub" or "mul" or "div" or "mod" or "and" or "or" or "xor" or "not" or "shl" or "shr" or "sal"
            or "sar"
            or "read" or "write" or "move" or "push" or "pop"
            or "in" or "out"
            or "ret" or "jump" or "call" or "cmp" or "jb" or "ja" or "je" or "jne" or "jbe" or "jae";
    }
}
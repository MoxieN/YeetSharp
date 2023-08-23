using System.Data;

namespace Yeet.Common.Assembler;

public abstract class Lexer
{
    public readonly bool IncludeCommas;
    public readonly bool IncludeWhitespaces;
    public readonly bool IncludeComments;

    private readonly string _text;

    private int _index;

    protected Lexer(string file, bool includeCommas = true, bool includeWhitespaces = true, bool includeComments = true)
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
                        {
                            tokens.Add(new Token(TokenType.Opcode, substring));
                        }
                        else if (substring[0] is 'r' && byte.TryParse(substring = substring[1..], out var num) && num <= 15)
                        {
                            tokens.Add(new Token(TokenType.Register, substring));
                        }
                        else
                        {
                            throw new SyntaxErrorException($"LEXER HALTED: Invalid string \"{substring}\"");
                        }
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

    protected abstract bool IsOpcode(string text);
}
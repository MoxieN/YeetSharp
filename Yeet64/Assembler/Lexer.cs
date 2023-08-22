namespace Yeet64.Assembler;
using System.Data;

public sealed class Lexer
{
    public enum TokenType
    {
        Instruction,
        Register,
        Comma,
        Whitespace,
        Number,
        Comment,
    }
    
    public sealed record Token(TokenType Type, string Text);
    
    ///<summary>Separate each word into tokens</summary>
    ///<returns>Returns file tokens</returns>
    ///<exception cref="FileNotFoundException">Could not find a file, make sure the path is correct.</exception>
    ///<exception cref="SyntaxErrorException">The character/string isn't recognised.</exception>
    public static List<Token> LexInstructions(string file)
    {
        if (!File.Exists(file)) throw new FileNotFoundException("LEXER HALTED: FileNotFound Exception");

        var tokens = new List<Token>();
        var text = File.ReadAllText(file);
        var index = 0;

        while (index < text.Length)
        {
            var character = text[index];

            if (character is ',')
            {
                tokens.Add(new Token(TokenType.Comma, character.ToString()));
                index++;
            }
            else if (character is ' ' or '\r' or '\n' or '\t')
            {
                tokens.Add(new Token(TokenType.Whitespace, character.ToString()));
                index++;
            }
            else if (char.IsDigit(character))
            {
                // Determine the start and end index of the number
                var start = index;
                var end = start;

                while (index < text.Length && char.IsDigit(text[index]))
                {
                    end++;
                    index++;
                }

                tokens.Add(new Token(TokenType.Number, text.Substring(start, end - start)));
            }
            else if (char.IsLetter(character))
            {
                var start = index;
                var end = start;

                char c;
                while (index < text.Length && (char.IsLetter(c = text[index]) || char.IsDigit(c)))
                {
                    end++;
                    index++;
                }
  
                var substring = text.Substring(start, end - start).ToLowerInvariant();
    
                // Check if substring is an instruction or a register
                if (IsInstruction(substring))
                {
                    tokens.Add(new Token(TokenType.Instruction, substring));
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
                var start = ++index;
                var end = start;

                while (index < text.Length && text[index] is not '\r' and not '\n')
                {
                    end++;
                    index++;
                }

                tokens.Add(new Token(TokenType.Comment, text.Substring(start, end - start).TrimStart()));
            }
            else
            {
                throw new SyntaxErrorException($"LEXER HALTED: Invalid character '{character}'");
            }
        }
        
        return tokens;
    }

    private static bool IsInstruction(string text) => text
        is "add" or "sub" or "mul" or "div" or "mod" or "and" or "or" or "xor" or "not" or "shl" or "shr" or "sal" or "sar"
        or "read" or "write" or "move" or "push" or "pop"
        or "in" or "out"
        or "ret" or "jump" or "call" or "cmp" or "jb" or "ja" or "je" or "jne" or "jbe" or "jae";
}
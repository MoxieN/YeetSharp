namespace Yeet8.Assembler;

public class Lexer : Yeet.Common.Assembler.Lexer
{
    public Lexer(string file, bool includeCommas = true, bool includeWhitespaces = true, bool includeComments = true)
        : base(file, includeCommas, includeWhitespaces, includeComments)
    { }

    protected override bool IsOpcode(string text) => text
        is "add" or "sub" or "mul" or "div" or "mod" or "and" or "or" or "xor" or "not" or "shl" or "shr" or "sal" or "sar"
        or "read" or "write" or "move" or "push" or "pop"
        or "in" or "out"
        or "ret" or "jump" or "call" or "jb" or "ja" or "je" or "jne" or "jbe" or "jae";
}
using System.Data;

namespace YeetSharp.Assembler;

public static class Lexer
{
    public static AsmInstruction[] LexInstructions(string file, params string[] arguments)
    {
        if (!File.Exists(file)) throw new FileNotFoundException("LEXER HALTED: FileNotFound Exception");

        var r = new List<AsmInstruction>();

        foreach (var l in File.ReadAllLines(file))
        {
            if (string.IsNullOrEmpty(l) || l.StartsWith('#'))
                continue;

            var s = l.Split(' ');

            var name = s[0];
            var flags = (byte)0;
            var op1 = (byte)0;
            var op2 = (byte)0;
            var op3 = (byte)0;

            if (s.Length >= 2)
            {
                var op1Str = s[1].ToLowerInvariant();

                if (op1Str.EndsWith(',')) op1Str = op1Str.Remove(op1Str.Length - 1);
                if (op1Str.StartsWith('r'))
                {
                    flags |= Flag.Op1Register;
                    op1Str = op1Str[1..];
                }

                try
                {
                    op1 = op1Str.StartsWith("0x") ? Convert.ToByte(op1Str, 16) : Convert.ToByte(op1Str, 10);
                }
                catch (FormatException)
                {
                    throw new SyntaxErrorException($"PARSER HALTED: Argument {op1Str} is invalid");
                }
            }

            if (s.Length >= 3)
            {
                var op2Str = s[2].ToLowerInvariant();

                if (op2Str.EndsWith(',')) op2Str = op2Str.Remove(op2Str.Length - 1);
                if (op2Str.StartsWith('r'))
                {
                    flags |= Flag.Op2Register;
                    op2Str = op2Str[1..];
                }

                try
                {
                    op2 = op2Str.StartsWith("0x") ? Convert.ToByte(op2Str, 16) : Convert.ToByte(op2Str, 10);
                }
                catch (FormatException)
                {
                    throw new SyntaxErrorException($"PARSER HALTED: Argument {op2Str} is invalid");
                }
            }

            if (s.Length >= 4)
            {
                var op3Str = s[3].ToLowerInvariant();

                if (op3Str.EndsWith(',')) op3Str = op3Str.Remove(op3Str.Length - 1);
                if (op3Str.StartsWith('r'))
                {
                    flags |= Flag.Op3Register;
                    op3Str = op3Str[1..];
                }

                try
                {
                    op3 = op3Str.StartsWith("0x") ? Convert.ToByte(op3Str, 16) : Convert.ToByte(op3Str, 10);
                }
                catch (FormatException)
                {
                    throw new SyntaxErrorException($"PARSER HALTED: Argument {op3Str} is invalid");
                }
            }

            r.Add(new AsmInstruction(name, flags, op1, op2, op3));
        }

        return r.ToArray();
    }
}
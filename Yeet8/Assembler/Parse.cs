using System.Data;
using YeetSharp.Interpreter;

namespace YeetSharp.Assembler;

public static class Parse
{
    public static byte[] ParseInstructions(AsmInstruction[] asmInstructions)
    {
        var code = new List<byte>();

        foreach (var i in asmInstructions)
        {
            var instruction = Instruction.Create(GetOpCodeByName(i.Name), i.Flags, i.Op1, i.Op2, i.Op3);
            code.AddRange(BitConverter.GetBytes(instruction));
        }

        return code.ToArray();
    }

    public static byte GetOpCodeByName(string op)
    {
        var opName = op.ToLowerInvariant() switch
        {
            "add" => OpCode.Add,
            "sub" => OpCode.Sub,
            "mul" => OpCode.Mul,
            "div" => OpCode.Div,
            "mod" => OpCode.Mod,
            "and" => OpCode.And,
            "or" => OpCode.Or,
            "xor" => OpCode.Xor,
            "not" => OpCode.Not,
            "shl" => OpCode.Shl,
            "shr" => OpCode.Shr,
            "sal" => OpCode.Sal,
            "sar" => OpCode.Sar,
            "read" => OpCode.Read,
            "write" => OpCode.Write,
            "move" => OpCode.Move,
            "push" => OpCode.Push,
            "pop" => OpCode.Pop,
            "in" => OpCode.In,
            "out" => OpCode.Out,
            "ret" => OpCode.Ret,
            "jump" => OpCode.Jump,
            "call" => OpCode.Call,
            "jb" => OpCode.Jb,
            "ja" => OpCode.Ja,
            "je" => OpCode.Je,
            "jne" => OpCode.Jne,
            "jbe" => OpCode.Jbe,
            "jae" => OpCode.Jae,
            _ => throw new SyntaxErrorException($"PARSER HALTED: Instruction \'{op}\' unrecognized.")
        };

        return opName;
    }
}
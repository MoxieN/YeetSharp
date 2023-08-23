namespace Yeet64.Interpreter;

public static class Executor
{
    public static void Execute()
    {
        if (!Computer.PoweredOn) return;

        Computer.R0 = Computer.StackSize;

        while (Computer.R0 < Computer.MemorySize && Computer.PoweredOn)
        {
            var instruction = (uint)
            (
                Computer.MemoryRead8(Computer.R0++)
                | (Computer.MemoryRead8(Computer.R0++) << 8)
                | (Computer.MemoryRead8(Computer.R0++) << 16)
                | (Computer.MemoryRead8(Computer.R0++) << 24)
            );

            var opcode = (instruction >> 27) & ((1U << 5) - 1);

            switch (opcode)
            {
                // Type 1 instructions
                case OpCode.Add:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);

                    SetRegister(destination, GetRegister(destination) + (isRegister ? GetRegister(source) : source));
                    break;
                }
                case OpCode.Sub: break;
                case OpCode.Mul: break;
                case OpCode.Div: break;
                case OpCode.Mod: break;
                case OpCode.And: break;
                case OpCode.Or: break;
                case OpCode.Xor: break;
                case OpCode.Not: break;
                case OpCode.Shl: break;
                case OpCode.Shr: break;
                case OpCode.Sal: break;
                case OpCode.Sar: break;
                case OpCode.Read: break;
                case OpCode.Write: break;
                case OpCode.Move:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);

                    SetRegister(destination, isRegister ? GetRegister(source) : source);
                    break;
                }
                case OpCode.In: break;
                case OpCode.Out:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);

                    Computer.PortWrite(GetRegister(destination), isRegister ? GetRegister(source) : source);
                    break;
                }
                case OpCode.Cmp: break;

                // Type 2 instructions
                case OpCode.Push:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var source = instruction & ((1U << 26) - 1);

                    Computer.MemoryWrite64(Computer.R1, isRegister ? GetRegister(source) : source);
                    Computer.R1 += sizeof(ulong);
                    break;
                }
                case OpCode.Jump: break;
                case OpCode.Call: break;
                case OpCode.Jb: break;
                case OpCode.Ja: break;
                case OpCode.Je: break;
                case OpCode.Jne: break;
                case OpCode.Jbe: break;
                case OpCode.Jae: break;

                // Type 3 instructions
                case OpCode.Pop:
                {
                    var destination = (instruction >> 23) & ((1U << 4) - 1);

                    Computer.R1 -= 8;
                    SetRegister(destination, Computer.MemoryRead64(Computer.R1));
                    break;
                }

                // Type 4 instructions
                case OpCode.Ret: break;
            }
        }
    }

    #region Helpers

    private static ulong GetRegister(uint index)
    {
        return index switch
        {
            0 => Computer.R0,
            1 => Computer.R1,
            2 => Computer.R2,
            3 => Computer.R3,
            4 => Computer.R4,
            5 => Computer.R5,
            6 => Computer.R6,
            7 => Computer.R7,
            8 => Computer.R8,
            9 => Computer.R9,
            10 => Computer.R10,
            11 => Computer.R11,
            12 => Computer.R12,
            13 => Computer.R13,
            14 => Computer.R14,
            15 => Computer.R15,
            _ => throw new InvalidOperationException($"Trying to get invalid register: R{index}")
        };
    }

    private static void SetRegister(uint index, ulong value)
    {
        switch (index)
        {
            case 0:
                Computer.R0 = value;
                break;
            case 1:
                Computer.R1 = value;
                break;
            case 2:
                Computer.R2 = value;
                break;
            case 3:
                Computer.R3 = value;
                break;
            case 4:
                Computer.R4 = value;
                break;
            case 5:
                Computer.R5 = value;
                break;
            case 6:
                Computer.R6 = value;
                break;
            case 7:
                Computer.R7 = value;
                break;
            case 8:
                Computer.R8 = value;
                break;
            case 9:
                Computer.R9 = value;
                break;
            case 10:
                Computer.R10 = value;
                break;
            case 11:
                Computer.R11 = value;
                break;
            case 12:
                Computer.R12 = value;
                break;
            case 13:
                Computer.R13 = value;
                break;
            case 14:
                Computer.R14 = value;
                break;
            case 15:
                Computer.R15 = value;
                break;
            default: throw new InvalidOperationException($"Trying to set invalid register: R{index}");
        }
    }

    #endregion
}
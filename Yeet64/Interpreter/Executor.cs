namespace Yeet64.Interpreter;

public static class Executor
{
    public static void Execute()
    {
        if (!Computer.PoweredOn) return;

        Computer.R0 = Computer.StackSize;

        while (Computer.R0 < Computer.MemorySize && Computer.PoweredOn)
        {
            var opcode = (uint)
            (
                Computer.MemoryRead8(Computer.R0++)
                | (Computer.MemoryRead8(Computer.R0++) << 8)
                | (Computer.MemoryRead8(Computer.R0++) << 16)
                | (Computer.MemoryRead8(Computer.R0++) << 24)
            );

            var instruction = (opcode >> 27) & ((1U << 5) - 1);

            switch (instruction)
            {
                // Type 1 instructions
                case Instruction.Add:
                {
                    var isRegister = (opcode & (1U << 26)) != 0;
                    var destination = (opcode >> 22) & ((1U << 4) - 1);
                    var source = opcode & ((1U << 22) - 1);

                    SetRegister(destination, GetRegister(destination) + (isRegister ? GetRegister(source) : source));
                    break;
                }
                case Instruction.Sub: break;
                case Instruction.Mul: break;
                case Instruction.Div: break;
                case Instruction.Mod: break;
                case Instruction.And: break;
                case Instruction.Or: break;
                case Instruction.Xor: break;
                case Instruction.Not: break;
                case Instruction.Shl: break;
                case Instruction.Shr: break;
                case Instruction.Sal: break;
                case Instruction.Sar: break;
                case Instruction.Read: break;
                case Instruction.Write: break;
                case Instruction.Move:
                {
                    var isRegister = (opcode & (1U << 26)) != 0;
                    var destination = (opcode >> 22) & ((1U << 4) - 1);
                    var source = opcode & ((1U << 22) - 1);

                    SetRegister(destination, isRegister ? GetRegister(source) : source);
                    break;
                }
                case Instruction.In: break;
                case Instruction.Out:
                {
                    var isRegister = (opcode & (1U << 26)) != 0;
                    var destination = (opcode >> 22) & ((1U << 4) - 1);
                    var source = opcode & ((1U << 22) - 1);

                    Computer.PortWrite(GetRegister(destination), isRegister ? GetRegister(source) : source);
                    break;
                }
                case Instruction.Cmp: break;

                // Type 2 instructions
                case Instruction.Push:
                {
                    var isRegister = (opcode & (1U << 26)) != 0;
                    var source = opcode & ((1U << 26) - 1);

                    Computer.MemoryWrite64(Computer.R1, isRegister ? GetRegister(source) : source);
                    Computer.R1 += sizeof(ulong);
                    break;
                }
                case Instruction.Jump: break;
                case Instruction.Call: break;
                case Instruction.Jb: break;
                case Instruction.Ja: break;
                case Instruction.Je: break;
                case Instruction.Jne: break;
                case Instruction.Jbe: break;
                case Instruction.Jae: break;

                // Type 3 instructions
                case Instruction.Pop:
                {
                    var destination = (opcode >> 23) & ((1U << 4) - 1);

                    Computer.R1 -= 8;
                    SetRegister(destination, Computer.MemoryRead64(Computer.R1));
                    break;
                }

                // Type 4 instructions
                case Instruction.Ret: break;
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
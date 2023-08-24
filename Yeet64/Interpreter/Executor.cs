using System;
using Yeet.Common;

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
                    var isRegister = (instruction & (1U << 26)) != 0; // Gets the 26th bit (starting from the right)
                    var destination = (instruction >> 22) & ((1U << 4) - 1); // Gets 4 bits starting from the 22th
                    var source = instruction & ((1U << 22) - 1); // Gets 22 bits starting from the first

                    // Gets register value if source is register, else uses source value directly to set the destination register's value
                    Computer.SetRegister(destination,
                        Computer.GetRegister(destination) + (isRegister ? Computer.GetRegister(source) : source));
                    break;
                }

                case OpCode.Sub:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);

                    Computer.SetRegister(destination,
                        Computer.GetRegister(destination) - (isRegister ? Computer.GetRegister(source) : source));
                    break;
                }

                case OpCode.Mul:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);

                    Computer.SetRegister(destination,
                        Computer.GetRegister(destination) * (isRegister ? Computer.GetRegister(source) : source));
                    break;
                }

                case OpCode.Div:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);

                    Computer.SetRegister(destination,
                        Computer.GetRegister(destination) / (isRegister ? Computer.GetRegister(source) : source));
                    break;
                }

                case OpCode.Mod:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);

                    Computer.SetRegister(destination,
                        Computer.GetRegister(destination) % (isRegister ? Computer.GetRegister(source) : source));
                    break;
                }

                case OpCode.And:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);

                    Computer.SetRegister(destination,
                        Computer.GetRegister(destination) & (isRegister ? Computer.GetRegister(source) : source));
                    break;
                }

                case OpCode.Or:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);

                    Computer.SetRegister(destination,
                        Computer.GetRegister(destination) | (isRegister ? Computer.GetRegister(source) : source));
                    break;
                }

                case OpCode.Xor:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);

                    Computer.SetRegister(destination,
                        Computer.GetRegister(destination) ^ (isRegister ? Computer.GetRegister(source) : source));
                    break;
                }

                case OpCode.Shl: throw new NotImplementedException();

                case OpCode.Shr: throw new NotImplementedException();

                case OpCode.Sal: throw new NotImplementedException();

                case OpCode.Sar: throw new NotImplementedException();

                case OpCode.Read: throw new NotImplementedException();

                case OpCode.Write:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);
                    
                    Computer.MemoryWrite64(destination, isRegister ? Computer.GetRegister(source) : source);
                    break;
                }

                case OpCode.Move:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);

                    Computer.SetRegister(destination, isRegister ? Computer.GetRegister(source) : source);
                    break;
                }

                case OpCode.In:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);

                    Computer.SetRegister(destination,
                        Computer.PortRead(isRegister ? Computer.GetRegister(source) : source));
                    break;
                }

                case OpCode.Out:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var destination = (instruction >> 22) & ((1U << 4) - 1);
                    var source = instruction & ((1U << 22) - 1);

                    Computer.PortWrite(Computer.GetRegister(destination),
                        isRegister ? Computer.GetRegister(source) : source);
                    break;
                }

                case OpCode.Cmp:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var operand1 = (instruction >> 22) & ((1U << 4) - 1);
                    var operand2 = instruction & ((1U << 22) - 1);

                    var source1 = Computer.GetRegister(operand1);
                    var source2 = isRegister ? Computer.GetRegister(operand2) : operand2;

                    if (source1 < source2) Computer.R2 |= Flag.Below;
                    if (source1 > source2) Computer.R2 |= Flag.Above;
                    if (source1 <= source2) Computer.R2 |= Flag.BelowOrEqual;
                    if (source1 >= source2) Computer.R2 |= Flag.AboveOrEqual;
                    if (source1 == source2) Computer.R2 |= Flag.Equal;
                    if (source1 != source2) Computer.R2 |= Flag.NotEqual;
                    break;
                }

                // Type 2 instructions
                case OpCode.Push:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var source = instruction & ((1U << 26) - 1);

                    Computer.PushStack(isRegister ? Computer.GetRegister(source) : source);
                    break;
                }

                case OpCode.Jump:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var source = instruction & ((1U << 26) - 1);

                    Computer.R0 = Computer.StackSize + (isRegister ? Computer.GetRegister(source) : source);
                    break;
                }

                case OpCode.Call:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var source = instruction & ((1U << 26) - 1);

                    // Push R0 to the stack for it to be retrieved later by ret
                    Computer.MemoryWrite64(Computer.R1, Computer.R0);
                    Computer.R1 += sizeof(ulong);

                    Computer.R0 = Computer.StackSize + (isRegister ? Computer.GetRegister(source) : source);
                    break;
                }

                case OpCode.Jb:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var source = instruction & ((1U << 26) - 1);

                    if ((Computer.R2 & Flag.Below) != 0)
                        Computer.R0 = isRegister ? Computer.GetRegister(source) : source;
                    break;
                }

                case OpCode.Ja:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var source = instruction & ((1U << 26) - 1);

                    if ((Computer.R2 & Flag.Above) != 0)
                        Computer.R0 = isRegister ? Computer.GetRegister(source) : source;
                    break;
                }

                case OpCode.Je:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var source = instruction & ((1U << 26) - 1);

                    if ((Computer.R2 & Flag.Equal) != 0)
                        Computer.R0 = isRegister ? Computer.GetRegister(source) : source;
                    break;
                }

                case OpCode.Jne:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var source = instruction & ((1U << 26) - 1);

                    if ((Computer.R2 & Flag.NotEqual) != 0)
                        Computer.R0 = isRegister ? Computer.GetRegister(source) : source;
                    break;
                }

                case OpCode.Jbe:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var source = instruction & ((1U << 26) - 1);

                    if ((Computer.R2 & Flag.BelowOrEqual) != 0)
                        Computer.R0 = isRegister ? Computer.GetRegister(source) : source;
                    break;
                }

                case OpCode.Jae:
                {
                    var isRegister = (instruction & (1U << 26)) != 0;
                    var source = instruction & ((1U << 26) - 1);

                    if ((Computer.R2 & Flag.AboveOrEqual) != 0)
                        Computer.R0 = isRegister ? Computer.GetRegister(source) : source;
                    break;
                }

                // Type 3 instructions
                case OpCode.Pop:
                {
                    var destination = (instruction >> 23) & ((1U << 4) - 1);
                    Computer.SetRegister(destination, Computer.PopStack());
                    break;
                }

                case OpCode.Not:
                {
                    var destination = (instruction >> 22) & ((1U << 4) - 1);

                    Computer.SetRegister(destination, ~Computer.GetRegister(destination));
                    break;
                }

                // Type 4 instructions
                case OpCode.Ret:
                {
                    // Get address value by popping R0 from stack (set earlier by call)
                    Computer.R1 -= 8;
                    Computer.R0 = Computer.MemoryRead64(Computer.R1);
                    break;
                }
            }
        }
    }

    #region Helpers

    #endregion
}
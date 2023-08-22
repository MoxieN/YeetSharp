using Yeet.Common;

namespace Yeet8.Interpreter;

public static class Executor
{
    private static ushort _position;

    public static void Execute()
    {
        if (!Computer.PoweredOn) return;

        var uintArray = new byte[4];
        var ushortArray = new byte[2];

        _position = Computer.UserMemory + Computer.StackSize;

        while (_position < Computer.MemorySize && Computer.PoweredOn)
        {
            uintArray[0] = Computer.MemoryRead(_position++);
            uintArray[1] = Computer.MemoryRead(_position++);
            uintArray[2] = Computer.MemoryRead(_position++);
            uintArray[3] = Computer.MemoryRead(_position++);

            var instruction = BitConverter.ToUInt32(uintArray);
            var binary = Convert.ToString(instruction, 2).PadLeft(32, '0');

            var opCode = Convert.ToByte(binary[..5], 2);
            var flags = Convert.ToByte(binary[5..8], 2);
            var op1 = Convert.ToByte(binary[8..16], 2);
            var op2 = Convert.ToByte(binary[16..24], 2);
            var op3 = Convert.ToByte(binary[24..32], 2);

            switch (opCode)
            {
                case OpCode.Add:
                {
                    var add1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var add2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;

                    var result = add1 + add2;
                    if (result > byte.MaxValue)
                        Utils.AbortExecution($"Value reached max value: {result}");

                    SetRegister(op1, (byte)result);
                    break;
                }
                case OpCode.Sub:
                {
                    var sub1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var sub2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;

                    var result = sub1 - sub2;
                    if (result < byte.MinValue)
                        Utils.AbortExecution($"Value reached min value: {result}");

                    SetRegister(op1, (byte)result);
                    break;
                }
                case OpCode.Mul:
                {
                    var mul1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var mul2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;

                    var result = mul1 * mul2;
                    if (result > byte.MaxValue)
                        Utils.AbortExecution($"Value reached max value: {result}");

                    SetRegister(op1, (byte)result);
                    break;
                }
                case OpCode.Div:
                {
                    var div1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var div2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;

                    var result = div1 / div2;
                    if (result < byte.MinValue)
                        Utils.AbortExecution($"Value reached min value: {result}");

                    SetRegister(op1, (byte)result);
                    break;
                }
                case OpCode.Mod:
                {
                    var mod1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var mod2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;
                    var result = mod1 % mod2;
                    SetRegister(op1, (byte)result);
                    break;
                }
                case OpCode.And:
                {
                    var and1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var and2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;
                    var result = and1 & and2;
                    SetRegister(op1, (byte)result);
                    break;
                }
                case OpCode.Or:
                {
                    var or1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var or2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;
                    var result = or1 | or2;
                    SetRegister(op1, (byte)result);
                    break;
                }
                case OpCode.Xor:
                {
                    var xor1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var xor2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;
                    var result = xor1 ^ xor2;
                    SetRegister(op1, (byte)result);
                    break;
                }
                case OpCode.Not:
                {
                    var not = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var result = ~not;
                    SetRegister(op1, (byte)result);
                    break;
                }
                case OpCode.Shl:
                {
                    throw new NotImplementedException();
                }
                case OpCode.Shr:
                {
                    throw new NotImplementedException();
                }
                case OpCode.Sal:
                {
                    throw new NotImplementedException();
                }
                case OpCode.Sar:
                {
                    throw new NotImplementedException();
                }
                case OpCode.Read:
                {
                    var address = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    Read(op1, address);
                    break;
                }
                case OpCode.Write:
                {
                    var address = (flags & Flag.Op1Register) != 0 ? GetRegister(op1) : op1;
                    var value = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    Write(address, value);
                    break;
                }
                case OpCode.Move:
                {
                    var value = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    Move(op1, value);
                    break;
                }
                case OpCode.Push:
                {
                    var value = (flags & Flag.Op1Register) != 0 ? GetRegister(op1) : op1;
                    Push(value);
                    break;
                }
                case OpCode.Pop:
                {
                    Pop(op1);
                    break;
                }
                case OpCode.In:
                {
                    var port = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    In(op1, port);
                    break;
                }
                case OpCode.Out:
                {
                    var port = (flags & Flag.Op1Register) != 0 ? GetRegister(op1) : op1;
                    var value = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    Out(port, value);
                    break;
                }
                case OpCode.Ret:
                {
                    ushortArray[1] = Pop();
                    ushortArray[0] = Pop();
                    var address = BitConverter.ToUInt16(ushortArray);
                    Jump(address);
                    break;
                }
                case OpCode.Jump:
                {
                    var address = (flags & Flag.Op1Register) != 0 ? GetRegister(op1) : op1;
                    Jump(address);
                    break;
                }
                case OpCode.Call:
                {
                    var address = (flags & Flag.Op1Register) != 0 ? GetRegister(op1) : op1;
                    var bytes = BitConverter.GetBytes(_position);
                    Push(bytes[0]);
                    Push(bytes[1]);
                    Jump(address);
                    break;
                }
                case OpCode.Jb:
                {
                    var address = (flags & Flag.Op1Register) != 0 ? GetRegister(op1) : op1;
                    var value1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var value2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;
                    Jb(address, value1, value2);
                    break;
                }
                case OpCode.Ja:
                {
                    var address = (flags & Flag.Op1Register) != 0 ? GetRegister(op1) : op1;
                    var value1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var value2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;
                    Ja(address, value1, value2);
                    break;
                }
                case OpCode.Je:
                {
                    var address = (flags & Flag.Op1Register) != 0 ? GetRegister(op1) : op1;
                    var value1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var value2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;
                    Je(address, value1, value2);
                    break;
                }
                case OpCode.Jne:
                {
                    var address = (flags & Flag.Op1Register) != 0 ? GetRegister(op1) : op1;
                    var value1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var value2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;
                    Jne(address, value1, value2);
                    break;
                }
                case OpCode.Jbe:
                {
                    var address = (flags & Flag.Op1Register) != 0 ? GetRegister(op1) : op1;
                    var value1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var value2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;
                    Jbe(address, value1, value2);
                    break;
                }
                case OpCode.Jae:
                {
                    var address = (flags & Flag.Op1Register) != 0 ? GetRegister(op1) : op1;
                    var value1 = (flags & Flag.Op2Register) != 0 ? GetRegister(op2) : op2;
                    var value2 = (flags & Flag.Op3Register) != 0 ? GetRegister(op3) : op3;
                    Jae(address, value1, value2);
                    break;
                }
            }
        }
    }

    private static void Read(byte register, byte address)
    {
        var value = Computer.MemoryRead(address);
        SetRegister(register, value);
    }

    private static void Write(byte address, byte value)
    {
        Computer.MemoryWrite(address, value);
    }

    private static void Move(byte register, byte value)
    {
        SetRegister(register, value);
    }

    private static void Push(byte value)
    {
        if (Computer.R0 >= Computer.StackSize)
            Utils.AbortExecution($"SP ({Computer.R0}) >= Stack size ({Computer.StackSize})");

        Computer.MemoryWrite(Computer.R0++, value);
    }

    private static void Pop(byte register)
    {
        if (Computer.R0 != 0)
        {
            var value = Computer.MemoryRead(--Computer.R0);
            SetRegister(register, value);
            return;
        }

        Utils.AbortExecution($"SP ({Computer.R0}) == 0");
    }

    private static void In(byte register, byte port)
    {
        var value = Computer.PortRead(port);
        SetRegister(register, value);
    }

    private static void Out(byte port, byte value)
    {
        Computer.PortWrite(port, value);
    }

    private static void Jump(ushort address)
    {
        _position = address;
    }

    private static void Jb(byte address, byte value1, byte value2)
    {
        if (value1 < value2)
            Jump(address);
    }

    private static void Ja(byte address, byte value1, byte value2)
    {
        if (value1 > value2)
            Jump(address);
    }

    private static void Je(byte address, byte value1, byte value2)
    {
        if (value1 == value2)
            Jump(address);
    }

    private static void Jne(byte address, byte value1, byte value2)
    {
        if (value1 != value2)
            Jump(address);
    }

    private static void Jbe(byte address, byte value1, byte value2)
    {
        if (value1 <= value2)
            Jump(address);
    }

    private static void Jae(byte address, byte value1, byte value2)
    {
        if (value1 >= value2)
            Jump(address);
    }

    private static byte Pop()
    {
        if (Computer.R0 == 0)
        {
            Utils.AbortExecution($"SP ({Computer.R0}) == 0");
        }

        return Computer.MemoryRead(--Computer.R0);
    }

    private static byte GetRegister(byte index)
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

    private static void SetRegister(byte index, byte value)
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
}
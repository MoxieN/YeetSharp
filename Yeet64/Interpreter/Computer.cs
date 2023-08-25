using System.Text;
using Yeet.Common;

namespace Yeet64.Interpreter;

public static class Computer
{
    public const ulong StackSize = 4 * 1024; // 4 KiB
    public const ulong MemorySize = 16 * 1024 * 1024; // 16 MiB
    public const ulong MaxCodeSize = MemorySize - StackSize;

    private static readonly byte[] Memory = new byte[MemorySize];
    private static readonly ulong[] Ports = new ulong[2];
    private static readonly Action[] PortEvents = new Action[2];

    public static bool PoweredOn { get; private set; }

    public static ulong
        R0, // Instruction Pointer (IP)
        R1, // Stack Pointer (SP)
        R2, // Flags Register (FR)
        R3,
        R4,
        R5,
        R6,
        R7,
        R8,
        R9,
        R10,
        R11,
        R12,
        R13,
        R14,
        R15;

    public static void Initialize()
    {
        R0 = 0;
        R1 = 0;
        R2 = 0;
        R3 = 0;
        R4 = 0;
        R5 = 0;
        R6 = 0;
        R7 = 0;
        R8 = 0;
        R9 = 0;
        R10 = 0;
        R11 = 0;
        R12 = 0;
        R13 = 0;
        R14 = 0;
        R15 = 0;

        PortEvents[0] = () => { }; // Does nothing (test port)
        PortEvents[1] = () => { PerformSystemCall(Ports[1]); }; // System call port

        PoweredOn = true;
    }

    public static string PrintRegisters()
    {
        var sb = new StringBuilder();

        sb.Append("R0: ");
        sb.Append(R0);
        sb.AppendLine();
        sb.Append("R1: ");
        sb.Append(R1);
        sb.AppendLine();
        sb.Append("R2: ");
        sb.Append(R2);
        sb.AppendLine();
        sb.Append("R3: ");
        sb.Append(R3);
        sb.AppendLine();
        sb.Append("R4: ");
        sb.Append(R4);
        sb.AppendLine();
        sb.Append("R5: ");
        sb.Append(R5);
        sb.AppendLine();
        sb.Append("R6: ");
        sb.Append(R6);
        sb.AppendLine();
        sb.Append("R7: ");
        sb.Append(R7);
        sb.AppendLine();
        sb.Append("R8: ");
        sb.Append(R8);
        sb.AppendLine();
        sb.Append("R9: ");
        sb.Append(R9);
        sb.AppendLine();
        sb.Append("R10: ");
        sb.Append(R10);
        sb.AppendLine();
        sb.Append("R11: ");
        sb.Append(R11);
        sb.AppendLine();
        sb.Append("R12: ");
        sb.Append(R12);
        sb.AppendLine();
        sb.Append("R13: ");
        sb.Append(R13);
        sb.AppendLine();
        sb.Append("R14: ");
        sb.Append(R14);
        sb.AppendLine();
        sb.Append("R15: ");
        sb.Append(R15);
        sb.AppendLine();

        return sb.ToString();
    }

    public static void ClearMemory()
    {
        for (var i = 0; i < Memory.Length; i++) Memory[i] = 0;
    }

    public static void ClearPorts()
    {
        for (var i = 0; i < Ports.Length; i++) Ports[i] = 0;
    }

    public static void Load(byte[] code)
    {
        var codeSize = (ulong)code.Length;

        if (codeSize > MaxCodeSize) Utils.AbortLoad($"Code size ({codeSize}) > Max code size ({MaxCodeSize})");

        for (var i = 0UL; i < codeSize; i++) Memory[StackSize + i] = code[i];
    }

    public static void MemoryWrite64(ulong address, ulong value)
    {
        Memory[address] = (byte)(value & 0xFF);
        Memory[address + 1] = (byte)((value >> 8) & 0xFF);
        Memory[address + 2] = (byte)((value >> 16) & 0xFF);
        Memory[address + 3] = (byte)((value >> 24) & 0xFF);
        Memory[address + 4] = (byte)((value >> 32) & 0xFF);
        Memory[address + 5] = (byte)((value >> 40) & 0xFF);
        Memory[address + 6] = (byte)((value >> 48) & 0xFF);
        Memory[address + 7] = (byte)((value >> 56) & 0xFF);
    }

    public static ulong MemoryRead64(ulong address)
    {
        return Memory[address]
               | ((ulong)Memory[address + 1] << 8)
               | ((ulong)Memory[address + 2] << 16)
               | ((ulong)Memory[address + 3] << 24)
               | ((ulong)Memory[address + 4] << 32)
               | ((ulong)Memory[address + 5] << 40)
               | ((ulong)Memory[address + 6] << 48)
               | ((ulong)Memory[address + 7] << 56);
    }

    public static void PortWrite(ulong port, ulong value)
    {
        try
        {
            Ports[port] = value;
            PortEvents[port]();
        }
        catch (IndexOutOfRangeException)
        {
            Utils.PrintError($"Port {port} out of range exception");
        }
    }

    public static ulong PortRead(ulong port)
    {
        return Ports[port];
    }

    public static void PushStack(ulong value)
    {
        MemoryWrite64(R1, value);
        R1 += sizeof(ulong);
    }

    public static ulong PopStack()
    {
        R1 -= 8;
        return MemoryRead64(R1);
    }

    #region Helpers

    private static void PerformSystemCall(ulong syscall)
    {
        R2 &= ~Flag.UnknownSyscall;

        switch (syscall)
        {
            case 0: // Emulator shut down
            {
                Utils.PrintDebug("Syscall 0 received");
                Utils.PrintInfo("Received shut down signal from program");
                PoweredOn = false;
                break;
            }
            case 1:
            {
                Utils.PrintDebug("Syscall 1 received");
                Console.Write((char) R3);
                break;
            }
            default:
            {
                R2 |= Flag.UnknownSyscall;
                break;
            }
        }
    }

    public static ulong GetRegister(uint index)
    {
        return index switch
        {
            0 => R0,
            1 => R1,
            2 => R2,
            3 => R3,
            4 => R4,
            5 => R5,
            6 => R6,
            7 => R7,
            8 => R8,
            9 => R9,
            10 => R10,
            11 => R11,
            12 => R12,
            13 => R13,
            14 => R14,
            15 => R15,
            _ => throw new InvalidOperationException($"Trying to get invalid register: R{index}")
        };
    }

    public static void SetRegister(uint index, ulong value)
    {
        switch (index)
        {
            case 0:
                R0 = value;
                break;
            case 1:
                R1 = value;
                break;
            case 2:
                R2 = value;
                break;
            case 3:
                R3 = value;
                break;
            case 4:
                R4 = value;
                break;
            case 5:
                R5 = value;
                break;
            case 6:
                R6 = value;
                break;
            case 7:
                R7 = value;
                break;
            case 8:
                R8 = value;
                break;
            case 9:
                R9 = value;
                break;
            case 10:
                R10 = value;
                break;
            case 11:
                R11 = value;
                break;
            case 12:
                R12 = value;
                break;
            case 13:
                R13 = value;
                break;
            case 14:
                R14 = value;
                break;
            case 15:
                R15 = value;
                break;
            default: throw new InvalidOperationException($"Trying to set invalid register: R{index}");
        }
    }

    #endregion

    #region Memory operations for internal use, not exposed by any instructions

    internal static void MemoryWrite8(ulong address, byte value)
    {
        Memory[address] = value;
    }

    internal static byte MemoryRead8(ulong address)
    {
        return Memory[address];
    }

    #endregion
}
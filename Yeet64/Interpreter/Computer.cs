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

        sb.Append("R0: "); sb.Append(R0); sb.AppendLine();
        sb.Append("R1: "); sb.Append(R1); sb.AppendLine();
        sb.Append("R2: "); sb.Append(R2); sb.AppendLine();
        sb.Append("R3: "); sb.Append(R3); sb.AppendLine();
        sb.Append("R4: "); sb.Append(R4); sb.AppendLine();
        sb.Append("R5: "); sb.Append(R5); sb.AppendLine();
        sb.Append("R6: "); sb.Append(R6); sb.AppendLine();
        sb.Append("R7: "); sb.Append(R7); sb.AppendLine();
        sb.Append("R8: "); sb.Append(R8); sb.AppendLine();
        sb.Append("R9: "); sb.Append(R9); sb.AppendLine();
        sb.Append("R10: "); sb.Append(R10); sb.AppendLine();
        sb.Append("R11: "); sb.Append(R11); sb.AppendLine();
        sb.Append("R12: "); sb.Append(R12); sb.AppendLine();
        sb.Append("R13: "); sb.Append(R13); sb.AppendLine();
        sb.Append("R14: "); sb.Append(R14); sb.AppendLine();
        sb.Append("R15: "); sb.Append(R15); sb.AppendLine();

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

        if (codeSize > MaxCodeSize)
        {
            Utils.AbortLoad($"Code size ({codeSize}) > Max code size ({MaxCodeSize})");
        }

        for (var i = 0UL; i < codeSize; i++) Memory[StackSize + i] = code[i];
    }

    public static void MemoryWrite64(ulong address, ulong value)
    {
        var bytes = BitConverter.GetBytes(value);
        var length = (ulong)bytes.Length;

        for (var i = 0UL; i < length; i++) Memory[address + i] = bytes[i];
    }

    public static ulong MemoryRead64(ulong address) => BitConverter.ToUInt64(new[]
    {
        Memory[address],
        Memory[address + 1],
        Memory[address + 2],
        Memory[address + 3],
        Memory[address + 4],
        Memory[address + 5],
        Memory[address + 6],
        Memory[address + 7]
    });

    public static void PortWrite(byte port, ulong value)
    {
        Ports[port] = value;
        PortEvents[port].Invoke();
    }

    public static ulong PortRead(byte port) => Ports[port];

    #region Helpers

    private static void PerformSystemCall(ulong syscall)
    {
        R2 &= ~Flag.UnknownSyscall;

        switch (syscall)
        {
            case 0: // Emulator shut down
            {
                PoweredOn = false;
                break;
            }
            default:
            {
                R2 |= Flag.UnknownSyscall;
                break;
            }
        }
    }

    #endregion

    #region Memory operations for internal use, not exposed by any instructions

    internal static void MemoryWrite8(ulong address, byte value) => Memory[address] = value;

    internal static byte MemoryRead8(ulong address) => Memory[address];

    #endregion
}
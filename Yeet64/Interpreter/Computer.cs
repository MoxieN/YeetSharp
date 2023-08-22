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

    public static bool PoweredOn;

    public static ulong
        R0,
        R1,
        R2,
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

    /// <summary>
    /// Initialize virtual CPU
    /// </summary>
    public static void Initialize()
    {
        // Registers initialization
        R0 = 0; // IP
        R1 = 0; // SP
        R2 = 0; // FR
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

        // Port events initialization
        PortEvents[0] = () => { }; // Does nothing (test port)
        PortEvents[1] = () => { }; // TODO: System call port

        PoweredOn = true;
    }

    /// <summary>
    /// Print current registers.
    /// </summary>
    public static void PrintRegisters()
    {
        Console.WriteLine("Registers:");
        Console.WriteLine($"- R0: {R0}");
        Console.WriteLine($"- R1: {R1}");
        Console.WriteLine($"- R2: {R2}");
        Console.WriteLine($"- R3: {R3}");
        Console.WriteLine($"- R4: {R4}");
        Console.WriteLine($"- R5: {R5}");
        Console.WriteLine($"- R6: {R6}");
        Console.WriteLine($"- R7: {R7}");
        Console.WriteLine($"- R8: {R8}");
        Console.WriteLine($"- R9: {R9}");
        Console.WriteLine($"- R10: {R10}");
        Console.WriteLine($"- R11: {R11}");
        Console.WriteLine($"- R12: {R12}");
        Console.WriteLine($"- R13: {R13}");
        Console.WriteLine($"- R14: {R14}");
        Console.WriteLine($"- R15: {R15}");
    }

    /// <summary>
    /// Clear memory.
    /// </summary>
    public static void ClearMemory()
    {
        for (var i = 0; i < Memory.Length; i++) Memory[i] = 0;
    }

    /// <summary>
    /// Clear ports.
    /// </summary>
    public static void ClearPorts()
    {
        for (var i = 0; i < Ports.Length; i++) Ports[i] = 0;
    }

    /// <summary>
    /// Load instructions to the virtual CPU.
    /// </summary>
    /// <param name="code">Instructions to load</param>
    public static void Load(byte[] code)
    {
        var codeSize = (ulong)code.Length;

        if (codeSize > MaxCodeSize)
        {
            Utils.AbortLoad($"Code size ({codeSize}) > Max code size ({MaxCodeSize})");
        }

        for (var i = 0UL; i < codeSize; i++) Memory[StackSize + i] = code[i];
    }

    /// <summary>
    /// Write value to memory address.
    /// </summary>
    /// <param name="address">Memory address to use.</param>
    /// <param name="value">Value to write to the address.</param>
    public static void MemoryWrite(ulong address, ulong value)
    {
        var bytes = BitConverter.GetBytes(value);
        var length = (ulong)bytes.Length;

        for (var i = 0UL; i < length; i++) Memory[address + i] = bytes[i];
    }

    /// <summary>
    /// Read value from memory address
    /// </summary>
    /// <param name="address">Memory address to lookup.</param>
    /// <returns>Value of address.</returns>
    public static ulong MemoryRead(ulong address)
    {
        return BitConverter.ToUInt64(new[]
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
    }

    /// <summary>
    /// Write to I/O port.
    /// </summary>
    /// <param name="port">Port to use.</param>
    /// <param name="value">Value to send to I/O port.</param>
    public static void PortWrite(byte port, ulong value)
    {
        Ports[port] = value;
        PortEvents[port].Invoke();
    }

    /// <summary>
    /// Read from I/O port.
    /// </summary>
    /// <param name="port">I/O port to lookup.</param>
    /// <returns>Value of I/O port</returns>
    public static ulong PortRead(byte port)
    {
        return Ports[port];
    }
}
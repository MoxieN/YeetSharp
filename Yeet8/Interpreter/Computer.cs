namespace Yeet8.Interpreter;

public static class Computer
{
    public const byte StackSize = byte.MaxValue;

    public const short UserMemory = 32513;
    public const short MaxCodeSize = short.MaxValue;

    public const ushort MemorySize = ushort.MaxValue;

    private static readonly byte[] Memory = new byte[MemorySize + 1]; // ~64 KB of RAM
    private static readonly byte[] Ports = new byte[4];
    private static readonly Action[] PortEvents = new Action[4];

    public static bool PoweredOn;

    public static byte
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
        R0 = 0; // SP
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

        // Port events initialization
        PortEvents[0] = () => Ports[0] = 0xDE; // Reserved port for testing (like a simulated POST)
        PortEvents[1] = () => { }; // Does nothing (test port)
        PortEvents[2] = () => PoweredOn = false; // Tells the processor to shut down
        PortEvents[3] = () => Console.Write((char)Ports[3]); // Prints an ASCII character to the screen

        // Self test
        PortEvents[0].Invoke();

        var value = Ports[0];

        if (value != 0xDE)
        {
            Console.WriteLine(
                $"INITIALIZATION HALTED: Power-On Self Test failed! Expected 0xDE in port 0, found 0x{value:X4}");
            for (;;)
            {
            }
        }

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
        for (var i = 0; i < Memory.Length; i++)
            Memory[i] = 0;
    }

    /// <summary>
    /// Clear ports.
    /// </summary>
    public static void ClearPorts()
    {
        for (var i = 0; i < Ports.Length; i++)
            Ports[i] = 0;
    }

    /// <summary>
    /// Load instructions to the virtual CPU.
    /// </summary>
    /// <param name="code">Instructions to load</param>
    public static void Load(byte[] code)
    {
        if (code.Length > MaxCodeSize)
        {
            Console.WriteLine($"LOAD HALTED: Code size ({code.Length}) > Max code size ({MaxCodeSize})");
            for (;;)
            {
            }
        }

        for (var i = 0; i < code.Length; i++)
            Memory[UserMemory + StackSize + i] = code[i];
    }

    /// <summary>
    /// Write value to memory address.
    /// </summary>
    /// <param name="address">Memory address to use.</param>
    /// <param name="value">Value to write to the address.</param>
    public static void MemoryWrite(ushort address, byte value)
    {
        Memory[address] = value;
    }

    /// <summary>
    /// Read value from memory address
    /// </summary>
    /// <param name="address">Memory address to lookup.</param>
    /// <returns>Value of address.</returns>
    public static byte MemoryRead(ushort address)
    {
        return Memory[address];
    }

    /// <summary>
    /// Write to I/O port.
    /// </summary>
    /// <param name="port">Port to use.</param>
    /// <param name="value">Value to send to I/O port.</param>
    public static void PortWrite(byte port, byte value)
    {
        Ports[port] = value;
        PortEvents[port].Invoke();
    }

    /// <summary>
    /// Read from I/O port.
    /// </summary>
    /// <param name="port">I/O port to lookup.</param>
    /// <returns>Value of I/O port</returns>
    public static byte PortRead(byte port)
    {
        return Ports[port];
    }
}
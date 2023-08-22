using Yeet.Common;
using Console = System.Console;

namespace YeetSharp;

public static class Program
{
    // Arguments
    private static string[] _arguments = Array.Empty<string>();

    // Argument variables
    private static int _arch = 64;
    private static bool _showRegisters;
    private static bool _showHelp;

    /// <summary>
    /// Program's entry point
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        // Set argument variables
        _showRegisters = args is ["-regs", ..];
        _arguments = args;

        HandleArgs();
        if (!_showHelp) StartCPU();
    }

    #region Func

    /// <summary>
    /// Handle every known arguments if present.
    /// </summary>
    private static void HandleArgs()
    {
        if (_arguments.Contains("-h")) ShowHelp();
        SetArchitecture();
    }

    /// <summary>
    /// 
    /// </summary>
    private static void ShowHelp()
    {
        _showHelp = true;

        Console.WriteLine("This program will emulate CPUs for the Yeet8 and Yeet64 architectures.");
        Console.WriteLine("YeetSharp was made with love by Sartox Software and MoxieN!");

        Console.WriteLine("Arguments:");
        Console.WriteLine("-> -h     | Prints this page");
        Console.WriteLine("-> -regs  | Prints the registers");
        Console.WriteLine("-> -aARCH* | Loads the correct CPU architecture, uses yeet64 by default (ex: -a8)");
        Console.WriteLine("'*' = (optional argument)");
    }

    /// <summary>
    /// Calls the correct CPU depending on arguments
    /// </summary>
    /// <exception cref="NotImplementedException">This architecture isn't implemented, check README.md for further details.</exception>
    private static void StartCPU()
    {
        switch (_arch)
        {
            case 64:
                Yeet64CPU();
                break;
            case 8:
                Yeet8CPU();
                break;
            default:
                Utils.PrintError(
                    "This architecture isn't implemented, please refer to the help section using the argument '-h'.");
                break;
        }
    }

    private static void SetArchitecture()
    {
        var archPresent = false;

        foreach (var arg in _arguments)
        {
            if (!arg.StartsWith("-a")) continue;
            if (archPresent)
                Utils.PrintError(
                    "You can't use more than one architecture, please refer to the help section using the argument '-h'.");
            archPresent = true;
            try
            {
                _arch = Convert.ToInt32(arg[2..]);
            }
            catch (FormatException)
            {
                Utils.PrintError(
                    "This architecture doesn't exist, please refer to the help section using the argument '-h'.");
            }
        }
    }

    #endregion

    #region CPUInstructions

    private static void Yeet64CPU()
    {
        Utils.PrintInfo("Starting Yeet64 CPU emulation");

        var lexer = new Yeet64.Assembler.Lexer("yeet64test.asm", false, false, false);
        var tokens = lexer.Run();

        var parser = new Yeet64.Assembler.Parser(ref tokens);
        var code = parser.Run();

        Yeet64.Interpreter.Computer.Initialize();
        Yeet64.Interpreter.Computer.ClearMemory();
        Yeet64.Interpreter.Computer.ClearPorts();
        Yeet64.Interpreter.Computer.Load(code.ToArray());
        Yeet64.Interpreter.Executor.Execute();

        Console.WriteLine();
        if (_showRegisters) Yeet8.Interpreter.Computer.PrintRegisters();

        Utils.PrintInfo("CPU Emulation ended");
    }

    private static void Yeet8CPU()
    {
        Utils.PrintInfo("Starting Yeet8 CPU emulation");

        var asmInstructions = Yeet8.Assembler.Lexer.LexInstructions("yeet8test.asm");
        foreach (var i in asmInstructions)
            Console.WriteLine(i.ToString());

        Yeet8.Interpreter.Computer.Initialize();
        Yeet8.Interpreter.Computer.ClearMemory();
        Yeet8.Interpreter.Computer.ClearPorts();

        var code = Yeet8.Assembler.Parse.ParseInstructions(asmInstructions);

        Yeet8.Interpreter.Computer.Load(code);
        Yeet8.Interpreter.Executor.Execute();

        Console.WriteLine();
        if (_showRegisters) Yeet8.Interpreter.Computer.PrintRegisters();

        Utils.PrintInfo("CPU Emulation ended");
    }

    #endregion
}
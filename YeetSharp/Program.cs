﻿using Microsoft.VisualBasic;
using Yeet.Common;
using Yeet.Common.Assembler;

namespace YeetSharp;

public static class Program
{
    // Arguments
    private static bool _showHelp;
    private static bool _debug;
    private static byte _arch = 64;
    private static string? _file;

    /// <summary>
    /// Program entry point
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main(string?[] args)
    {
        #if DEBUG
        _debug = true;
        Utils.PrintDebug("YeetSharp started in DEBUG mode");
        #endif
        
        HandleArgs(ref args);
        if (!_showHelp) StartCPU();
    }

    #region Arguments Handling

    /// <summary>
    /// Handle every known argument, if any.
    /// </summary>
    private static void HandleArgs(ref string?[] args)
    {
        if (args.Contains("-h")) ShowHelp();
        SetArguments(ref args);
    }

    /// <summary>
    /// Show all commands and their usage.
    /// </summary>
    private static void ShowHelp()
    {
        _showHelp = true;

        Console.WriteLine("This program will emulate CPUs for the Yeet-16 and Yeet-64 architectures.");
        Console.WriteLine("YeetSharp was made with love by Sartox Software and MoxieN!");

        Console.WriteLine("Arguments:");
        Console.WriteLine("-> -h      | Prints this page");
        Console.WriteLine("-> -regs   | Prints the registers");
        Console.WriteLine("-> -aARCH* | Loads the correct CPU architecture, uses Yeet-64 by default (ex: -a16)");
        Console.WriteLine("'*' = (optional argument)");
    }

    private static void GetFile(string file)
    {
        
        
    }

    /// <summary>
    /// Calls the correct CPU emulation method depending on arguments
    /// </summary>
    /// <exception cref="NotImplementedException">This architecture isn't implemented, check README.md for further details.</exception>
    private static void StartCPU()
    {
        switch (_arch)
        {
            case 64:
            {
                Yeet64CPU();
                break;
            }
            case 16:
            {
                Yeet16CPU();
                break;
            }
            default:
            {
                Utils.PrintError(
                    "This architecture isn't implemented, please refer to the help section using the argument '-h'.");
                break;
            }
        }
    }

    private static void SetArguments(ref string?[] args)
    {
        var archPresent = false;

        for (var index = 0; index < args.Length; index++)
        {
            var arg = args[index];
            switch (arg)
            {
                case "-regs":
                    _debug = true;
                    continue;
                case "-file":
                {
                    if (!File.Exists(args[index+1])) Utils.AbortLoad("This file doesn't exist.");
                    _file = args[index + 1];
                    break;
                }
            }

            if (!arg.StartsWith("-a")) continue;
            if (archPresent)
                Utils.PrintError(
                    "You can't use more than one architecture, please refer to the help section using the argument '-h'.");

            archPresent = true;

            try
            {
                _arch = Convert.ToByte(arg[2..]);
            }
            catch (FormatException)
            {
                Utils.PrintError(
                    "This architecture doesn't exist, please refer to the help section using the argument '-h'.");
            }
        }

        if (string.IsNullOrEmpty(_file)) Utils.AbortLoad("Please specify a file.");
    }

    #endregion

    #region CPU Architectures

    private static void Yeet64CPU()
    {
        Utils.PrintInfo("Starting Yeet-64 CPU emulation");

        var lexer = new Lexer(_file, false, false, false);
        var tokens = lexer.Run();

        var parser = new Parser(ref tokens, new Yeet64.Instruction());
        var code = parser.Run();

        Yeet64.Interpreter.Computer.Initialize();
        Yeet64.Interpreter.Computer.ClearMemory();
        Yeet64.Interpreter.Computer.ClearPorts();
        Yeet64.Interpreter.Computer.Load(code.ToArray());
        Yeet64.Interpreter.Executor.Execute();

        Utils.PrintInfo("CPU Emulation ended");

        if (!_debug) return;
        Utils.PrintDebug("Dumping CPU registers...");
        Console.WriteLine(Yeet64.Interpreter.Computer.PrintRegisters());
    }

    private static void Yeet16CPU()
    {
        throw new NotImplementedException();

        /*Utils.PrintInfo("Starting Yeet-16 CPU emulation");

        var lexer = new Lexer(_file, false, false, false);
        var tokens = lexer.Run();

        var parser = new Parser(ref tokens, new Yeet16.Instruction());
        var code = parser.Run();

        Yeet16.Interpreter.Computer.Initialize();
        Yeet16.Interpreter.Computer.ClearMemory();
        Yeet16.Interpreter.Computer.ClearPorts();
        Yeet16.Interpreter.Computer.Load(code.ToArray());
        Yeet16.Interpreter.Executor.Execute();

        Utils.PrintInfo("CPU Emulation ended");

        if (_showRegisters)
        {
            Utils.PrintDebug("Dumping CPU registers...");
            Yeet16.Interpreter.Computer.PrintRegisters();
        }*/
    }

    #endregion
}
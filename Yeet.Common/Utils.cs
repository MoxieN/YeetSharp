using System;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using Colorful;
using Console = Colorful.Console;

namespace Yeet.Common;

public static class Utils
{
    [DoesNotReturn]
    public static void AbortExecution(string message)
    {
        Console.Write("EXECUTION HALTED:", Color.Red);
        Console.WriteLine(message);
        Environment.Exit(1);
    }

    [DoesNotReturn]
    public static void AbortLoad(string message)
    {
        Console.Write("LOAD HALTED", Color.Red);
        Console.WriteLine($": {message}");
        Environment.Exit(1);
    }

    public static void PrintError(string str)
    {
        const string prefixError = "{0}ERR{1} ";

        var error = new Formatter[]
        {
            new("[", Color.IndianRed),
            new("]", Color.IndianRed)
        };

        Console.WriteFormatted(prefixError, Color.White, error);
        Console.WriteLine(str);
    }

    public static void PrintInfo(string str)
    {
        const string prefixInfo = "{0}INFO{1} ";

        var info = new Formatter[]
        {
            new("[", Color.RoyalBlue),
            new("]", Color.RoyalBlue)
        };

        Console.WriteFormatted(prefixInfo, Color.White, info);
        Console.WriteLine(str);
    }
}
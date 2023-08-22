using System.Diagnostics.CodeAnalysis;

namespace Yeet.Common;

public static class Utils
{
    [DoesNotReturn]
    public static void AbortExecution(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("EXECUTION HALTED");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($": {message}");
        Environment.Exit(1);
    }
    
    [DoesNotReturn]
    public static void AbortLoad(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("LOAD HALTED");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($": {message}");
        Environment.Exit(1);
    }
}
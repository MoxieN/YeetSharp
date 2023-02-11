namespace Yeet.Common;

public static class Utils
{
    public static void Abort(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("EXECUTION HALTED");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($": {message}");
        Environment.Exit(1);
    }
}
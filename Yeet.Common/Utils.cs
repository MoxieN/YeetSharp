namespace Yeet.Common;

public static class Utils
{
    public static void Abort(string message)
    {
        Console.WriteLine($"EXECUTION HALTED: {message}");
        Environment.Exit(1);
    }
}
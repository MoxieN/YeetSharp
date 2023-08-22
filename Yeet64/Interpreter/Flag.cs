namespace Yeet64.Interpreter;

public static class Flag
{
    public const ulong Below = 1 << 0;
    public const ulong Above = 1 << 1;
    public const ulong Equal = 1 << 2;
    public const ulong NotEqual = 1 << 3;
    public const ulong BelowOrEqual = 1 << 4;
    public const ulong AboveOrEqual = 1 << 5;
    public const ulong UnknownSyscall = 1 << 6;
}
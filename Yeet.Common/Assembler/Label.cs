namespace Yeet.Common.Assembler;

/// <summary>
/// Label record for the assembler
/// </summary>
/// <param name="Name">Name of the label</param>
/// <param name="Address">Position in memory where the label is defined</param>
public sealed record Label(string Name, ulong Address);
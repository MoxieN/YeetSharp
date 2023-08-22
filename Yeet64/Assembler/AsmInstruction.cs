namespace Yeet64.Assembler;

public record AsmInstruction(string Name, byte Flags, uint Op1, uint Op2);
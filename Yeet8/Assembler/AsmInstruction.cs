namespace Yeet8.Assembler;

public record AsmInstruction(string Name, byte Flags, byte Op1, byte Op2, byte Op3);
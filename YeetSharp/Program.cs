using YeetSharp;

var write1 = Instruction.Create(OpCode.Write, Flag.Op1Register, Register.R2, 5);
var out2 = Instruction.Create(OpCode.Out, Flag.Op2Register, 0x02, Register.R2);
var in3 = Instruction.Create(OpCode.In, Flag.Op1Register, Register.R3, 0x02);
var push4 = Instruction.Create(OpCode.Push, Flag.Op1Register, Register.R3);

Console.WriteLine($"[write r2, 5] = 0x{write1:X4}");
Console.WriteLine($"[out 0x02, r2] = 0x{out2:X4}");
Console.WriteLine($"[in r3, 0x02] = 0x{in3:X4}");
Console.WriteLine($"[push r3] = 0x{push4:X4}");
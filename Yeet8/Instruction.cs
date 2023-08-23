using System.Text;

namespace Yeet8;

public static class Instruction
{
    public static uint Create(byte opcode, byte flags = 0, byte op1 = 0, byte op2 = 0, byte op3 = 0)
    {
        var builder = new StringBuilder();

        builder.Append(Convert.ToString(opcode, 2).PadLeft(5, '0'));
        builder.Append(Convert.ToString(flags, 2).PadLeft(3, '0'));
        builder.Append(Convert.ToString(op1, 2).PadLeft(8, '0'));
        builder.Append(Convert.ToString(op2, 2).PadLeft(8, '0'));
        builder.Append(Convert.ToString(op3, 2).PadLeft(8, '0'));

        return Convert.ToUInt32(builder.ToString(), 2);
    }
}
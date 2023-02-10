using System.Text;

namespace YeetSharp;

public static class Instruction
{
    public static uint Create(byte opCode, byte flags, byte op1 = 0, byte op2 = 0, byte op3 = 0)
    {
        var builder = new StringBuilder();

        builder.Append(Convert.ToString(opCode, 2).PadLeft(5, '0'));
        builder.Append(Convert.ToString(flags, 2).PadLeft(3, '0'));
        builder.Append(Convert.ToString(op1, 2).PadLeft(8, '0'));
        builder.Append(Convert.ToString(op2, 2).PadLeft(8, '0'));
        builder.Append(Convert.ToString(op3, 2).PadLeft(8, '0'));

        return Convert.ToUInt32(builder.ToString(), 2);
    }
}
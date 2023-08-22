using System.Text;

namespace Yeet64;

public static class OpCode
{
    public static uint CreateType1(byte instruction, bool isRegister, uint destination, uint source)
    {
        var builder = new StringBuilder();

        builder.Append(Convert.ToString(instruction, 2).PadLeft(5, '0'));
        builder.Append(isRegister ? '1' : '0');
        builder.Append(Convert.ToString(destination, 2).PadLeft(4, '0'));
        builder.Append(Convert.ToString(source, 2).PadLeft(22, '0'));

        return Convert.ToUInt32(builder.ToString(), 2);
    }

    public static uint CreateType2(byte instruction, bool isRegister, uint source)
    {
        var builder = new StringBuilder();

        builder.Append(Convert.ToString(instruction, 2).PadLeft(5, '0'));
        builder.Append(isRegister ? '1' : '0');
        builder.Append(Convert.ToString(source, 2).PadLeft(26, '0'));

        return Convert.ToUInt32(builder.ToString(), 2);
    }

    public static uint CreateType3(byte instruction, byte source)
    {
        var builder = new StringBuilder();

        builder.Append(Convert.ToString(instruction, 2).PadLeft(5, '0'));
        builder.Append(Convert.ToString(source, 2).PadLeft(4, '0'));
        builder.Append("00000000000000000000000");

        return Convert.ToUInt32(builder.ToString(), 2);
    }

    public static uint CreateType4(byte instruction)
    {
        var builder = new StringBuilder();

        builder.Append(Convert.ToString(instruction, 2).PadLeft(5, '0'));
        builder.Append("000000000000000000000000000");

        return Convert.ToUInt32(builder.ToString(), 2);
    }
}
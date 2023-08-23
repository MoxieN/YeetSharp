namespace Yeet.Common;

public abstract class Instruction
{
    public abstract uint CreateType1(byte instruction, bool isRegister, uint destination, uint source);
    public abstract uint CreateType2(byte instruction, bool isRegister, uint source);
    public abstract uint CreateType3(byte instruction, byte source);
    public abstract uint CreateType4(byte instruction);
}
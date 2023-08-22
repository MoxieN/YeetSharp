namespace Yeet.Common;

public static class OpCode
{
    public const byte
        Add = 0x00,
        Sub = 0x01,
        Mul = 0x02,
        Div = 0x03,
        Mod = 0x04,
        And = 0x05,
        Or = 0x06,
        Xor = 0x07,
        Not = 0x08,
        Shl = 0x09,
        Shr = 0x0A,
        Sal = 0x0B,
        Sar = 0x0C,
        Read = 0x0D,
        Write = 0x0E,
        Move = 0x0F,
        Push = 0x10,
        Pop = 0x11,
        In = 0x12,
        Out = 0x13,
        Ret = 0x14,
        Jump = 0x15,
        Call = 0x16,
        Jb = 0x17,
        Ja = 0x18,
        Je = 0x19,
        Jne = 0x20,
        Jbe = 0x21,
        Jae = 0x22;
}
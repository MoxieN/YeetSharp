namespace Yeet64;

public static class OpCode
{
    public const byte
        Add = 0b00000,
        Sub = 0b00001,
        Mul = 0b00010,
        Div = 0b00011,
        Mod = 0b00100,
        And = 0b00101,
        Or = 0b00110,
        Xor = 0b00111,
        Not = 0b01000,
        Shl = 0b01001,
        Shr = 0b01010,
        Sal = 0b01011,
        Sar = 0b01100,
        Read = 0b01101,
        Write = 0b01110,
        Move = 0b01111,
        Push = 0b10000,
        Pop = 0b10001,
        In = 0b10010,
        Out = 0b10011,
        Ret = 0b10100,
        Jump = 0b10101,
        Call = 0b10110,
        Cmp = 0b10111,
        Jb = 0b11000,
        Ja = 0b11001,
        Je = 0b11010,
        Jne = 0b11011,
        Jbe = 0b11100,
        Jae = 0b11101;
}
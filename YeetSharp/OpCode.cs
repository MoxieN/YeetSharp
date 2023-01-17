namespace YeetSharp;

public static class OpCode
{
    public static byte Add = 0x00;
    public static byte Sub = 0x01;
    public static byte Mul = 0x02;
    public static byte Div = 0x03;
    public static byte Mod = 0x04;
    public static byte And = 0x05;
    public static byte Or = 0x06;
    public static byte Xor = 0x07;
    public static byte Not = 0x08;
    public static byte Shl = 0x09;
    public static byte Shr = 0x0A;
    public static byte Read = 0x0B;
    public static byte Write = 0x0C;
    public static byte Push = 0x0D;
    public static byte Pop = 0x0E;
    public static byte In = 0x0F;
    public static byte Out = 0x10;
    public static byte Ret = 0x11;
    public static byte Jump = 0x12;
    public static byte Call = 0x13;
    public static byte Jb = 0x14;
    public static byte Ja = 0x15;
    public static byte Je = 0x16;
    public static byte Jne = 0x17;
    public static byte Jbe = 0x18;
    public static byte Jae = 0x19;
}
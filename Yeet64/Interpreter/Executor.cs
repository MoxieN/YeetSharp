namespace Yeet64.Interpreter;

public static class Executor
{
    public static void Execute()
    {
        if (!Computer.PoweredOn) return;
    }

    private static ulong GetRegister(byte index)
    {
        return index switch
        {
            0 => Computer.R0,
            1 => Computer.R1,
            2 => Computer.R2,
            3 => Computer.R3,
            4 => Computer.R4,
            5 => Computer.R5,
            6 => Computer.R6,
            7 => Computer.R7,
            8 => Computer.R8,
            9 => Computer.R9,
            10 => Computer.R10,
            11 => Computer.R11,
            12 => Computer.R12,
            13 => Computer.R13,
            14 => Computer.R14,
            15 => Computer.R15,
            _ => throw new InvalidOperationException($"Trying to get invalid register: R{index}")
        };
    }

    private static void SetRegister(byte index, ulong value)
    {
        switch (index)
        {
            case 0: Computer.R0 = value; break;
            case 1: Computer.R1 = value; break;
            case 2: Computer.R2 = value; break;
            case 3: Computer.R3 = value; break;
            case 4: Computer.R4 = value; break;
            case 5: Computer.R5 = value; break;
            case 6: Computer.R6 = value; break;
            case 7: Computer.R7 = value; break;
            case 8: Computer.R8 = value; break;
            case 9: Computer.R9 = value; break;
            case 10: Computer.R10 = value; break;
            case 11: Computer.R11 = value; break;
            case 12: Computer.R12 = value; break;
            case 13: Computer.R13 = value; break;
            case 14: Computer.R14 = value; break;
            case 15: Computer.R15 = value; break;
            default: throw new InvalidOperationException($"Trying to set invalid register: R{index}");
        }
    }
    
}
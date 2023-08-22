var showRegisters = args is ["-regs", ..];

const bool yeet64 = true;
const bool yeet8 = false;

if (yeet64)
{
    var lexer = new Yeet64.Assembler.Lexer("yeet64test.asm", false, false, false);
    var tokens = lexer.Run();

    var parser = new Yeet64.Assembler.Parser(ref tokens);
    var code = parser.Run();

    Yeet64.Interpreter.Computer.Initialize();
    Yeet64.Interpreter.Computer.ClearMemory();
    Yeet64.Interpreter.Computer.ClearPorts();
    Yeet64.Interpreter.Computer.Load(code.ToArray());
    Yeet64.Interpreter.Executor.Execute();

    Console.WriteLine();
    if (showRegisters) Yeet8.Interpreter.Computer.PrintRegisters();
}

if (yeet8)
{
    var asmInstructions = Yeet8.Assembler.Lexer.LexInstructions("yeet8test.asm");
    foreach (var i in asmInstructions)
        Console.WriteLine(i.ToString());

    Yeet8.Interpreter.Computer.Initialize();
    Yeet8.Interpreter.Computer.ClearMemory();
    Yeet8.Interpreter.Computer.ClearPorts();

    var code = Yeet8.Assembler.Parse.ParseInstructions(asmInstructions);

    Yeet8.Interpreter.Computer.Load(code);
    Yeet8.Interpreter.Executor.Execute();

    Console.WriteLine();
    if (showRegisters) Yeet8.Interpreter.Computer.PrintRegisters();
}
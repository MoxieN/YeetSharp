var showRegisters = args is ["-regs", ..];

#region Yeet64

var tokens = Yeet64.Assembler.Lexer.LexInstructions("yeet64test.asm").Where(x => x.Type
    is not Yeet64.Assembler.Lexer.TokenType.Comma
    and not Yeet64.Assembler.Lexer.TokenType.Comment
    and not Yeet64.Assembler.Lexer.TokenType.Whitespace
).ToArray();
var code = Yeet64.Assembler.Parser.ParseInstructions(tokens);

#endregion

/*#region Yeet8

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

#endregion*/

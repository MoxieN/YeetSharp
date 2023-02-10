using Yeet8.Assembler;
using Yeet8.Interpreter;

var asmInstructions = Lexer.LexInstructions("file.asm");
foreach (var i in asmInstructions)
    Console.WriteLine(i.ToString());

Computer.Initialize();
Computer.ClearMemory();
Computer.ClearPorts();

var code = Parse.ParseInstructions(asmInstructions);

Computer.Load(code);
Executor.Execute();

Console.WriteLine();
Computer.PrintRegisters();
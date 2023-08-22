using System.Data;
using System.Diagnostics;

namespace Yeet64.Assembler;

public class Parser
{
    private readonly List<Lexer.Token> _tokens;

    private int _index;

    public Parser(ref List<Lexer.Token> tokens)
    {
        _tokens = tokens;
        _index = 0;
    }

    public List<byte> Run()
    {
        var code = new List<byte>();

        while (_index < _tokens.Count)
        {
            var instruction = ExpectToken(Lexer.TokenType.Instruction);

            byte instructionType;
            byte instructionNumber;

            switch (instruction.Text)
            {
                // Type 1 instructions
                case "add":
                {
                    instructionNumber = 0b00000;
                    instructionType = 1;
                    break;
                }
                case "sub":
                {
                    instructionNumber = 0b00001;
                    instructionType = 1;
                    break;
                }
                case "mul":
                {
                    instructionNumber = 0b00010;
                    instructionType = 1;
                    break;
                }
                case "div":
                {
                    instructionNumber = 0b00011;
                    instructionType = 1;
                    break;
                }
                case "mod":
                {
                    instructionNumber = 0b00100;
                    instructionType = 1;
                    break;
                }
                case "and":
                {
                    instructionNumber = 0b00101;
                    instructionType = 1;
                    break;
                }
                case "or":
                {
                    instructionNumber = 0b00110;
                    instructionType = 1;
                    break;
                }
                case "xor":
                {
                    instructionNumber = 0b00111;
                    instructionType = 1;
                    break;
                }
                case "not":
                {
                    instructionNumber = 0b01000;
                    instructionType = 1;
                    break;
                }
                case "shl":
                {
                    instructionNumber = 0b01001;
                    instructionType = 1;
                    break;
                }
                case "shr":
                {
                    instructionNumber = 0b01010;
                    instructionType = 1;
                    break;
                }
                case "sal":
                {
                    instructionNumber = 0b01011;
                    instructionType = 1;
                    break;
                }
                case "sar":
                {
                    instructionNumber = 0b01100;
                    instructionType = 1;
                    break;
                }
                case "read":
                {
                    instructionNumber = 0b01101;
                    instructionType = 1;
                    break;
                }
                case "write":
                {
                    instructionNumber = 0b01110;
                    instructionType = 1;
                    break;
                }
                case "move":
                {
                    instructionNumber = 0b01111;
                    instructionType = 1;
                    break;
                }
                case "in":
                {
                    instructionNumber = 0b10010;
                    instructionType = 1;
                    break;
                }
                case "out":
                {
                    instructionNumber = 0b10011;
                    instructionType = 1;
                    break;
                }
                case "cmp":
                {
                    instructionNumber = 0b10111;
                    instructionType = 1;
                    break;
                }

                // Type 2 instructions
                case "push":
                {
                    instructionNumber = 0b10000;
                    instructionType = 2;
                    break;
                }
                case "jump":
                {
                    instructionNumber = 0b10101;
                    instructionType = 2;
                    break;
                }
                case "call":
                {
                    instructionNumber = 0b10110;
                    instructionType = 2;
                    break;
                }
                case "jb":
                {
                    instructionNumber = 0b11000;
                    instructionType = 2;
                    break;
                }
                case "ja":
                {
                    instructionNumber = 0b11001;
                    instructionType = 2;
                    break;
                }
                case "je":
                {
                    instructionNumber = 0b11010;
                    instructionType = 2;
                    break;
                }
                case "jne":
                {
                    instructionNumber = 0b11011;
                    instructionType = 2;
                    break;
                }
                case "jbe":
                {
                    instructionNumber = 0b11100;
                    instructionType = 2;
                    break;
                }
                case "jae":
                {
                    instructionNumber = 0b11101;
                    instructionType = 2;
                    break;
                }

                // Type 3 instructions
                case "pop":
                {
                    instructionNumber = 0b10001;
                    instructionType = 3;
                    break;
                }

                // Type 4 instructions
                case "ret":
                {
                    instructionNumber = 0b10100;
                    instructionType = 4;
                    break;
                }

                default: throw new SyntaxErrorException($"PARSER HALTED: Unknown instruction \"{instruction.Text}\"");
            }

            switch (instructionType)
            {
                case 1:
                {
                    var operand1 = ExpectToken(Lexer.TokenType.Register);
                    var operand2 = ExpectToken(Lexer.TokenType.Register, Lexer.TokenType.Number);
                    var opcode = OpCode.CreateType1(
                        instructionNumber,
                        operand2.Type is Lexer.TokenType.Register,
                        uint.Parse(operand1.Text),
                        uint.Parse(operand2.Text)
                    );

                    code.AddRange(BitConverter.GetBytes(opcode));
                    break;
                }
                case 2:
                {
                    var operand1 = ExpectToken(Lexer.TokenType.Register, Lexer.TokenType.Number);
                    var opcode = OpCode.CreateType2(
                        instructionNumber,
                        operand1.Type is Lexer.TokenType.Register,
                        uint.Parse(operand1.Text)
                    );

                    code.AddRange(BitConverter.GetBytes(opcode));
                    break;
                }
                case 3:
                {
                    var operand1 = ExpectToken(Lexer.TokenType.Register);
                    var opcode = OpCode.CreateType3(
                        instructionNumber,
                        byte.Parse(operand1.Text)
                    );

                    code.AddRange(BitConverter.GetBytes(opcode));
                    break;
                }
                case 4:
                {
                    var opcode = OpCode.CreateType4(
                        instructionNumber
                    );

                    code.AddRange(BitConverter.GetBytes(opcode));
                    break;
                }
                default: throw new UnreachableException();
            }
        }

        return code;
    }

    private Lexer.Token ExpectToken(Lexer.TokenType type)
    {
        if (_index >= _tokens.Count) throw new ArgumentOutOfRangeException(nameof(_index));

        var token = _tokens[_index++];
        if (token.Type != type)
        {
            throw new SyntaxErrorException($"PARSER EXCEPTION: Expected token type \"{type}\", found \"{token.Type}\"");
        }

        return token;
    }

    private Lexer.Token ExpectToken(Lexer.TokenType type1, Lexer.TokenType type2)
    {
        if (_index >= _tokens.Count) throw new ArgumentOutOfRangeException(nameof(_index));

        var token = _tokens[_index++];
        if (token.Type != type1 && token.Type != type2)
        {
            throw new SyntaxErrorException(
                $"PARSER EXCEPTION: Expected token type \"{type1}\" or \"{type2}\", found \"{token.Type}\"");
        }

        return token;
    }
}
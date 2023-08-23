using System.Data;
using System.Diagnostics;
using Yeet.Common.Assembler;

namespace Yeet8.Assembler;

public class Parser
{
    private readonly List<Token> _tokens;

    private int _index;

    public Parser(ref List<Token> tokens)
    {
        _tokens = tokens;
        _index = 0;
    }

    public List<byte> Run()
    {
        var code = new List<byte>();

        while (_index < _tokens.Count)
        {
            var opcode = ExpectToken(TokenType.Opcode);

            byte opcodeType, opcodeNumber;
            switch (opcode.Text)
            {
                // R$, IM$, IM$
                case "add":
                {
                    opcodeNumber = OpCode.Add;
                    opcodeType = 1;
                    break;
                }
                case "sub":
                {
                    opcodeNumber = OpCode.Sub;
                    opcodeType = 1;
                    break;
                }
                case "mul":
                {
                    opcodeNumber = OpCode.Mul;
                    opcodeType = 1;
                    break;
                }
                case "div":
                {
                    opcodeNumber = OpCode.Div;
                    opcodeType = 1;
                    break;
                }
                case "mod":
                {
                    opcodeNumber = OpCode.Mod;
                    opcodeType = 1;
                    break;
                }
                case "and":
                {
                    opcodeNumber = OpCode.And;
                    opcodeType = 1;
                    break;
                }
                case "or":
                {
                    opcodeNumber = OpCode.Or;
                    opcodeType = 1;
                    break;
                }
                case "xor":
                {
                    opcodeNumber = OpCode.Xor;
                    opcodeType = 1;
                    break;
                }
                case "shl":
                {
                    opcodeNumber = OpCode.Shl;
                    opcodeType = 1;
                    break;
                }
                case "shr":
                {
                    opcodeNumber = OpCode.Shr;
                    opcodeType = 1;
                    break;
                }
                case "sal":
                {
                    opcodeNumber = OpCode.Sal;
                    opcodeType = 1;
                    break;
                }
                case "sar":
                {
                    opcodeNumber = OpCode.Sar;
                    opcodeType = 1;
                    break;
                }

                // R$, IM$
                case "not":
                {
                    opcodeNumber = OpCode.Not;
                    opcodeType = 2;
                    break;
                }
                case "read":
                {
                    opcodeNumber = OpCode.Read;
                    opcodeType = 2;
                    break;
                }
                case "move":
                {
                    opcodeNumber = OpCode.Move;
                    opcodeType = 2;
                    break;
                }
                case "in":
                {
                    opcodeNumber = OpCode.In;
                    opcodeType = 2;
                    break;
                }

                // IM$, IM$
                case "write":
                {
                    opcodeNumber = OpCode.Write;
                    opcodeType = 3;
                    break;
                }
                case "out":
                {
                    opcodeNumber = OpCode.Out;
                    opcodeType = 3;
                    break;
                }

                // IM$
                case "push":
                {
                    opcodeNumber = OpCode.Push;
                    opcodeType = 4;
                    break;
                }
                case "jump":
                {
                    opcodeNumber = OpCode.Jump;
                    opcodeType = 4;
                    break;
                }
                case "call":
                {
                    opcodeNumber = OpCode.Call;
                    opcodeType = 4;
                    break;
                }

                // R$
                case "pop":
                {
                    opcodeNumber = OpCode.Pop;
                    opcodeType = 5;
                    break;
                }

                // IM$, IM$, IM$
                case "jb":
                {
                    opcodeNumber = OpCode.Jb;
                    opcodeType = 6;
                    break;
                }
                case "ja":
                {
                    opcodeNumber = OpCode.Ja;
                    opcodeType = 6;
                    break;
                }
                case "je":
                {
                    opcodeNumber = OpCode.Je;
                    opcodeType = 6;
                    break;
                }
                case "jne":
                {
                    opcodeNumber = OpCode.Jne;
                    opcodeType = 6;
                    break;
                }
                case "jbe":
                {
                    opcodeNumber = OpCode.Jbe;
                    opcodeType = 6;
                    break;
                }
                case "jae":
                {
                    opcodeNumber = OpCode.Jae;
                    opcodeType = 6;
                    break;
                }

                // <none>
                case "ret":
                {
                    opcodeNumber = OpCode.Ret;
                    opcodeType = 7;
                    break;
                }

                default: throw new UnreachableException();
            }

            switch (opcodeType)
            {
                case 1:
                {
                    var destination = ExpectToken(TokenType.Register);
                    var source1 = ExpectToken(TokenType.Register, TokenType.Number);
                    var source2 = ExpectToken(TokenType.Register, TokenType.Number);

                    var flags = Flag.Op1Register;
                    if (source1.Type is TokenType.Register) flags |= Flag.Op2Register;
                    if (source2.Type is TokenType.Register) flags |= Flag.Op3Register;

                    var instruction = Instruction.Create(
                        opcodeNumber,
                        flags,
                        byte.Parse(destination.Text),
                        byte.Parse(source1.Text),
                        byte.Parse(source2.Text)
                    );

                    code.AddRange(BitConverter.GetBytes(instruction));
                    break;
                }
                case 2:
                {
                    var destination = ExpectToken(TokenType.Register);
                    var source = ExpectToken(TokenType.Register, TokenType.Number);

                    var flags = Flag.Op1Register;
                    if (source.Type is TokenType.Register) flags |= Flag.Op2Register;

                    var instruction = Instruction.Create(
                        opcodeNumber,
                        flags,
                        byte.Parse(destination.Text),
                        byte.Parse(source.Text)
                    );

                    code.AddRange(BitConverter.GetBytes(instruction));
                    break;
                }
                case 3:
                {
                    var destination = ExpectToken(TokenType.Register, TokenType.Number);
                    var source = ExpectToken(TokenType.Register, TokenType.Number);

                    byte flags = 0;
                    if (destination.Type is TokenType.Register) flags |= Flag.Op1Register;
                    if (source.Type is TokenType.Register) flags |= Flag.Op2Register;

                    var instruction = Instruction.Create(
                        opcodeNumber,
                        flags,
                        byte.Parse(destination.Text),
                        byte.Parse(source.Text)
                    );

                    code.AddRange(BitConverter.GetBytes(instruction));
                    break;
                }
                case 4:
                {
                    var destination = ExpectToken(TokenType.Register, TokenType.Number);
                    var instruction = Instruction.Create(
                        opcodeNumber,
                        destination.Type is TokenType.Register ? Flag.Op1Register : (byte)0,
                        byte.Parse(destination.Text)
                    );

                    code.AddRange(BitConverter.GetBytes(instruction));
                    break;
                }
                case 5:
                {
                    var destination = ExpectToken(TokenType.Register);
                    var instruction = Instruction.Create(
                        opcodeNumber,
                        Flag.Op1Register,
                        byte.Parse(destination.Text)
                    );

                    code.AddRange(BitConverter.GetBytes(instruction));
                    break;
                }
                case 6:
                {
                    var destination = ExpectToken(TokenType.Register, TokenType.Number);
                    var source1 = ExpectToken(TokenType.Register, TokenType.Number);
                    var source2 = ExpectToken(TokenType.Register, TokenType.Number);

                    byte flags = 0;
                    if (destination.Type is TokenType.Register) flags |= Flag.Op1Register;
                    if (source1.Type is TokenType.Register) flags |= Flag.Op2Register;
                    if (source2.Type is TokenType.Register) flags |= Flag.Op3Register;

                    var instruction = Instruction.Create(
                        opcodeNumber,
                        flags,
                        byte.Parse(destination.Text),
                        byte.Parse(source1.Text),
                        byte.Parse(source2.Text)
                    );

                    code.AddRange(BitConverter.GetBytes(instruction));
                    break;
                }
                case 7:
                {
                    var instruction = Instruction.Create(
                        opcodeNumber
                    );

                    code.AddRange(BitConverter.GetBytes(instruction));
                    break;
                }
                default: throw new UnreachableException();
            }
        }

        return code;
    }

    private Token ExpectToken(TokenType type)
    {
        if (_index >= _tokens.Count) throw new ArgumentOutOfRangeException(nameof(_index));

        var token = _tokens[_index++];
        if (token.Type != type)
        {
            throw new SyntaxErrorException($"PARSER EXCEPTION: Expected token type \"{type}\", found \"{token.Type}\"");
        }

        return token;
    }

    private Token ExpectToken(TokenType type1, TokenType type2)
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
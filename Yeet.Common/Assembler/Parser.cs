using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;

namespace Yeet.Common.Assembler;

public sealed class Parser
{
    private readonly List<Token> _tokens;
    private readonly Instruction _instruction;

    private ulong _instructionAddress;
    private ulong _labelAddress;
    private bool _onLabel;

    private int _index;

    public Parser(ref List<Token> tokens, Instruction instruction)
    {
        _tokens = tokens;
        _instruction = instruction;
        _instructionAddress = 0;
        _labelAddress = 0;
        _onLabel = false;
        _index = 0;
    }

    public List<byte> Run()
    {
        var code = new List<byte>();

        while (_index < _tokens.Count)
        {
            var token = ExpectToken(TokenType.Opcode, TokenType.Label);

            /*if (token.Type == TokenType.Label)
            {
                if(!_onLabel)
                {
                    _onLabel = true;
                    _labelAddress = _instructionAddress;
                }
                continue;
            }*/

            _instructionAddress += 4;

            byte opcodeType, opcodeNumber;
            switch (token.Text)
            {
                // Type 1 instructions
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
                case "not":
                {
                    opcodeNumber = OpCode.Not;
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
                case "read":
                {
                    opcodeNumber = OpCode.Read;
                    opcodeType = 1;
                    break;
                }
                case "write":
                {
                    opcodeNumber = OpCode.Write;
                    opcodeType = 1;
                    break;
                }
                case "move":
                {
                    opcodeNumber = OpCode.Move;
                    opcodeType = 1;
                    break;
                }
                case "in":
                {
                    opcodeNumber = OpCode.In;
                    opcodeType = 1;
                    break;
                }
                case "out":
                {
                    opcodeNumber = OpCode.Out;
                    opcodeType = 1;
                    break;
                }
                case "cmp":
                {
                    opcodeNumber = OpCode.Cmp;
                    opcodeType = 1;
                    break;
                }

                // Type 2 instructions
                case "push":
                {
                    opcodeNumber = OpCode.Push;
                    opcodeType = 2;
                    break;
                }
                case "jump":
                {
                    opcodeNumber = OpCode.Jump;
                    opcodeType = 2;
                    break;
                }
                case "call":
                {
                    opcodeNumber = OpCode.Call;
                    opcodeType = 2;
                    break;
                }
                case "jb":
                {
                    opcodeNumber = OpCode.Jb;
                    opcodeType = 2;
                    break;
                }
                case "ja":
                {
                    opcodeNumber = OpCode.Ja;
                    opcodeType = 2;
                    break;
                }
                case "je":
                {
                    opcodeNumber = OpCode.Je;
                    opcodeType = 2;
                    break;
                }
                case "jne":
                {
                    opcodeNumber = OpCode.Jne;
                    opcodeType = 2;
                    break;
                }
                case "jbe":
                {
                    opcodeNumber = OpCode.Jbe;
                    opcodeType = 2;
                    break;
                }
                case "jae":
                {
                    opcodeNumber = OpCode.Jae;
                    opcodeType = 2;
                    break;
                }

                // Type 3 instructions
                case "pop":
                {
                    opcodeNumber = OpCode.Pop;
                    opcodeType = 3;
                    break;
                }

                // Type 4 instructions
                case "ret":
                {
                    opcodeNumber = OpCode.Ret;
                    opcodeType = 4;
                    break;
                }

                default: throw new UnreachableException();
            }

            switch (opcodeType)
            {
                case 1:
                {
                    var operand1 = ExpectToken(TokenType.Register);
                    var operand2 = ExpectToken(TokenType.Register, TokenType.Number);
                    var instruction = _instruction.CreateType1(
                        opcodeNumber,
                        operand2.Type is TokenType.Register,
                        uint.Parse(operand1.Text),
                        uint.Parse(operand2.Text)
                    );

                    code.AddRange(BitConverter.GetBytes(instruction));
                    break;
                }
                case 2:
                {
                    var operand1 = ExpectToken(TokenType.Register, TokenType.Number);
                    var instruction = _instruction.CreateType2(
                        opcodeNumber,
                        operand1.Type is TokenType.Register,
                        uint.Parse(operand1.Text)
                    );

                    code.AddRange(BitConverter.GetBytes(instruction));
                    break;
                }
                case 3:
                {
                    var operand1 = ExpectToken(TokenType.Register);
                    var instruction = _instruction.CreateType3(
                        opcodeNumber,
                        byte.Parse(operand1.Text)
                    );

                    code.AddRange(BitConverter.GetBytes(instruction));
                    break;
                }
                case 4:
                {
                    var instruction = _instruction.CreateType4(
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
            Utils.AbortExecution($"PARSER EXCEPTION: Expected token type \"{type}\", found \"{token.Type}\"");

        return token;
    }

    private Token ExpectToken(TokenType type1, TokenType type2)
    {
        if (_index >= _tokens.Count) throw new ArgumentOutOfRangeException(nameof(_index));

        var token = _tokens[_index++];
        if (token.Type != type1 && token.Type != type2)
            throw new SyntaxErrorException(
                $"PARSER EXCEPTION: Expected token type \"{type1}\" or \"{type2}\", found \"{token.Type}\"");

        return token;
    }
}
# **Yeet-8 Specifications**

## Registers (8-bit)
* R0 - Stack Pointer / SP
* R1
* R2
* R3
* R4
* ..
* R15

## Convention

* R$ - Any register
* IM$ - Any register **or** immediate value
* IMM - Immediate (constant) value

## Instructions

**There are 29 instructions in total.**

### Arithmetic

* add R$, IM$, IM$ - Adds two numbers
* sub R$, IM$, IM$ - Subtracts two numbers
* mul R$, IM$, IM$ - Multiplies two numbers
* div R$, IM$, IM$ - Divides two numbers
* mod R$, IM$, IM$ - Computes the reminder using two numbers
* and R$, IM$, IM$ - Computes the bitwise AND using two numbers
* or R$, IM$, IM$ - Computes the bitwise OR using two numbers
* xor R$, IM$, IM$ - Computes the bitwise XOR using two numbers
* not R$, IM$ - Computes the bitwise NOT of a number
* shl R$, IM$, IM$ - Computes the logical left shift using two numbers
* shr R$, IM$, IM$ - Computes the logical right shift using two numbers
* sal R$, IM$, IM$ - Computes the arithmetic left shift using two numbers
* sar R$, IM$, IM$ - Computes the arithmetic right shift using two numbers

### Memory

* read R$, IM$ - Read from memory
* write IM$, IM$ - Write to memory
* move R$, IM$ - Move between registers

### Stack

* push IM$ - Push value to stack
* pop R$ - Pop value from stack

### I/O

* in R$, IM$ - Read from I/O port
* out IM$, IM$ - Write to I/O port

### Control Flow

* ret - Returns from call
* jump IM$ - Jumps to specified address
* call IM$ - Calls specified address
* jb IM$, IM$, IM$ - Jumps if below
* ja IM$, IM$, IM$ - Jumps if above
* je IM$, IM$, IM$ - Jumps if equal
* jne IM$, IM$, IM$ - Jumps if not equal
* jbe IM$, IM$, IM$ - Jumps if below or equal
* jae IM$, IM$, IM$ - Jumps if above or equal

## Layout

**All instructions are of fixed width (32-bit).**

00000  000   00000000 00000000 00000000
OpCode Flags Op1      Op2      Op3

## OpCodes

* add - 0x00
* sub - 0x01
* mul - 0x02
* div - 0x03
* mod - 0x04
* and - 0x05
* or - 0x06
* xor - 0x07
* not - 0x08
* shl - 0x09
* shr - 0x0A
* sal - 0x0B
* sar - 0x0C
* read - 0x0D
* write - 0x0E
* move - 0x0F
* push - 0x10
* pop - 0x11
* in - 0x12
* out - 0x13
* ret - 0x14
* jump - 0x15
* call - 0x16
* jb - 0x17
* ja - 0x18
* je - 0x19
* jne - 0x20
* jbe - 0x21
* jae - 0x22

## Flags

Flag 0: Op1 is register
Flag 1: Op2 is register
Flag 2: Op3 is register

## Ports

* 0x00 - Reserved port for POST (Power-On Self Test)
* 0x01 - Test port which does nothing
* 0x02 - Tells the processor to shut down
* 0x03 - Prints an ASCII character to the screen
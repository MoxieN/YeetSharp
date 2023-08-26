# Computes number
move r3, 60
add r3, 5

move r7, 66

# Pushes number to stack, then pops it to another register
push r7
pop r4

# Prints 'A' and '\n' to screen via syscall 01h
move r5, 1
out r5, 1 # Print decimal value's corresponding character stored in R3
move r3, 10
out r5, 1

# Tells the emulator to shut down via a system call 00h
out r5, 0
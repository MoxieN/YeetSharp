# Computes number
move r3, 38
add r3, 30

move r7, 66

# Pushes number to stack, then pops it to another register
push r7
pop r4

# Prints 'A' to screen via system call
move r3, 1
out r3, 48


# Tells the emulator to shut down via a system call
move r5, 1
out r5, 0
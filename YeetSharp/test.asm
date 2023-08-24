# Computes number
move r3, 35
add r3, 30

# Pushes number to stack, then pops it to another register
push r3
pop r4

# Prints 'A' to screen via system call
# out 1, 41

# Tells the emulator to shut down via a system call
move r5, 1
out 0, r5
# Computes number
move r3, 35
add r3, 30
jump label

label:
# Pushes number to stack, then pops it to another register
push r3
pop r4

# Tells the emulator to shut down via a system call
move r5, 1
out r5, 0
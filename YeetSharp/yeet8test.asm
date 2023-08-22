# Calculates 'A' character
move r1, 35
add r2, r1, 30

# Pushes character to stack, then pops it to another register
push r2
pop r3

# Prints 'A' to the screen
out 0x03 r3

# Tells the processor to shut down
out 0x02
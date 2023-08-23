# Calculates 'A' character
move r1, 35
add r2, r1, 30

# Pushes character to stack, then pops it to another register
push r2
pop r3

# Prints 'A' to the screen
out 3, r3

# Prints a new line to the screen
out 3, 10

# Tells the processor to shut down
out 2, 0
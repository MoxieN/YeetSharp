# Calculates number ('A' in ASCII)
move r1, 35
add r1, 30
move r2, r1

# Pushes character to stack, then pops it to another register
push r2
pop r3
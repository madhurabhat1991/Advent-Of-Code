import re

DAY_NUMBER = "03"
EXAMPLE_NUMBER = "2"
EXAMPLE_FILE = f"..\\2024\\Day{DAY_NUMBER}\\example{EXAMPLE_NUMBER}.txt"
INPUT_FILE = f"..\\2024\\Day{DAY_NUMBER}\\input.txt"

# change input to EXAMPLE_PATH or INPUT_PATH
with open(INPUT_FILE, 'r') as f:
    puzzle_input = ''.join([line.strip() for line in f])

def part1(puzzle_input):
    return sum([int(a)*int(b) for a, b in re.findall(r'mul\((\d+),(\d+)\)', puzzle_input)])

def part2(puzzle_input):
    total = 0
    mul = True
    for d, a, b in re.findall( r"(do\(\)|don't\(\)|mul\((-?\d+),(-?\d+)\))", puzzle_input):
       if d == "don't()":
           mul = False
       elif d == "do()":
           mul = True
       elif mul:
           total += int(a)*int(b)
    return total

print(f"Part 1: {part1(puzzle_input)}")
print(f"Part 2: {part2(puzzle_input)}")
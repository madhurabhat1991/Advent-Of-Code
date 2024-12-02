DAY_NUMBER = "01"
EXAMPLE_FILE = f"..\\2024\\Day{DAY_NUMBER}\\example.txt"
INPUT_FILE = f"..\\2024\\Day{DAY_NUMBER}\\input.txt"

# change input to EXAMPLE_FILE or INPUT_FILE
with open(INPUT_FILE, 'r') as f:
    puzzle_input = [line.strip() for line in f]
    left, right = [], []
    for line in puzzle_input:
        l, r = line.split()
        left.append(int(l))
        right.append(int(r))

def part1(left, right):
    left = sorted(left)
    right = sorted(right)
    return sum(map(lambda x, y: abs(x-y) , left, right))

def part2(left, right):
    return sum(map(lambda x: x * right.count(x) , left))

print(f"Part 1: {part1(left, right)}")
print(f"Part 2: {part2(left, right)}")
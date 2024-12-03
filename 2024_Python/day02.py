DAY_NUMBER = "02"
EXAMPLE_FILE = f"..\\2024\\Day{DAY_NUMBER}\\example.txt"
INPUT_FILE = f"..\\2024\\Day{DAY_NUMBER}\\input.txt"

# change input to EXAMPLE_PATH or INPUT_PATH
with open(INPUT_FILE, 'r') as f:
    puzzle_input = [list([int(x) for x in line.strip().split(" ")]) for line in f]

def safe(report, counter):
    sum = 0
    diff_list = [abs(report[i+1] - report[i]) for i in range(len(report)-1)]
    sign_list = [-1 if (report[i+1] - report[i]) < 0 else 1 if (report[i+1] - report[i]) > 0 else 0 for i in range(len(report)-1)]
    sign_grouped = dict([(i, sign_list.count(i)) for i in set(sign_list)])
    
    if all(x >= 1 and x <= 3 for x in diff_list) and all(x == sign_list[0] for x in sign_list):
        return True
    elif (len(diff_list) > 1 and len([i for i in diff_list if i > 3]) == 1) or (len(sign_grouped) > 1 and min(sign_grouped.values()) == 1):
        suspect_index = 0
        if len(diff_list) > 1 and len([i for i in diff_list if i > 3]) == 1:
            suspect_index = diff_list.index([i for i in diff_list if i > 3][0])
        elif len(sign_grouped) > 1 and min(sign_grouped.values()) == 1:
            suspect_index = sign_list.index([key for key, value in sign_grouped.items() if value == min(sign_grouped.values()) == 1][0])
        temp1, temp2 = list(report), list(report)
        temp1.pop(suspect_index + 1)
        temp2.pop(suspect_index)
        counter += 1
        if counter == 1 and (safe(temp1, counter) or safe(temp2, counter)):
            return True
    return False

def part1(puzzle_input):
    sum = 0
    for report in puzzle_input:
        diff_list = [abs(report[i+1] - report[i]) for i in range(len(report)-1)]
        sign_list = [-1 if (report[i+1] - report[i]) < 0 else 1 if (report[i+1] - report[i]) > 0 else 0 for i in range(len(report)-1)]
        sum += all(x >= 1 and x <= 3 for x in diff_list) and all(x == sign_list[0] for x in sign_list)
    return sum

def part2(puzzle_input):
    sum = 0
    for report in puzzle_input:
        counter = 0
        sum += 1 if safe(report, counter) else 0
    return sum

print(f"Part 1: {part1(puzzle_input)}")
print(f"Part 2: {part2(puzzle_input)}")
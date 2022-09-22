import re


# Function to convert into UwU text
def generateUwU(input_text):
    # the length of the input text
    length = len(input_text)

    # variable declaration for the output text
    output_text = ''

    # check the cases for every individual character
    for i in range(length):

        # initialize the variables
        current_char = input_text[i]
        previous_char = '&# 092;&# 048;'

        # assign the value of previous_char
        if i > 0:
            previous_char = input_text[i - 1]

        # change 'L' and 'R' to 'W'
        if current_char == 'L' or current_char == 'R':
            output_text += 'W'

        # change 'l' and 'r' to 'w'
        elif current_char == 'l' or current_char == 'r':
            output_text += 'w'

        # if the current character is 'o' or 'O'
        # also check the previous character
        elif current_char == 'O' or current_char == 'o':
            if previous_char == 'N' or previous_char == 'n' or previous_char == 'M' or previous_char == 'm':
                output_text += "yo"
            else:
                output_text += current_char

        # if no case match, write it as it is
        else:
            output_text += current_char

    return output_text


def fileUwuInator(filename):
    rotator = 0
    index = 0
    rows = []

    with open(filename + ".txt") as f:
        lines = f.read()
        pattern = re.compile('(?:,|\n|^)("(?:(?:"")*[^"]*)*"|[^",\n]*|(?:\n|$))')
        for m in re.finditer(pattern, lines):
            if rotator % 3 == 0:
                rows.append(m.group(1) + ",")

            if rotator % 3 == 1:
                rows[index] += generateUwU(m.group(1)) + ","

            if rotator % 3 == 2:
                rows[index] += m.group(1)
                index += 1
            rotator += 1

        uwuF = open(filename + "_uwu.txt", "x")
        uwuF.write("\r".join(rows))


# Driver code
if __name__ == '__main__':
    fileUwuInator("enUS")
    fileUwuInator("enUS_names")

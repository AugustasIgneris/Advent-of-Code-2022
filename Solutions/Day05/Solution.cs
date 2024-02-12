string[] input = File.ReadAllLines("Input.txt");

PartOne(input); // Answer VRWBSFZWM
PartTwo(input); // Answer RBTWJWMCF

static void PartOne(string[] input)
{
    int crateCharInterval = 4;
    int instructionStart = 10;
    int bottomCrate = 7;
    int numberOfStacks = 9;

    Stack<char>[] stacks = Enumerable.Range(0, numberOfStacks).Select((_) => new Stack<char>()).ToArray();

    for (int i = bottomCrate; i >= 0; i--)
    {
        string line = input[i];

        for (int lineIndex = 1, stackIndex = 0; lineIndex < line.Length; lineIndex += crateCharInterval, stackIndex++)
        {
            char crate = line[lineIndex];
            if (crate != ' ')
            {
                stacks[stackIndex].Push(line[lineIndex]);
            }
        }
    }

    for (int i = instructionStart; i < input.Length; i++)
    {
        string line = input[i];
        var (from, to, count) = ParseInstruction(line);

        for (int j = 0; j < count; j++)
        {
            stacks[to].Push(stacks[from].Pop());
        }
    }

    var topCrates = new string((stacks.Select(s => s.Peek()).ToArray()));
    Console.WriteLine($"Top crates, when moving one at a time, are: {topCrates}");
}

static void PartTwo(string[] input)
{
    int crateCharInterval = 4;
    int instructionStart = 10;
    int bottomCrate = 7;
    int numberOfStacks = 9;

    List<char>[] stacks = Enumerable.Range(0, numberOfStacks).Select((_) => new List<char>()).ToArray();

    for (int i = bottomCrate; i >= 0; i--)
    {
        string line = input[i];

        for (int lineIndex = 1, stackIndex = 0; lineIndex < line.Length; lineIndex += crateCharInterval, stackIndex++)
        {
            char crate = line[lineIndex];
            if (crate != ' ')
            {
                stacks[stackIndex].Add(line[lineIndex]);
            }
        }
    }

    for (int i = instructionStart; i < input.Length; i++)
    {
        string line = input[i];
        var (from, to, count) = ParseInstruction(line);
        var fromStack = stacks[from];

        for (int j = count; j > 0; j--)
        {
            var crate = fromStack[fromStack.Count - j];
            stacks[to].Add(crate);
        }

        fromStack.RemoveRange(fromStack.Count - count, count);
    }

    var topCrates = new string((stacks.Select(s => s.Last()).ToArray()));
    Console.WriteLine($"Top crates, when moving all at a time, are: {topCrates}");
}

static (int from, int to, int count) ParseInstruction(string line)
{
    int from, to, count;

    string[] tokens = line.Split(' ');
    count = int.Parse(tokens[1]);
    from = int.Parse(tokens[3]) - 1;
    to = int.Parse(tokens[5]) - 1;

    return (from, to, count);
}

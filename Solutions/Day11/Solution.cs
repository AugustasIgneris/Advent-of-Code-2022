string[] input = File.ReadAllLines("Input.txt");

PartOne(input); // 102399;
PartTwo(input); // 23641658401

static void PartOne(string[] input)
{
    List<Monkey> monkeys = GetMonkeys(input);

    for (int i = 0; i < 20; i++)
    {
        for (int m = 0; m < monkeys.Count; m++)
        {
            Monkey monkey = monkeys[m];

            for (int item = 0; item < monkey.Items.Count; item++)
            {
                long worryLevel = monkey.Items[item];
                worryLevel = monkey.Operation.Inspect(worryLevel);
                worryLevel /= 3;
                int target = monkey.Test.GetTarget(worryLevel);
                monkeys[target].Items.Add(worryLevel);
                monkey.Throws++;
            }

            monkey.Items.Clear();
        }
    }

    long monkeyBusiness = GetMonkeyBusiness(monkeys);
    Console.WriteLine($"Monkey business after 20 rounds is {monkeyBusiness}");
}

static void PartTwo(string[] input)
{
    List<Monkey> monkeys = GetMonkeys(input);

    // Common divisor for all monkey tests
    // Somehow it's intuitive that worry level above this number does not matter
    // Yet I can't explain it
    int commonDivisor = monkeys.Aggregate(1, (sum, monkey) => sum * monkey.Test.DivisibleBy);

    for (int i = 0; i < 10000; i++)
    {
        for (int m = 0; m < monkeys.Count; m++)
        {
            Monkey monkey = monkeys[m];

            for (int item = 0; item < monkey.Items.Count; item++)
            {
                long worryLevel = monkey.Items[item];
                worryLevel = monkey.Operation.Inspect(worryLevel);
                worryLevel = worryLevel % commonDivisor;
                int target = monkey.Test.GetTarget(worryLevel);
                monkeys[target].Items.Add(worryLevel);
                monkey.Throws++;
            }

            monkey.Items.Clear();
        }
    }

    long monkeyBusiness = GetMonkeyBusiness(monkeys);
    Console.WriteLine($"Monkey business after 10000 rounds is {monkeyBusiness}");
}

static List<Monkey> GetMonkeys(string[] input)
{
    List<Monkey> monkeys = new List<Monkey>();
    for (int i = 1; i < input.Length; i += 7)
    {
        List<long> startingItems = input[i]
            .Substring(input[i].IndexOf(':') + 1)
            .Split(',')
            .Select(s => long.Parse(s))
            .ToList();

        Operation op = new Operation(input[i + 1]);
        Test test = new Test(input[i + 2], input[i + 3], input[i + 4]);
        monkeys.Add(new Monkey(startingItems, op, test));
    }

    return monkeys;
}

static long GetMonkeyBusiness(List<Monkey> monkeys)
{
    long monkeyBusiness = monkeys.Select(m => m.Throws).OrderDescending().Take(2).Aggregate((long)1, (sum, throws) => sum * throws);
    return monkeyBusiness;
}

class Monkey
{
    public Monkey(List<long> items, Operation operation, Test test)
    {
        Operation = operation;
        Test = test;
        Items = items;
    }

    public List<long> Items { get; set; }
    public Operation Operation { get; }
    public Test Test { get; }
    public int Throws { get; set; }
}

class Operation
{
    private readonly char _opType;
    private readonly int _modifier;

    public Operation(string line)
    {
        int opIndex = line.IndexOfAny(['+', '*']);
        _opType = line[opIndex];
        ReadOnlySpan<char> modifierSpan = line.AsSpan(opIndex + 2);

        if (modifierSpan.SequenceEqual("old"))
        {
            _modifier = int.MinValue;
        }
        else
        {
            _modifier = int.Parse(modifierSpan);
        }
    }

    public long Inspect(long worryLevel)
    {
        var modifier = _modifier == int.MinValue ? worryLevel : _modifier;

        if (_opType == '+')
        {
            return checked(worryLevel + modifier); 
        }
        else
        {
            return checked(worryLevel * modifier);
        }
    }
}

class Test
{
    private readonly int _monkeyIfTrue;
    private readonly int _monkeyIfFalse;

    public Test(string condition, string ifTrue, string ifFalse)
    {
        DivisibleBy = int.Parse(condition.AsSpan(condition.LastIndexOf(' ') + 1));
        _monkeyIfTrue = int.Parse(ifTrue.AsSpan(ifTrue.LastIndexOf(' ') + 1));
        _monkeyIfFalse = int.Parse(ifFalse.AsSpan(ifFalse.LastIndexOf(' ') + 1));
    }

    public int DivisibleBy { get; }

    public int GetTarget(long worryLevel)
    {
        return worryLevel % DivisibleBy == 0 ? _monkeyIfTrue : _monkeyIfFalse;
    }
}

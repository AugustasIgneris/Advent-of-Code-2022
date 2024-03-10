using System.Collections.ObjectModel;

string[] input = File.ReadAllLines("Input.txt");

PartOne(input); // 5198
PartTwo(input); // 22344

static void PartOne(string[] input)
{
    int sum = 0;

    foreach ((int index, PacketContent left, PacketContent right) in GetPairs(input))
    {
        if (left.CompareTo(right) < 0)
        {
            sum += index;
        }
    }

    Console.WriteLine($"The sum of indeces of correctly ordered packets is {sum}");
}

static void PartTwo(string[] input)
{
    List<PacketContent> packets = [];

    var divider1 = PacketContent.Parse("[[2]]");
    var divider2 = PacketContent.Parse("[[6]]");

    packets.Add(divider1);
    packets.Add(divider2);

    foreach (string line in input)
    {
        if (line == "")
        {
            continue;
        }

        packets.Add(PacketContent.Parse(line));
    }

    packets.Sort();

    int key = (packets.IndexOf(divider1) + 1) * (packets.IndexOf(divider2) + 1);
    Console.WriteLine($"The decoder key is {key}");
}

static IEnumerable<(int index, PacketContent left, PacketContent right)> GetPairs(string[] input) {
    int pair = 1;
    for (int i = 0; i < input.Length; i+=3)
    {
        yield return (pair++, PacketContent.Parse(input[i]), PacketContent.Parse(input[i + 1]));
    }
}

record PacketContent : IComparable<PacketContent>
{
    public IReadOnlyList<PacketContent> Children { get; }
    public int? Value { get; }

    PacketContent(IReadOnlyList<PacketContent> children)
    {
        Children = children;
    }

    PacketContent(int value)
    {
        Children = ReadOnlyCollection<PacketContent>.Empty;
        Value = value;
    }

    public int CompareTo(PacketContent? other)
    {
        return CompareTo(other, 0);
    }


    public int CompareTo(PacketContent? other, int level)
    {
        level++;

        if (other == null)
        {
            return -1;
        }

        if (Value.HasValue)
        {
            if (other.Value.HasValue)
            {
                // Compare two values directly
                return Value.Value.CompareTo(other.Value);
            }
            else
            {
                // Wrap this value so it can be compared
                return new PacketContent([this]).CompareTo(other, level);
            }
        }
        else if (other.Value.HasValue)
        {
            // Wrap other value so it can be compared
            return CompareTo(new PacketContent([other]), level);
        }

        // Compare two lists
        for (int i = 0; i < Children.Count; i++)
        {
            if (!(other.Children.Count > i))
            {
                // Other list is shorter thus this list is greater
                return 1;
            }

            int ret = Children[i].CompareTo(other.Children[i], level);
            if (ret != 0)
            {
                return ret;
            }
        }

        if (Children.Count < other.Children.Count)
        {
            // Other list is longer thus this list is lesser
            return -1;
        }

        return 0;
    }

    public static PacketContent Parse(string packetSpan)
    {
        Stack<List<PacketContent>> listStack = new();

        for (int i = 0; i < packetSpan.Length; i++)
        {
            char currentChar = packetSpan[i];
            if (currentChar == '[')
            {
                listStack.Push([]);
            }
            else if (currentChar == ']')
            {
                var child = new PacketContent(listStack.Pop());

                if (listStack.TryPeek(out List<PacketContent>? current))
                {
                    current.Add(child);
                }
                else
                {
                    return child;
                }
            }
            else if (currentChar == ',')
            {
                continue;
            }
            else
            {
                int endIndex = packetSpan.IndexOfAny(['[', ']', ','], i + 1) - 1;
                int number;

                if (endIndex == i)
                {
                    number = (int)char.GetNumericValue(currentChar);
                }
                else
                {
                    number = int.Parse(packetSpan.AsSpan(i, endIndex - i + 1));
                    i = endIndex;
                }

                listStack.Peek().Add(new PacketContent(number));
            }
        }

        throw new InvalidDataException(packetSpan);
    }

    public override string? ToString()
    {
        if (Value.HasValue)
        {
            return Value.Value.ToString();
        }
        else
        {
            return '[' + string.Join(',', Children) + ']';
        }
    }
}

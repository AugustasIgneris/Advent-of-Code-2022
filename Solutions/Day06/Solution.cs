
PartOne(); // Answer 1833
PartTwo(); // Answer 3425

static void PartOne()
{
    int packetStart = GetUniqueSequence(4);
    Console.WriteLine($"The packet starts at index {packetStart}");
}

static void PartTwo()
{
    int messageStart = GetUniqueSequence(14);
    Console.WriteLine($"The message starts at index {messageStart}");
}

static int GetUniqueSequence(int length)
{
    using var stream = new StreamReader(File.OpenRead("Input.txt"));
    var sequence = new List<int>(length);
    int messageStart = 0;
    for (; !stream.EndOfStream && sequence.Count < length; messageStart++)
    {
        int current = stream.Read();
        int previous = sequence.IndexOf(current);

        if (previous != -1)
        {
            sequence.RemoveRange(0, previous + 1);
        }

        sequence.Add(current);
    }

    return messageStart;
}

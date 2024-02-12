
string[] input = File.ReadAllLines("Input.txt");

PartOne(input); // Answer 16060
PartTwo(input); // BACEKLHF

static void PartOne(string[] input)
{
    int cycle = 0;
    int signalStrength = 0;
    int register = 1;
    int nextSample = 20;

    foreach (string line in input)
    {
        int addition = 0;
        bool add = false;
        if (line == "noop")
        {
            cycle++;
        }
        else
        {
            cycle += 2;
            add = true;
        }

        if (cycle >= nextSample)
        {
            signalStrength += nextSample * register;
            nextSample += 40;
            if (nextSample > 220)
            {
                break;
            }
        }

        if (add)
        {
            addition = int.Parse(line.AsSpan(5));
            register += addition;
        }
    }

    Console.WriteLine($"Signal strength is {signalStrength}");
}

static void PartTwo(string[] input)
{
    Console.WriteLine($"\nPart two\n");

    int cycle = 0;
    int register = 1;
    int nextSample = 40;

    foreach (string line in input)
    {
        int addition = 0;
        bool add = false;
        int instructionCycles = 1;

        if (line != "noop")
        {
            instructionCycles = 2;
            add = true;
        }

        for (int i = 0; i < instructionCycles; i++)
        {
            cycle += 1;

            if (cycle > nextSample)
            {
                Console.WriteLine();
                nextSample += 40;
            }

            // CRT starts from 0
            if (Math.Abs(((cycle - 1) % 40) - register) < 2)
            {
                Console.Write('#');
            }
            else
            {
                Console.Write('.');
            }
        }

        if (add)
        {
            addition = int.Parse(line.AsSpan(5));
            register += addition;
        }
    }
}

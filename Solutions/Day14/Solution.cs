string[] input = File.ReadAllLines("Input.txt");

PartOne(input); // 1298
PartTwo(input); // 25585

static void PartOne(string[] input)
{
    char[,] cave = ReadCave(input, true, out Node hole);

    //Console.WriteLine("========================== BEFORE ==========================\n");
    //PrintCave(cave);
    int sandAtRest = 0;
    bool fellThrough = false;
    while (!fellThrough)
    {
        int sandX = hole.x;
        int sandY = hole.y;

        while (true)
        {
            int newSandY = sandY + 1;

            if (newSandY > cave.GetLength(1))
            {
                // Sand fell down below any of the rocks
                fellThrough = true;
                break;
            }

            if (cave[sandX, newSandY] == '.')
            {
                // Fell straight down
                sandY = newSandY;
                continue;
            }

            // Check if sand can fall to the left
            int newSandX = sandX - 1;

            if (newSandX < 0)
            {
                // Sand falls to the void on the left
                fellThrough = true;
                break;
            }


            if (cave[newSandX, newSandY] == '.')
            {
                sandY = newSandY;
                sandX = newSandX;
                continue;
            }

            // Check if sand can fall to the right
            newSandX = sandX + 1;

            if (newSandX >= cave.GetLength(0))
            {
                // Sand falls to the void on the right
                fellThrough = true;
                break;
            }

            if (cave[newSandX, newSandY] == '.')
            {
                sandY = newSandY;
                sandX = newSandX;
                continue;
            }

            // Nowhere to go - sand is at rest
            cave[sandX, sandY] = 'o';
            sandAtRest += 1;
            break;
        }
    }

    //Console.WriteLine("\n========================== AFTER ==========================\n");
    //PrintCave(cave);
    Console.WriteLine($"PART ONE: The units of sand that came to rest is {sandAtRest}");
}

static void PartTwo(string[] input)
{
    char[,] cave = ReadCave(input, false, out Node hole);

    //Console.WriteLine("========================== BEFORE ==========================\n");
    //PrintCave(cave);
    int sandAtRest = 0;
    bool blockedSource = false;
    while (!blockedSource)
    {
        int sandX = hole.x;
        int sandY = hole.y;

        while (true)
        {
            int newSandY = sandY + 1;

            if (cave[sandX, newSandY] == '.')
            {
                // Fell straight down
                sandY = newSandY;
                continue;
            }

            // Check if sand can fall to the left
            int newSandX = sandX - 1;

            if (cave[newSandX, newSandY] == '.')
            {
                sandY = newSandY;
                sandX = newSandX;
                continue;
            }

            // Check if sand can fall to the right
            newSandX = sandX + 1;

            if (cave[newSandX, newSandY] == '.')
            {
                sandY = newSandY;
                sandX = newSandX;
                continue;
            }

            // Nowhere to go - sand is at rest
            cave[sandX, sandY] = 'o';
            sandAtRest += 1;

            if (sandX == hole.x && sandY == hole.y)
            {
                blockedSource = true;
            }

            break;
        }
    }

    //Console.WriteLine("\n========================== AFTER ==========================\n");
    //PrintCave(cave);
    Console.WriteLine($"PART TWO: The units of sand that came to rest is {sandAtRest}");
}

static char[,] ReadCave(string[] input, bool finite, out Node hole)
{
    char[,] cave = new char[750, 750];
    int minX = int.MaxValue;
    int maxX = 0;
    int minY = 0;
    int maxY = 0;
    cave[500, 0] = '*';

    foreach (string line in input)
    {
        Node? previous = null;

        foreach (Node node in GetNodes(line))
        {
            if (previous != null)
            {
                int xDiff = node.x - previous.Value.x;
                if (xDiff != 0)
                {
                    int increment = Math.Sign(xDiff);
                    for (int x = previous.Value.x; x != node.x;)
                    {
                        x += increment;
                        cave[x, node.y] = '#';
                    }
                }
                else
                {
                    int yDiff = node.y - previous.Value.y;
                    int increment = Math.Sign(yDiff);

                    for (int y = previous.Value.y; y != node.y;)
                    {
                        y += increment;
                        cave[node.x, y] = '#';
                    }
                }
            }
            else
            {
                cave[node.x, node.y] = '#';
            }

            previous = node;
        }
    }

    for (int y = 0; y < cave.GetLength(1); y++)
    {
        for (int x = 0; x < cave.GetLength(0); x++)
        {
            if (cave[x, y] == '\0')
            {
                // Fill the cave with 'air' to match the puzzle better
                cave[x, y] = '.';
            }
            else
            {
                if (x > maxX)
                {
                    maxX = x;
                }
                else if (x < minX)
                {
                    minX = x;
                }
                if (y > maxY)
                {
                    maxY = y;
                }
            }
        }
    }

    if (!finite)
    {
        minX -= 50;
        maxX += 150;
        maxY += 2;
    }

    char[,] trimmedCave = new char[maxX - minX + 1, maxY - minY + 1];

    for (int x = minX; x <= maxX; x++)
    {
        for (int y = minY; y <= maxY; y++)
        {
            trimmedCave[x - minX, y - minY] = cave[x, y];
        }
    }

    hole = new Node(500 - minX, 0);
    cave = trimmedCave;

    if (!finite)
    {
        for (int x = 0; x < cave.GetLength(0); x++)
        {
            cave[x, maxY] = '#';
        }
    }

    return cave;
}

static IEnumerable<Node> GetNodes(string line)
{
    int start = 0;
    while (start < line.Length)
    {
        int xEnd = line.IndexOf(',', start);
        int yEnd = line.IndexOf(' ', xEnd);

        if (yEnd == -1)
        {
            yEnd = line.Length - 1;
        }

        int x = int.Parse(line.AsSpan(start, xEnd - start));
        int y = int.Parse(line.AsSpan(xEnd + 1, yEnd - xEnd));

        yield return new Node(x, y);

        start = yEnd + 4;
    }
}

static void PrintCave(char[,] cave)
{
    for (int y = 0; y < cave.GetLength(1); y++)
    {
        for (int x = 0; x < cave.GetLength(0); x++)
        {
            Console.Write(cave[x, y]);
        }
        Console.WriteLine();
    }
}

record struct Node(int x, int y)
{
    public static implicit operator (int x, int y)(Node value) => (value.x, value.y);
    public static implicit operator Node((int x, int y) value) => new(value.x, value.y);
}

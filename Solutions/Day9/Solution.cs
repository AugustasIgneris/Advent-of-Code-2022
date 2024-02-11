string[] input = File.ReadAllLines("Input.txt");

PartOne(input); // Answer 5960
PartTwo(input); // Answer 2327

static void PartOne(string[] input)
{
    var visited = new HashSet<Point>();
    var hPos = new Point(0, 0);
    var tPos = new Point(0, 0);

    // Start position
    visited.Add(new Point(tPos.X, tPos.Y));

    foreach (string line in input)
    {
        char direction = line[0];
        int steps = int.Parse(line.AsSpan(2));

        for (int step = 0; step < steps; step++)
        {
            MoveHead(hPos, direction);

            if (MoveTail(hPos, tPos))
            {
                visited.Add(new Point(tPos.X, tPos.Y));
            }
        }
    }

    Console.WriteLine($"The number of unique positions visited by the tail is {visited.Count}");
}

static void PartTwo(string[] input)
{
    var visited = new HashSet<Point>();
    Point[] knots = Enumerable.Range(0, 10).Select((_) => new Point(0, 0)).ToArray();

    // Start position
    visited.Add(new Point(knots[knots.Length - 1].X, knots[knots.Length - 1].Y));

    foreach (string line in input)
    {
        char direction = line[0];
        int steps = int.Parse(line.AsSpan(2));

        for (int step = 0; step < steps; step++)
        {
            MoveHead(knots[0], direction);

            // Update knots
            for (int i = 1; i < knots.Length; i++)
            {
                Point head = knots[i - 1];
                Point tail = knots[i];

                if (MoveTail(head, tail))
                {
                    if (i == knots.Length - 1)
                    {
                        visited.Add(new Point(tail.X, tail.Y));
                    }
                }
                else
                {
                    // If this knot does not need to move then neither do the rest
                    break;
                }
            }

        }
    }

    Console.WriteLine($"The number of unique positions visited by the tail when rope consists of {knots.Length} knots is {visited.Count}");
}

static void MoveHead(Point knot, char direction)
{
    // Update head
    if (direction == 'U')
    {
        knot.Y += 1;
    }
    else if (direction == 'D')
    {
        knot.Y -= 1;
    }
    else if (direction == 'R')
    {
        knot.X += 1;
    }
    else
    {
        knot.X -= 1;
    }
}

static bool MoveTail(Point head, Point tail)
{
    int xDiff = head.X - tail.X;
    int yDiff = head.Y - tail.Y;

    if (Math.Abs(xDiff) > 1 || Math.Abs(yDiff) > 1)
    {
        tail.X += Math.Sign(xDiff);
        tail.Y += Math.Sign(yDiff);
        return true;
    }

    return false;
}

record Point
{
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; set; }
    public int Y { get; set; }
}

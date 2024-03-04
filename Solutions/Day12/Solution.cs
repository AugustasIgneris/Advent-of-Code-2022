string[] input = File.ReadAllLines("Input.txt");

PartOne(input); // 449
PartTwo(input); // 443

static void PartOne(string[] input)
{
    Node start = Node.Default;
    Node end = Node.Default;

    for (int i = 0; i < input.Length; i++)
    {
        for (int j = 0; j < input[i].Length; j++)
        {
            if (input[i][j] == 'S')
            {
                start = new Node(j, i);
            }
            else if (input[i][j] == 'E')
            {
                end = new Node(j, i);
            }
        }
    }

    IEnumerable<Node> steps = AStart(input, start, end);
    Console.WriteLine($"Number of steps to reach the target location from {start} is {steps.Count() - 1}");
}

static void PartTwo(string[] input)
{
    var startNodes = new List<Node>();
    Node end = Node.Default;

    for (int i = 0; i < input.Length; i++)
    {
        for (int j = 0; j < input[i].Length; j++)
        {
            if (input[i][j] == 'S' || input[i][j] == 'a')
            {
                startNodes.Add(new Node(j, i));
            }
            else if (input[i][j] == 'E')
            {
                end = new Node(j, i);
            }
        }
    }

    // Certainly not optimal but the solution is long enough
    int fewestSteps = startNodes.Select((s) => AStart(input, s, end)).Select(p => p.Count() - 1).Where(p => p > 0).Min();
    Console.WriteLine($"Fewest number of steps to reach the target location is {fewestSteps}");
}

static IEnumerable<Node> AStart(string[] input, Node start, Node end)
{
    // The set of discovered nodes that may need to be (re-)expanded.
    var openSet = new HashSet<Node>
    {
        start
    };
    var openSetQueue = new PriorityQueue<Node, int>();
    openSetQueue.Enqueue(start, GetDistance(start, end));

    // For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from the start
    // to n currently known.
    var cameFrom = new Dictionary<Node, Node>();

    var gScore = new Dictionary<Node, int>
    {
        { start, 0 }
    };

    var fScore = new Dictionary<Node, int>
    {
        { start, GetDistance(start, end) }
    };

    while (openSetQueue.TryDequeue(out Node current, out _))
    {
        openSet.Remove(current);
        if (current == end)
        {
            return ReconstructPath(cameFrom, current);
        }

        foreach (Node neightbour in GetEligibleNeighbours(input, current, end))
        {
            int tentativeScore = gScore[current] + 1;

            if (!gScore.TryGetValue(neightbour, out int score) || score > tentativeScore)
            {
                cameFrom[neightbour] = current;
                gScore[neightbour] = tentativeScore;
                int estimate = tentativeScore + GetDistance(neightbour, end);
                fScore[neightbour] = estimate;

                if (!openSet.Contains(neightbour))
                {
                    openSetQueue.Enqueue(neightbour, estimate);
                    openSet.Add(neightbour);
                }
            }
        }
    }

    return [];
}

static IEnumerable<Node> GetEligibleNeighbours(string[] input, Node node, Node end)
{
    return GetNeighbours(input, node, end).Where(n => CanMove(input, node, n));
}

static bool CanMove(string[] input, Node current, Node neightbour)
{
    char currentCh = getHeight(input, current);
    char neightbourCh = getHeight(input, neightbour);
    return currentCh + 1 >= neightbourCh;
}

static char getHeight(string[] input, Node node)
{
    char ch = input[node.Y][node.X];
    if (ch == 'S')
    {
        ch = 'a';
    }
    else if (ch == 'E')
    {
        ch = 'z';
    }

    return ch;
}


static IEnumerable<Node> GetNeighbours(string[] input, Node node, Node end)
{
    if (node.X + 1 < input[0].Length)
    {
        yield return new Node(node.X + 1, node.Y);
    }

    if (node.Y + 1 < input.Length)
    {
        yield return new Node(node.X, node.Y + 1);
    }

    if (node.X - 1 >= 0)
    {
        yield return new Node(node.X - 1, node.Y);
    }

    if (node.Y - 1 >= 0)
    {
        yield return new Node(node.X, node.Y - 1);
    }
}

static IEnumerable<Node> ReconstructPath(Dictionary<Node, Node> cameFrom, Node current)
{
    var path = new List<Node>
    {
        current
    };

    while (cameFrom.TryGetValue(current, out Node previous))
    {
        current = previous;
        path.Add(current);
    }

    return path.Reverse<Node>();
}

static int GetDistance(Node node, Node end)
{
    return Math.Abs(end.X - node.X) + Math.Abs(end.Y - node.Y);
}

readonly record struct Node
{
    public static readonly Node Default = new Node(0, 0);

    public int X { get; }
    public int Y { get; }

    public Node(int x, int y) : this()
    {
        X = x;
        Y = y;
    }
}

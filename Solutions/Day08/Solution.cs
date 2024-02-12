string[] input = File.ReadAllLines("Input.txt");
List<Tree> visibilityList = GetVisibilityList(input).ToList();

PartOne(visibilityList); // Answer 1676
PartTwo(visibilityList); // Answer 313200

static void PartOne(List<Tree> visiblityList)
{
    int visibleTrees = visiblityList
        .Where(tree => tree.Visible.Any(v => v))
        .Count();

    Console.WriteLine($"The number of trees visible from outside the grid is {visibleTrees}");
}

static void PartTwo(List<Tree> visiblityList)
{
    int highestScenicScore = visiblityList
        .Max(tree => tree.VisibleTrees.Aggregate(1, (t, sum) => sum * t));
    Console.WriteLine($"The highest scenic score possible for any tree is {highestScenicScore}");
}

static IEnumerable<Tree> GetVisibilityList(string[] input)
{
    int width = input[0].Length;
    int height = input.Length;
    var grid = new Tree[height, width];

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            grid[y, x] = new Tree(int.Parse(input[y][x].ToString()));
        }
    }

    for (int x = 0; x < width; x++)
    {
        // Top to bottom
        int tallest = int.MinValue;
        var blockStack = new Stack<Tree>();
        for (int y = 0; y < height; y++)
        {
            ComputeVisibility(grid[y, x], ref tallest, blockStack, Tree.Direction.Up);
        }

        // Bottom to top
        tallest = int.MinValue;
        blockStack.Clear();
        for (int y = height - 1; y >= 0; y--)
        {
            ComputeVisibility(grid[y, x], ref tallest, blockStack, Tree.Direction.Down);
        }
    }

    for (int y = 0; y < height; y++)
    {
        // Left to right
        int tallest = int.MinValue;
        var blockStack = new Stack<Tree>();
        for (int x = 0; x < width; x++)
        {
            ComputeVisibility(grid[y, x], ref tallest, blockStack, Tree.Direction.Left);
        }

        // Right to left
        tallest = int.MinValue;
        blockStack.Clear();
        for (int x = width - 1; x >= 0; x--)
        {
            ComputeVisibility(grid[y, x], ref tallest, blockStack, Tree.Direction.Right);
        }
    }

    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            var tree = grid[y, x];
            yield return grid[y, x];
        }
    }

    static void ComputeVisibility(Tree tree, ref int tallest, Stack<Tree> blockStack, Tree.Direction direction)
    {
        // Simply compare to the tallest tree in the chosen direction
        if (tree.Height > tallest)
        {
            tree.Visible[(int)direction] = true;
            tallest = tree.Height;
        }

        // Keep a stack of trees that can block the current tree (A)
        // If the tree on the stack (B) cannot block it, then tree A can see all of trees B can see
        // Repeat until the stack is empty or tree A is blocked.
        // Since A was bigger than all of the trees it removed from the stack
        // We now only need to compare the next tree with A and the remainder of the stack.
        while (blockStack.TryPeek(out Tree? blockingTree))
        {
            if (blockingTree.Height < tree.Height)
            {
                tree.VisibleTrees[(int)direction] += blockStack.Pop().VisibleTrees[(int)direction];

                if (blockStack.Count == 0)
                {
                    // Include the very first tree
                    tree.VisibleTrees[(int)direction] += 1;
                }
            }
            else
            {
                tree.VisibleTrees[(int)direction] += 1;
                break;
            }

        }

        blockStack.Push(tree);
    }
}

record Tree
{
    public enum Direction {
        Up = 0,
        Down,
        Left,
        Right
    }

    public Tree(int height)
    {
        Height = height;
    }

    public int Height { get; }
    public bool[] Visible { get; } = new bool[4];
    public int[] VisibleTrees { get; } = new int[4];
}

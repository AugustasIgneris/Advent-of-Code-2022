string[] input = File.ReadAllLines("Input.txt");
List<Directory> sizes = GetDirectorySizes(input);

PartOne(sizes); // Answer 1444896
PartTwo(sizes); // Answer 404395

static void PartOne(List<Directory> directories)
{
    int totalSize = directories.Where(d => d.Size < 100000).Sum(d => d.Size);
    Console.WriteLine($"The total size of directories under size 100000 is: {totalSize}");
}

static void PartTwo(List<Directory> directories)
{
    Directory closest = directories[0];

    int spaceNeeded = 30000000 - (70000000 - directories[0].Size);

    foreach (var dir in directories)
    {
        if (dir.Size >= spaceNeeded && dir.Size < closest.Size)
        {
            closest = dir;
        }
    }

    Console.WriteLine($"The size of the smallest directory larger than {spaceNeeded} is {closest.Size}");
}


static List<Directory> GetDirectorySizes(string[] input)
{
    var directories = new List<Directory>();
    var dirStack = new Stack<Directory>();
    var cd = "cd".AsSpan();
    var root = "/".AsSpan();
    var popd = "..".AsSpan();
    var dir = "dir".AsSpan();
    var commandPrefix = "$".AsSpan();

    void PopDirectory()
    {
        Directory complete = dirStack.Pop();
        dirStack.Peek().Size += complete.Size;
    }

    foreach (string line in input)
    {
        var lineSpan = line.AsSpan();

        if (lineSpan.StartsWith(commandPrefix))
        {
            // Only cd and ls are supported
            // ls requires no action
            var command = lineSpan.Slice(2, 2);
            if (command.SequenceEqual(cd))
            {
                var arg = lineSpan.Slice(5);
                if (arg.SequenceEqual(root) && dirStack.Count > 0)
                {
                    while (dirStack.Count > 1)
                    {
                        PopDirectory();
                    }
                }
                else if (arg.SequenceEqual(popd))
                {
                    PopDirectory();
                }
                else
                {
                    var directory = new Directory(arg.ToString());
                    dirStack.Push(directory);
                    directories.Add(directory);
                }
            }
        }
        else if (lineSpan.StartsWith(dir))
        {
            continue;
        }
        else
        {
            int size = int.Parse(lineSpan.Slice(0, lineSpan.IndexOf(' ') + 1));
            dirStack.Peek().Size += size;
        }
    }

    while (dirStack.Count > 1)
    {
        PopDirectory();
    }

    return directories;
}


record Directory
{
    public Directory(string Name)
    {
        this.Name = Name;
    }

    public int Size { get; set; } = 0;
    public string Name { get; }
}

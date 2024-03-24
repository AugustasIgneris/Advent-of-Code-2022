string[] input = File.ReadAllLines("Input.txt");
PartOne(input); // 4560025
PartTwo(input); // 12480406634249

static void PartOne(string[] input)
{
    int row = 2000000;
    List<RowSpan> spans = [];
    Sensor[] sensors = Parse(input).ToArray();
    HashSet<Position> beaconsOnLine = [];

    foreach (Sensor sensor in sensors)
    {
        if (sensor.Beacon.Y == row)
        {
            beaconsOnLine.Add(sensor.Beacon);
        }

        if (sensor.Intercept(row) is RowSpan span)
        {
            spans.Add(span);
        }
    }

    spans.Sort();

    Stack<RowSpan> mergedSpans = new();

    for (int i = 0; i < spans.Count; i++)
    {
        RowSpan current = spans[i];

        if (mergedSpans.Count == 0)
        {
            mergedSpans.Push(spans[i]);
            continue;
        }

        RowSpan previous = mergedSpans.Peek();

        if (previous.End >= current.Start)
        {
            if (current.End > previous.End)
            {
                RowSpan newSpan = new (previous.Start, current.End);
                mergedSpans.Pop();
                mergedSpans.Push(newSpan);
            }
        }
        else
        {
            mergedSpans.Push(current);
        }
    }

    int total = mergedSpans.Sum(s => s.Length) - beaconsOnLine.Count;
    Console.WriteLine($"The number of positions on row {row} that cannot contain a beacon is {total}");
}

static void PartTwo(string[] input)
{
    int maxPos = 4000000;
    int minPos = 0;
    Sensor[] sensors = Parse(input).ToArray();
    Position? beaconPos = null;
    List<RowSpan> spans = [];

    for (int i = minPos; i <= maxPos && beaconPos == null; i++)
    {
        spans.Clear();

        foreach (Sensor sensor in sensors)
        {
            if (sensor.Intercept(i) is RowSpan span && span.Clamp(minPos, maxPos) is RowSpan clampedSpans)
            {
                spans.Add(clampedSpans);
            }
        }

        if (spans.Count < 1)
        {
            throw new InvalidDataException();
        }

        spans.Sort();

        RowSpan previous = spans[0];
        for (int j = 1; j < spans.Count; j++)
        {
            RowSpan current = spans[j];

            if (previous.End >= current.Start)
            {
                if (current.End > previous.End)
                {
                    previous = new(previous.Start, current.End);
                }
            }
            else
            {
                beaconPos = new Position(previous.End + 1, i);
                break;
            }
        }

        if ((previous.Start != minPos || previous.End != maxPos) && beaconPos == null)
        {
            throw new InvalidDataException();
        }
    }

    if (beaconPos == null)
    {
        throw new InvalidDataException();
    }

    long frequency =  (long)beaconPos.X * 4000000 + beaconPos.Y;
    Console.WriteLine($"The tuning frequency of the beacon is {frequency}");
}

static IEnumerable<Sensor> Parse(string[] input)
{
    foreach (string line in input)
    {
        ReadOnlySpan<char> span = line.AsSpan();
        int start = line.IndexOf('=') + 1;
        int end = line.IndexOf(',', start + 1);

        int sX = int.Parse(span.Slice(start, end - start));

        start = line.IndexOf('=', end) + 1;
        end = line.IndexOf(':', start + 1);

        int sY = int.Parse(span.Slice(start, end - start));

        start = line.IndexOf('=', end) + 1;
        end = line.IndexOf(',', start + 1);

        int bX = int.Parse(span.Slice(start, end - start));

        start = line.IndexOf('=', end) + 1;

        int bY = int.Parse(span.Slice(start));

        Position sensorPos = new(sX, sY);
        Position beaconPos = new(bX, bY);
        Sensor sensor = new(sensorPos, beaconPos);
        yield return sensor;
    }
}

record Position
{
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }
    public int Y { get; }
}

record Sensor
{
    public Sensor(Position pos, Position closestBeacon)
    {
        Pos = pos;
        Beacon = closestBeacon;
        DistanceToBeacon = GetDistance(pos, closestBeacon);
    }

    public Position Pos { get; }
    public Position Beacon { get; }
    public int DistanceToBeacon { get; }

    public RowSpan? Intercept(int row)
    {
        int distanceToRow = int.Abs(Pos.Y - row);
        int radius = DistanceToBeacon - distanceToRow;

        if (radius < 0)
        {
            return null;
        }

        return new RowSpan(Pos.X - radius, Pos.X + radius);
    }

    public static int GetDistance(Position pos1, Position pos2)
    {
        return int.Abs(pos1.X - pos2.X) + int.Abs(pos1.Y - pos2.Y);
    }
}

record RowSpan: IComparable<RowSpan>
{
    public RowSpan(int start, int end)
    {
        Start = start;
        End = end;
    }

    public int Start { get; }
    public int End { get; }
    public int Length
    {
        get
        {
            return End - Start + 1;
        }
    }

    public int CompareTo(RowSpan? other)
    {
        ArgumentNullException.ThrowIfNull(other);

        int ret = Start.CompareTo(other.Start);
        if (ret == 0)
        {
            ret = End.CompareTo(other.End);
        }

        return ret;
    }

    public RowSpan? Clamp(int min, int max)
    {
        if (Start < min && End < min)
        {
            return null;
        }

        if (Start > max && End > max)
        {
            return null;
        }

        int start = Math.Max(min, Start);
        int end = Math.Min(End, max);

        return new RowSpan(start, end);
    }
}


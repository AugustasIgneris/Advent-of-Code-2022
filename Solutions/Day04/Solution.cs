string[] input = File.ReadAllLines("Input.txt");

PartOne(input); // Answer 580
PartTwo(input); // Answer 895

static void PartOne(string[] input)
{
    int overlapping = 0;
    foreach (string line in input)
    {
        var pairs = line.Split(',');
        var pair1Sections = pairs[0].Split("-").Select(s => int.Parse(s)).ToArray();
        var pair2Sections = pairs[1].Split("-").Select(s => int.Parse(s)).ToArray();

        if ((pair1Sections[0] <= pair2Sections[0] && pair1Sections[1] >= pair2Sections[1]) ||
            (pair2Sections[0] <= pair1Sections[0] && pair2Sections[1] >= pair1Sections[1]))
        {
            overlapping++;
        }

    }

    Console.WriteLine($"Total number of pairs with completely overlapping sections are: {overlapping}");
}

static void PartTwo(string[] input)
{
    int overlapping = 0;
    foreach (string line in input)
    {
        var pairs = line.Split(',');
        var pair1Sections = pairs[0].Split("-").Select(s => int.Parse(s)).ToArray();
        var pair2Sections = pairs[1].Split("-").Select(s => int.Parse(s)).ToArray();

        if (IsSectionInRange(pair1Sections, pair2Sections[0]) || IsSectionInRange(pair1Sections, pair2Sections[1]) ||
            IsSectionInRange(pair2Sections, pair1Sections[0]) || IsSectionInRange(pair2Sections, pair1Sections[1]))
        {
            overlapping++;
        }

    }

    Console.WriteLine($"Total number of pairs with any overlapping sections are: {overlapping}");
}

static bool IsSectionInRange(int[] sectionRange, int section)
{
    return section >= sectionRange[0] && section <= sectionRange[1];
}

string[] input = File.ReadAllLines("Input.txt");

PartOne(input); // Answer 7691
PartTwo(input); // Answer 2508

static void PartOne(string[] input)
{
    int prioritySum = 0;
    var buckets = new int[52];

    for (int i = 0; i < input.Length; i++)
    {
        string line = input[i];

        int compartmentBoundary = line.Length / 2;

        // Flag to indicate that a certain item is contained in the first compartment of this rucksack
        int containsItem = i + 1;

        for (int j = 0; j < compartmentBoundary; j++)
        {
            int bucket = GetBucket(line[j]);
            buckets[bucket] = containsItem;
        }

        for (int j = compartmentBoundary; j < line.Length; j++)
        {
            int bucket = GetBucket(line[j]);
            if (buckets[bucket] == containsItem)
            {
                // Priority is 1 based.
                int priority = bucket + 1;
                prioritySum += priority;
                break;
            }
        }
    }

    Console.WriteLine($"The sum of the common compartment item priorities is {prioritySum}");
}

static void PartTwo(string[] input)
{
    int prioritySum = 0;
    var buckets = new int[52];

    for (int i = 0; i < input.Length; i++)
    {
        string line = input[i];

        // Flag to indicate that previous and current group members contains a given item
        int containsItem = i + 1;

        // Flag to indicate that the previous group members contains a given item.
        int previousContainItem = i;

        // The current group member. Groups consist of 3 elves.
        int groupMemberIndex = i % 3;

        for (int j = 0; j < line.Length; j++)
        {
            int bucket = GetBucket(line[j]);

            if (groupMemberIndex < 2)
            {
                // For the second member of the group we need to check that the previous member contains this item
                if (groupMemberIndex != 1 || buckets[bucket] == previousContainItem)
                {
                    buckets[bucket] = containsItem;
                }
            }
            else
            {
                // For the third member we only care of the previous members contain this item
                if (buckets[bucket] == previousContainItem)
                {
                    // Priority is 1 based.
                    int priority = bucket + 1;
                    prioritySum += priority;
                    break;
                }
            }
        }
    }

    Console.WriteLine($"The sum of the common group item priorities is {prioritySum}");
}

/**
 * Array index for the character based on priority 0 - 51.
 */
static int GetBucket(char c)
{
    if (c < 'a')
    {
        // A - Z == 26 - 51
        return 26 + c - 'A';
    } else
    {
        // a - z == 0 - 25
        return c - 'a';
    }
}
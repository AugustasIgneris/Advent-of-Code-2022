string[] input = File.ReadAllLines("Input.txt");

PartOne(input); // Answer 70296
PartTwo(input, 3); // Answer 205381

static void PartOne(string[] input) {
    int currentCallories = 0;
    int highestCalorries = 0;

    foreach (string line in input)
    {
        if (line == "")
        {
            if (highestCalorries < currentCallories)
            {
                highestCalorries = currentCallories;
            }

            currentCallories = 0;
        }
        else
        {
            currentCallories += int.Parse(line);
        }
    }

    Console.WriteLine($"The highest number of callories carried by an elf is: {highestCalorries}");
}

static void PartTwo(string[] input, int numberOfTopElves)
{
    int currentCallories = 0;
    var topCalorries = new PriorityQueue<int, int>(3);

    foreach (string line in input)
    {
        if (line == "")
        {
            if (topCalorries.Count < numberOfTopElves || topCalorries.Peek() < currentCallories)
            {
                topCalorries.Enqueue(currentCallories, currentCallories);
            }

            if (topCalorries.Count > numberOfTopElves)
            {
                topCalorries.Dequeue();
            }

            currentCallories = 0;
        }
        else
        {
            currentCallories += int.Parse(line);
        }
    }

    var totalTopCallories = topCalorries.UnorderedItems.Sum(e => e.Element);

    Console.WriteLine($"The total number of callories carried by the top {numberOfTopElves} is: {totalTopCallories}");
}

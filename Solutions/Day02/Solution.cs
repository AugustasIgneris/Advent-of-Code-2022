string[] input = File.ReadAllLines("Input.txt");

PartOne(input); // Answer 11767
PartTwo(input); // Answer 13886

static void PartOne(string[] input)
{
    int score = 0;

    foreach (string line in input)
    {
        char opponentPlay = line[0];
        char yourPlay = (char)('C' - ('Z' - line[2])); // Convert 'X', 'Y', 'Z' to 'A', 'B', 'C'

        if (opponentPlay == yourPlay)
        {
            // Draw
            score += 3;
        }
        else if (yourPlay == GetWinningPlay(opponentPlay))
        {
            // Win
            score += 6;
        }

        score += GetPlayScore(yourPlay);
    }

    Console.WriteLine($"Total part one score is: {score}");
}

static void PartTwo(string[] input)
{
    int score = 0;

    foreach (string line in input)
    {
        char opponentPlay = line[0];
        char strategy = line[2];

        if (strategy == 'X')
        {
            // Loss
            score += GetPlayScore(GetLosingPlay(opponentPlay));
        }
        else if (strategy == 'Y')
        {
            // Draw
            score += 3;
            score += GetPlayScore(opponentPlay);
        }
        else
        {
            // Win
            score += 6;
            score += GetPlayScore(GetWinningPlay(opponentPlay));
        }
    }

    Console.WriteLine($"Total part two score is: {score}");
}

static char GetWinningPlay(char opponentPlay)
{
    return opponentPlay switch
    {
        'A' => 'B',
        'B' => 'C',
        _ => 'A'
    };
}

static char GetLosingPlay(char opponentPlay)
{
    return opponentPlay switch
    {
        'A' => 'C',
        'B' => 'A',
        _ => 'B'
    };
}

static int GetPlayScore(char play)
{
    return play switch
    {
        'A' => 1,
        'B' => 2,
        _ => 3
    };
}
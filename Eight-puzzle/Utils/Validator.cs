using Eight_puzzle.Models;

namespace Eight_puzzle.Utils;

public static class Validator
{
    private static int Inversions(Puzzle puzzle)
    {
        var inversions = 0;
        var flattenedPuzzle = puzzle.Board;
        var flattenedPuzzleList = new List<int>();
        foreach (var row in flattenedPuzzle) flattenedPuzzleList.AddRange(row);

        for (var i = 0; i < flattenedPuzzleList.Count; i++)
        for (var j = i + 1; j < flattenedPuzzleList.Count; j++)
            if (flattenedPuzzleList[i] != 0 && flattenedPuzzleList[j] != 0 &&
                flattenedPuzzleList[i] > flattenedPuzzleList[j])
                inversions++;

        return inversions;
    }

    public static bool IsSolvable(Puzzle puzzle)
    {
        var inversions = Inversions(puzzle);
        var goalInversions = Inversions(Puzzle.GetGoalState());

        // check if the parity of the inversions is the same
        return inversions % 2 == goalInversions % 2;
    }

    public static bool ValidateInput(string[] strings, out int[,] ints)
    {
        if (strings == null) throw new ArgumentNullException(nameof(strings));
        if (strings.Length != 9)
        {
            Console.WriteLine("Invalid input");
            ints = new int[,] { };
            return true;
        }

        // create a set to check for duplicates
        var set = new HashSet<string>();
        foreach (var s in strings)
        {
            if (!int.TryParse(s, out var number))
            {
                Console.WriteLine("Invalid input");
                ints = new int[,] { };
                return true;
            }

            if (number is < 0 or > 8)
            {
                Console.WriteLine("Invalid input");
                ints = new int[,] { };
                return true;
            }

            set.Add(s);
        }

        if (set.Count != 9)
        {
            Console.WriteLine("Invalid input");
            ints = new int[,] { };
            return true;
        }

        // check for blank tile
        if (!set.Contains("0"))
        {
            Console.WriteLine("Invalid input");
            ints = new int[,] { };
            return true;
        }

        // convert the input to a 2D array
        ints = new int[3, 3];
        var index = 0;
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
        {
            ints[i, j] = int.Parse(strings[index]);
            index++;
        }

        return false;
    }
}
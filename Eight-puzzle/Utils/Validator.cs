namespace Eight_puzzle.Utils;

public static class Validator
{
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
        {
            for (var j = 0; j < 3; j++)
            {
                ints[i, j] = int.Parse(strings[index]);
                index++;
            }
        }
    
        return false;
    }
}
using System.IO.Hashing;
using System.Text;

namespace Numberlink_puzzle.Model;

public class Puzzle
{
    public Puzzle(int rows, int columns, int[,] grid)
    {
        Rows = rows;
        Columns = columns;
        Grid = grid;
        PathsCount = CountPaths();
    }

    public int Rows { get; set; }
    public int Columns { get; set; }
    public int[,] Grid { get; set; }
    public int PathsCount { get; set; }

    // Counts number of paths in the puzzle
    private int CountPaths()
    {
        var count = 0;
        for (var i = 0; i < Rows; i++)
        for (var j = 0; j < Columns; j++)
            if (Grid[i, j] > 0)
                count++;

        count /= 2;

        return count;
    }
    
    
    // checks if the puzzle is solved
    public bool IsSolved()
    {
        
        // checks if all the paths can still be connected
        var maxNumber = Grid.Cast<int>().Max();

        for (int number = 1; number <= maxNumber; number++)
        {
            var cellsWithNumber = new List<(int, int)>();
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (Grid[i, j] == number)
                    {
                        cellsWithNumber.Add((i, j));
                    }
                }
            }

            // this rule was initially in assignment 1, looks like its optional
            // if (cellsWithNumber.Count < 2)
            // {
            //     return false;
            // }
            
            
            // check if path is valid for each pair of cells
            for (int i = 0; i < cellsWithNumber.Count - 1; i++)
            {
                for (int j = i + 1; j < cellsWithNumber.Count; j++)
                {
                    if (!IsPathValid(cellsWithNumber[i], cellsWithNumber[j]))
                    {
                        return false;
                    }
                }
            }
        }

        return true;
    }
    
    // checks if the path between two cells is valid
    private bool IsPathValid((int, int) start, (int, int) end)
    {
        var visited = new bool[Rows, Columns];

        var queue = new Queue<(int, int)>();
        queue.Enqueue(start);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current == end)
            {
                return true;
            }

            visited[current.Item1, current.Item2] = true;

            var neighbors = GetNeighboringCells(current, Rows, Columns);
            foreach (var neighbor in neighbors)
            {
                if (Grid[neighbor.Item1, neighbor.Item2] == Grid[start.Item1, start.Item2] &&
                    !visited[neighbor.Item1, neighbor.Item2])
                {
                    queue.Enqueue(neighbor);
                }
            }
        }

        return false;
    }
    
    // gets neighboring cells
    private List<(int, int)> GetNeighboringCells((int, int) cell, int rows, int columns)
    {
        var result = new List<(int, int)>();

        if (cell.Item1 > 0)
        {
            result.Add((cell.Item1 - 1, cell.Item2));
        }

        if (cell.Item1 < rows - 1)
        {
            result.Add((cell.Item1 + 1, cell.Item2));
        }

        if (cell.Item2 > 0)
        {
            result.Add((cell.Item1, cell.Item2 - 1));
        }

        if (cell.Item2 < columns - 1)
        {
            result.Add((cell.Item1, cell.Item2 + 1));
        }

        return result;
    }

    // checks to see if the initial puzzle given (by input) is valid
    public bool IsValidFirstPuzzle()
    {
        // if there are no paths, the puzzle is not valid
        if (PathsCount == 0)
            return false;

        // if there are duplicates, the puzzle is not valid
        if (HasDuplicates())
            return false;

        // check if any number is skipped
        var numbers = new List<int>();
        for (var i = 0; i < Rows; i++)
        for (var j = 0; j < Columns; j++)
            if (Grid[i, j] > 0)
                numbers.Add(Grid[i, j]);

        numbers.Sort();
        for (var i = 1; i < numbers.Count; i++)
            if (numbers[i] - numbers[i - 1] > 1)
                return false;

        return true;
    }
    
    // checks to see if the puzzle has duplicates - (more than 2 numbers equal to each other on the board)
    // (used only for the initial puzzle given by input)
    private bool HasDuplicates()
    {
        // count the number of times each number appears
        var numbers = new Dictionary<int, int>();
        for (var i = 0; i < Rows; i++)
        for (var j = 0; j < Columns; j++)
            if (Grid[i, j] > 0)
            {
                if (numbers.ContainsKey(Grid[i, j]))
                    numbers[Grid[i, j]]++;
                else
                    numbers.Add(Grid[i, j], 1);
            }
        // if any number appears more than twice, the puzzle has duplicates
        return numbers.Where(number => number.Key != 0).Any(number => number.Value > 2);
    }

    public List<Puzzle> GetSuccessorStates()
    {
        var successorStates = new List<Puzzle>();
        
        // for each possible path/link, find the first cell with blanks, then attempt to move in each direction
        for (var i = 0; i < PathsCount; i++)
        {
            // find first cell with blanks
            var firstCell = FindFirstCellWithBlanks(i + 1);
            if (firstCell.Item1 == -1)
                continue;
            // check for all blanks around the cell
            var blanks = FindBlanksAroundCell(firstCell.Item1, firstCell.Item2);
            // for each blank, create a new puzzle with the current path number in the blank
            foreach (var blank in blanks)
            {
                var newBoard = Grid.Clone() as int[,] ?? throw new InvalidOperationException();
                newBoard[blank.Item1, blank.Item2] = i + 1;
                successorStates.Add(new Puzzle(Rows, Columns, newBoard));
            }
        }

        return successorStates;
    }

    // find all blanks around a cell and return list of the coordinates for the blanks
    private List<(int, int)> FindBlanksAroundCell(int x, int y)
    {
        var blanks = new List<(int, int)>();
        if (x > 0 && Grid[x - 1, y] == 0)
            blanks.Add((x - 1, y));
        if (x < Rows - 1 && Grid[x + 1, y] == 0)
            blanks.Add((x + 1, y));
        if (y > 0 && Grid[x, y - 1] == 0)
            blanks.Add((x, y - 1));
        if (y < Columns - 1 && Grid[x, y + 1] == 0)
            blanks.Add((x, y + 1));

        return blanks;
    }

    // find the first cell with blanks
    private (int, int) FindFirstCellWithBlanks(int i)
    {
        for (var x = 0; x < Rows; x++)
        for (var y = 0; y < Columns; y++)
            if (Grid[x, y] == i)
            {
                if (x > 0 && Grid[x - 1, y] == 0)
                    return (x, y);
                if (x < Rows - 1 && Grid[x + 1, y] == 0)
                    return (x, y);
                if (y > 0 && Grid[x, y - 1] == 0)
                    return (x, y);
                if (y < Columns - 1 && Grid[x, y + 1] == 0)
                    return (x, y);
            }

        return (-1, -1);
    }
    
    // prints the puzzle
    public override string ToString()
    {
        var sb = new StringBuilder();
        
        // print the puzzle
        // Ex:
        // 1 2 3
        // 4 5 6
        // 7 8 0
        
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++) sb.Append(Grid[i, j] + " ");
            sb.Append(Environment.NewLine);
        }

        return sb.ToString();
    }
    
    
    // allows us to compare puzzles (without taking irrelevant properties into account)
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Puzzle)obj;
        var otherStr = other.ToString();

        return otherStr.Equals(ToString());
    }
    
    // override for the Equals method (just in case we need it)
    protected bool Equals(Puzzle other)
    {
        return other.ToString().Equals(ToString());
    }
    
    // fixes the sets of puzzles not being able to be compared correctly 
    public override int GetHashCode()
    {
        var crc = new Crc32();
        crc.Append(Encoding.ASCII.GetBytes(ToString()));
        var hash = crc.GetCurrentHash();
        var i = BitConverter.ToInt32(hash, 0);

        return i;
    }
    
    // wrapper method for getting the successor states
    public IEnumerable<Puzzle> GetChildren()
    {
        return GetSuccessorStates();
    }
}
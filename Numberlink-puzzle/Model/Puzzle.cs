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
    public Puzzle? Parent { get; set; }

    private int CountPaths()
    {
        var count = 0;
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                if (Grid[i, j] > 0)
                {
                    count++;
                }
            }
        }
        
        count /= 2;
        
        return count;
    }

    public bool IsSolved()
    {
        // Check if there are any 0s in the grid
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                if (Grid[i, j] == 0)
                {
                    return false;
                }
            }
        }

        // Check if all paths are connected
        var visited = new bool[Rows, Columns];
        var pathCount = 0;

        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                if (Grid[i, j] > 0 && !visited[i, j])
                {
                    pathCount++;
                    DFS(i, j, visited);
                }
            }
        }

        return pathCount == PathsCount;
    }

    private void DFS(int i, int j, bool[,] visited)
    {
        visited[i, j] = true;

        // Check adjacent cells in horizontal or vertical direction
        for (var dx = -1; dx <= 1; dx++)
        {
            for (var dy = -1; dy <= 1; dy++)
            {
                if (dx != 0 && dy != 0)
                {
                    continue;
                }

                var newX = i + dx;
                var newY = j + dy;

                if (newX >= 0 && newX < Rows && newY >= 0 && newY < Columns && Grid[newX, newY] == Grid[i, j] && !visited[newX, newY])
                {
                    DFS(newX, newY, visited);
                }
            }
        }
    }


    public bool IsValidFirstPuzzle()
    {
        // if the number of paths is odd, the puzzle is not valid
        if (PathsCount % 2 != 0)
            return false;
        
        // if there are no paths, the puzzle is not valid
        if (PathsCount == 0)
            return false;
        
        // if there are duplicates, the puzzle is not valid
        if (HasDuplicates())
            return false;
        
        // check if any number is skipped
        var numbers = new List<int>();
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                if (Grid[i, j] > 0)
                {
                    numbers.Add(Grid[i, j]);
                }
            }
        }
        
        numbers.Sort();
        for (var i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] - numbers[i - 1] > 1)
                return false;
        }

        return true;

    }

    private bool HasDuplicates()
    {
        var numbers = new Dictionary<int, int>();
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                if (Grid[i, j] > 0)
                {
                    if (numbers.ContainsKey(Grid[i, j]))
                    {
                        numbers[Grid[i, j]]++;
                    }
                    else
                    {
                        numbers.Add(Grid[i, j], 1);
                    }
                }
            }
        }
        
        foreach (var number in numbers)
        {
            if (number.Key == 0)
                continue;
            if (number.Value > 2)
                return true;
        }
        
        return false;

    }
    
    public List<Puzzle> GetSuccessorStates()
    {
        var successorStates = new List<Puzzle>();
    
        // Iterate through the grid
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                var number = Grid[i, j];

                // If the cell does not have a number or the number has already been processed, continue to the next cell
                if (number == 0 || successorStates.Any(state => state.Grid[i, j] == number))
                {
                    continue;
                }

                // Attempt to add the same number next to it (up, down, left, right)
                for (var dx = -1; dx <= 1; dx++)
                {
                    for (var dy = -1; dy <= 1; dy++)
                    {
                        if (dx != 0 && dy != 0)
                        {
                            continue; // Only add the same number in horizontal or vertical direction, not diagonal
                        }

                        var newX = i + dx;
                        var newY = j + dy;

                        // Check if the new position is within the puzzle boundaries and empty
                        if (newX >= 0 && newX < Rows && newY >= 0 && newY < Columns && Grid[newX, newY] == 0)
                        {
                            // Create a new puzzle with the same number added to the new position
                            var newGrid = (int[,])Grid.Clone();
                            newGrid[newX, newY] = number;
                            var newPuzzle = new Puzzle(Rows, Columns, newGrid);

                            // Add the new puzzle to the successor states list
                            successorStates.Add(newPuzzle);
                        }
                    }
                }
            }
        }

        return successorStates;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var i = 0; i < Rows; i++)
        {
            for (var j = 0; j < Columns; j++)
            {
                sb.Append(Grid[i, j] + " ");
            }
            sb.Append(Environment.NewLine);
        }

        return sb.ToString();
    }

    public IEnumerable<Puzzle> GetChildren()
    {
        return GetSuccessorStates();
    }
}
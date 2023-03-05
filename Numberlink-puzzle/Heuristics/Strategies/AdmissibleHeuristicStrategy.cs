using Numberlink_puzzle.Heuristics.Interfaces;
using Numberlink_puzzle.Model;

namespace Numberlink_puzzle.Heuristics.Strategies;

public class AdmissibleHeuristicStrategy : IHeuristicStrategy
{
    public int GetHeuristicValue(Puzzle puzzle)
    {
        // calculate the number of zeros in the puzzle
        var zeros = 0;
        for (var i = 0; i < puzzle.Rows; i++)
        for (var j = 0; j < puzzle.Columns; j++)
            if (puzzle.Grid[i, j] == 0)
                zeros++;
        
        return IsRuleViolated(puzzle) ? int.MaxValue : zeros;
    }

    private bool IsRuleViolated(Puzzle puzzle)
    {
        // check if all the paths can still be connected
        for (int i = 0; i < puzzle.PathsCount; i++)
        {
            if (!CanBeStillConnected(puzzle, i + 1))
            {
                return true;
            }
        }

        return false;
    }

    private bool CanBeStillConnected(Puzzle puzzle, int target)
    {
        var tempGrid = puzzle.Grid.Clone() as int[,] ?? throw new InvalidOperationException();

        // Find all cells with the target value
        var cellsWithTarget = new List<(int, int)>();
        for (int i = 0; i < puzzle.Rows; i++)
        {
            for (int j = 0; j < puzzle.Columns; j++)
            {
                if (tempGrid[i, j] == target)
                {
                    cellsWithTarget.Add((i, j));
                }
            }
        }

        // For each cell with the target value, check if it is connected to any other cell with the target value
        while (cellsWithTarget.Count > 0)
        {
            var cell = cellsWithTarget[0];
            cellsWithTarget.RemoveAt(0);

            // Check if the cell is connected to any neighboring cells with the same number
            if (!IsCellConnected(tempGrid, cell))
            {
                return false;
            }

            // Update cellsWithTarget to remove cells that have already been checked
            cellsWithTarget.RemoveAll(c => IsCellConnected(tempGrid, c));
        }

        return true;
    }

    
    private List<(int, int)> GetNeighboringCells(int[,] grid, (int, int) cell)
    {
        var rows = grid.GetLength(0);
        var cols = grid.GetLength(1);

        var neighboringCells = new List<(int, int)>();
        if (cell.Item1 > 0)
            neighboringCells.Add((cell.Item1 - 1, cell.Item2));
        if (cell.Item2 > 0)
            neighboringCells.Add((cell.Item1, cell.Item2 - 1));
        if (cell.Item1 < rows - 1)
            neighboringCells.Add((cell.Item1 + 1, cell.Item2));
        if (cell.Item2 < cols - 1)
            neighboringCells.Add((cell.Item1, cell.Item2 + 1));

        return neighboringCells;
    }


    private bool IsCellConnected(int[,] grid, (int, int) cell)
    {
        var neighbors = GetNeighboringCells(grid, cell);
        foreach (var neighbor in neighbors)
        {
            if (grid[neighbor.Item1, neighbor.Item2] == grid[cell.Item1, cell.Item2] || grid[neighbor.Item1, neighbor.Item2] == 0)
            {
                return true;
            }
        }

        return false;
    }


}
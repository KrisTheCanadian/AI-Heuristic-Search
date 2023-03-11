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
        for (var i = 0; i < puzzle.PathsCount; i++)
            if (!CanBeStillConnected(puzzle, i + 1))
                return true;

        return false;
    }

    private bool CanBeStillConnected(Puzzle puzzle, int target)
    {
        // get first cell with the target value
        var firstCell = GetFirstCellWithValue(puzzle.Grid, target);

        // if the target value is not found, return true
        if (firstCell.Item1 == -1) return true;

        // do dfs to check if all the cells can be reached are equal to target or 0
        var visited = new bool[puzzle.Rows, puzzle.Columns];
        var stack = new Stack<(int, int)>();
        stack.Push(firstCell);
        visited[firstCell.Item1, firstCell.Item2] = true;

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            var neighboringCellsOfCurrent = GetNeighboringCells(puzzle.Grid, current);

            foreach (var cell in neighboringCellsOfCurrent)
            {
                if (visited[cell.Item1, cell.Item2]) continue;

                if (puzzle.Grid[cell.Item1, cell.Item2] != target && puzzle.Grid[cell.Item1, cell.Item2] != 0) continue;

                visited[cell.Item1, cell.Item2] = true;
                stack.Push(cell);
            }
        }

        // get all the cells with the target value
        var cellsWithTargetValue = new List<(int, int)>();
        for (var i = 0; i < puzzle.Rows; i++)
        for (var j = 0; j < puzzle.Columns; j++)
            if (puzzle.Grid[i, j] == target)
                cellsWithTargetValue.Add((i, j));

        // check if all the cells with the target value are visited
        foreach (var cell in cellsWithTargetValue)
            if (!visited[cell.Item1, cell.Item2])
                return false;
        return true;
    }

    private (int, int) GetFirstCellWithValue(int[,] puzzleGrid, int target)
    {
        for (var i = 0; i < puzzleGrid.GetLength(0); i++)
        for (var j = 0; j < puzzleGrid.GetLength(1); j++)
            if (puzzleGrid[i, j] == target)
                return (i, j);

        return (-1, -1);
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
}
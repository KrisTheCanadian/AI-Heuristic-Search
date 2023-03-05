using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics.Interfaces;

namespace Eight_puzzle.Utils.Heuristics.Strategies;

public class ManhattanDistanceStrategy : IHeuristicStrategy
{
    public int GetHeuristicValue(Puzzle puzzle)
    {
        var goalState = Puzzle.GetGoalState();
        var distance = 0;

        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
        {
            var value = puzzle.Board[i][j];
            if (value == 0) continue;

            var (x, y) = goalState.GetTileCoordinates(value);
            distance += Math.Abs(i - x) + Math.Abs(j - y);
        }

        return distance;
    }
}
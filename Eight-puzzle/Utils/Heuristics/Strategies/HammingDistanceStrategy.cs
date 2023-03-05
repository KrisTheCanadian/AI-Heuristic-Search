using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics.Interfaces;

namespace Eight_puzzle.Utils.Heuristics.Strategies;

public class HammingDistanceStrategy : IHeuristicStrategy
{
    public int GetHeuristicValue(Puzzle puzzle)
    {
        var distance = 0;
        var goalState = Puzzle.GetGoalState();
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
            if (puzzle.Board[i][j] != goalState.Board[i][j])
                distance++;
        return distance;
    }
}
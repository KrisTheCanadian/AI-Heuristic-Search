using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics.Interfaces;

namespace Eight_puzzle.Utils.Heuristics.Strategies;

public class HammingDistanceStrategy : IHeuristicStrategy
{
    public int GetHeuristicValue(Puzzle puzzle)
    {
        var distance = 0;
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                var value = puzzle.Board[i][j];
                if (value == 0) continue;
                var goalRow = (value - 1) / 3;
                var goalCol = (value - 1) % 3;
                if (i != goalRow || j != goalCol) distance++;
            }
        }
        return distance;
    }
}
using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics.Interfaces;

namespace Eight_puzzle.Utils.Heuristics.Strategies;

public class PermutationInversionStrategy: IHeuristicStrategy
{
    public int GetHeuristicValue(Puzzle puzzle)
    {
        var distance = 0;
        var permutation = new List<int>();
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                var value = puzzle.Board[i][j];
                if (value == 0) continue;
                permutation.Add(value);
            }
        }
        for (var i = 0; i < permutation.Count; i++)
        {
            for (var j = i + 1; j < permutation.Count; j++)
            {
                if (permutation[i] > permutation[j]) distance++;
            }
        }
        return distance;
    }
}
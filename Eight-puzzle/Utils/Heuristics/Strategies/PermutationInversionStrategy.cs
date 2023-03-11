using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics.Interfaces;

namespace Eight_puzzle.Utils.Heuristics.Strategies;

public class PermutationInversionStrategy : IHeuristicStrategy
{
    public int GetHeuristicValue(Puzzle puzzle)
    {
        var state2d = puzzle.Board;

        // Convert 2d array to 1d array
        var state = state2d.SelectMany(x => x).ToArray();

        var goalState2d = Puzzle.GetGoalState().Board;

        // Convert 2d array to 1d array
        var goalState = goalState2d.SelectMany(x => x).ToArray();


        var count = 0;

        // Count the number of inversions
        for (var i = 0; i < 9; i++)
        for (var j = i + 1; j < 9; j++)
            if (state[j] != 0 && state[i] != 0)
                if (Array.IndexOf(goalState, state[i]) > Array.IndexOf(goalState, state[j]))
                    count++;

        return count;
    }
}
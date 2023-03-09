using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics.Interfaces;

namespace Eight_puzzle.Utils.Heuristics.Strategies;

public class DoubleManhattanDistanceStrategy : IHeuristicStrategy
{
    public int GetHeuristicValue(Puzzle puzzle)
    {
        // non-admissible heuristic from assignment 1
        // create a new manhattan distance strategy, get the heuristic and multiply the result by 2
        var manhattan = new ManhattanDistanceStrategy().GetHeuristicValue(puzzle);
        return manhattan * 2;
    }
}
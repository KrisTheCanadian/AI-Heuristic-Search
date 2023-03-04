using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics.Interfaces;

namespace Eight_puzzle.Utils.Heuristics.Strategies;

public class DoubleManhattanDistanceStrategy : IHeuristicStrategy
{
    public int GetHeuristicValue(Puzzle puzzle)
    {
        var manhattan = new ManhattanDistanceStrategy().GetHeuristicValue(puzzle);
        return manhattan * 2;
    }
}

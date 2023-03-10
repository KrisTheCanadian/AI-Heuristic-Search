using Numberlink_puzzle.Heuristics.Interfaces;
using Numberlink_puzzle.Model;

namespace Numberlink_puzzle.Heuristics.Strategies;

public class NonAdmissibleHeuristicStrategy : IHeuristicStrategy
{
    public int GetHeuristicValue(Puzzle puzzle)
    {
        IHeuristicStrategy strategy = new AdmissibleHeuristicStrategy();
        var value = strategy.GetHeuristicValue(puzzle);
        if(value == int.MaxValue) { return int.MaxValue; }
        return value * 2;
    }
}
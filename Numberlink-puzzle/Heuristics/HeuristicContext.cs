using Numberlink_puzzle.Heuristics.Interfaces;
using Numberlink_puzzle.Model;

namespace Numberlink_puzzle.Heuristics;

public class HeuristicContext
{
    private IHeuristicStrategy HeuristicStrategy { get; }
    
    public HeuristicContext(IHeuristicStrategy heuristicStrategy)
    {
        HeuristicStrategy = heuristicStrategy;
    }

    public int GetHeuristicValue(Puzzle puzzle)
    {
        return HeuristicStrategy.GetHeuristicValue(puzzle);
    }
}
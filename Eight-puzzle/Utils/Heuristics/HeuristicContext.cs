using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics.Interfaces;

namespace Eight_puzzle.Utils.Heuristics;

public class HeuristicContext
{
    public HeuristicContext(IHeuristicStrategy heuristicStrategy)
    {
        HeuristicStrategy = heuristicStrategy;
    }

    private IHeuristicStrategy HeuristicStrategy { get; }

    public int GetHeuristicValue(Puzzle puzzle)
    {
        return HeuristicStrategy.GetHeuristicValue(puzzle);
    }
}
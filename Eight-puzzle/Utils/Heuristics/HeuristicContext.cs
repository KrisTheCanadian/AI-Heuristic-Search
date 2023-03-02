using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics.Interfaces;

namespace Eight_puzzle.Utils.Heuristics;

public class HeuristicContext
{
    private IHeuristicStrategy HeuristicStrategy { get; set; }
    
    public HeuristicContext(IHeuristicStrategy heuristicStrategy)
    {
        HeuristicStrategy = heuristicStrategy;
    }
    
    public int GetHeuristicValue(Puzzle puzzle)
    {
        return HeuristicStrategy.GetHeuristicValue(puzzle);
    }
}
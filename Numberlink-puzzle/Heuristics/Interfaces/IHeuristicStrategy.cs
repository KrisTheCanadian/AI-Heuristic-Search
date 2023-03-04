using Numberlink_puzzle.Model;

namespace Numberlink_puzzle.Heuristics.Interfaces;

public interface IHeuristicStrategy
{
    int GetHeuristicValue(Puzzle puzzle);
}
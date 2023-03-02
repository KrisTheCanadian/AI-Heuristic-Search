using Eight_puzzle.Models;

namespace Eight_puzzle.Utils.Heuristics.Interfaces;

public interface IHeuristicStrategy
{
    int GetHeuristicValue(Puzzle puzzle);
}
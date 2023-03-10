using Numberlink_puzzle.Model;

namespace Numberlink_puzzle.Search.Interfaces;

public interface ISearchStrategy
{
    public long ExpandedNodes { get; set; }
    Puzzle? Search(Puzzle puzzle);
}
using Numberlink_puzzle.Model;

namespace Numberlink_puzzle.Search.Interfaces;

public interface ISearchStrategy
{
    Puzzle? Search(Puzzle puzzle);
}
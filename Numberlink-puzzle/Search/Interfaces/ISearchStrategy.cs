using Numberlink_puzzle.Model;

namespace Numberlink_puzzle.Search.Interfaces;

public interface ISearchStrategy
{
    List<Puzzle> Search(Puzzle puzzle);
}
using Eight_puzzle.Models;

namespace Eight_puzzle.Utils.Search.Interfaces;

public interface ISearchStrategy
{
    List<Puzzle> Search(Puzzle puzzle);
}
using Eight_puzzle.Models;

namespace Eight_puzzle.Utils.Search.Interfaces;

public interface ISearchStrategy
{
    long NodesExpanded { get; set; }
    List<Puzzle> Search(Puzzle puzzle);
}
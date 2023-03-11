using Eight_puzzle.Models;
using Eight_puzzle.Utils.Search.Interfaces;

namespace Eight_puzzle.Utils.Search;

public class SearchContext
{
    public SearchContext(ISearchStrategy searchStrategy)
    {
        SearchStrategy = searchStrategy;
    }

    private ISearchStrategy SearchStrategy { get; }

    public List<Puzzle> Search(Puzzle puzzle)
    {
        return SearchStrategy.Search(puzzle);
    }

    public long GetNodesExpanded()
    {
        return SearchStrategy.NodesExpanded;
    }
}
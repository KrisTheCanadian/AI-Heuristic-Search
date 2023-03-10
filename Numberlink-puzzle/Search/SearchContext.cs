using Numberlink_puzzle.Model;
using Numberlink_puzzle.Search.Interfaces;

namespace Numberlink_puzzle.Search;

public class SearchContext
{
    public SearchContext(ISearchStrategy searchStrategy)
    {
        SearchStrategy = searchStrategy;
    }

    private ISearchStrategy SearchStrategy { get; }

    public Puzzle? Search(Puzzle puzzle)
    {
        return SearchStrategy.Search(puzzle);
    }

    public long GetExpandedNodes()
    {
        return SearchStrategy.ExpandedNodes;
    }
}
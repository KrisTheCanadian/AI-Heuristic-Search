using Numberlink_puzzle.Model;
using Numberlink_puzzle.Search.Interfaces;

namespace Numberlink_puzzle.Search;

public class SearchContext
{
    private ISearchStrategy SearchStrategy { get; set; }
    
    public SearchContext(ISearchStrategy searchStrategy)
    {
        SearchStrategy = searchStrategy;
    }
    
    public List<Puzzle> Search(Puzzle puzzle)
    {
        return SearchStrategy.Search(puzzle);
    }
}
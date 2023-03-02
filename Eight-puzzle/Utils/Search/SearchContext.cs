using Eight_puzzle.Models;
using Eight_puzzle.Utils.Search.Interfaces;

namespace Eight_puzzle.Utils.Search;

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
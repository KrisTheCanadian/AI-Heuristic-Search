using Numberlink_puzzle.Heuristics;
using Numberlink_puzzle.Model;
using Numberlink_puzzle.Search.Interfaces;

namespace Numberlink_puzzle.Search.Strategies;

public class BestFirstSearch : ISearchStrategy
{
    private readonly HeuristicContext _heuristicContext;

    public BestFirstSearch(HeuristicContext heuristicContext)
    {
        _heuristicContext = heuristicContext;
    }

    public Puzzle? Search(Puzzle puzzle)
    {
        if (puzzle.IsSolved()) return puzzle;

        var openList = new PriorityQueue<Puzzle, int>();
        var closedList = new HashSet<Puzzle>();

        openList.Enqueue(puzzle, _heuristicContext.GetHeuristicValue(puzzle));

        while (openList.Count > 0)
        {
            var current = openList.Dequeue();

            if (current.IsSolved()) { return current; }

            var children = current.GetChildren();

            foreach (var child in children)
            {
                if (closedList.Contains(child)) continue;
                if (openList.UnorderedItems.Any(x => x.Element.Equals(child))) continue;
                var heuristic = _heuristicContext.GetHeuristicValue(child);
                if(heuristic == int.MaxValue) continue; // avoid integer overflow
                openList.Enqueue(child, heuristic);
                child.Parent = current;
            }

            closedList.Add(current);
        }

        return null;
    }

}
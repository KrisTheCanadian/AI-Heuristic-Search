using Numberlink_puzzle.Heuristics;
using Numberlink_puzzle.Model;
using Numberlink_puzzle.Search.Interfaces;

namespace Numberlink_puzzle.Search.Strategies;

public class AStarSearch : ISearchStrategy
{
    private readonly HeuristicContext _heuristicContext;

    public AStarSearch(HeuristicContext heuristicContext)
    {
        _heuristicContext = heuristicContext;
    }

    public Puzzle? Search(Puzzle puzzle)
    {
        if (puzzle.IsSolved()) return puzzle;

        var frontier = new PriorityQueue<Puzzle, int>();
        frontier.Enqueue(puzzle, _heuristicContext.GetHeuristicValue(puzzle));

        var costSoFar = new Dictionary<Puzzle, int>();
        costSoFar[puzzle] = _heuristicContext.GetHeuristicValue(puzzle);

        while (frontier.Count > 0)
        {
            var current = frontier.Dequeue();

            if (current.IsSolved()) { return current; }

            var children = current.GetChildren();
            foreach (var child in children)
            {
                var newCost = costSoFar[current] + 1;
                if (costSoFar.ContainsKey(child) && newCost >= costSoFar[child]) continue;
                costSoFar[child] = newCost;
                var heuristic = _heuristicContext.GetHeuristicValue(child);
                if(heuristic == int.MaxValue) continue; // avoid integer overflow
                frontier.Enqueue(child, newCost + heuristic);
                child.Parent = current;
            }
        }

        return null;
    }
    
}
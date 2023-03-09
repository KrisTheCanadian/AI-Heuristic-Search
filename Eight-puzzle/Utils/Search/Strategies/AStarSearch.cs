using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics;
using Eight_puzzle.Utils.Search.Interfaces;

namespace Eight_puzzle.Utils.Search.Strategies;

public class AStarSearch : ISearchStrategy
{
    private readonly HeuristicContext _heuristicContext;

    public AStarSearch(HeuristicContext heuristicContext)
    {
        _heuristicContext = heuristicContext;
    }

    public List<Puzzle> Search(Puzzle puzzle)
    {
        var goalState = Puzzle.GetGoalState();
        
        // If the puzzle is already solved, return an empty list
        if (puzzle.Equals(goalState)) return new List<Puzzle> { puzzle };

        // Create a priority queue to store the frontier, and a dictionary to store the cameFrom and costSoFar
        var frontier = new PriorityQueue<Puzzle, int>();
        var cameFrom = new Dictionary<Puzzle, Puzzle?>();
        var costSoFar = new Dictionary<Puzzle, int>();
        
        // Add the initial puzzle to the frontier
        frontier.Enqueue(puzzle, _heuristicContext.GetHeuristicValue(puzzle));
        cameFrom[puzzle] = null;
        costSoFar[puzzle] = _heuristicContext.GetHeuristicValue(puzzle);

        while (frontier.Count > 0)
        {
            // Get the puzzle with the lowest cost (f(n) = g(n) + h(n))
            var current = frontier.Dequeue();

            // If the current puzzle is the goal state, return the path
            if (current.Equals(goalState))
            {
                // reconstruct the path
                var path = new List<Puzzle> { current };
                while (current != null && cameFrom.ContainsKey(current))
                {
                    current = cameFrom[current];
                    if (current != null) path.Insert(0, current);
                }

                return path;
            }
            
            
            // Get the children of the current puzzle
            var children = current.GetChildren();
            
            // Loop through the children
            foreach (var child in children)
            {
                // calculate f(n) = g(n) + h(n) , where g(n) is the costSoFar of the current puzzle (the number of moves it took to get to the current puzzle)
                var childHeuristicValue = _heuristicContext.GetHeuristicValue(child);
                var newCost = costSoFar[current] + 1 + childHeuristicValue;
                
                // If the child is already in the frontier, and the new cost is greater than the old cost, continue
                if (costSoFar.ContainsKey(child) && newCost >= costSoFar[child]) continue;
                
                // NOTE: we don't need to check if the child is already in the frontier - we assume consistency (which is true for admissible heuristics)
                
                // update the costSoFar, frontier, and cameFrom
                costSoFar[child] = newCost;
                frontier.Enqueue(child, newCost);
                cameFrom[child] = current;
            }
        }

        return new List<Puzzle>();
    }
}
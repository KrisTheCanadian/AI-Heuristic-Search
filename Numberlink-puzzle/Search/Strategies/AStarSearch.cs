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

    public long ExpandedNodes { get; set; }

    public Puzzle? Search(Puzzle puzzle)
    {
        
        // if the puzzle is already solved, return the puzzle
        if (puzzle.IsSolved()) return puzzle;

        // create a frontier (open list) and a cost dictionary
        var frontier = new PriorityQueue<Puzzle, int>();
        frontier.Enqueue(puzzle, _heuristicContext.GetHeuristicValue(puzzle));
        
        var costSoFar = new Dictionary<Puzzle, int>
        {
            // f(n) = g(n) + h(n), where g(n) is 0 and h(n) is the heuristic value
            [puzzle] = _heuristicContext.GetHeuristicValue(puzzle)
        };

        while (frontier.Count > 0)
        {
            // get the node with the lowest f(n) value
            var current = frontier.Dequeue();
            
            // check to see if the current node is the goal
            if (current.IsSolved())
            {
                // set the number of expanded nodes
                ExpandedNodes = costSoFar.Count + 1;
                
                return current;
            }
            
            // get successors of the current node
            var children = current.GetChildren();
            
            foreach (var child in children)
            {
                // add g(n)
                var heuristic = _heuristicContext.GetHeuristicValue(child);
                // f(n) = g(n) + h(n)
                var newCost = costSoFar[current] + 1 + heuristic;
                
                // skip if the heuristic value is int.MaxValue
                if(heuristic == int.MaxValue) continue; // avoid integer overflow
                
                // if the cost is less, update the cost and add the child to the frontier
                if (!costSoFar.ContainsKey(child) || newCost < costSoFar[child])
                {
                    costSoFar[child] = newCost;
                    frontier.Enqueue(child, newCost);
                }

                // NOTE: we don't need to check if the child is already in the frontier - we assume consistency (which is true for admissible heuristics)
                
                // update the cost
                costSoFar[child] = newCost;
                
                // add the child to the frontier
                frontier.Enqueue(child, newCost);
            }
        }
        
        // set the number of expanded nodes
        ExpandedNodes = costSoFar.Count;
        
        // if the frontier is empty, return null (no solution)
        return null;
    }
    
}
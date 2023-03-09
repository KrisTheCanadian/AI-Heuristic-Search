using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics;
using Eight_puzzle.Utils.Search.Interfaces;

namespace Eight_puzzle.Utils.Search.Strategies;

public class BestFirstSearch : ISearchStrategy
{
    private readonly HeuristicContext _heuristicContext;

    public BestFirstSearch(HeuristicContext heuristicContext)
    {
        _heuristicContext = heuristicContext;
    }

    public List<Puzzle> Search(Puzzle puzzle)
    {
        var goalState = Puzzle.GetGoalState();
        // If the puzzle is already solved, return the puzzle
        if (puzzle.Equals(goalState)) return new List<Puzzle> { puzzle };

        var openList = new PriorityQueue<Puzzle, int>();
        var closedList = new HashSet<Puzzle>();

        // Add the initial puzzle to the open list
        openList.Enqueue(puzzle, _heuristicContext.GetHeuristicValue(puzzle));

        while (openList.Count > 0)
        {
            // Get the puzzle with the lowest heuristic value
            var current = openList.Dequeue();

            // If the puzzle is the goal state, return the path
            if (current.Equals(goalState))
            {
                // reconstruct the path from the goal state to the initial state by following the parent pointers
                var path = new List<Puzzle> { current };
                while (current != null && current.Parent != null)
                {
                    current = current.Parent;
                    if (current != null) path.Insert(0, current);
                }

                return path;
            }

            // Get the children of the current puzzle
            var children = current.GetChildren();
            
            
            // Add the children to the open list if they are not already in the open list or closed list
            foreach (var child in children)
            {
                // If the child is already in the closed list, skip it
                if (closedList.Contains(child)) continue;
                
                // If the child is already in the open list, skip it
                if (openList.UnorderedItems.Any(x => x.Element.Equals(child))) continue;
                
                // Add the child to the open list
                openList.Enqueue(child, _heuristicContext.GetHeuristicValue(child));
                
                // Set the parent of the child to the current puzzle
                child.Parent = current;
            }

            // Add the current puzzle to the closed list
            closedList.Add(current);
        }

        return new List<Puzzle>();
    }
}
using Eight_puzzle.Models;
using Eight_puzzle.Utils.Search.Interfaces;

namespace Eight_puzzle.Utils.Search.Strategies;

public class DepthFirstSearch : ISearchStrategy
{
    
    public long NodesExpanded { get; set; }
    
    
    public List<Puzzle> Search(Puzzle puzzle)
    {
        
        var goalState = Puzzle.GetGoalState();
        // If the puzzle is already solved, return the puzzle
        if (puzzle.Equals(goalState)) return new List<Puzzle> { puzzle };

        // Create a stack, visited set, and path list
        var stack = new Stack<Puzzle>();
        var visited = new HashSet<Puzzle>();
        var path = new List<Puzzle>();
        
        // Add the puzzle to the stack & visited
        stack.Push(puzzle);
        visited.Add(puzzle);
        
        // While the stack is not empty
        while (stack.Count > 0)
        {
            // Pop the top puzzle off the stack
            var current = stack.Pop();
            
            // If the puzzle is the goal state, add it to the path
            if (current.Equals(goalState))
            {
                // rebuild the path from the goal state to the start state in reverse order (start state -> goal state)
                path.Add(current);
                while (current.Parent != null)
                {
                    path.Insert(0, current.Parent);
                    current = current.Parent;
                }
                
                // set the number of nodes expanded
                NodesExpanded = visited.Count + 1;

                return path;
            }

            // Get the children of the current puzzle
            var children = current.GetChildren();
            visited.Add(current);
            
            // For each child, if it has not been visited, add it to the stack
            foreach (var child in children)
            {
                if (visited.Contains(child)) continue;
                stack.Push(child);
            }
        }
        
        // set the number of nodes expanded
        NodesExpanded = visited.Count;

        return path;
    }
}
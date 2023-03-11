using Eight_puzzle.Models;
using Eight_puzzle.Utils.Search.Interfaces;

namespace Eight_puzzle.Utils.Search.Strategies;

public class BreadthFirstSearch : ISearchStrategy
{
    public long NodesExpanded { get; set; }

    public List<Puzzle> Search(Puzzle puzzle)
    {
        var goalState = Puzzle.GetGoalState();
        // If the puzzle is already solved, return the puzzle
        if (puzzle.Equals(goalState)) return new List<Puzzle> { puzzle };

        // Create a queue, visited set, and path list
        var queue = new Queue<Puzzle>();
        var visited = new HashSet<string>();
        var path = new List<Puzzle>();

        // Add the puzzle to the queue & visited
        queue.Enqueue(puzzle);
        visited.Add(puzzle.ToString());

        // While the queue is not empty
        while (queue.Count > 0)
        {
            // Dequeue the first puzzle off the queue
            var current = queue.Dequeue();

            // If the puzzle is the goal state, add it to the path
            if (current.Equals(goalState))
            {
                // recompose the path from the goal state to the start state in reverse order (start state -> goal state)
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
            visited.Add(current.ToString());


            // For each child, if it has not been visited, add it to the queue            
            foreach (var child in children)
            {
                if (visited.Contains(child.ToString())) continue;
                queue.Enqueue(child);
            }
        }

        // set the number of nodes expanded
        NodesExpanded = visited.Count;

        return path;
    }
}
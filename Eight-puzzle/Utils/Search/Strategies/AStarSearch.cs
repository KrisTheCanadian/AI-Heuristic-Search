using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics;
using Eight_puzzle.Utils.Search.Interfaces;

namespace Eight_puzzle.Utils.Search.Strategies;

public class AStarSearch : ISearchStrategy
{
    private readonly HeuristicContext _heuristicContext;

    public long NodesExpanded { get; set; }

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
        var frontierSet = new HashSet<Puzzle>();
        var cameFrom = new Dictionary<Puzzle, Puzzle?>();
        var costSoFar = new Dictionary<Puzzle, int>();
        var frontierPriorities = new Dictionary<Puzzle, int>();

        // Add the initial puzzle to the frontier
        var initialHeuristicValue = _heuristicContext.GetHeuristicValue(puzzle);
        frontier.Enqueue(puzzle, initialHeuristicValue);
        frontierSet.Add(puzzle);
        cameFrom[puzzle] = null;
        costSoFar[puzzle] = 0;
        frontierPriorities[puzzle] = initialHeuristicValue;

        while (frontier.Count > 0)
        {
            // Get the puzzle with the lowest cost (f(n) = g(n) + h(n))
            var current = frontier.Dequeue();
            frontierSet.Remove(current);

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

                // set the number of nodes expanded
                NodesExpanded = cameFrom.Count + 1;

                return path;
            }


            // Get the children of the current puzzle
            var children = current.GetChildren();

            // Loop through the children
            foreach (var child in children)
            {
                // calculate f(n) = g(n) + h(n) , where g(n) is the costSoFar of the current puzzle (the number of moves it took to get to the current puzzle)
                var childHeuristicValue = _heuristicContext.GetHeuristicValue(child);
                var newCost = costSoFar[current] + 1;

                // If the child is already in the frontier, and the new cost is greater than or equal to the old cost, continue
                if (costSoFar.ContainsKey(child) && newCost >= costSoFar[child]) continue;

                // update the costSoFar, frontier, and cameFrom
                costSoFar[child] = newCost;
                cameFrom[child] = current;

                // Check if the child is already in the frontier
                if (frontierSet.Contains(child))
                {
                    // If the child is already in the frontier, and the new cost is lower than the old cost, remove the child from the frontier and enqueue it again with the new priority
                    var oldPriority = frontierPriorities[child];
                    var newPriority = newCost + childHeuristicValue;
                    if (newPriority < oldPriority)
                    {
                        // Remove the child from the frontier
                        frontierSet.Remove(child);
                        frontierPriorities.Remove(child);

                        // Enqueue the child with the new priority
                        frontier.Enqueue(child, newPriority);
                        frontierSet.Add(child);
                        frontierPriorities.Add(child, newPriority);
                    }
                }
                else
                {
                    // If the child is not already in the frontier, add it to the frontier with its priority
                    var priority = newCost + childHeuristicValue;
                    frontier.Enqueue(child, priority);
                    frontierSet.Add(child);
                    frontierPriorities[child] = priority;
                }
            }
        }

        // set the number of nodes expanded
        NodesExpanded = cameFrom.Count;

        return new List<Puzzle>();
    }
}
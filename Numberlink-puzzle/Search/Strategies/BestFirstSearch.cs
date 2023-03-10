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

    public long ExpandedNodes { get; set; }

    public Puzzle? Search(Puzzle puzzle)
    {
        // if the puzzle is already solved, return the puzzle
        if (puzzle.IsSolved()) return puzzle;
        
        // create a frontier (open list) and a cost dictionary
        var openList = new PriorityQueue<Puzzle, int>();
        var closedList = new HashSet<Puzzle>();

        // add the initial puzzle to the open list (only check h(n))
        openList.Enqueue(puzzle, _heuristicContext.GetHeuristicValue(puzzle));

        while (openList.Count > 0)
        {
            // get the node with the lowest h(n) value
            var current = openList.Dequeue();
            
            // check to see if the current node is the goal
            if (current.IsSolved())
            {
                // set the number of expanded nodes
                ExpandedNodes = closedList.Count + 1;
                
                return current;
            }
            
            // get successors of the current node
            var children = current.GetChildren();

            // add the children to the open list if they are not already in the open list or closed list
            foreach (var child in children)
            {
                // skip if the child is already in the closed list
                if (closedList.Contains(child)) continue;
                
                // skip if the child is already in the open list
                if (openList.UnorderedItems.Any(x => x.Element.Equals(child))) continue;
                
                // evaluate the heuristic value for the child
                var heuristic = _heuristicContext.GetHeuristicValue(child);
                
                if(heuristic == int.MaxValue) continue; // avoid integer overflow
                
                // add the child to the open list
                openList.Enqueue(child, heuristic);
            }

            // add the current node to the closed list
            closedList.Add(current);
        }
        
        // set the number of expanded nodes
        ExpandedNodes = closedList.Count;
        
        // if the open list is empty, return null (no solution)
        return null;
    }

}
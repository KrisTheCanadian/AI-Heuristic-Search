using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics;
using Eight_puzzle.Utils.Search.Interfaces;

namespace Eight_puzzle.Utils.Search.Strategies
{
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
            if (puzzle.Equals(goalState)) return new List<Puzzle> { puzzle };
            
            var openList = new PriorityQueue<Puzzle, int>();
            var closedList = new HashSet<Puzzle>();
            
            openList.Enqueue(puzzle, _heuristicContext.GetHeuristicValue(puzzle));

            while (openList.Count > 0)
            {
                var current = openList.Dequeue();
                
                if (current.Equals(goalState))
                {
                    var path = new List<Puzzle> { current };
                    while (current != null && current.Parent != null)
                    {
                        current = current.Parent;
                        if (current != null) path.Insert(0, current);
                    }
                    return path;
                }
                
                var children = current.GetChildren();
                
                foreach(var child in children)
                {
                    if (closedList.Contains(child)) continue;
                    if (openList.UnorderedItems.Any(x => x.Element.Equals(child))) continue;
                    openList.Enqueue(child, _heuristicContext.GetHeuristicValue(child));
                    child.Parent = current;
                }
                
                closedList.Add(current);
            }
            
            return new List<Puzzle>();
        }
        
    }
}

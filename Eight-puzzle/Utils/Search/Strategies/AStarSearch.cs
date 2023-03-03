using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics;
using Eight_puzzle.Utils.Search.Interfaces;

namespace Eight_puzzle.Utils.Search.Strategies
{
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
            if (puzzle.Equals(goalState)) return new List<Puzzle> { puzzle };

            var frontier = new PriorityQueue<Puzzle, int>();
            frontier.Enqueue(puzzle, 0);

            var cameFrom = new Dictionary<Puzzle, Puzzle?>();
            var costSoFar = new Dictionary<Puzzle, int>();
            cameFrom[puzzle] = null;
            costSoFar[puzzle] = 0;

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.Equals(goalState))
                {
                    var path = new List<Puzzle> { current };
                    while (current != null && cameFrom.ContainsKey(current))
                    {
                        current = cameFrom[current];
                        if (current != null) path.Insert(0, current);
                    }
                    return path;
                }

                var children = current.GetChildren();
                foreach (var child in children)
                {
                    var newCost = costSoFar[current] + 1;
                    if (costSoFar.ContainsKey(child) && newCost >= costSoFar[child]) continue;
                    costSoFar[child] = newCost;
                    var priority = newCost + _heuristicContext.GetHeuristicValue(child);
                    frontier.Enqueue(child, priority);
                    cameFrom[child] = current;
                }
            }

            return new List<Puzzle>();
        }
    }
}

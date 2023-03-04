using Numberlink_puzzle.Heuristics;
using Numberlink_puzzle.Model;
using Numberlink_puzzle.Search.Interfaces;

namespace Numberlink_puzzle.Search.Strategies
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
            if (puzzle.IsSolved()) return new List<Puzzle> { puzzle };

            var frontier = new PriorityQueue<Puzzle, int>();
            frontier.Enqueue(puzzle, _heuristicContext.GetHeuristicValue(puzzle));

            var cameFrom = new Dictionary<Puzzle, Puzzle?>();
            var costSoFar = new Dictionary<Puzzle, int>();
            cameFrom[puzzle] = null;
            costSoFar[puzzle] = _heuristicContext.GetHeuristicValue(puzzle);

            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();

                if (current.IsSolved())
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
                    frontier.Enqueue(child, newCost + _heuristicContext.GetHeuristicValue(child));
                    cameFrom[child] = current;
                }
            }

            return new List<Puzzle>();
        }
    }
}

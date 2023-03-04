using Numberlink_puzzle.Model;
using Numberlink_puzzle.Search.Interfaces;

namespace Numberlink_puzzle.Search.Strategies;

public class BreadthFirstSearch: ISearchStrategy
{
    public List<Puzzle> Search(Puzzle puzzle)
    {
        if(puzzle.IsSolved()) return new List<Puzzle> {puzzle};
        
        var queue = new Queue<Puzzle>();
        var visited = new HashSet<string>();
        var path = new List<Puzzle>();
        queue.Enqueue(puzzle);
        visited.Add(puzzle.ToString());
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.IsSolved())
            {
                path.Add(current);
                while (current.Parent != null)
                {
                    path.Insert(0, current.Parent);
                    current = current.Parent;
                }
                return path;
            }

            var children = current.GetChildren();
            foreach (var child in children)
            {
                if (visited.Contains(child.ToString())) continue;
                visited.Add(current.ToString());
                queue.Enqueue(child);
            }
            
        }
        return path;
    }
}
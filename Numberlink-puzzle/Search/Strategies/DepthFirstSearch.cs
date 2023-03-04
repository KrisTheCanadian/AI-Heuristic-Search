using Numberlink_puzzle.Model;
using Numberlink_puzzle.Search.Interfaces;

namespace Numberlink_puzzle.Search.Strategies;

public class DepthFirstSearch: ISearchStrategy
{
    public List<Puzzle> Search(Puzzle puzzle)
    {
        if(puzzle.IsSolved()) return new List<Puzzle> {puzzle};
        
        var stack = new Stack<Puzzle>();
        var visited = new HashSet<string>();
        var path = new List<Puzzle>();
        stack.Push(puzzle);
        visited.Add(puzzle.ToString());
        while (stack.Count > 0)
        {
            var current = stack.Pop();
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
                visited.Add(child.ToString());
                stack.Push(child);
            }
        }
        return path;
    }
}
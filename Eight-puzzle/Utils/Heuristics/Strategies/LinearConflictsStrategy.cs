using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics.Interfaces;

namespace Eight_puzzle.Utils.Heuristics.Strategies;

public class LinearConflictsStrategy : IHeuristicStrategy
{
    public int GetHeuristicValue(Puzzle puzzle)
    {
        var conflicts = 0;
        var goalState = Puzzle.GetGoalState();
        
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                var value = puzzle.Board[i][j];
                if (value == 0) continue;
                
                var (x, y) = goalState.GetTileCoordinates(value);
                if (i == x && j != y)
                {
                    for (var k = 0; k < 3; k++)
                    {
                        if (k == j) continue;
                        var value2 = puzzle.Board[i][k];
                        if (value2 == 0) continue;
                        var (x2, y2) = goalState.GetTileCoordinates(value2);
                        if (x2 == x && y2 == y && k > j)
                        {
                            conflicts++;
                        }
                    }
                }
            }
        }
        
        var manhattan = new ManhattanDistanceStrategy().GetHeuristicValue(puzzle);

        return manhattan + conflicts;
    }
}
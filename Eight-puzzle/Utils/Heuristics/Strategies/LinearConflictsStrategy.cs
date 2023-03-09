using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics.Interfaces;

namespace Eight_puzzle.Utils.Heuristics.Strategies
{
    public class LinearConflictsStrategy : IHeuristicStrategy
    {
        public int GetHeuristicValue(Puzzle puzzle)
        {
            var conflicts = 0;
            var goalState = Puzzle.GetGoalState();
            
            // Check for linear conflicts in the same row
            for (var i = 0; i < 3; i++)
            {
                // Check for linear conflicts in the same column
                for (var j = 0; j < 3; j++)
                {
                    var value = puzzle.Board[i][j];
                    if (value == 0) continue;

                    var (x, y) = goalState.GetTileCoordinates(value);
                    conflicts += GetLinearConflicts(puzzle, goalState, i, j, x, y);
                }
            }

            return conflicts;
        }

        private static int GetLinearConflicts(Puzzle puzzle, Puzzle goalState, int i, int j, int x, int y)
        {
            var conflicts = 0;

            // Check for linear conflicts in the same row
            if (i == x)
            {
                for (var k = 0; k < 3; k++)
                {
                    if (k == j || puzzle.Board[i][k] == 0) continue; 
                    
                    // Get the coordinates of the corresponding tile in the goal state
                    var (x2, y2) = goalState.GetTileCoordinates(puzzle.Board[i][k]); 
                    
                    // If the tile is in the same row and to the right of the current tile, it is causing a linear conflict
                    if (x2 == x && y2 > y) 
                    {
                        conflicts++;
                    }
                }
            }

            // Check for linear conflicts in the same column
            if (j == y)
            {
                for (var k = 0; k < 3; k++)
                {
                    if (k == i || puzzle.Board[k][j] == 0) continue;
                    
                    // Get the coordinates of the corresponding tile in the goal state
                    var (x2, y2) = goalState.GetTileCoordinates(puzzle.Board[k][j]); 
                    
                    // If the tile is in the same column and below the current tile, it is causing a linear conflict
                    if (y2 == y && x2 > x) 
                    {
                        conflicts++;
                    }
                }
            }

            return conflicts;
        }
    }
}

using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics.Interfaces;

namespace Eight_puzzle.Utils.Heuristics.Strategies
{
    public class LinearConflictsStrategy : IHeuristicStrategy
    {
        public int GetHeuristicValue(Puzzle puzzle)
        {
            var goalState = Puzzle.GetGoalState();
            var conflicts = 0;

            for (var i = 0; i < puzzle.Board.Count; i++)
            {
                for (var j = 0; j < puzzle.Board[i].Count; j++)
                {
                    var tile = puzzle.Board[i][j];

                    if (tile == 0) continue;

                    var (goalRow, goalCol) = goalState.GetTileCoordinates(tile);

                    if (goalRow != i)
                    {
                        conflicts += GetRowConflicts(puzzle, goalState.Board, i, j, goalCol);
                    }
                    
                    if (goalCol != j)
                    {
                        conflicts += GetColConflicts(puzzle, goalState.Board, i, j, goalRow);
                    }
                }
            }

            return conflicts;
        }

        private static int GetRowConflicts(Puzzle puzzle, List<List<int>> goalState, int row, int col, int goalCol)
        {
            var conflicts = 0;

            for (var k = 0; k < puzzle.Board[row].Count; k++)
            {
                if (k == col || puzzle.Board[row][k] == 0) continue;

                var goalK = (puzzle.Board[row][k] - 1) % puzzle.Board[row].Count;

                if (goalK <= goalCol && k > col || goalK >= goalCol && k < col)
                {
                    if (goalK >= 0 && goalK < puzzle.Board[row].Count && goalState[row][goalK] != 0)
                    {
                        conflicts++;
                    }
                }
            }

            return conflicts;
        }


        private static int GetColConflicts(Puzzle puzzle, List<List<int>> goalState, int row, int col, int goalRow)
        {
            var conflicts = 0;

            for (var k = 0; k < puzzle.Board.Count; k++)
            {
                if (k == row || puzzle.Board[k][col] == 0) continue;

                var goalK = (puzzle.Board[k][col] - 1) / puzzle.Board.Count;

                if (goalK <= goalRow && k > row || goalK >= goalRow && k < row)
                {
                    if (goalK >= 0 && goalK < puzzle.Board.Count && goalState[goalK][col] != 0)
                    {
                        conflicts++;
                    }
                }
            }

            return conflicts;
        }
    }
}

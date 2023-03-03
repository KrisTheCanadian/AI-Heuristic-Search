using Eight_puzzle.Models;
using Eight_puzzle.Utils.Heuristics.Interfaces;

namespace Eight_puzzle.Utils.Heuristics.Strategies
{
    public class PermutationInversionStrategy : IHeuristicStrategy
    {
        public int GetHeuristicValue(Puzzle puzzle)
        {
            var distance = 0;
            var permutation = new List<int>();
            var goalState = Puzzle.GetGoalState();

            // Create permutation from puzzle state
            for (var i = 0; i < 3; i++)
            {
                for (var j = 0; j < 3; j++)
                {
                    var value = puzzle.Board[i][j];
                    if (value != 0)
                    {
                        permutation.Add(value);
                    }
                }
            }

            // Calculate number of inversions in permutation
            for (var i = 0; i < permutation.Count - 1; i++)
            {
                for (var j = i + 1; j < permutation.Count; j++)
                {
                    if (permutation[i] > permutation[j])
                    {
                        distance++;
                    }
                }
            }

            // If the blank tile is on an odd row counting from the bottom, add 1 to the distance
            if (IsOdd(goalState.GetBlankTileRow() + 1))
            {
                distance++;
            }

            return distance;
        }

        private static bool IsOdd(int n)
        {
            return n % 2 != 0;
        }
    }
}
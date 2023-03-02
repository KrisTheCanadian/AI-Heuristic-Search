using System.Text;

namespace Eight_puzzle.Models;

public class Puzzle
{
    private List<List<int>> Board { get; set; }

    public Puzzle(int[,] board)
    {
        Board = new List<List<int>>();
        for (var i = 0; i < 3; i++)
        {
            var row = new List<int>();
            for (var j = 0; j < 3; j++)
            {
                row.Add(board[i, j]);
            }

            Board.Add(row);
        }
    }
    
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var row in Board)
        {
            foreach (var c in row)
            {
                sb.Append(c);
                sb.Append(' ');
            }

            sb.Append(Environment.NewLine);
        }

        return sb.ToString();
    }
}
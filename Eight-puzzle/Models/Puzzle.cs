using System.Text;
using Eight_puzzle.Utils;

namespace Eight_puzzle.Models;

public class Puzzle
{
    public List<List<int>> Board { get; }
    public Puzzle? Parent { get; set; }

    private static Puzzle _goalState = new(new[,]
    {
        {1, 2, 3},
        {4, 5, 6},
        {7, 8, 0}
    });

    private Puzzle(int[,] board)
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
    
    public static bool CreatePuzzle(out Puzzle puzzle)
    {
        // Read the initial state of the puzzle
        var input = Console.ReadLine();

        if (input == null)
        {
            throw new Exception("Invalid input");
        }

        var inputArray = input.Split(' ');

        if (Validator.ValidateInput(inputArray, out var initialState)) throw new Exception("Invalid input");

        // create the puzzle
        puzzle = new Puzzle(initialState);
        return false;
    }

    public static Puzzle GetGoalState()
    {
        return _goalState;
    }
    
    public static void SetGoalState(Puzzle goalState)
    {
        _goalState = goalState;
    }

    private IEnumerable<Puzzle> GetSuccessorStates()
    {
        var successorStates = new List<Puzzle>();
        // operations: up, down, left, right
        
        // check if the blank tile can move up
        var blankTile = GetBlankTile();
        switch (blankTile)
        {
            case -1:
                throw new Exception("Invalid puzzle state");
            case > 2:
            {
                // blank tile is not in the first row
                var up = MoveUp();
                if (up != null)
                {
                    up.Parent = this;
                    successorStates.Add(up);
                }

                break;
            }
        }

        if(blankTile < 6)
        {
            // blank tile is not in the last row
            var down = MoveDown();
            if (down != null)
            {
                down.Parent = this;
                successorStates.Add(down);
            }
        }
        
        if(blankTile % 3 != 0)
        {
            // blank tile is not in the first column
            var left = MoveLeft();
            if (left != null)
            {
                left.Parent = this;
                successorStates.Add(left);
            }
        }

        if (blankTile % 3 == 2) return successorStates;
        // blank tile is not in the last column
        var right = MoveRight();
        if (right == null) return successorStates;
        right.Parent = this;
        successorStates.Add(right);

        return successorStates;
        
    }

    public IEnumerable<Puzzle> GetChildren()
    {
        return GetSuccessorStates();    
    }

    private int GetBlankTile()
    {
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                if (Board[i][j] == 0)
                {
                    return i * 3 + j;
                }
            }
        }

        return -1;
    }

    private Puzzle? MoveUp()
    {
        var blankTile = GetBlankTile();
        switch (blankTile)
        {
            case -1:
                throw new Exception("Invalid puzzle state");
            case < 3:
                // blank tile is in the first row
                return null;
        }

        var newBoard = new int[3, 3];
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                newBoard[i, j] = Board[i][j];
            }
        }

        var temp = newBoard[blankTile / 3 - 1, blankTile % 3];
        newBoard[blankTile / 3 - 1, blankTile % 3] = 0;
        newBoard[blankTile / 3, blankTile % 3] = temp;
        return new Puzzle(newBoard);
    }

    private Puzzle? MoveDown()
    {
        var blankTile = GetBlankTile();
        switch (blankTile)
        {
            case -1:
                throw new Exception("Invalid puzzle state");
            case > 5:
                // blank tile is in the last row
                return null;
        }
        
        var newBoard = new int[3, 3];
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                newBoard[i, j] = Board[i][j];
            }
        }
        
        var temp = newBoard[blankTile / 3 + 1, blankTile % 3];
        newBoard[blankTile / 3 + 1, blankTile % 3] = 0;
        newBoard[blankTile / 3, blankTile % 3] = temp;
        return new Puzzle(newBoard);
    }
    
    private Puzzle? MoveLeft()
    {
        var blankTile = GetBlankTile();
        if(blankTile == -1){ throw new Exception("Invalid puzzle state"); }
        if(blankTile % 3 == 0)
        {
            // blank tile is in the first column
            return null;
        }
        
        var newBoard = new int[3, 3];
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                newBoard[i, j] = Board[i][j];
            }
        }
        
        var temp = newBoard[blankTile / 3, blankTile % 3 - 1];
        newBoard[blankTile / 3, blankTile % 3 - 1] = 0;
        newBoard[blankTile / 3, blankTile % 3] = temp;
        return new Puzzle(newBoard);
    }
    
    private Puzzle? MoveRight()
    {
        var blankTile = GetBlankTile();
        if(blankTile == -1){ throw new Exception("Invalid puzzle state"); }
        if(blankTile % 3 == 2)
        {
            // blank tile is in the last column
            return null;
        }
        
        var newBoard = new int[3, 3];
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                newBoard[i, j] = Board[i][j];
            }
        }
        
        var temp = newBoard[blankTile / 3, blankTile % 3 + 1];
        newBoard[blankTile / 3, blankTile % 3 + 1] = 0;
        newBoard[blankTile / 3, blankTile % 3] = temp;
        return new Puzzle(newBoard);
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

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;
        
        var other = (Puzzle) obj;
        
        return other.ToString().Equals(ToString());
    }

    protected bool Equals(Puzzle other)
    {
        return Board.Equals(other.Board);
    }

    public override int GetHashCode()
    {
        return Board.GetHashCode();
    }
}
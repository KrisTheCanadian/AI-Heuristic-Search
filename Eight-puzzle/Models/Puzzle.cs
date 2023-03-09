using System.IO.Hashing;
using System.Text;
using Eight_puzzle.Utils;

namespace Eight_puzzle.Models;

public class Puzzle
{
    // goal state
    private static Puzzle _goalState = new(new[,]
    {
        { 1, 2, 3 },
        { 4, 5, 6 },
        { 7, 8, 0 }
    });

    // private constructor (switched to a factory method instead)
    private Puzzle(int[,] board)
    {
        Board = new List<List<int>>();
        for (var i = 0; i < 3; i++)
        {
            var row = new List<int>();
            for (var j = 0; j < 3; j++) row.Add(board[i, j]);

            Board.Add(row);
        }
    }

    // board is a 2D array of integers
    public List<List<int>> Board { get; }
    
    // parent puzzle (null if this is the initial state)
    public Puzzle? Parent { get; set; }

    // factory method (creates a puzzle from a 2D array of integers)
    public static bool CreatePuzzle(out Puzzle puzzle)
    {
        // Read the initial state of the puzzle
        var input = Console.ReadLine();

        if (input == null) throw new Exception("Invalid input");

        var inputArray = input.Split(' ');

        // validate the input (check if it is a valid permutation of the numbers 0-8)
        if (Validator.ValidateInput(inputArray, out var initialState)) throw new Exception("Invalid input");

        // create the puzzle
        puzzle = new Puzzle(initialState);
        return false;
    }

    // get the coordinates of a tile in the puzzle
    public (int, int) GetTileCoordinates(int value)
    {
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
            if (Board[i][j] == value)
                return (i, j);

        throw new ArgumentException($"Tile with value {value} not found in puzzle.");
    }

    public static Puzzle GetGoalState()
    {
        return _goalState;
    }

    public static void SetGoalState(Puzzle goalState)
    {
        _goalState = goalState;
    }

    // create successor states of the current puzzle
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

        // move down
        if (blankTile < 6)
        {
            // blank tile is not in the last row
            var down = MoveDown();
            if (down != null)
            {
                down.Parent = this;
                successorStates.Add(down);
            }
        }
        
        // move left
        if (blankTile % 3 != 0)
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
        
        // move right
        // blank tile is not in the last column
        var right = MoveRight();
        if (right == null) return successorStates;
        right.Parent = this;
        successorStates.Add(right);

        return successorStates;
    }

    // wrapper for GetSuccessorStates()
    public IEnumerable<Puzzle> GetChildren()
    {
        return GetSuccessorStates();
    }

    // get the index of the blank tile
    // used in successor states generation
    private int GetBlankTile()
    {
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
            if (Board[i][j] == 0)
                return i * 3 + j;

        return -1;
    }

    // move the blank tile up
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
        for (var j = 0; j < 3; j++)
            newBoard[i, j] = Board[i][j];

        var temp = newBoard[blankTile / 3 - 1, blankTile % 3];
        newBoard[blankTile / 3 - 1, blankTile % 3] = 0;
        newBoard[blankTile / 3, blankTile % 3] = temp;
        return new Puzzle(newBoard);
    }

    // move the blank tile down
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
        for (var j = 0; j < 3; j++)
            newBoard[i, j] = Board[i][j];

        var temp = newBoard[blankTile / 3 + 1, blankTile % 3];
        newBoard[blankTile / 3 + 1, blankTile % 3] = 0;
        newBoard[blankTile / 3, blankTile % 3] = temp;
        return new Puzzle(newBoard);
    }

    // move the blank tile left
    private Puzzle? MoveLeft()
    {
        var blankTile = GetBlankTile();
        if (blankTile == -1) throw new Exception("Invalid puzzle state");
        if (blankTile % 3 == 0)
            // blank tile is in the first column
            return null;

        var newBoard = new int[3, 3];
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
            newBoard[i, j] = Board[i][j];

        var temp = newBoard[blankTile / 3, blankTile % 3 - 1];
        newBoard[blankTile / 3, blankTile % 3 - 1] = 0;
        newBoard[blankTile / 3, blankTile % 3] = temp;
        return new Puzzle(newBoard);
    }

    // move the blank tile right
    private Puzzle? MoveRight()
    {
        var blankTile = GetBlankTile();
        if (blankTile == -1) throw new Exception("Invalid puzzle state");
        if (blankTile % 3 == 2)
            // blank tile is in the last column
            return null;

        var newBoard = new int[3, 3];
        for (var i = 0; i < 3; i++)
        for (var j = 0; j < 3; j++)
            newBoard[i, j] = Board[i][j];

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

    // check only puzzle state for equality
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        var other = (Puzzle)obj;
        var otherStr = other.ToString();

        return otherStr.Equals(ToString());
    }

    // override the equals operator
    protected bool Equals(Puzzle other)
    {
        return other.ToString().Equals(ToString());
    }
    
    // fixes the hashcode collision problem for sets
    public override int GetHashCode()
    {
        var crc = new Crc32();
        crc.Append(Encoding.ASCII.GetBytes(ToString()));
        var hash = crc.GetCurrentHash();
        var i = BitConverter.ToInt32(hash, 0);

        return i;
    }
}
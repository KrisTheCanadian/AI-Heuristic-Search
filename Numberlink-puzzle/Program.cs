using Numberlink_puzzle.Model;

Console.WriteLine("Enter the number of rows: ");
var rows = int.Parse(Console.ReadLine()!);
Console.WriteLine("Enter the number of columns: ");
var columns = int.Parse(Console.ReadLine()!);

Console.WriteLine("Note: 0 means empty, numbers > 0 are the numbers of the path");
Console.Write("Enter the grid: ");
// input looks like this: 3 0 0 2 0 2 1 0 0 0 3 1 4 0 0 4
var grid = new int[rows, columns];

// split the input into rows
var input = Console.ReadLine()!.Split(' ');
var index = 0;
for (var i = 0; i < rows; i++)
{
    for (var j = 0; j < columns; j++)
    {
        grid[i, j] = int.Parse(input[index]);
        index++;
    }
}

var puzzle = new Puzzle(rows, columns, grid);

if (!puzzle.IsValidFirstPuzzle())
{
    Console.WriteLine("The puzzle is not valid!");
    return;
}

Console.WriteLine();
Console.WriteLine("The puzzle is valid: " + puzzle.IsValidFirstPuzzle());
Console.WriteLine("The number of paths is: " + puzzle.PathsCount);
Console.WriteLine("Puzzle: \n" + puzzle);

Console.WriteLine("Puzzle Successors: ");
var successors = puzzle.GetSuccessorStates();

foreach (var successor in successors)
{
    Console.WriteLine(successor);
}

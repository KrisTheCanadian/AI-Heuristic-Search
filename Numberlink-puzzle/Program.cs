using System.Diagnostics;
using Numberlink_puzzle.Heuristics;
using Numberlink_puzzle.Heuristics.Interfaces;
using Numberlink_puzzle.Heuristics.Strategies;
using Numberlink_puzzle.Model;
using Numberlink_puzzle.Search;
using Numberlink_puzzle.Search.Strategies;

Console.WriteLine("Numberlink");
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
for (var j = 0; j < columns; j++)
{
    grid[i, j] = int.Parse(input[index]);
    index++;
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


Console.WriteLine("1. Best-first search");
Console.WriteLine("2. A* search");
Console.Write("Enter the search strategy: ");

var searchStrategy = Console.ReadLine();
Console.WriteLine();

if (searchStrategy == null) throw new Exception("Invalid input");

var strategy = int.Parse(searchStrategy);

// if option 3 or 4 is chosen, ask the user for the heuristic function

HeuristicContext? heuristicContext = null;

Console.WriteLine("1. Linear conflicts heuristic");
Console.WriteLine("2. Double Manhattan distance heuristic (InAdmissible)");
Console.Write("Enter the heuristic function: ");

searchStrategy = Console.ReadLine();
Console.WriteLine();

if (searchStrategy == null) throw new Exception("Invalid input");

var heuristic = int.Parse(searchStrategy);

// create the heuristic strategy
IHeuristicStrategy? heuristicStrategy = heuristic switch
{
    1 => new AdmissibleHeuristicStrategy(),
    2 => new NonAdmissibleHeuristicStrategy(),
    _ => throw new Exception("Invalid input")
};

heuristicContext = new HeuristicContext(heuristicStrategy);

// create the search context (strategy pattern)
var searchContext = new SearchContext(strategy switch
    {
        1 => new BestFirstSearch(heuristicContext!),
        2 => new AStarSearch(heuristicContext!),
        _ => throw new Exception("Invalid input")
    });

// start timer 
var watch = Stopwatch.StartNew();
watch.Start();

// search for the solution (According to the chosen strategy)
var solution = searchContext.Search(puzzle);

watch.Stop();

Console.WriteLine("Solution: \n" + solution);
Console.WriteLine("Time elapsed: " + watch.ElapsedMilliseconds + " ms");
Console.WriteLine($"Memory used: {GC.GetTotalMemory(false) / 1024 / 1024} MB");
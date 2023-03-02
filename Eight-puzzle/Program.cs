using Eight_puzzle.Models;
using Eight_puzzle.Utils.Search;
using Eight_puzzle.Utils.Search.Interfaces;
using Eight_puzzle.Utils.Search.Strategies;

Console.WriteLine("Enter the initial state of the puzzle:");
Console.WriteLine("Enter the numbers 0-8, where 0 represents the blank tile, separated by spaces:");

if (Puzzle.CreatePuzzle(out var puzzle)) return;
Console.WriteLine("Initial state:");
Console.WriteLine(puzzle);

Console.WriteLine("Enter the desired/goal state of the puzzle:");
Console.WriteLine("Enter the numbers 0-8, where 0 represents the blank tile, separated by spaces:");

if (Puzzle.CreatePuzzle(out var goalState)) return;
Console.WriteLine("Goal state:");
Console.WriteLine(goalState);

Puzzle.SetGoalState(goalState);

// ask the user for the search strategy
Console.WriteLine("1. Breadth-first search");
Console.WriteLine("2. Depth-first search");
Console.Write("Enter the search strategy: ");

var input = Console.ReadLine();
Console.WriteLine();

if (input == null) throw new Exception("Invalid input");

var strategy = int.Parse(input);

// create the search strategy
ISearchStrategy searchStrategy = strategy switch
{
    1 => new BreadthFirstSearch(),
    2 => new DepthFirstSearch(),
    _ => throw new Exception("Invalid input")
};

// create the search context
var searchContext = new SearchContext(searchStrategy);

// start the timer
var watch = System.Diagnostics.Stopwatch.StartNew();
watch.Start();

// search for the solution
var solution = searchContext.Search(puzzle);

watch.Stop();

// print the solution
Console.WriteLine("Solution:");
foreach (var state in solution) {
    Console.WriteLine(state);
}

Console.WriteLine($"Number of moves: {solution.Count - 1}");
Console.WriteLine($"Time taken: {watch.ElapsedMilliseconds} ms");
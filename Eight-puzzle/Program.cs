using Eight_puzzle.Models;
using Eight_puzzle.Utils;
using Eight_puzzle.Utils.Heuristics;
using Eight_puzzle.Utils.Heuristics.Interfaces;
using Eight_puzzle.Utils.Heuristics.Strategies;
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
Console.WriteLine("3. Best-first search");
Console.WriteLine("4. A* search");
Console.Write("Enter the search strategy: ");

var input = Console.ReadLine();
Console.WriteLine();

if (input == null) throw new Exception("Invalid input");

var strategy = int.Parse(input);

// if option 3 or 4 is chosen, ask the user for the heuristic function

HeuristicContext? heuristicContext = null;

if(input is "3" or "4")
{
    Console.WriteLine("1. Hamming distance");
    Console.WriteLine("2. Manhattan distance");
    Console.WriteLine("3. Permutation inversion heuristic");
    Console.WriteLine("4. Linear conflicts heuristic");
    Console.WriteLine("5. Double Manhattan distance heuristic (InAdmissible)");
    Console.Write("Enter the heuristic function: ");

    input = Console.ReadLine();
    Console.WriteLine();

    if (input == null) throw new Exception("Invalid input");

    var heuristic = int.Parse(input);

    // create the heuristic strategy
    IHeuristicStrategy heuristicStrategy = heuristic switch
    {
        1 => new HammingDistanceStrategy(),
        2 => new ManhattanDistanceStrategy(),
        3 => new PermutationInversionStrategy(),
        4 => new LinearConflictsStrategy(),
        5 => new DoubleManhattanDistanceStrategy(),
        _ => throw new Exception("Invalid input")
    };

    // create the heuristic context
    heuristicContext = new HeuristicContext(heuristicStrategy);
}

// create the search strategy
ISearchStrategy searchStrategy = strategy switch
{
    1 => new BreadthFirstSearch(),
    2 => new DepthFirstSearch(),
    3 => new BestFirstSearch(heuristicContext!),
    4 => new AStarSearch(heuristicContext!),
    _ => throw new Exception("Invalid input")
};

// create the search context
var searchContext = new SearchContext(searchStrategy);

// check to see if the puzzle is solvable
if (!Validator.IsSolvable(puzzle)) {
    Console.WriteLine("The puzzle is not solvable.");
    return;
}

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
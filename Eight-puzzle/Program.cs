using Eight_puzzle.Models;
using Eight_puzzle.Utils;

Console.WriteLine("Enter the initial state of the puzzle:");
Console.WriteLine("Enter the numbers 0-8, where 0 represents the blank tile, separated by spaces:");

// Read the initial state of the puzzle
// make sure each character is a digit and is between 0 and 9 inclusive
var input = Console.ReadLine();

if (input == null) {
    Console.WriteLine("Invalid input");
    return; 
}

var inputArray = input.Split(' ');

if (Validator.ValidateInput(inputArray, out var initialState)) return;

// create the puzzle
var puzzle = new Puzzle(initialState);
Console.WriteLine(puzzle);

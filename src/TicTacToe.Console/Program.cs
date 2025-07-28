using TicTacToe.Console;

// Example 1: Console only
var consoleGame = new ConsoleGame();

// Example 2: Console with file logging
// var logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "tic-tac-toe-log.txt");
// var consoleRenderer = new ConsoleGameRenderer();
// var loggingRenderer = new FileLogRenderer(logPath, consoleRenderer);
// var gameWithLogging = new ConsoleGame(loggingRenderer);

consoleGame.Run();

using System;

namespace FinalProject
{
    class Program
    {
        static void Main(string[] args)
        {
            GameFlow gameFlow = new GameFlow();
            gameFlow.InitialiseGame();
            Console.ReadLine();
        }
    }
    class Board
    {

        int[,] board;
        GameFlow gameFlow;

        public int rows;
        public int columns;

        public Board(int rows, int columns, GameFlow gFlow)
        {
            this.gameFlow = gFlow;
            this.rows = rows;
            this.columns = columns;
            board = new int[this.rows, this.columns];
            DisplayGrid();
        }
        public void DropPiece(int column, bool player)
        {
            column--; //subtract one from the column
            for (int i = (this.rows - 1); i >= 0; i--)
            {
                if (board[i, column] == 0)
                {
                    if (player)
                    {
                        board[i, column] = 1;
                        Console.WriteLine();
                        Console.WriteLine("Player 1 dropped a piece in row " + (i + 1) + " column " + (column + 1));    //Drop piece
                        Console.WriteLine();
                    }
                    else
                    {
                        board[i, column] = 2;
                        Console.WriteLine();
                        Console.WriteLine("Player 2 dropped a piece in row " + (i + 1) + " column " + (column + 1));    //Drop piece
                        Console.WriteLine();
                    }
                    break;
                }
            }

            DisplayGrid();

            if (CheckForWin(player))
            {
                gameFlow.Victory(player);
            }
        }
        private bool CheckForWin(bool playerturn)
        {
            //check through board to see if there is any piece belonging to the player that just dropped
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (playerturn == true)//check for player 1 win
                    {
                        if (board[i, j] == 1)
                        {
                            if (CheckForLine(i, j, playerturn))
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (board[i, j] == 2)//check for player 2 win
                        {
                            if (CheckForLine(i, j, playerturn))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false; //return false if a player doesnt win

        }
        private bool CheckForLine(int row, int column, bool playerTurn)
        {
            int checkingNumber = 0; //this sets the number we are looking for based on which player just dropped a piece
            if (playerTurn)
            {
                checkingNumber = 1;
            }
            else
            {
                checkingNumber = 2;
            }

            int piececount = 0; // keep track of how many pieces we have in a row

            for (int i = 0; i <= 3; i++)
            {
                if (column - i >= 0)//column cant go below 0
                {
                    if (board[row, column - i] == checkingNumber)
                    {
                        piececount++;

                        if (piececount >= 4)//once we have 4 pieces in a row, return victory.
                        {
                            return true;
                        }
                    }
                }
            }

            //check 3 spaces up
            piececount = 0; //reset counter to 0 for the next check
            for (int i = 0; i <= 3; i++)
            {
                if (row + i < rows)//row cant go above the top row
                {
                    if (board[row + i, column] == checkingNumber)
                    {
                        piececount++;

                        if (piececount >= 4)//once we have 4 pieces in a row, return victory.
                        {
                            return true;
                        }
                    }
                }
            }

            //check 3 spaces diagonal right up
            piececount = 0;//reset counter to 0 for the next check
            for (int i = 0; i <= 3; i++)
            {
                if (row + i < rows)//row cant go above the top row
                {
                    if (column - i >= 0) //column cant go below 0
                    {
                        if (board[row + i, column - i] == checkingNumber)
                        {
                            piececount++;

                            if (piececount >= 4)//once we have 4 pieces in a row, return victory.
                            {
                                return true;
                            }
                        }
                    }
                }

            }
            //check 3 spaces diagonal left up
            piececount = 0;//reset counter to 0 for the next check
            for (int i = 0; i <= 3; i++)
            {

                if (row + i < rows)//row cant go above the top row
                {
                    if (column + i < columns) //column cant go below 0
                    {
                        if (board[row + i, column + i] == checkingNumber)
                        {
                            piececount++;

                            if (piececount >= 4)
                            {
                                return true;
                            }

                        }
                    }
                }

            }

            return false;
        }
        public void DisplayGrid()
        {

            Console.WriteLine("  1  2  3  4  5  6  7 ");
            Console.WriteLine("-----------------------");
            for (int i = 0; i < rows; i++)
            {
                if (i > 0)
                {
                    Console.WriteLine();

                }
                Console.Write("|");


                for (int j = 0; j < columns; j++)
                {
                    if (board[i, j] == 1)//player1
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    }
                    else if (board[i, j] == 2)//player 2
                    {
                        Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.Write(" " + board[i, j] + " ");
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                Console.Write("|");
            }
            Console.WriteLine();
            Console.WriteLine("-----------------------");
            Console.WriteLine();

        }

        public bool CheckIfSpace(int column)
        {
            return true;
        }
    }

    class GameFlow
    {
        public bool playAgainstAI;

        public bool isGameInitialised = false;
        public bool isVictoryAchieved = false;
        bool isPlayer1Turn = true;

        Player player1;
        Player player2;

        Board board;

        private void PlayGame()
        {
            string command = "";  //Command format: Drop 4 (Drop Piece in column 4)

            do
            {
                if (isPlayer1Turn)
                {  //handle player turn

                    HandlePlayerTurn(isPlayer1Turn, command, board);
                }
                else // second players turn
                {


                    if (playAgainstAI) //handle ai turn
                    {
                        HandleAITurn(isPlayer1Turn, board);
                    }
                    else //handle player 2 turn
                    {
                        HandlePlayerTurn(isPlayer1Turn, command, board);
                    }

                }

                if (isVictoryAchieved)
                {
                    break;
                }
            } while (command != "exit");
        }
        public void InitialiseGame()
        {
            int difficultyLevel = 0;
            Console.WriteLine("Welcome to Connect 4! Your goal is to get 4 pieces to line up straight or diagonally. First player to achieve this will win the round, Good Luck!");//Welcome Message
            do
            {
                Console.WriteLine("Would you like to play against ai or another human?");
                string cmd = Console.ReadLine();
                cmd = cmd.Trim();
                cmd = cmd.ToLower();
                string[] split = cmd.Split();
                if (split[0] == "ai")
                {
                    Console.WriteLine("Would you like the ai to be easy or normal");
                    cmd = Console.ReadLine();
                    cmd = cmd.Trim();
                    cmd = cmd.ToLower();
                    split = cmd.Split();

                    if (split[0] == "easy")
                    {
                        Console.WriteLine("The difficulty is easy");
                        difficultyLevel = 0;
                    }
                    else if (split[0] == "normal")
                    {
                        Console.WriteLine("The difficulty is normal");
                        difficultyLevel = 1;
                    }
                    else
                    {
                        Console.WriteLine("#ERROR Incorrect difficulty input, defaulting to normal");
                    }
                    playAgainstAI = true;
                    isGameInitialised = true;
                }
                else if (split[0] == "human")
                {

                    playAgainstAI = false;
                    isGameInitialised = true;
                }
                else
                {
                    Console.WriteLine("#ERROR: Please Enter whether you want to play against an ai or human");
                }
            } while (!isGameInitialised);

            board = new Board(6, 7, this);//create the board
            player1 = new Player(); //initialise player 1
            if (playAgainstAI)
            {
                player2 = new AI(difficultyLevel); //initialise player 2(or ai)
            }
            else
            {
                player2 = new Player(); //initialise player 2(or ai)
            }

            PlayGame(); //Start the game
        }
        private void HandlePlayerTurn(bool isPlayer1Turn, string command, Board board)
        {
            Console.WriteLine();

            if (isPlayer1Turn)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Player 1 Turn, choose where will you like to drop your piece (ex.'drop 1'");
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Player 2 Turn, choose where will you like to drop your piece (ex.'drop 1'");
            }

            Console.BackgroundColor = ConsoleColor.Black;

            command = Console.ReadLine();
            command = command.Trim();
            command = command.ToLower();
            string[] split = command.Split();

            if (split[0] == "drop")
            {
                if (split.Length > 1)
                {
                    int column = int.Parse(split[1]);
                    if (column >= 1 && column <= board.columns) //column be between 1-7
                    {
                        board.DropPiece(column, isPlayer1Turn);
                        EndTurn(isPlayer1Turn);
                    }
                    else
                    {
                        Console.WriteLine("#ERROR: That Column does not exist! try to drop a piece between columns 1-7!");
                    }
                }
                else
                {
                    Console.WriteLine("#ERROR: Enter a command as well as the perameters! ex) 'drop 3'");
                }
            }
            else if (split[0] == "?")
            {
                Console.WriteLine("Help: Command Examples: '?', 'drop 3', 'exit'");
            }
        }
        private void HandleAITurn(bool playerTurn, Board board)
        {

            player2.TakeTurn(board);
            EndTurn(playerTurn);

        }
        private void EndTurn(bool playerTurn)
        {

            if (playerTurn)
            {

                isPlayer1Turn = false;


            }
            else
            {
                isPlayer1Turn = true;

            }
        }

        public void Victory(bool playerTurn)
        {
            if (playerTurn)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("Congratulations, Player 1!");
                isVictoryAchieved = true;
                Console.Read();
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("Congratulations, Player 2!");
                isVictoryAchieved = true;
                Console.Read();
            }
        }
    }

    class Player
    {
        public bool isAIControlled;
        public virtual void TakeTurn(Board board)
        {

        }

    }

    class Human : Player
    {
        public Human()
        {
            this.isAIControlled = false;
        }

        public override void TakeTurn(Board board)
        {

        }
    }

    class AI : Player
    {
        int difficulty = 0;
        int lastDropLocation = -1;
        public AI(int difficulty)
        {
            this.isAIControlled = true;
            this.difficulty = difficulty;
        }

        public override void TakeTurn(Board board)
        {
            if (difficulty == 0) //easy, totally random
            {
                Random rand = new Random();
                int randomColumn = rand.Next(1, 8);
                if (board.CheckIfSpace(randomColumn))
                {
                    board.DropPiece(randomColumn, false);
                }
                else
                {
                    TakeTurn(board);
                }
            }
            if (difficulty == 1) //normal, ai will always place pieces next to a place they have already tried to put a piece.
            {
                if (lastDropLocation == -1)
                {
                    Random rand = new Random();
                    int randomColumn = rand.Next(1, 8);
                    lastDropLocation = randomColumn;
                    if (board.CheckIfSpace(randomColumn))
                    {
                        board.DropPiece(randomColumn, false);
                    }
                    else
                    {
                        TakeTurn(board);
                    }
                }
                else
                {
                    Random rand = new Random();

                    if (lastDropLocation == 1)
                    {
                        int randomColumn = rand.Next(1, 2);
                        lastDropLocation = randomColumn;

                        if (board.CheckIfSpace(randomColumn))
                        {
                            board.DropPiece(randomColumn, false);
                        }
                        else
                        {
                            TakeTurn(board);
                        }
                    }
                    else if (lastDropLocation == 8)
                    {
                        int randomColumn = rand.Next(7, 8);
                        lastDropLocation = randomColumn;

                        if (board.CheckIfSpace(randomColumn))
                        {
                            board.DropPiece(randomColumn, false);
                        }
                        else
                        {
                            TakeTurn(board);
                        }
                    }
                    else
                    {
                        int randomColumn = rand.Next(lastDropLocation - 1, lastDropLocation + 1);
                        lastDropLocation = randomColumn;

                        if (board.CheckIfSpace(randomColumn))
                        {
                            board.DropPiece(randomColumn, false);
                        }
                        else
                        {
                            TakeTurn(board);
                        }
                    }
                }
            }
        }
    }
}

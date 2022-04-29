using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Sudoku
{
    class Program
    {
        //Coordinate relative to the 2D array
        static (int, int) coordinates = (0, 0);
        const int board_len = 37;
        //List of moves used by the undo algorithm
        static List<Move> moves = new List<Move>();
        //List of moves used by the redo algorithm
        static List<Move> redo_moves = new List<Move>();
        //List of all moves used by the replay algorithm
        static List<Move> all_moves = new List<Move>();
        static Random rand = new Random();
        static int counter = 0;

        //2D matrix the user plays on
        static int[,] matrix = new int[9, 9] { { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

        //2D matrix containing original unsolved board, used by the solving algorithm
        static int[,] old_matrix = new int[9, 9] {  { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                    { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                    { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                    { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                    { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                    { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                    { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                    { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                    { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

        //2D matrix used to create new boards
        static int[,] create_matrix = new int[9, 9] {  { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                       { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                       { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                       { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                       { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                       { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                       { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                       { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                       { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

        //2D matrix used to validate create_matrix during the creation phase
        static int[,] create_matrix_copy = new int[9, 9] {  { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                            { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };

        //2D matrix contaning the solution to the puzzle, used to check whether the user has been successful in completing it
        static int[,] solution_matrix = new int[9, 9] {  { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                                         { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };


        static void Main(string[] args)
        {
            //Encoding method to support unicode
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.ForegroundColor = ConsoleColor.White;

            //Set console size
            Console.SetWindowSize(170, 41);

            //Call to main menu method
            menu();
        }

        /*
        * Main menu method
        * String msg - string to display to user
        */
        static void menu(String msg = "")
        {
            //Clear screen and call to Welcome method
            Console.Clear();
            Welcome();
            
            //Show user options
            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "Enter the option's number to start");
            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "1. Play New Game");
            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "2. Play Saved Game");
            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "3. How to play");
            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "4. Quit");
            
            //If there is a message, display it
            Message(msg, 35);

            //Set cursor at reasonable position
            int top = Console.CursorTop;
            Console.SetCursorPosition(Console.WindowWidth / 3, top);

            //Read user entered key
            var key = Console.ReadKey().Key;

            switch (key)
            {
                //Start new game
                case ConsoleKey.D1:

                    //User options
                    Console.WriteLine("\n " + new string(' ', Console.WindowWidth / 3) + "Choose a difficulty:");
                    Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "1. Easy");
                    Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "2. Medium");
                    Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "3. Hard");
                    Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "4. Extreme");

                    //Read key
                    var new_key = Console.ReadKey().Key;

                    switch(new_key){
                        
                        //Start game of easy difficulty
                        case ConsoleKey.D1:
                                Console.Clear();
                                Message("Your board is being created", 8, 60);
                                
                                //Clear all lists and arrays
                                moves.Clear();
                                redo_moves.Clear();
                                for(int i = 0; i < 9; i++){
                                    for(int j = 0; j < 9; j++){
                                        matrix[i, j] = 0;
                                        old_matrix[i, j] = 0;
                                        create_matrix[i, j] = 0;
                                        create_matrix_copy[i, j] = 0;
                                    }
                                }
                                //Call to CreateBoard algorithm
                                CreateBoard(30);
                            break;

                        //Start game of medium difficulty
                        case ConsoleKey.D2:
                                Console.Clear();
                                Message("Your board is being created", 8, 60);

                                //Clear all lists and arrays
                                moves.Clear();
                                redo_moves.Clear();
                                for(int i = 0; i < 9; i++){
                                    for(int j = 0; j < 9; j++){
                                        matrix[i, j] = 0;
                                        old_matrix[i, j] = 0;
                                        create_matrix[i, j] = 0;
                                        create_matrix_copy[i, j] = 0;
                                    }
                                }
                                //Call to CreateBoard algorithm
                                CreateBoard(45);
                            break;

                        //Start game of hard difficulty
                        case ConsoleKey.D3:
                                Console.Clear();
                                Message("Your board is being created", 8, 60);

                                //Clear all lists and arrays
                                moves.Clear();
                                redo_moves.Clear();
                                for(int i = 0; i < 9; i++){
                                    for(int j = 0; j < 9; j++){
                                        matrix[i, j] = 0;
                                        old_matrix[i, j] = 0;
                                        create_matrix[i, j] = 0;
                                        create_matrix_copy[i, j] = 0;
                                    }
                                }
                                //Call to CreateBoard algorithm
                                CreateBoard(60);
                            break;

                        //Start game of extreme difficulty
                        case ConsoleKey.D4:
                                Console.Clear();
                                Message("Your board is being created", 8, 60);

                                //Clear all lists and arrays
                                moves.Clear();
                                redo_moves.Clear();
                                for(int i = 0; i < 9; i++){
                                    for(int j = 0; j < 9; j++){
                                        matrix[i, j] = 0;
                                        old_matrix[i, j] = 0;
                                        create_matrix[i, j] = 0;
                                        create_matrix_copy[i, j] = 0;
                                    }
                                }
                                //Call to CreateBoard algorithm
                                CreateBoard(80);
                            break;
                        //Invalid key entered
                        default:
                                //Return to main menu with invalid option message
                                menu("Invalid option, enter a valid one");
                            break;

                    }
                    break;

                //Load Game
                case ConsoleKey.D2:
                    Console.Clear();
                    //Call to Loadgame algorithm
                    LoadGame();
                    break;

                //Rules
                case ConsoleKey.D3:
                    //Call to Rules method and RulesMenu algorithm
                    Rules();
                    RulesMenu();
                    break;

                //Exit Application
                case ConsoleKey.D4:
                    Quit();
                    break;

                //Return to main menu with invalid option message
                default:
                    menu("Invalid option, enter a valid one");
                    break;
            }
        }


        /*
        * Game algorithm - where the player interacts with the board
        */
        static void Game()
        {
            //Clear console
            Console.Clear();

            //Reset coordinates
            coordinates = (0, 0);

            //Call to DisplayBoard to show board to user and DisplayOptions to display options to user
            DisplayBoard();
            DisplayOptions();

            //Set position at the top left square of the board
            Console.SetCursorPosition((Console.WindowWidth / 2) - (board_len / 2) + 1, 1); //First position

            while (true)
            {
                //Get curos position
                (int, int) pos = Console.GetCursorPosition();

                //Get value and coordinates
                int val = matrix[coordinates.Item2, coordinates.Item1];

                //Read key
                var key = Console.ReadKey().Key;

                switch (key)
                {
                    //Up arrow is clicked event
                    case ConsoleKey.UpArrow:

                        MessageClear();

                        /*
                        By default, upon clicking any arrow key in the terminal while also being inside a running application,
                        the terminal will overwrite any character upon which the cursor is on and move one unit to the right.
                        The following few lines of code are to couteract it. Similar code is also present on the cases for the 
                        other arrow keys.
                        */

                        //Get cursor position
                        (int, int) pos_up = Console.GetCursorPosition();

                        //Counteract termial behaviour
                        Console.SetCursorPosition(pos_up.Item1 - 1, pos_up.Item2);
                        if (val == 0)
                            Console.Write(' ');
                        else
                            Console.Write(val);
                        Console.SetCursorPosition(pos_up.Item1 - 1, pos_up.Item2);

                        //Check that cursor stays within board boundaries
                        if (pos_up.Item2 > 1)
                        {
                            Console.SetCursorPosition(pos_up.Item1 - 1, pos_up.Item2 - 2);
                            coordinates = (coordinates.Item1, coordinates.Item2 - 1);
                        }
                        else
                            Message("You reached the top of the board");
                        break;

                    //Down arrow is clicked event
                    case ConsoleKey.DownArrow:

                        MessageClear();

                        (int, int) pos_down = Console.GetCursorPosition();

                        //Counteract termial behaviour
                        Console.SetCursorPosition(pos_down.Item1 - 1, pos_down.Item2);
                        if (val == 0)
                            Console.Write(' ');
                        else
                            Console.Write(val);
                        Console.SetCursorPosition(pos_down.Item1 - 1, pos_down.Item2);

                        //Check that cursor stays within board boundaries
                        if (pos_down.Item2 < 17)
                        {
                            //Message(pos_down.ToString());
                            Console.SetCursorPosition(pos_down.Item1 - 1, pos_down.Item2 + 2);
                            coordinates = (coordinates.Item1, coordinates.Item2 + 1);
                        }
                        else
                            Message("You reached the bottom of the board");
                        break;

                    //Left arrow is clicked event
                    case ConsoleKey.LeftArrow:

                        MessageClear();

                        (int, int) pos_left = Console.GetCursorPosition();

                        //Counteract termial behaviour
                        Console.SetCursorPosition(pos_left.Item1 - 1, pos_left.Item2);
                        if (val == 0)
                            Console.Write(' ');
                        else
                            Console.Write(val);
                        Console.SetCursorPosition(pos_left.Item1 - 1, pos_left.Item2);

                        //Check that cursor stays within board boundaries
                        if (pos_left.Item1 > (Console.WindowWidth / 2) - (board_len / 2) + 2)
                        {
                            //Message(pos_left.ToString());
                            Console.SetCursorPosition(pos_left.Item1 - 5, pos_left.Item2);
                            coordinates = (coordinates.Item1 - 1, coordinates.Item2);
                        }
                        else
                            Message("You reached the left side of the board");
                        break;

                    //Right arrow is clicked event
                    case ConsoleKey.RightArrow:

                        MessageClear();

                        (int, int) pos_right = Console.GetCursorPosition();

                        //Counteract termial behaviour
                        Console.SetCursorPosition(pos_right.Item1 - 1, pos_right.Item2);
                        if (val == 0)
                            Console.Write(' ');
                        else
                            Console.Write(val);
                        Console.SetCursorPosition(pos_right.Item1 - 1, pos_right.Item2);

                        //Check that cursor stays within board boundaries
                        if (pos_right.Item1 < (Console.WindowWidth / 2) + (board_len / 2) - 2)
                        {
                            //Message(pos_right.ToString());
                            Console.SetCursorPosition(pos_right.Item1 + 3, pos_right.Item2);
                            coordinates = (coordinates.Item1 + 1, coordinates.Item2);
                        }
                        else
                            Message("You reached the right side of the board");
                        break;

                    //Number 1-9 is clicked event
                    case ConsoleKey.D1:
                    case ConsoleKey.D2:
                    case ConsoleKey.D3:
                    case ConsoleKey.D4:
                    case ConsoleKey.D5:
                    case ConsoleKey.D6:
                    case ConsoleKey.D7:
                    case ConsoleKey.D8:
                    case ConsoleKey.D9:

                        MessageClear();

                        (int, int) num_pos = Console.GetCursorPosition();

                        //Stops user from changing pre-defined values comparing matrix with old_matrix
                        if (old_matrix[coordinates.Item2, coordinates.Item1] != 0)
                        {
                            Console.SetCursorPosition(num_pos.Item1 - 1, num_pos.Item2);
                            Console.Write(val);
                            Console.SetCursorPosition(num_pos.Item1 - 1, num_pos.Item2);
                            Message("Cannot change pre-defined values");
                        }
                        //Else it's an empty call that may be edited
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Beep();
                            Console.SetCursorPosition(num_pos.Item1 - 1, num_pos.Item2);
                            //Set entered value in matrix and add move to lists
                            moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], num_pos.Item1 - 1, num_pos.Item2, coordinates.Item1, coordinates.Item2));
                            matrix[coordinates.Item2, coordinates.Item1] = (Int32.Parse(key.ToString().Substring(1)));
                            all_moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], num_pos.Item1 - 1, num_pos.Item2, coordinates.Item1, coordinates.Item2));

                            //Check whether board has been completed
                            int counter = 0;
                            if(checkFullBoard()){
                                for(int i = 0; i < 9; i++){
                                    for(int j = 0; j < 9; j++){
                                        if(matrix[i, j] == solution_matrix[i, j])
                                            counter++;
                                    }
                                }
                                //Check that all cells are correct
                                if(counter == 81){
                                    Message("Congratulations! You have completed the board correctly");
                                    Message("Enter R to Replay, M for menu or X to Quit", 21);
                                
                                    //Options to replay, return to main menu or exit application
                                    while(true){
                                        var new_key = Console.ReadKey().Key;
                                        
                                        if(new_key == ConsoleKey.R)
                                            Replay();
                                        else if(new_key == ConsoleKey.M)
                                            menu();
                                        else if(new_key == ConsoleKey.X)
                                            Quit();
                                        else
                                            Message("Invalid option, please enter a valid one");
                                    }
                                

                                }
                                else
                                    Message("The board is not correct");

                                }
                            Console.ForegroundColor = ConsoleColor.White;
                        }

                        break;

                    //Letter D is clicked event - Delete value
                    case ConsoleKey.D:

                        (int, int) d_pos = Console.GetCursorPosition();

                        //Stops user from changing pre-defined values
                        if (old_matrix[coordinates.Item2, coordinates.Item1] != 0)
                        {
                            Replace(val, d_pos);
                            Message("Cannot change pre-defined values");
                        }
                        else
                        {
                            //Replace number with empty space and set value to 0 in matrix
                            Console.SetCursorPosition(d_pos.Item1 - 1, d_pos.Item2);
                            Console.Write(" ");
                            Console.SetCursorPosition(d_pos.Item1 - 1, d_pos.Item2);
                            //Add move to lists
                            moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], d_pos.Item1 - 1, d_pos.Item2, coordinates.Item1, coordinates.Item2));
                            matrix[coordinates.Item2, coordinates.Item1] = 0;
                            all_moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], d_pos.Item1 - 1, d_pos.Item2, coordinates.Item1, coordinates.Item2));
                        }
                        break;

                    //Letter U is clicked event - Undo move
                    case ConsoleKey.U:

                        (int, int) u_pos = Console.GetCursorPosition();
                        //Delete character entered
                        Replace(val, u_pos);

                        //Check if any move has been made
                        if (!moves.Any())
                        {
                            Message("No moves to undo!");
                            break;
                        }

                        //Get last move and delete from list
                        Move move = moves[moves.Count - 1];
                        moves.Remove(moves[moves.Count - 1]);
                        
                        //Undo move
                        Console.SetCursorPosition(move.x, move.y);
                        Replace(move.val, (move.x + 1, move.y));
                        //Update coordinates, add move to lists and change cell to previous state
                        coordinates = (move.matrix_x, move.matrix_y);
                        redo_moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], move.x, move.y, coordinates.Item1, coordinates.Item2));
                        matrix[coordinates.Item2, coordinates.Item1] = move.val;
                        all_moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], move.x - 1, move.y, coordinates.Item1, coordinates.Item2));
                        break;

                    //Letter R is clicked event - Redo move
                    case ConsoleKey.R:

                        (int, int) r_pos = Console.GetCursorPosition();
                        //Replace character enetered
                        Replace(val, r_pos);

                        //Check if any move has been made
                        if (!redo_moves.Any())
                        {
                            Message("No moves to redo!");
                            break;
                        }

                        //Get last move and delete from list
                        Move redo_move = redo_moves[redo_moves.Count - 1];
                        redo_moves.Remove(redo_moves[redo_moves.Count - 1]);

                        //Redo move
                        Console.SetCursorPosition(redo_move.x, redo_move.y);
                        Replace(redo_move.val, (redo_move.x + 1, redo_move.y));
                        //Update coordinates, add move to lists and change cell to previous state
                        coordinates = (redo_move.matrix_x, redo_move.matrix_y);
                        moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], redo_move.x, redo_move.y, coordinates.Item1, coordinates.Item2));
                        matrix[coordinates.Item2, coordinates.Item1] = redo_move.val;
                        all_moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], redo_move.x - 1, redo_move.y, coordinates.Item1, coordinates.Item2));
                        break;

                    //Letter S is clicked event - Save game
                    case ConsoleKey.S:
                        
                        (int, int) s_pos = Console.GetCursorPosition();

                        //Delete character entered
                        Replace(val, s_pos);

                        //Create file if it doesn't exist
                        if(!File.Exists("boards.csv")){
                            File.Create("boards.csv");
                        }

                        //Streamwrite to write values to csv file
                        using(StreamWriter sw = new StreamWriter("boards.csv", true)){
                            
                            //Save changed board - matrix
                            for(int i = 0; i < matrix.GetLength(1); i++){
                                for(int j = 0; j < matrix.GetLength(0); j++){

                                    if(j%8 != 0 || j == 0)
                                        sw.Write(matrix[i, j] + ",");
                                    else
                                        sw.Write(matrix[i, j]);

                                }
                                sw.Write("\n");
                            }

                            sw.Write("\n");

                            //Save original board - old_matrix
                            for(int i = 0; i < old_matrix.GetLength(1); i++){
                                for(int j = 0; j < old_matrix.GetLength(0); j++){
                                    
                                    if(j%8 != 0 || j == 0)
                                        sw.Write(old_matrix[i, j] + ",");
                                    else
                                        sw.Write(old_matrix[i, j]);

                                }
                                sw.Write("\n");
                            }

                            sw.Write("\n");
                            sw.Close();
                        }
                        
                        Message("Game saved");

                        break;

                    //Letter M is clicked event - Menu
                    case ConsoleKey.M:

                        (int, int) m_pos = Console.GetCursorPosition();

                        Replace(val, m_pos);

                        //User warning
                        Message("Have you saved the game? If not, your progress might be lost. Are you sure you want to leave? (Y/N)");
                        while(true){

                            //Read key
                            var m_key = Console.ReadKey().Key;

                            Replace(val, m_pos);

                            //Return to menu or game
                            if(m_key == ConsoleKey.Y)
                                menu();
                            else if(m_key == ConsoleKey.N)
                                Game();
                            else
                                Message("Enter Y to go to the Main Menu or N to go back to the game", 21);
                        }
                        break;

                    //Letter V is clicked event - Solve board
                    case ConsoleKey.V:

                        (int, int) v_pos = Console.GetCursorPosition();

                        //Retrieve original board
                        Replace(val, v_pos);
                        for(int i = 0; i < 9; i++){
                            for(int j = 0; j < 9; j++){
                                matrix[i, j] = old_matrix[i, j];
                            }
                        }

                        //Call solving algorithm
                        SolveBoard();
                        
                        //Options to replay, return to main menu or quit
                        while(true){
                            var v_key = Console.ReadKey().Key;

                            Replace(val, v_pos);

                            if(v_key == ConsoleKey.M)
                                menu();
                            else if(v_key == ConsoleKey.X)
                                Quit();
                            else if(v_key == ConsoleKey.R)
                                Replay();
                            else{
                                Message("Enter R to Replay, M for menu or X to Quit", 21);
                                Console.SetCursorPosition(v_pos.Item1-1, v_pos.Item2);
                            }
                        }
                        break;

                    //Letter X is clicked event - Exit application
                    case ConsoleKey.X:
                        Quit();
                        break;

                    //Undefined character is pressed
                    default:

                        (int, int) new_pos = Console.GetCursorPosition();
                        int diff = new_pos.Item1 - pos.Item1;

                        if (new_pos == pos)
                            break;

                        //Fix board layout
                        else if (diff > 1)
                        {
                            Console.SetCursorPosition(pos.Item1, pos.Item2);
                            Console.Write(" |");
                            Console.SetCursorPosition(pos.Item1, pos.Item2);
                        }
                        else
                        {
                            Console.SetCursorPosition(pos.Item1, pos.Item2);
                            if (val == 0)
                                Console.Write(' ');
                            else
                                Console.Write(val);
                            Console.SetCursorPosition(pos.Item1, pos.Item2);
                        }

                        //User warning
                        Message("Please enter a valid value");
                        break;
                }
                Console.ForegroundColor = ConsoleColor.White;

            }
        }

        /*
        * Message method - used to display different messages to the user in different parts of the screen
        * int top - distance from top of screen
        * int left - distance from left of screen
        */
        static void Message(String msg, int top = 20, int left = 1)
        {
            Console.ForegroundColor = ConsoleColor.White;
            (int, int) pos = Console.GetCursorPosition();
            Console.SetCursorPosition(left, top);
            Console.WriteLine(msg + new string(' ', Console.WindowWidth - msg.Length));
            Console.SetCursorPosition(pos.Item1, pos.Item2);
        }

        /*
        * MessageClear method - used to clear previously written message
        * int top - distance from top of screen
        */
        static void MessageClear(int top = 20)
        {
            (int, int) pos = Console.GetCursorPosition();
            Console.SetCursorPosition(0, top);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(pos.Item1, pos.Item2);
        }

        /*
        * Replace method - used counteract console behaviour, replaces user entered with overwritten value
        * int val - value that was overwritten
        * (int, int) pos - coordinated of that value
        */
        static void Replace(int val, (int, int) pos)
        {
            Console.SetCursorPosition(pos.Item1 - 1, pos.Item2);
            if (val == 0)
            {
                Console.Write(' ');
            }
            else
            {
                Console.Write(val);
            }
            Console.SetCursorPosition(pos.Item1 - 1, pos.Item2);
        }

        /*
        * LoadGame method - used load previously saved games from the boards.csv file
        * String msg - used to display messages to user
        */
        static void LoadGame(String msg = ""){

            //Directory to file
            String dir = "boards.csv";

            //Check if file exists
            if(!File.Exists(dir) || new FileInfo(dir).Length == 0){
                menu("No games have been played");
            }
            
            Console.Clear();

            int num_saved_games = 0;

            //Streamreader to read csv file
            using(StreamReader sr = new StreamReader(dir)){
                
                int line_count = File.ReadAllLines(dir).Count();    //Amount of lines in csv
                num_saved_games = line_count/20;                    //Amount of saved games
                String[] lines = File.ReadAllLines(dir);            //All the contents of the file

                //Display all boards on console so that user can see all saved games
                for(int current_board = 0; current_board < num_saved_games; current_board++){      //Board being procesed

                    //Skip to relevant board in file
                    if(current_board != 0){
                        for(int i = 0; i < 11; i++){
                            sr.ReadLine();
                        }
                    }  

                    //Temporarily load board to program
                    for(int i = 0; i < matrix.GetLength(1); i++){
                    
                        int[] values = Array.ConvertAll(sr.ReadLine().Split(','), int.Parse);       //Current line
                    
                        for(int j = 0; j < matrix.GetLength(0); j++){
                            matrix[i, j] = values[j];
                        }
                    }

                    //Output board to console. The second and third parameter determine where the board is placed in console
                    if(current_board%3 == 0)
                        DisplayBoard('x', 0, (current_board-current_board%3)/3);
                    else if(current_board%3 == 1)
                        DisplayBoard('x', 1, (current_board-current_board%3)/3);
                    else
                        DisplayBoard('x', 2, (current_board-current_board%3)/3);
                }

            }

            //User can choose which board to load
            Message("Select a game to load   1 - " + num_saved_games + ", X to quit or M for the Main Menu ", Convert.ToInt32(21*Math.Ceiling((double)num_saved_games/3)));

            Console.SetCursorPosition(1, Convert.ToInt32(21*Math.Ceiling((double)num_saved_games/3))+1);

            Message(msg, Convert.ToInt32(21*Math.Ceiling((double)num_saved_games/3))+4);

            //Choose board number
            var key = Console.ReadKey();

            //Quit program if X is entered
            if(key.Key.Equals(ConsoleKey.X)){
                Quit();
            }

            //Back to main menu if M is enetred
            if(key.Key.Equals(ConsoleKey.M))
                menu();

            //Error handling - not a number
            if((int.TryParse(key.Key.ToString().Substring(1), out int val)) == false){
                LoadGame("Please enter a value");
            }

            int num_key = Int32.Parse(key.Key.ToString().Substring(1));

            //Error handling - not a valid board number
            if(!(num_key <= num_saved_games) || !(num_key > 0)){
                LoadGame("There are only " + num_saved_games + " saved games, enter a valid game");
            }

            //Load user chosen board into program
            using(StreamReader sr = new StreamReader(dir)){

                //Skip to relevant board
                if(num_key != 1){
                    for(int i = 0; i < 20*num_key-20; i++){
                        sr.ReadLine();
                    }
                }
                
                //Load user chosen board into program
                for(int i = 0; i < matrix.GetLength(1); i++){
                    
                    int[] values = Array.ConvertAll(sr.ReadLine().Split(','), int.Parse);       //Current line
                    
                    for(int j = 0; j < matrix.GetLength(0); j++){
                        matrix[i, j] = values[j];
                    }
                }
                
                sr.ReadLine();

                //Load user chosen original board into program (used if user wants the program to solve the board automatically)
                for(int i = 0; i < matrix.GetLength(1); i++){
                    
                    int[] values = Array.ConvertAll(sr.ReadLine().Split(','), int.Parse);       //Current line
                    
                    for(int j = 0; j < matrix.GetLength(0); j++){
                        old_matrix[i, j] = values[j];
                    }
                }
            }

            //Delete loaded board so boards aren't saved twice
            List<String> lines_list = File.ReadAllLines(dir).ToList();
            for(int i = 0; i < 20; i++){
                lines_list.RemoveAt((20*num_key-20));
            }
            File.WriteAllLines(dir, lines_list.ToArray());

            //Play the board
            Game();

        }

        /*
        * Replay method - used to replay moves after game is completed
        */
        static void Replay(){

            //Retrieve original board
            for(int i = 0; i < 9; i++){
                for(int j = 0; j < 9; j++){
                    matrix[i, j] = old_matrix[i, j];
                }
            }

            //Display move and wait 1.5s
            DisplayBoard();
            Thread.Sleep(1500);

            //Display all the moves done, one by one
            for(int i = 0; i < all_moves.Count; i++){
                matrix[all_moves[i].matrix_y, all_moves[i].matrix_x] = all_moves[i].val;
                
                Thread.Sleep(1000);
                DisplayBoard();
            }

        }


        /*
        * CreateBoard algorithm - used to generate a complete new board
        * int diff - degree of difficulty
        */
        static void CreateBoard(int diff)
        {
            //Array of valid values
            int[] nums = {1, 2, 3, 4, 5, 6, 7, 8, 9};

            //Call to shuffling algorithm to achieve randomness
            nums = Shuffle(nums);

            int x = create_matrix.GetLength(0);
            int y = create_matrix.GetLength(1);

            //For every row and column, if the selected cell is free, try entering each value from shuffled
            //array and check whether there are violations using the Create_checkValue algorithm. If there are
            //no violations, set value and continue to next cell, otherwise try again. This algorithm uses 
            //recursion and can therefore backtrack if no possbile combination is found down the process.
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (create_matrix[i, j] == 0)
                    {
                        foreach(int val in nums)
                        {
                            if (Create_checkValue(i, j, val))
                            {
                                create_matrix[i, j] = val;
                                CreateBoard(diff);
                                create_matrix[i, j] = 0;
                            }
                        }
                        return;
                    }
                }
            }
            //Once a full board has been found prepare it
            PrepareBoard(diff);
        }

        /*
        * PrepareBoard algorithm - used to remove values from complete board
        * int diff - degree of difficulty
        */
        static void PrepareBoard(int diff){

            //Save solution to matrix
            for(int i = 0; i < 9; i++){
                for(int j = 0; j < 9; j++){
                    solution_matrix[i, j] = create_matrix[i, j];
                }
            }

            //Conduct algorithm as often as specified difficulty
            while(diff>0){

                //Choose random cell
                int x = rand.Next(0, 9);
                int y = rand.Next(0, 9);

                //If cell is not yet free, try removing a value and check whether there is still a single solution
                //using the CreateSolve algorithm
                if(create_matrix[x, y] != 0){

                    //save value
                    int redo = create_matrix[x, y];

                    create_matrix[x, y] = 0;

                    create_matrix_copy = create_matrix;

                    counter = 0;
                    CreateSolve();

                    //Replace value if multiple solutions are found
                    if(counter > 1){
                        create_matrix[x, y] = redo;
                    }
                    diff--;
                }
            }

            //Save boards
            for(int i = 0; i < 9; i++){
                for(int j = 0; j < 9; j++){
                    matrix[i, j] = create_matrix[i, j];
                }
            }

            for(int i = 0; i < 9; i++){
                for(int j = 0; j < 9; j++){
                    old_matrix[i, j] = matrix[i, j];
                }
            }
            //Start game
            Game();
        }

        /*
        * Create_checkValue algorithm - used to check for violations
        * int x - x coordinate
        * int y - y coordinate
        * int num - proposed value
        */
        static bool Create_checkValue(int x, int y, int num)
        {
            int hor = create_matrix.GetLength(0);
            int ver = create_matrix.GetLength(1);

            //Check for vertical violations
            for (int i = 0; i < hor; i++)
            {
                if (create_matrix[x, i] == num)
                    return false;
            }

            //Check for horizontal violations
            for (int i = 0; i < ver; i++)
            {
                if (create_matrix[i, y] == num)
                    return false;
            }

            //Check 3x3 sqaures violations
            int xx = (x / 3) * 3;
            int yy = (y / 3) * 3;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (create_matrix[xx + i, yy + j] == num)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /*
        * CreateSolve algorithm - used to check for multiple solutions upon creating a board
        */
        static void CreateSolve()
        {
            int x = create_matrix_copy.GetLength(0);
            int y = create_matrix_copy.GetLength(1);

            //Same backtracking algorithm previously used
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (create_matrix_copy[i, j] == 0)
                    {
                        for (int num = 1; num < 10; num++)
                        {
                            if (Create_checkValue(i, j, num))
                            {
                                create_matrix_copy[i, j] = num;
                                CreateSolve();
                                create_matrix_copy[i, j] = 0;
                            }
                        }
                        return;
                    }
                }
            }
            counter++;
        }

        /*
        * SolveBoard algorithm - used to solve board
        */
        static void SolveBoard()
        {
            int x = matrix.GetLength(0);
            int y = matrix.GetLength(1);

            //Same backtracking algorithm previously used
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        for (int num = 1; num < 10; num++)
                        {
                            if (checkValue(i, j, num))
                            {
                                matrix[i, j] = num;
                                SolveBoard();
                                matrix[i, j] = 0;
                            }
                        }
                        return;
                    }
                }
            }

            //Winning message
            DisplayBoard();
            Message("Sudoku solved!!! Press (R) to Replay or the (M)enu button to return to the main menu");
        }

        /*
        * checkValue algorithm - used to check for board violations
        */
        static bool checkValue(int x, int y, int num)
        {
            int hor = matrix.GetLength(0);
            int ver = matrix.GetLength(1);

            //Check for vertical violations
            for (int i = 0; i < hor; i++)
            {
                if (matrix[x, i] == num)
                    return false;
            }

            //Check for horizontal violations
            for (int i = 0; i < ver; i++)
            {
                if (matrix[i, y] == num)
                    return false;
            }
            //Check 3x3 sqaures violations
            int xx = (x / 3) * 3;
            int yy = (y / 3) * 3;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[xx + i, yy + j] == num)
                    {
                        return false;
                    }
                }
            }


            return true;
        }

        /*
        * checkFullBoard algorithm - used to check whether to board is full
        */
        static bool checkFullBoard()
        {
            int x = matrix.GetLength(0);
            int y = matrix.GetLength(1);

            //Check whether all cells are not equal to 0
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if (matrix[i, j] == 0)
                        return false;
                }
            }

            return true;
        }

        /*
        * Shuffle algorithm - used to shuffle array
        */
        public static int[] Shuffle (int[] array)
        {
            int n = array.Length;

            //Randomize array
            for(int i = 0; i < array.Length; i++){
                int ran = rand.Next(n--);
                int temp = array[n];
                array[n] = array[ran];
                array[ran] = temp;
            }

            //Return randomized array
            return array;
        }

        /*
        * DisplayBoard algorithm - used to Display board to console
        * char margin - check whether to center the board
        * int x_pos - x position on screen 1 - 3
        * int y_pos - y position on screen 1 - 3
        */
        static void DisplayBoard(char margin = 'c', int x_pos = 0, int y_pos = 0)
        {
            //Centered option
            if(margin == 'c'){
                Console.Clear();
                
                //Top of board
                String left_margin = new string(' ', (Console.WindowWidth - board_len) / 2);

                Console.Write(left_margin + "╔═══╤═══╤═══╦═══╤═══╤═══╦═══╤═══╤═══╗");

                //Enter values
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    Console.Write("\n");

                    Console.Write(left_margin + "║");

                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j] == 0)
                        {
                            if (j % 3 == 2)
                                Console.Write("   ║");
                            else
                                Console.Write("   │");
                        }
                        else
                        {
                            if (j % 3 == 2)
                                Console.Write(" " + matrix[i, j] + " ║");
                            else
                                Console.Write(" " + matrix[i, j] + " │");
                        }

                    }
                    int ii = i + 1;
                    if (ii < matrix.GetLength(0) && i % 3 == 2)
                        Console.Write("\n" + left_margin + "╠═══╪═══╪═══╬═══╪═══╪═══╬═══╪═══╪═══╣");
                    else if (ii < matrix.GetLength(0))
                        Console.Write("\n" + left_margin + "╟───┼───┼───╫───┼───┼───╫───┼───┼───╢");


                    }

                //Bottom of board
                Console.Write("\n" + left_margin + "╚═══╧═══╧═══╩═══╧═══╧═══╩═══╧═══╧═══╝");
            }
            //If not centered
            else{
                
                //Top of board
                String left_margin = new string(' ', (Console.WindowWidth/3 - board_len - 5));

                Console.SetCursorPosition((left_margin.Length*x_pos)+(board_len*x_pos), 1 + 21*y_pos);

                Console.Write(left_margin + "╔═══╤═══╤═══╦═══╤═══╤═══╦═══╤═══╤═══╗");
                
                //Enter values
                for (int i = 0; i < matrix.GetLength(0); i++)
                {
                    Console.SetCursorPosition((left_margin.Length*x_pos)+(board_len*x_pos), (21*y_pos) + i*2+2);

                    Console.Write(left_margin + "║");

                    for (int j = 0; j < matrix.GetLength(1); j++)
                    {
                        if (matrix[i, j] == 0)
                        {
                            if (j % 3 == 2)
                                Console.Write("   ║");
                            else
                                Console.Write("   │");
                        }
                        else
                        {
                            if (j % 3 == 2)
                                Console.Write(" " + matrix[i, j] + " ║");
                            else
                                Console.Write(" " + matrix[i, j] + " │");
                        }

                    }
                    
                    int ii = i + 1;
                    if (ii < matrix.GetLength(0) && i % 3 == 2){
                        
                        Console.SetCursorPosition((left_margin.Length*x_pos)+(board_len*x_pos), (21*y_pos) + i*2+3);
                        Console.Write(left_margin + "╠═══╪═══╪═══╬═══╪═══╪═══╬═══╪═══╪═══╣");
                    }
                        
                    else if (ii < matrix.GetLength(0)){

                        Console.SetCursorPosition((left_margin.Length*x_pos)+(board_len*x_pos), (21*y_pos) + i*2+3);
                        Console.Write(left_margin + "╟───┼───┼───╫───┼───┼───╫───┼───┼───╢");
                    }

                }
                //Bottom of board
                Console.SetCursorPosition((left_margin.Length*x_pos)+(board_len*x_pos), (21*y_pos) + 19);
                Console.Write(left_margin + "╚═══╧═══╧═══╩═══╧═══╧═══╩═══╧═══╧═══╝");
            }
        }

        /*
        * DisplayOptions method - display game options to user
        */
        static void DisplayOptions()
        {
            //Array of all options
            String[] options = { "D - DELETE", "U - UNDO", "R - REDO", "M - MENU", "V - SOLVE", "S - SAVE", "X - QUIT" };

            //Display all options
            for (int i = 0; i < options.Length; i++)
            {
                Console.SetCursorPosition((Console.WindowWidth / 2) + board_len, i + 2);
                Console.Write(options[i]);
            }
        }

        /*
        * Method to display rules to user
        */
        static void Rules()
        {

            //Clear console
            Console.Clear();

            //Get all text from file
            String[] text = File.ReadAllLines("rules.txt");

            //Deleting comment
            text[0] = "";

            //Display rules
            foreach (String line in text)
            {
                Console.WriteLine(' ' + line);
            }
        }

        /*
        * RulesMenu method - Options at end of rules
        */
        static void RulesMenu()
        {
            Console.WriteLine("\n\n Click (M)enu to return to back menu or (X) to quit program");

            //Read key
            var key = Console.ReadKey().Key;

            //Return to main menu or exit application
            while (key != ConsoleKey.M || key != ConsoleKey.X)
            {
                if (key == ConsoleKey.M)
                    menu();
                else if (key == ConsoleKey.X)
                {
                    Quit();
                }
                else
                {
                    Message("Invalid option, please try again", 45);
                }
            }
        }

        /*
        * Welcome method - Big Welcome to Sudoku text
        */
        static void Welcome()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            String welcome =

new string(' ', Console.WindowWidth / 50) + @"__/\\\______________/\\\_________________/\\\\\\_________________________________________________________________________________________________________        " + "\n" +
new string(' ', Console.WindowWidth / 50) + @" _\/\\\_____________\/\\\________________\////\\\_________________________________________________________________________________________________________       " + "\n" +
new string(' ', Console.WindowWidth / 50) + @"  _\/\\\_____________\/\\\___________________\/\\\_________________________________________________________________________________/\\\____________________      " + "\n" +
new string(' ', Console.WindowWidth / 50) + @"   _\//\\\____/\\\____/\\\______/\\\\\\\\_____\/\\\________/\\\\\\\\_____/\\\\\_______/\\\\\__/\\\\\_______/\\\\\\\\_____________/\\\\\\\\\\\_____/\\\\\____     " + "\n" +
new string(' ', Console.WindowWidth / 50) + @"    __\//\\\__/\\\\\__/\\\_____/\\\/////\\\____\/\\\______/\\\//////____/\\\///\\\___/\\\///\\\\\///\\\___/\\\/////\\\___________\////\\\////____/\\\///\\\__    " + "\n" +
new string(' ', Console.WindowWidth / 50) + @"     ___\//\\\/\\\/\\\/\\\_____/\\\\\\\\\\\_____\/\\\_____/\\\__________/\\\__\//\\\_\/\\\_\//\\\__\/\\\__/\\\\\\\\\\\_______________\/\\\_______/\\\__\//\\\_   " + "\n" +
new string(' ', Console.WindowWidth / 50) + @"      ____\//\\\\\\//\\\\\_____\//\\///////______\/\\\____\//\\\________\//\\\__/\\\__\/\\\__\/\\\__\/\\\_\//\\///////________________\/\\\_/\\__\//\\\__/\\\__  " + "\n" +
new string(' ', Console.WindowWidth / 50) + @"       _____\//\\\__\//\\\_______\//\\\\\\\\\\__/\\\\\\\\\__\///\\\\\\\\__\///\\\\\/___\/\\\__\/\\\__\/\\\__\//\\\\\\\\\\______________\//\\\\\____\///\\\\\/___ " + "\n" +
new string(' ', Console.WindowWidth / 50) + @"        ______\///____\///_________\//////////__\/////////_____\////////_____\/////_____\///___\///___\///____\//////////________________\/////_______\/////_____";

            String sudoku =

new string(' ', Console.WindowWidth / 5) + @"_____/\\\\\\\\\\\____/\\\________/\\\__/\\\\\\\\\\\\__________/\\\\\_______/\\\________/\\\__/\\\________/\\\_" + "\n" +
new string(' ', Console.WindowWidth / 5) + @" ___ /\\\/////////\\\_\/\\\_______\/\\\_\/\\\////////\\\______/\\\///\\\____\/\\\_____/\\\//__\/\\\_______\/\\\_       " + "\n" +
new string(' ', Console.WindowWidth / 5) + @"  __\//\\\______\///__\/\\\_______\/\\\_\/\\\______\//\\\___/\\\/__\///\\\__\/\\\__/\\\//_____\/\\\_______\/\\\_      " + "\n" +
new string(' ', Console.WindowWidth / 5) + @"   ___\////\\\_________\/\\\_______\/\\\_\/\\\_______\/\\\__/\\\______\//\\\_\/\\\\\\//\\\_____\/\\\_______\/\\\_     " + "\n" +
new string(' ', Console.WindowWidth / 5) + @"    ______\////\\\______\/\\\_______\/\\\_\/\\\_______\/\\\_\/\\\_______\/\\\_\/\\\//_\//\\\____\/\\\_______\/\\\_    " + "\n" +
new string(' ', Console.WindowWidth / 5) + @"     _________\////\\\___\/\\\_______\/\\\_\/\\\_______\/\\\_\//\\\______/\\\__\/\\\____\//\\\___\/\\\_______\/\\\_   " + "\n" +
new string(' ', Console.WindowWidth / 5) + @"      _ /\\\______\//\\\__\//\\\______/\\\__\/\\\_______/\\\___\///\\\__/\\\____\/\\\_____\//\\\__\//\\\______/\\\__  " + "\n" +
new string(' ', Console.WindowWidth / 5) + @"       _\///\\\\\\\\\\\/____\///\\\\\\\\\/___\/\\\\\\\\\\\\/______\///\\\\\/_____\/\\\______\//\\\__\///\\\\\\\\\/___ " + "\n" +
new string(' ', Console.WindowWidth / 5) + @"        ___\///////////________\/////////_____\////////////__________\/////_______\///________\///_____\/////////_____";



            Console.WriteLine("\n" + welcome + "\n\n" + sudoku + "\n\n\n");

            Console.ForegroundColor = ConsoleColor.White;
        }

        /*
        * Quit method - used to clear console and exit Application
        */
        static void Quit(){
            Console.Clear();
            Environment.Exit(1);
        }
    }
}

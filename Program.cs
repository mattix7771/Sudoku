using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Sudoku
{
    class Program
    {
        static (int, int) coordinates = (0, 0);
        const int board_len = 37;
        static List<Move> moves = new List<Move>();
        static List<Move> redo_moves = new List<Move>();
        static List<Move> all_moves = new List<Move>();
        static Random rand = new Random();
        static int counter = 0;

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
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.ForegroundColor = ConsoleColor.White;
            //Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.SetWindowSize(170, 41);


            menu();



        }

        static void menu(String msg = "")
        {
            
            Console.Clear();
            Welcome();
            
            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "Enter the option's number to start");

            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "1. Play New Game");
            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "2. Play Saved Game");
            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "3. How to play");
            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "4. Quit");
            
            Message(msg, 35);

            int top = Console.CursorTop;
            Console.SetCursorPosition(Console.WindowWidth / 3, top);

            var key = Console.ReadKey().Key;

            switch (key)
            {
                case ConsoleKey.D1:

                    Console.WriteLine("\n " + new string(' ', Console.WindowWidth / 3) + "Choose a difficulty:");
                    Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "1. Easy");
                    Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "2. Medium");
                    Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "3. Hard");
                    Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "4. Extreme");

                    var new_key = Console.ReadKey().Key;

                    switch(new_key){

                        case ConsoleKey.D1:
                                Console.Clear();
                                Message("Your board is being created", 8, 60);
                                
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
                                CreateBoard(30);
                            break;

                        case ConsoleKey.D2:
                                Console.Clear();
                                Message("Your board is being created", 8, 60);

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
                                CreateBoard(45);
                            break;

                        case ConsoleKey.D3:
                                Console.Clear();
                                Message("Your board is being created", 8, 60);

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
                                CreateBoard(60);
                            break;

                        case ConsoleKey.D4:
                                Console.Clear();
                                Message("Your board is being created", 8, 60);

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
                                CreateBoard(80);
                            break;

                        default:
                                menu("Invalid option, enter a valid one");
                            break;

                    }
                    break;

                case ConsoleKey.D2:
                    Console.Clear();
                    LoadGame();
                    break;

                case ConsoleKey.D3:
                    Rules();
                    RulesMenu();
                    break;

                case ConsoleKey.D4:
                    Quit();
                    break;

                default:
                    menu("Invalid option, enter a valid one");
                    break;
            }
        }


        static void Game()
        {
            Console.Clear();

            coordinates = (0, 0);

            DisplayBoard();
            DisplayOptions();

            Console.SetCursorPosition((Console.WindowWidth / 2) - (board_len / 2) + 1, 1); //First position

            while (true)
            {
                (int, int) pos = Console.GetCursorPosition();
                int val = matrix[coordinates.Item2, coordinates.Item1];

                var key = Console.ReadKey().Key;




                switch (key)
                {
                    case ConsoleKey.UpArrow:

                        MessageClear();

                        /*
                        By default, upon clicking any arrow key in the terminal while also being inside a running application,
                        the terminal will overwrite any character upon which the cursor is on and move one unit to the right.
                        The following few lines of code are to couteract it. Similar code is also present on the cases for the 
                        other arrow keys.
                        */

                        (int, int) pos_up = Console.GetCursorPosition();

                        Console.SetCursorPosition(pos_up.Item1 - 1, pos_up.Item2);
                        if (val == 0)
                            Console.Write(' ');
                        else
                            Console.Write(val);
                        Console.SetCursorPosition(pos_up.Item1 - 1, pos_up.Item2);

                        if (pos_up.Item2 > 1)
                        {
                            //Message(pos_up.ToString());
                            Console.SetCursorPosition(pos_up.Item1 - 1, pos_up.Item2 - 2);
                            coordinates = (coordinates.Item1, coordinates.Item2 - 1);
                        }
                        else
                            Message("You reached the top of the board");
                        break;

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


                        if (pos_down.Item2 < 17)
                        {
                            //Message(pos_down.ToString());
                            Console.SetCursorPosition(pos_down.Item1 - 1, pos_down.Item2 + 2);
                            coordinates = (coordinates.Item1, coordinates.Item2 + 1);
                        }
                        else
                            Message("You reached the bottom of the board");
                        break;

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


                        if (pos_left.Item1 > (Console.WindowWidth / 2) - (board_len / 2) + 2)
                        {
                            //Message(pos_left.ToString());
                            Console.SetCursorPosition(pos_left.Item1 - 5, pos_left.Item2);
                            coordinates = (coordinates.Item1 - 1, coordinates.Item2);
                        }
                        else
                            Message("You reached the left side of the board");
                        break;

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


                        if (pos_right.Item1 < (Console.WindowWidth / 2) + (board_len / 2) - 2)
                        {
                            //Message(pos_right.ToString());
                            Console.SetCursorPosition(pos_right.Item1 + 3, pos_right.Item2);
                            coordinates = (coordinates.Item1 + 1, coordinates.Item2);
                        }
                        else
                            Message("You reached the right side of the board");
                        break;

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

                        //Stops user from changing pre-defined values
                        if (old_matrix[coordinates.Item2, coordinates.Item1] != 0)
                        {
                            Console.SetCursorPosition(num_pos.Item1 - 1, num_pos.Item2);
                            Console.Write(val);
                            Console.SetCursorPosition(num_pos.Item1 - 1, num_pos.Item2);
                            Message("Cannot change pre-defined values");
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Beep();
                            Console.SetCursorPosition(num_pos.Item1 - 1, num_pos.Item2);
                            moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], num_pos.Item1 - 1, num_pos.Item2, coordinates.Item1, coordinates.Item2));
                            matrix[coordinates.Item2, coordinates.Item1] = (Int32.Parse(key.ToString().Substring(1)));
                            all_moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], num_pos.Item1 - 1, num_pos.Item2, coordinates.Item1, coordinates.Item2));

                            int counter = 0;
                            if(checkFullBoard()){
                                for(int i = 0; i < 9; i++){
                                    for(int j = 0; j < 9; j++){
                                        if(matrix[i, j] == solution_matrix[i, j])
                                            counter++;
                                    }
                                }
                            
                                if(counter == 81){
                                    Message("Congratulations! You have completed the board correctly");
                                    Message("Enter R to Replay, M for menu or X to Quit", 21);
                                
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
                            moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], d_pos.Item1 - 1, d_pos.Item2, coordinates.Item1, coordinates.Item2));
                            matrix[coordinates.Item2, coordinates.Item1] = 0;
                            all_moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], d_pos.Item1 - 1, d_pos.Item2, coordinates.Item1, coordinates.Item2));
                        }
                        break;

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
                        coordinates = (move.matrix_x, move.matrix_y);
                        redo_moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], move.x, move.y, coordinates.Item1, coordinates.Item2));
                        matrix[coordinates.Item2, coordinates.Item1] = move.val;
                        all_moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], move.x - 1, move.y, coordinates.Item1, coordinates.Item2));
                        break;

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
                        coordinates = (redo_move.matrix_x, redo_move.matrix_y);
                        moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], redo_move.x, redo_move.y, coordinates.Item1, coordinates.Item2));
                        matrix[coordinates.Item2, coordinates.Item1] = redo_move.val;
                        all_moves.Add(new Move(matrix[coordinates.Item2, coordinates.Item1], redo_move.x - 1, redo_move.y, coordinates.Item1, coordinates.Item2));
                        break;

                    //save game
                    case ConsoleKey.S:
                        
                        (int, int) s_pos = Console.GetCursorPosition();

                        //Delete character entered
                        Replace(val, s_pos);

                        //Create file if it foesn't exist
                        if(!File.Exists("boards.csv")){
                            File.Create("boards.csv");
                        }

                        //Streamwrite to write values to csv file
                        using(StreamWriter sw = new StreamWriter("boards.csv", true)){
                            
                            //Save changed board
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

                            //Save original board
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

                    case ConsoleKey.M:

                        (int, int) m_pos = Console.GetCursorPosition();

                        Replace(val, m_pos);

                        Message("Have you saved the game? If not, your progress might be lost. Are you sure you want to leave? (Y/N)");
                        while(true){
                            var m_key = Console.ReadKey().Key;

                            Replace(val, m_pos);

                            if(m_key == ConsoleKey.Y)
                                menu();
                            else if(m_key == ConsoleKey.N)
                                Game();
                            else
                                Message("Enter Y to go to the Main Menu or N to go back to the game", 21);
                        }
                        break;

                    case ConsoleKey.V:

                        (int, int) v_pos = Console.GetCursorPosition();

                        Replace(val, v_pos);
                        for(int i = 0; i < 9; i++){
                            for(int j = 0; j < 9; j++){
                                matrix[i, j] = old_matrix[i, j];
                            }
                        }
                        SolveBoard();
                        
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

                    case ConsoleKey.X:
                        Quit();
                        break;

                    default:

                        (int, int) new_pos = Console.GetCursorPosition();
                        int diff = new_pos.Item1 - pos.Item1;

                        if (new_pos == pos)
                            break;
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


                        Message("Please enter a valid value");
                        break;
                }
                Console.ForegroundColor = ConsoleColor.White;

            }
        }


        static void Message(String msg, int top = 20, int left = 1)
        {
            Console.ForegroundColor = ConsoleColor.White;
            (int, int) pos = Console.GetCursorPosition();
            Console.SetCursorPosition(left, top);
            Console.WriteLine(msg + new string(' ', Console.WindowWidth - msg.Length));
            Console.SetCursorPosition(pos.Item1, pos.Item2);
        }

        static void MessageClear(int top = 20)
        {
            (int, int) pos = Console.GetCursorPosition();
            Console.SetCursorPosition(0, top);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(pos.Item1, pos.Item2);
        }

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

        static void LoadGame(String msg = ""){

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

        static void Replay(){

            for(int i = 0; i < 9; i++){
                for(int j = 0; j < 9; j++){
                    matrix[i, j] = old_matrix[i, j];
                }
            }

            DisplayBoard();
            Thread.Sleep(1500);

            for(int i = 0; i < all_moves.Count; i++){
                matrix[all_moves[i].matrix_y, all_moves[i].matrix_x] = all_moves[i].val;
                
                Thread.Sleep(1000);
                DisplayBoard();
            }

        }

        static void CreateBoard(int diff)
        {
            int[] nums = {1, 2, 3, 4, 5, 6, 7, 8, 9};
            nums = Shuffle(nums);

            int x = create_matrix.GetLength(0);
            int y = create_matrix.GetLength(1);

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

        static void PrepareBoard(int diff){

            for(int i = 0; i < 9; i++){
                for(int j = 0; j < 9; j++){
                    solution_matrix[i, j] = create_matrix[i, j];
                }
            }

            while(diff>0){

                int x = rand.Next(0, 9);
                int y = rand.Next(0, 9);

                if(create_matrix[x, y] != 0){

                    int redo = create_matrix[x, y];

                    create_matrix[x, y] = 0;

                    create_matrix_copy = create_matrix;

                    counter = 0;
                    CreateSolve();
                    if(counter > 1){
                        create_matrix[x, y] = redo;
                    }
                    diff--;

                }

            }

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
            Game();
        }



        static bool Create_checkValue(int x, int y, int num)
        {
            int hor = create_matrix.GetLength(0);
            int ver = create_matrix.GetLength(1);

            for (int i = 0; i < hor; i++)
            {
                if (create_matrix[x, i] == num)
                    return false;
            }
            for (int i = 0; i < ver; i++)
            {
                if (create_matrix[i, y] == num)
                    return false;
            }
            //somehow check 3x3 sqaures
            int x0 = (x / 3) * 3;
            int y0 = (y / 3) * 3;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (create_matrix[x0 + i, y0 + j] == num)
                    {
                        return false;
                    }
                }
            }


            return true;
        }

        static void CreateSolve()
        {
            int x = create_matrix_copy.GetLength(0);
            int y = create_matrix_copy.GetLength(1);

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

        static void SolveBoard()
        {
            int x = matrix.GetLength(0);
            int y = matrix.GetLength(1);

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

            DisplayBoard();
            Message("Sudoku solved!!! Press (R) to Replay or the (M)enu button to return to the main menu");
        }

        static bool checkValue(int x, int y, int num)
        {
            int hor = matrix.GetLength(0);
            int ver = matrix.GetLength(1);

            for (int i = 0; i < hor; i++)
            {
                if (matrix[x, i] == num)
                    return false;
            }
            for (int i = 0; i < ver; i++)
            {
                if (matrix[i, y] == num)
                    return false;
            }
            //somehow check 3x3 sqaures
            int x0 = (x / 3) * 3;
            int y0 = (y / 3) * 3;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (matrix[x0 + i, y0 + j] == num)
                    {
                        return false;
                    }
                }
            }


            return true;
        }

        static bool checkFullBoard()
        {
            int x = matrix.GetLength(0);
            int y = matrix.GetLength(1);

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

        static void DisplayBoard(char margin = 'c', int x_pos = 0, int y_pos = 0)
        {
            if(margin == 'c'){
                Console.Clear();

                String left_margin = new string(' ', (Console.WindowWidth - board_len) / 2);

                Console.Write(left_margin + "╔═══╤═══╤═══╦═══╤═══╤═══╦═══╤═══╤═══╗");

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
                Console.Write("\n" + left_margin + "╚═══╧═══╧═══╩═══╧═══╧═══╩═══╧═══╧═══╝");
            }
            else{

                String left_margin = new string(' ', (Console.WindowWidth/3 - board_len - 5));

                Console.SetCursorPosition((left_margin.Length*x_pos)+(board_len*x_pos), 1 + 21*y_pos);

                Console.Write(left_margin + "╔═══╤═══╤═══╦═══╤═══╤═══╦═══╤═══╤═══╗");
                
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

                Console.SetCursorPosition((left_margin.Length*x_pos)+(board_len*x_pos), (21*y_pos) + 19);
                Console.Write(left_margin + "╚═══╧═══╧═══╩═══╧═══╧═══╩═══╧═══╧═══╝");
            }
        }

        static void DisplayOptions()
        {
            String[] options = { "D - DELETE", "U - UNDO", "R - REDO", "M - MENU", "V - SOLVE", "S - SAVE", "X - QUIT" };

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
        * 
        */
        static void RulesMenu()
        {
            Console.WriteLine("\n\n Click (M)enu to return to back menu or (X) to quit program");

            var key = Console.ReadKey().Key;

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

        static void Sudoku()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

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



            Console.WriteLine("\n" + sudoku + "\n\n\n");

            Console.ForegroundColor = ConsoleColor.White;
        }

        static void Quit(){
            Console.Clear();
            Environment.Exit(1);
        }
    }
}

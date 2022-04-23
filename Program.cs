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

        static int[,] matrix = new int[9, 9] { { 0, 0, 6, 5, 0, 8, 4, 0, 0 },
                                               { 5, 2, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 8, 7, 0, 0, 0, 0, 3, 1 },
                                               { 0, 0, 3, 0, 1, 0, 0, 8, 0 },
                                               { 9, 0, 0, 8, 6, 3, 0, 0, 5 },
                                               { 0, 5, 0, 0, 9, 0, 6, 0, 0 },
                                               { 1, 3, 0, 0, 0, 0, 2, 5, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 7, 4 },
                                               { 0, 0, 5, 2, 0, 6, 3, 0, 0 }
            };

        static int[,] old_matrix = new int[9, 9] {  { 0, 0, 6, 5, 0, 8, 4, 0, 0 },
                                                    { 5, 2, 0, 0, 0, 0, 0, 0, 0 },
                                                    { 0, 8, 7, 0, 0, 0, 0, 3, 1 },
                                                    { 0, 0, 3, 0, 1, 0, 0, 8, 0 },
                                                    { 9, 0, 0, 8, 6, 3, 0, 0, 5 },
                                                    { 0, 5, 0, 0, 9, 0, 6, 0, 0 },
                                                    { 1, 3, 0, 0, 0, 0, 2, 5, 0 },
                                                    { 0, 0, 0, 0, 0, 0, 0, 7, 4 },
                                                    { 0, 0, 5, 2, 0, 6, 3, 0, 0 }
            };

        /*static int[,] matrix = new int[9, 9] { { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                                               { 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };*/

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
            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "2. Load Game");
            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "3. How to play");

            Message(msg, 35);
            msg = null;

            int top = Console.CursorTop;
            Console.SetCursorPosition(Console.WindowWidth / 3, top);

            int option = Console.Read();

            switch (option)
            {
                case '1':
                    Console.Clear();
                    Game();
                    //Console.WriteLine("Option to play game");
                    break;

                case '2':
                    Console.Clear();
                    LoadGame();
                    //Console.WriteLine("Option to solve board");
                    break;

                case '3':
                    Rules();
                    RulesMenu();
                    break;

                default:
                    menu("Invalid option, enter a valid one");
                    break;
            }
        }


        static void Game()
        {
            DisplayBoard();
            DisplayOptions();

            Console.SetCursorPosition((Console.WindowWidth / 2) - (board_len / 2) + 1, 1); //First position

            while (true)
            {
                (int, int) pos = Console.GetCursorPosition();
                int val = matrix[coordinates.Item2, coordinates.Item1];

                var key = Console.ReadKey().Key;

                /*
                List<ConsoleKey> forbidden_keys = new List<ConsoleKey>();
                forbidden_keys.Add(ConsoleKey.Tab);
                
                if(forbidden_keys.Contains(Console.ReadKey().Key)){
                    key = Console.ReadKey(false).Key;
                }
                else{
                    key = Console.ReadKey(true).Key;
                }*/




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
                        matrix[coordinates.Item2, coordinates.Item1] = move.val;
                        break;

                    case ConsoleKey.S:
                        
                        (int, int) s_pos = Console.GetCursorPosition();
                        //Delete character entered
                        Replace(val, s_pos);

                        if(!File.Exists("boards.csv")){
                            File.Create("boards.csv");
                        }

                        using(StreamWriter sw = new StreamWriter("boards.csv", true)){
                            
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

                            for(int i = 0; i < old_matrix.GetLength(1); i++){
                                for(int j = 0; j < old_matrix.GetLength(0); j++){
                                    
                                    if(j%8 != 0 || j == 0)
                                        sw.Write(matrix[i, j] + ",");
                                    else
                                        sw.Write(matrix[i, j]);

                                }
                                sw.Write("\n");
                            }

                            sw.Write("\n");
                            sw.Close();
                        }
                        
                        
                        break;

                    case ConsoleKey.M:
                        menu("OVVERWRITE That duking stoopid message pls");
                        break;

                    case ConsoleKey.V:
                        undoValue(pos, val);
                        SolveBoard();
                        break;

                    case ConsoleKey.X:
                        Console.Clear();
                        Environment.Exit(1);
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

                //Message(matrix[coordinates.Item1, coordinates.Item2].ToString());
                /*
                int x = Console.GetCursorPosition().Left;
                int y = Console.GetCursorPosition().Top;
                String coo = x.ToString() + ", " + y.ToString();
                Message(coo);*/

            } //while (!Console.ReadKey().Equals('x'));
        }


        static void Message(String msg, int top = 20)
        {
            Console.ForegroundColor = ConsoleColor.White;
            (int, int) pos = Console.GetCursorPosition();
            Console.SetCursorPosition(0, top);
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

        static void undoValue((int, int) pos, int val)
        {
            Console.SetCursorPosition(pos.Item1 - 1, pos.Item2);
            if (val == 0)
                Console.Write(' ');
            else
                Console.Write(val);
            Console.SetCursorPosition(pos.Item1 - 1, pos.Item2);
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

        /*static int CreateBoard()
        {

        }*/

        static void LoadGame(){

            //Check if file exists
            if(!File.Exists("boards.csv")){
                menu("No games have been played");
            }
            
            Console.Clear();

            using(StreamReader sr = new StreamReader("boards.csv")){
                
                String[] lines = File.ReadAllLines("boards.csv");

                //Export matrix from csv
                for(int i = 0; i < matrix.GetLength(1); i++){
                    
                    int[] values = Array.ConvertAll(sr.ReadLine().Split(','), int.Parse);
                    
                    for(int j = 0; j < matrix.GetLength(0); j++){
                        matrix[i, j] = values[j];
                    }
                }

                //Delete exported matrix
                /*File.WriteAllLines("boards.csv", lines.Skip(10).ToArray());

                //Export old_matrix from csv
                for(int i = 0; i < matrix.GetLength(1); i++){
                    
                    int[] values = Array.ConvertAll(sr.ReadLine().Split(','), int.Parse);
                    
                    for(int j = 0; j < matrix.GetLength(0); j++){
                        matrix[i, j] = values[j];
                    }
                }*/
                
            }

            RulesMenu();

        }

        static void SolveBoard()
        {
            matrix = old_matrix;

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
            Message("Sudoku solved!!! Press the (M)enu button to return to the main menu");
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

        static void DisplayBoard(char c = 'c')
        {
            Console.Clear();

            Console.Write(new string(' ', (Console.WindowWidth - board_len) / 2) + "╔═══╤═══╤═══╦═══╤═══╤═══╦═══╤═══╤═══╗");

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write("\n");
                Console.Write(new string(' ', (Console.WindowWidth - board_len) / 2) + "║");

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
                    Console.Write("\n" + new string(' ', (Console.WindowWidth - board_len) / 2) + "╠═══╪═══╪═══╬═══╪═══╪═══╬═══╪═══╪═══╣");
                else if (ii < matrix.GetLength(0))
                    Console.Write("\n" + new string(' ', (Console.WindowWidth - board_len) / 2) + "╟───┼───┼───╫───┼───┼───╫───┼───┼───╢");


            }
            Console.Write("\n" + new string(' ', (Console.WindowWidth - board_len) / 2) + "╚═══╧═══╧═══╩═══╧═══╧═══╩═══╧═══╧═══╝");

        }

        static void DisplayOptions()
        {
            String[] options = { "D - DELETE", "U - UNDO", "M - MENU", "V - SOLVE", "S - SAVE", "X - QUIT" };

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

                key = Console.ReadKey().Key;

                if (key == ConsoleKey.M)
                    menu();
                else if (key == ConsoleKey.X)
                {
                    Console.Clear();
                    Environment.Exit(1);
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
    }
}

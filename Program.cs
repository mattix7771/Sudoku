using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    class Program
    {
        static (int, int) coordinates = (0,0);

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

            Welcome();

            menu();

            //Console.Clear();

            DisplayBoard();

            Console.SetCursorPosition(1, 1); //First position

            while(true)
            {
                Console.ForegroundColor = ConsoleColor.Blue;

                var key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:

                        MessageClear();

                        (int, int) pos_up = Console.GetCursorPosition();
                        if (pos_up.Item2 != 1)
                        {
                            Console.SetCursorPosition(pos_up.Item1, pos_up.Item2 - 1);
                            coordinates = (coordinates.Item1, coordinates.Item2 - 1);
                        }
                        else
                            Message("You reached the top of the board");
                        break;

                    case ConsoleKey.DownArrow:

                        MessageClear();

                        (int, int) pos_down = Console.GetCursorPosition();
                        if (pos_down.Item2 != 9)
                        {
                            Console.SetCursorPosition(pos_down.Item1, pos_down.Item2 + 1);
                            coordinates = (coordinates.Item1, coordinates.Item2 + 1);
                        }
                        else
                            Message("You reached the bottom of the board");
                        break;

                    case ConsoleKey.LeftArrow:

                        MessageClear();

                        (int, int) pos_left = Console.GetCursorPosition();
                        if (pos_left.Item1 != 1)
                        {
                            Console.SetCursorPosition(pos_left.Item1 - 3, pos_left.Item2);
                            coordinates = (coordinates.Item1 - 1, coordinates.Item2);
                        }
                        else
                            Message("You reached the left side of the board");
                        break;

                    case ConsoleKey.RightArrow:

                        MessageClear();

                        (int, int) pos_right = Console.GetCursorPosition();
                        if (pos_right.Item1 != 25)
                        {
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

                        int val = matrix[coordinates.Item2, coordinates.Item1];
                        (int, int) num_pos = Console.GetCursorPosition();

                        if (matrix[coordinates.Item2, coordinates.Item1] != 0)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.SetCursorPosition(num_pos.Item1 - 1, num_pos.Item2);
                            Console.Write(val);
                            Console.SetCursorPosition(num_pos.Item1 - 1, num_pos.Item2);
                            Message("Cannot change pre-defined values");
                        }
                        else
                        {
                            Console.Beep();
                            Console.SetCursorPosition(num_pos.Item1 - 1, num_pos.Item2);
                        }

                        break;

                    default:
                        (int, int) pos = Console.GetCursorPosition();
                        Console.SetCursorPosition(pos.Item1 - 1, pos.Item2);
                        Console.Write(' ');
                        Console.SetCursorPosition(pos.Item1 - 1, pos.Item2);
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

        static void menu(){

            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "Enter the option's number to start");

            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "1. Play Game");
            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "2. Solve board");
            Console.WriteLine(new string(' ', Console.WindowWidth / 3) + "3. How to play");

            int top = Console.CursorTop;
            Console.SetCursorPosition(Console.WindowWidth / 3, top);

            int option = Console.Read();

            switch(option){
                case '1':
                    Console.Clear();
                    //Sudoku();
                    //Console.WriteLine("Option to play game");
                    break;
                
                case '2':
                    Console.Clear();
                    //Sudoku();
                    //Console.WriteLine("Option to solve board");
                    break;

                default:
                    Console.Clear();
                    //Sudoku();
                    //Console.WriteLine("Please enter a valid option");
                    break;
            }
        }

        static void Message(String msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            (int, int) pos = Console.GetCursorPosition();
            Console.SetCursorPosition(0, 12);
            Console.WriteLine(msg + new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(pos.Item1, pos.Item2);
        }

        static void MessageClear()
        {
            (int, int) pos = Console.GetCursorPosition();
            Console.SetCursorPosition(0, 12);
            Console.WriteLine(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(pos.Item1, pos.Item2);
        }

        /*static int CreateBoard()
        {

        }*/

        static void SolveBoard()
        {
            int x = matrix.GetLength(0);
            int y = matrix.GetLength(1);

            for(int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    if(matrix[i,j] == 0)
                    {
                        for (int num = 1; num < 10; num++)
                        {
                            if(checkValue(i, j, num))
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
        }

        static bool checkValue(int x, int y, int num)
        {
            int hor = matrix.GetLength(0);
            int ver = matrix.GetLength(1);

            for(int i = 0; i < hor; i++)
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

            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if(matrix[x0+i,y0+j] == num)
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
                    if (matrix[i,j] == 0)
                        return false;
                }
            }

            return true;
        }

        static void DisplayBoard()
        {
            Console.Write("___________________________");

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                Console.Write("\n");
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if (matrix[i, j] == 0)
                    {
                        Console.Write("| |");
                    }
                    else
                        Console.Write("|" + matrix[i, j] + "|");
                }

            }
            Console.Write("\n‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾");

            //Environment.Exit(1);
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

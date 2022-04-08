using System;

namespace Sudoku
{
    class Program
    {
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


            SolveBoard();

            Console.WriteLine("\nNo additional solutions");
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
            //Environment.Exit(1);
        }
    }
}

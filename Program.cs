using System;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            int[,] matrix = new int[9, 9] { { 3, 0, 6, 5, 0, 8, 4, 0, 0 },
                                            { 5, 2, 0, 0, 0, 0, 0, 0, 0 },
                                            { 0, 8, 7, 0, 0, 0, 0, 3, 1 },
                                            { 0, 0, 3, 0, 1, 0, 0, 8, 0 },
                                            { 9, 0, 0, 8, 6, 3, 0, 0, 5 },
                                            { 0, 5, 0, 0, 9, 0, 6, 0, 0 },
                                            { 1, 3, 0, 0, 0, 0, 2, 5, 0 },
                                            { 0, 0, 0, 0, 0, 0, 0, 7, 4 },
                                            { 0, 0, 5, 2, 0, 6, 3, 0, 0 } 
            };

            DisplayBoard(matrix);



        }

        static int CreateBoard()
        {
            int i = 0;
            return i;
        }

        static int[,] SolveBoard(int[,] matrix)
        {
            int hor = matrix.GetLength(0);
            int ver = matrix.GetLength(1);

            for(int i = 0; i <= hor; i++)
            {
  
            }


            return matrix;
        }

        static bool isDone(int[,] matrix)
        {
            int hor = matrix.GetLength(0);
            int ver = matrix.GetLength(1);

            for(int i = 0; i < hor; i++)
            {
                for (int j = 0; j < ver; j++)
                {
                    if(matrix[i,j] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        static void DisplayBoard(int[,] matrix)
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
        }
    }
}

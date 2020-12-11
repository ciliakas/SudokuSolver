using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    internal static class Painter
    {
        public static void Paint(this Board board)
        {
            var cells = board.Cells;
            var size = board.Size;
            WriteTop(size);
            for (var i = 0; i < size * size - 1; i++)
            {
                WriteNumberRow(size, cells[i]);
                if (i % size == size - 1)
                {
                    WriteBoxSpace(size);
                }
                else
                {
                    WriteRowSpace(size);
                }
            }
            WriteNumberRow(size, cells.Last());
            WriteBottom(size);
        }

        private static void WriteNumberRow(int size, IEnumerable<int> row)
        {
            var rowArray = row as int[] ?? row.ToArray();
            Console.Write("║");
            for (var i = 0; i < size * size - 1; i++)
            {
                Console.Write(" " + GetValueSymbol(rowArray[i]) + " ");
                Console.Write(i % size != size - 1 ? "│" : "║");
            }
            Console.WriteLine(" " + GetValueSymbol(rowArray.Last()) + " ║");
        }

        private static string GetValueSymbol(int i)
        {
            if (i < 0)
            {
                return (-i).ToString();
            }
            if (i == 0)
            {
                return " ";
            }
            return i > 9 ? ((char)(i + 55)).ToString() : i.ToString();
        }

        private static void WriteSudokuLine(int size, string s1, string s2, string s3, string s4, string s5)
        {
            Console.Write(s1);
            for (var i = 0; i < size * size - 1; i++)
            {
                Console.Write(s2);
                Console.Write(i % size != size - 1 ? s3 : s4);
            }
            Console.WriteLine(s2 + s5);
        }

        private static void WriteTop(int size)
        {
            WriteSudokuLine(size, "╔", "═══", "╤", "╦", "╗");
        }

        private static void WriteRowSpace(int size)
        {
            WriteSudokuLine(size, "╟", "───", "┼", "╫", "╢");
        }

        private static void WriteBoxSpace(int size)
        {
            WriteSudokuLine(size, "╠", "═══", "╪", "╬", "╣");
        }

        private static void WriteBottom(int size)
        {
            WriteSudokuLine(size, "╚", "═══", "╧", "╩", "╝");
        }
    }
}

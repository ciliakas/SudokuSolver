using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sudoku
{
    public static class Program
    {
        private static async Task Main()
        {
            var sp = new Stopwatch();
            var board = new Board(3);
            board.Paint();
            sp.Start();
            await board.SolveRecursively(false);
            sp.Stop();
            board.Paint();
            Console.WriteLine(board.CheckState());
            Console.WriteLine(sp.Elapsed);

            sp.Reset();
            var exampleBoard = Board.Example();
            exampleBoard.Paint();
            sp.Start();
            await exampleBoard.SolveRecursively(false);
            sp.Stop();
            exampleBoard.Paint();
            Console.WriteLine(exampleBoard.CheckState());
            Console.WriteLine(sp.Elapsed);


            // The current algorithm with sequential position picking takes about 25 seconds (25 ,26 ,30, 12);
            //00:00:17.6926065
        }
    }
}

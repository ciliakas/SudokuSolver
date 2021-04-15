using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Sudoku
{
    public static class Program
    {
        private static async Task Main()
        {
            var sp = new Stopwatch();
            //var board = new Board(3);
            //board.Paint();
            //sp.Start();
            //await board.SolveRecursively(false);
            //sp.Stop();
            //board.Paint();
            //Console.WriteLine(board.CheckState());
            //Console.WriteLine(sp.Elapsed);

            //sp.Reset();



            var exampleBoard = BoardExamples.Example();
            var exampleBoard1 = BoardExamples.Example();

            exampleBoard.Paint();
            //for (int i = 0; i < 81; i++)
            //{
            //    exampleBoard.BoxFromPosition(i);
            //}

            //exampleBoard.SolveDeductively();

            var primitiveSolver = new PrimitiveSolve();
            var recursiveSolver = new RecursiveSolve();
            var deductiveSolver = new DeductiveSolve();
            exampleBoard.Solve(deductiveSolver);
            exampleBoard.Paint();
            Console.WriteLine("Correct solve");
            exampleBoard1.Solve(recursiveSolver);
            exampleBoard1.Paint();
            Console.WriteLine(exampleBoard1.Equals(exampleBoard));
            //exampleBoard.SolveDeductively();


            //exampleBoard.SolvePrimitively();
            //exampleBoard.SolveDeductively();

            //implement strategy pattern and  clean up code!!!!! NOTICE ME
            // move the things that solve the board to different classes, and the actual common things leave in the board
            // for example checking of states is left in the board


            //sp.Start();
            //await exampleBoard.SolveRecursively(false);
            //sp.Stop();
            //exampleBoard.Paint();
            //Console.WriteLine(exampleBoard.CheckState());
            //Console.WriteLine(sp.Elapsed);


            // The current algorithm with sequential position picking takes about 25 seconds (25 ,26 ,30, 12);
            //00:00:17.6926065
        }
    }
}

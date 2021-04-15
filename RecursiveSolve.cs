using System;

namespace Sudoku
{
    public class RecursiveSolve : ISolvingStrategy
    {
        public bool ShowProcess { get; }
        public bool Randomized { get; }

        public RecursiveSolve(bool showProcess = false, bool randomized = true)
        {
            ShowProcess = showProcess;
            Randomized = randomized;
        }

        public void Solve(Board board)
        {
            SolveRecursively(board);
        }

        private bool SolveRecursively(Board board)
        {
            var size = board.Size;
            var cells = board.Cells;
            var state = board.BoardState;
            if (state == BoardState.ValidEnded) return true;
            if (state != BoardState.ValidInPlay) return false;

            var baseBoard = cells;
            var random = new Random();
            var sequence = Randomized ? random.Sequence(1, size * size + 1) : Extensions.Fill(1, size * size);
            foreach (var num in sequence)
            {
                var tempBoard = new Board(size, baseBoard.Clone());
                tempBoard.FillNext(num);
                var screenShot = ShowProcess ? new Board(size, tempBoard.Cells.Clone()) : null;
                if (!SolveRecursively(tempBoard)) continue;

                board.Cells = tempBoard.Cells;
                if (ShowProcess)
                {
                    screenShot.Paint();
                }
                return true;
            }
            return false;
        }
    }
}
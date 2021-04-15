using System;

namespace Sudoku
{
    public class PrimitiveSolve : ISolvingStrategy
    {
        public void Solve(Board board)
        {
            var size = board.Size;
            var cells = board.Cells;
            var baseBoard = board.Cells.Clone();
            var random = new Random();
            do
            {
                var sequence = random.Sequence(1, size * size + 1);
                var changed = false;
                var tempBoard = new Board(size, cells.Clone());
                foreach (var num in sequence)
                {
                    tempBoard = new Board(size, cells.Clone());
                    tempBoard.FillNext(num);
                    if ((tempBoard.BoardState & BoardState.Invalid) == BoardState.Invalid) continue;

                    changed = true;
                    break;
                }
                cells = changed ? tempBoard.Cells.Clone() : baseBoard.Clone();
            } while (board.BoardState != BoardState.ValidEnded);
        }
    }
}
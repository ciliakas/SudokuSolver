using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    public class DeductiveSolve : ISolvingStrategy
    {
        private Board _board;
        private List<List<int>> Cells => _board.Cells;
        private Tuple<int, int> Get2DPositionFrom1D(int position) => _board.Get2DPositionFrom1D(position);
        private int RowFromPosition(int position) => _board.RowFromPosition(position);
        private int ColumnFromPosition(int position) => _board.ColumnFromPosition(position);
        private int BoxFromPosition(int position) => _board.BoxFromPosition(position);
        private List<int> ConvertColumnToList(int position) => _board.ConvertColumnToList(position);
        private List<int> ConvertBoxToList(int position) => _board.ConvertBoxToList(position);
        private int MaxPosition => _board.MaxPosition;
        private int MaxValue => _board.MaxValue;

        //public DeductiveSolve(Board board)
        //{
        //}

        public void Solve(Board board)
        {
            _board = board;
            SolveDeductively();
        }

        private void SolveDeductively()
        {
            /*
             * Possible rules:
             * 1. Only one empty space left in row/column/box DONE
             * 2. The number can not be anywhere else in row/column/box
             *
             * How would the only option left strategy work?
             * for all positions
             *      for all possible values
             *          check if this number is possible in its row
             *          check if this number is possible in its column         
             *          check if this number is possible in its box
             *
             *
             */
            // I want to go from the most basic to the most advanced, and after every fill, reset to basic strats
            //int pos = 0;
            while (true)
            {
                if (TryStrategyForBoard(TryOneMissingStrategy))
                {
                    _board.Paint();
                    continue;
                }

                if (TryStrategyForBoard(TryOnlyOptionLeftStrategy))
                {
                    _board.Paint();
                    continue;
                }
                break;
            }

        }


        private bool TryOnlyOptionLeftStrategy(int position, int rowPosition, int columnPosition)
        {
            // This method assumes all check are done beforehand
            //if (position < 0 || position >= MaxPosition) throw new ArgumentException(); // e.g. position should be between 0 and 80
            //if (Cells[rowPosition][columnPosition] != 0) return false;\

            /*
            Mes norim suzinot, ar jokia kita value cia netinka
            Reiskias pirma mastom, kad tinka visi 9
            Einam per kiekviena, ir bandom atmest
            Jeigu sugebam visus isskyrus viena atmesti, reikias liks tik vienas
             */
            var possibilities = Enumerable.Range(1, MaxValue).ToList();
            var impossibles = new List<int> { 0 };

            var row = Cells[RowFromPosition(position)];
            var column = ConvertColumnToList(ColumnFromPosition(position));
            var box = ConvertBoxToList(BoxFromPosition(position));

            //blet, tai mes einam per kiekviena, ir norim gauti kad value impoosible
            //ir jei visuose trijuose 

            //gerai blet, pereinam per visus possibilities linijoj, atmetam kuriuos randam
            //kartojam tris kartus
            // jei po proceduros liko vienas, reiskias tik jis ir gali buti

            //BLET PASKIAUSIAS
            // einam per kiekviena possi, jei tik kazkur jis negalimas, ismetam

            //Might need to run this for every type of line
            foreach (var possibility in possibilities)
            {
                if (!IsValuePossible(row, possibility))
                {
                    impossibles.Add(possibility);
                    continue;
                }
                if (!IsValuePossible(column, possibility))
                {
                    impossibles.Add(possibility);
                    continue;
                }
                if (!IsValuePossible(box, possibility))
                {
                    impossibles.Add(possibility);
                    continue;
                }
            }

            if (impossibles.Count == MaxValue)
            {
                Console.WriteLine($"{rowPosition} {columnPosition} = {impossibles.FindMissingNumber()}");
                Cells[rowPosition][columnPosition] = impossibles.FindMissingNumber();
                return true;
            }

            return false;
        }

        private bool IsValuePossible(ICollection<int> line, int value)
        {
            return !line.Contains(value);
        }

        private bool TryOneMissingStrategy(int position, int rowPosition, int columnPosition)
        {
            // This method assumes all check are done beforehand
            //if (position < 0 || position >= MaxPosition) throw new ArgumentException(); // e.g. position should be between 0 and 80
            //if (Cells[rowPosition][columnPosition] != 0) return false;

            //fix this shite -- I'm not sure how
            var row = Cells[RowFromPosition(position)];
            if (row.Count(i => i == 0) == 1)
            {
                Console.WriteLine($"{rowPosition} {columnPosition} = {row.FindMissingNumber()}");
                Cells[rowPosition][columnPosition] = row.FindMissingNumber();
                return true;
            }
            var column = ConvertColumnToList(ColumnFromPosition(position));
            if (column.Count(i => i == 0) == 1)
            {
                Console.WriteLine($"{rowPosition} {columnPosition} = {column.FindMissingNumber()}");
                Cells[rowPosition][columnPosition] = column.FindMissingNumber();
                return true;
            }
            var box = ConvertBoxToList(BoxFromPosition(position));
            if (box.Count(i => i == 0) == 1)
            {
                Console.WriteLine($"{rowPosition} {columnPosition} = {box.FindMissingNumber()}");
                Cells[rowPosition][columnPosition] = box.FindMissingNumber();
                return true;
            }
            return false;
        }

        private bool TryStrategyForBoard(Func<int, int, int, bool> solvingStrategy)
        {
            for (var i = 0; i < MaxPosition; i++)
            {
                var (rowPosition, columnPosition) = Get2DPositionFrom1D(i);
                if (Cells[rowPosition][columnPosition] != 0) continue;

                if (solvingStrategy(i, rowPosition, columnPosition))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
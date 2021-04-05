using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    [Flags]
    public enum BoardState
    {
        Valid = 1,// 000001
        Invalid = 2, // 000010
        InPlay = 4, // 000100
        Ended = 8, // 001000
        ValidInPlay = Valid | InPlay,
        ValidEnded = Valid | Ended
        //InvalidInPlay = Invalid | InPlay,
        //InvalidEnded = Invalid | Ended
    }

    public class Board : IEquatable<Board>
    {
        private int _size;
        private int _maxValue;
        private int _maxRcbPosition; // Rcb - Row, Column, Box
        private int _maxPosition; // Rcb - Row, Column, Box
        public int Size
        {
            get => _size;
            private set
            {
                _size = value;
                _maxValue = value * value;
                _maxRcbPosition = _maxValue - 1;
                _maxPosition = _maxValue * _maxValue;
            }

        }

        public List<List<int>> Cells { get; private set; }

        private Board()
        {

        }

        public static Board DifficultBoard()
        {
            var row0 = new List<int> { 8, 0, 0, 0, 0, 0, 0, 0, 0 };
            var row1 = new List<int> { 0, 0, 3, 6, 0, 0, 0, 0, 0 };
            var row2 = new List<int> { 0, 7, 0, 0, 9, 0, 2, 0, 0 };
            var row3 = new List<int> { 0, 5, 0, 0, 0, 7, 0, 0, 0 };
            var row4 = new List<int> { 0, 0, 0, 0, 4, 5, 7, 0, 0 };
            var row5 = new List<int> { 0, 0, 0, 1, 0, 0, 0, 3, 0 };
            var row6 = new List<int> { 0, 0, 1, 0, 0, 0, 0, 6, 8 };
            var row7 = new List<int> { 0, 0, 8, 5, 0, 0, 0, 1, 0 };
            var row8 = new List<int> { 0, 9, 0, 0, 0, 0, 4, 0, 0 };
            var rowList = new List<List<int>> { row0, row1, row2, row3, row4, row5, row6, row7, row8 };
            return new Board(3, rowList);
        }

        public Board(int size)
        {
            if (size < 2 || size > 10) throw new ArgumentException();
            Size = size;

            Cells = GenerateEmptyBoard();
        }
        private Board(int size, List<List<int>> cells)
        {
            Size = size;
            Cells = cells;
        }

        public static Board Example()
        {
            var row0 = new List<int> { 0, 0, 0, 0, 4, 0, 0, 0, 6 };
            var row1 = new List<int> { 4, 1, 0, 5, 2, 0, 0, 0, 0 };
            var row2 = new List<int> { 0, 8, 3, 7, 0, 0, 5, 0, 0 };
            var row3 = new List<int> { 3, 0, 0, 8, 0, 0, 0, 0, 0 };
            var row4 = new List<int> { 0, 0, 0, 0, 7, 0, 4, 3, 0 };
            var row5 = new List<int> { 0, 2, 4, 0, 0, 0, 0, 6, 7 };
            var row6 = new List<int> { 7, 4, 1, 0, 9, 3, 6, 5, 8 };
            var row7 = new List<int> { 0, 0, 0, 0, 8, 1, 0, 0, 2 };
            var row8 = new List<int> { 2, 9, 8, 6, 5, 7, 1, 4, 0 };
            var rowList = new List<List<int>> { row0, row1, row2, row3, row4, row5, row6, row7, row8 };
            return new Board(3, rowList);
        }

        public void SolveDeductively()
        {
            /*
             * Possible rules:
             * 1. Only one empty space left in row/column/box
             * 2. The number can not be anywhere else in row/column/box
             *
             * 1.1 Check for empty one in list? Convert the r/c/b to a list, and check if there is only 1 zero, if it is the only one,
             */
            for (var i = 0; i < _maxPosition; i++)
            {
                if (TrySingleChoiceStrategy(i))
                {
                    i = 0;
                }
            }
        }

        public bool TrySingleChoiceStrategy(int position)
        {
            if (position < 0 || position >= _maxPosition)
                throw new ArgumentException(); // e.g. position should be between 0 and 80
            var (rowPosition, columnPosition) = Get2DPositionFrom1D(position);
            if (Cells[rowPosition][columnPosition] != 0) return false;

        var row = RowFromPosition(position);
            if (row.Count(i => i == 0) == 1)
            {
                Cells[rowPosition][columnPosition] = FindMissingNumber(row);
                return true;
            }
            var column = ColumnFromPosition(position);
            if (column.Count(i => i == 0) == 1)
            {
                Cells[rowPosition][columnPosition] = FindMissingNumber(column);
                return true;
            }
            var box = BoxFromPosition(position);
            if (box.Count(i => i == 0) == 1)
            {
                Cells[rowPosition][columnPosition] = FindMissingNumber(box);
                return true;
            }

            //Console.WriteLine(row.Count(i => i == 0));
            //Console.WriteLine(column.Count(i => i == 0));
            //Console.WriteLine(box.Count(i => i == 0));
            return false;
        }

        public Tuple<int, int> Get2DPositionFrom1D(int position)
        {
            if (position < 0 || position >= _maxPosition) throw new ArgumentException(); // e.g. position should be between 0 and 80
            return new Tuple<int, int>(position / _maxValue, position % _maxValue);
        }

        public int FindMissingNumber(IList<int> list)
        {
            var fullSum = 0;
            for (var i = 1; i <= list.Count; i++)
            {
                fullSum += i;
            }
            return fullSum - list.Sum();
        }

        public List<int> RowFromPosition(int position)
        {
            if (position < 0 || position >= _maxPosition) throw new ArgumentException(); // e.g. position should be between 0 and 80
            return Cells[position / _maxValue];
        }

        public List<int> ColumnFromPosition(int position)
        {
            if (position < 0 || position >= _maxPosition) throw new ArgumentException(); // e.g. position should be between 0 and 80
            return ConvertColumnToList(position % _maxValue);
        }

        public List<int> BoxFromPosition(int position)
        {
            if (position < 0 || position >= _maxPosition) throw new ArgumentException(); // e.g. position should be between 0 and 80

            //for (int i = 0; i < _maxPosition; i++)
            //{
            //    Console.Write(i / _maxValue % Size + " ");
            //    Console.WriteLine(i % _maxValue / Size);
            //}
            //Console.Write(position / _maxValue % Size + " ");
            //Console.WriteLine(position % _maxValue / Size);
            return ConvertBoxToList(position / _maxValue % Size, position % _maxValue / Size);
        }





        private List<List<int>> GenerateEmptyBoard()
        {
            var emptyBoard = new List<List<int>>();
            for (var i = 0; i < Size * Size; i++)
            {
                var emptyRow = new List<int>();
                for (var j = 0; j < Size * Size; j++)
                {
                    emptyRow.Add(0);
                }
                emptyBoard.Add(emptyRow);
            }
            return emptyBoard;
        }

        public void SolvePrimitively()
        {
            var baseBoard = Cells.Clone();
            var random = new Random();
            do
            {
                var tempBoard = new Board();
                var sequence = random.Sequence(1, Size * Size + 1);
                var changed = false;
                foreach (var num in sequence)
                {
                    tempBoard = new Board(Size, Cells.Clone());
                    tempBoard.FillNext(num);
                    if ((tempBoard.CheckState() & BoardState.Invalid) == BoardState.Invalid) continue;

                    changed = true;
                    break;
                }
                Cells = changed ? tempBoard.Cells.Clone() : baseBoard.Clone();
                this.Paint();
            } while (CheckState() != BoardState.ValidEnded);
        }

        public async Task<bool> SolveRecursively(bool showProcess = true, bool randomized = true)
        {
            var state = CheckState();
            if (state == BoardState.ValidEnded) return true;
            if (state != BoardState.ValidInPlay) return false;

            var baseBoard = Cells;
            var random = new Random();
            var sequence = randomized ? random.Sequence(1, Size * Size + 1) : Extensions.Fill(1, Size * Size);
            foreach (var num in sequence)
            {
                var tempBoard = new Board(Size, baseBoard.Clone());
                tempBoard.FillNext(num);
                var screenShot = showProcess ? new Board(Size, tempBoard.Cells.Clone()) : null;
                if (!await tempBoard.SolveRecursively(showProcess)) continue;

                Cells = tempBoard.Cells;
                if (showProcess)
                {
                    screenShot.Paint();
                }
                return true;
            }
            return false;
        }

        private void FillNext(int value)
        {
            if (value < 1 || value > _maxValue) throw new ArgumentException();

            foreach (var row in Cells)
            {
                for (var j = 0; j < row.Count; j++)
                {
                    if (row[j] != 0) continue;
                    row[j] = value;
                    return;
                }
            }
        }

        private List<int> ConvertBoxToList(int number)
        {
            if (number < 0 || number > _maxRcbPosition) throw new ArgumentException();

            return ConvertBoxToList(number / Size, number % Size);
        }

        private List<int> ConvertBoxToList(int row, int column)
        {
            if (row < 0 || column < 0 || row > Size - 1 || column > Size - 1) throw new ArgumentException();

            return Cells.Skip(row * Size).Take(Size).SelectMany(col => col.Skip(column * Size).Take(Size).ToList()).ToList();
        }

        private List<int> ConvertColumnToList(int column)
        {
            if (column < 0 || column > _maxRcbPosition) throw new ArgumentException();

            return Cells.Select(row => row[column]).ToList();
        }

        public BoardState CheckState()
        {
            var playState = BoardState.Ended;
            if (Cells.Any(row => row.Contains(0)))
            {
                playState = BoardState.InPlay;
            }
            for (var i = 0; i < _maxValue; i++)
            {
                var boxState = CheckBox(i);
                var rowState = CheckRow(i);
                var columnState = CheckColumn(i);
                var state = (boxState | rowState | columnState) & BoardState.Invalid;
                if (state == BoardState.Invalid) return playState | BoardState.Invalid;
            }
            return playState | BoardState.Valid;
        }

        private static BoardState CheckLineState(IList<int> list)
        {
            var playState = list.Contains(0) ? BoardState.InPlay : BoardState.Ended;
            for (var i = 0; i < list.Count - 1; i++)
            {
                if (list[i] == 0) continue;
                for (var j = i + 1; j < list.Count; j++)
                {
                    if (list[j] == 0) continue;
                    if (list[i] != list[j]) continue;

                    return playState | BoardState.Invalid;
                }
            }
            return playState | BoardState.Valid;
        }

        private BoardState CheckBox(int number)
        {
            if (number < 0 || number > _maxRcbPosition) throw new ArgumentException();

            return CheckBox(number / Size, number % Size);
        }

        private BoardState CheckBox(int row, int column)
        {
            return CheckLineState(ConvertBoxToList(row, column));
        }

        private BoardState CheckRow(int row)
        {
            if (row < 0 || row > _maxRcbPosition) throw new ArgumentException();
            return CheckLineState(Cells[row]);
        }

        private BoardState CheckColumn(int column)
        {
            return CheckLineState(ConvertColumnToList(column));
        }

        public void WriteToCell(int row, int column, int value)
        {
            if (row < 0 || column < 0 || value < 1 || row > _maxRcbPosition || column > _maxRcbPosition || value > _maxValue) throw new ArgumentException();
            if (Cells[row][column] != 0) return;

            Cells[row][column] = value;
        }

        private void OverwriteCell(int row, int column, int value)
        {
            if (row < 0 || column < 0 || value < 1 || row > _maxRcbPosition || column > _maxRcbPosition || value > _maxValue) throw new ArgumentException();

            Cells[row][column] = value;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var line in Cells)
            {
                foreach (var cell in line)
                {
                    sb.Append(cell.ToString());
                }
                sb.Append("\n");
            }
            return sb.ToString();
        }

        public bool Equals(Board other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            for (var row = 0; row < Cells.Count; row++)
            {
                for (var cell = 0; cell < Cells[row].Count; cell++)
                {
                    if (Cells[row][cell] != other.Cells[row][cell]) return false;
                }
            }
            return Size == other.Size;
        }
    }
}

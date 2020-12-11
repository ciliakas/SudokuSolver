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
        private int _maxPosition;
        public int Size
        {
            get => _size;
            private set
            {
                _size = value;
                _maxValue = value * value;
                _maxPosition = _maxValue - 1;
            }

        }

        public List<List<int>> Cells { get; private set; }

        private Board()
        {

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
            var row0 = new List<int> { 5, 0, 0, 0, 0, 0, 0, 0, 0 };
            var row1 = new List<int> { 3, 4, 0, 1, 0, 0, 0, 0, 7 };
            var row2 = new List<int> { 0, 9, 0, 0, 0, 6, 0, 0, 0 };
            var row3 = new List<int> { 6, 0, 0, 0, 2, 0, 0, 0, 9 };
            var row4 = new List<int> { 0, 0, 4, 9, 8, 0, 1, 0, 0 };
            var row5 = new List<int> { 0, 1, 0, 0, 4, 0, 0, 0, 0 };
            var row6 = new List<int> { 0, 0, 0, 0, 0, 3, 0, 2, 6 };
            var row7 = new List<int> { 0, 0, 0, 0, 0, 0, 8, 0, 0 };
            var row8 = new List<int> { 9, 0, 0, 0, 0, 0, 3, 4, 0 };
            var rowList = new List<List<int>> { row0, row1, row2, row3, row4, row5, row6, row7, row8 };
            return new Board(3, rowList);
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
            var sequence = randomized? random.Sequence(1, Size * Size + 1) : Extensions.Fill(1, Size * Size);
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
            if (number < 0 || number > _maxPosition) throw new ArgumentException();
           
            return ConvertBoxToList(number / Size, number % Size);
        }

        private List<int> ConvertBoxToList(int row, int column)
        {
            if (row < 0 || column < 0 || row > Size - 1 || column > Size - 1) throw new ArgumentException();
            
            return Cells.Skip(row * Size).Take(Size).SelectMany(col => col.Skip(column * Size).Take(Size).ToList()).ToList();
        }

        private List<int> ConvertColumnToList(int column)
        {
            if (column < 0 || column > _maxPosition) throw new ArgumentException();
           
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
            if (number < 0 || number > _maxPosition) throw new ArgumentException();

            return CheckBox(number / Size, number % Size);
        }

        private BoardState CheckBox(int row, int column)
        {
            return CheckLineState(ConvertBoxToList(row, column));
        }

        private BoardState CheckRow(int row)
        {
            if (row < 0 || row > _maxPosition) throw new ArgumentException();
            return CheckLineState(Cells[row]);
        }

        private BoardState CheckColumn(int column)
        {
            return CheckLineState(ConvertColumnToList(column));
        }

        public void WriteToCell(int row, int column, int value)
        {
            if (row < 0 || column < 0 || value < 1 || row > _maxPosition || column > _maxPosition || value > _maxValue) throw new ArgumentException();
            if (Cells[row][column] != 0) return;

            Cells[row][column] = value;
        }

        private void OverwriteCell(int row, int column, int value)
        {
            if (row < 0 || column < 0 || value < 1 || row > _maxPosition || column > _maxPosition || value > _maxValue) throw new ArgumentException();

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

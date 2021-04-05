using System;
using System.Collections.Generic;
using System.Text;

namespace Sudoku
{
    static class BoardExamples
    {
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
    }
}

using System;

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
}
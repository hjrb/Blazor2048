using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor2048
{

    /// <summary>
    /// a class that implements the game 2048
    /// </summary>
    public class Game2048
    {

        /// <summary>
        /// ctor
        /// </summary>
        public Game2048()
        {
            Size = 4;
            Cells = new int[Size * Size];
            if (!NoAutoAdd) Add();
        }

        /// <summary>
        /// the size of the game, default is 4
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// if set to true (default is false) after a move has been performend the Add function is not being called
        /// </summary>
        public bool NoAutoAdd { get; set; } = false;

        /// <summary>
        /// the cells. for various pratical reasons (and for better performance) a 1d array is used instead of a 2d grid
        /// </summary>
        public int[] Cells { get; set; }

        /// <summary>
        /// the counter of the moves
        /// </summary>
        public int MovesCounter { get; set; } = 0;

        /// <summary>
        /// the sum of cell values
        /// </summary>
        public int Total => Cells.Sum();
        /// <summary>
        /// the maximum cell value
        /// </summary>
        public int Max => Cells.Max();

        /// <summary>
        /// the index of the last cell that was set by Add()
        /// </summary>
        public int LastAddedCellIndex { get; private set; }

        public int this[int row, int col]
        {
            get { return Cells[row * Size + col]; }
            set { Cells[row * Size + col] = value; }
        }

        public bool Iterate(Func<int, bool> move)
        {
            bool anyMove = false;
            for (int index = 0; index < Size; ++index)
            {
                anyMove |= move(index);
            }
            if (anyMove)
            {
                if (!NoAutoAdd) Add();
            }
            return anyMove;
        }

        bool DoVerticalMove(int otherRow, int column, ref int currentRow, ref bool moved) {
            if (this[otherRow, column] == this[currentRow, column])
            {
                moved = true;
                this[otherRow, column] += this[currentRow, column];
                this[currentRow, column] = 0;
                return false;
            }
            else if (this[otherRow, column] == 0)
            {
                moved = true;
                this[otherRow, column] = this[currentRow, column];
                this[currentRow, column] = 0;
                currentRow = otherRow;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DoHorizontalMove(int row, int otherColumn, ref int currentColumn, ref bool moved)
        {
            if (this[row, otherColumn] == this[row, currentColumn])
            {
                moved = true;
                this[row, otherColumn] += this[row, currentColumn];
                this[row, currentColumn] = 0;
                return false;
            }
            else if (this[row, otherColumn] == 0)
            {
                moved = true;
                this[row, otherColumn] = this[row, currentColumn];
                this[row, currentColumn] = 0;
                currentColumn = otherColumn;
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool Down()
        {
            return Iterate((column) =>
            {
                bool anyMove = false;
                for (int row = Size - 2; row >= 0; --row)
                {
                    if (this[row, column] == 0) continue;
                    int curentRow = row;
                    for (int otherRow = row + 1; otherRow < Size; otherRow++)
                    {
                        if (!DoVerticalMove(otherRow, column, ref curentRow, ref anyMove)) break;
                    }
                }
                return anyMove;
            });
        }

        public bool Up()
        {
            return Iterate((column) =>
            {
                bool anyMove = false;
                for (int row = 1; row < Size; ++row)
                {
                    if (this[row, column] == 0) continue;
                    int currentRow= row;
                    for (int otherRow = row - 1; otherRow >= 0; otherRow--)
                    {
                        if (!DoVerticalMove(otherRow, column, ref currentRow, ref anyMove)) break;
                    }
                }
                return anyMove;
            });
        }

        public bool Left()
        {
            return Iterate((row) =>
            {
                bool anyMove = false;
                for (int column = 1; column < Size; ++column)
                {
                    if (this[row, column] == 0) continue;
                    int currentColumn = column;
                    for (int otherColumn = column - 1; otherColumn >= 0; otherColumn--)
                    {
                        if (!DoHorizontalMove(row, otherColumn, ref currentColumn, ref anyMove)) break;
                    }
                }
                return anyMove;
            });
        }

        public bool Right()
        {

            return Iterate((row) =>
            {
                bool anyMove = false;
                for (int column = Size - 2; column >= 0; --column)
                {
                    if (this[row, column] == 0) continue;
                    int currentColumn = column;
                    for (int otherColumn = column + 1; otherColumn < Size; otherColumn++)
                    {
                        if (!DoHorizontalMove(row, otherColumn, ref currentColumn, ref anyMove)) break;
                    }
                }
                return anyMove;
            });
        }

        /// <summary>
        /// a delegate that get the index and value of a cell as input and returns true (meets some criteria)
        /// </summary>
        /// <param name="index">index of the cell in the array</param>
        /// <param name="value">value of the cell</param>
        /// <returns>true if the desired criteria is met e.g. val==0 || val==2*i</returns>
        public delegate bool CellSelector(int value, int index);

        /// <summary>
        /// a enumerators for the cell indizes that meet criteria of the cond
        /// </summary>
        /// <param name="cond">the delegate that compuse the where condidtion</param>
        /// <returns>the enumeratoration of cell indizes that meet the condition cond</returns>
        public IEnumerable<int> Where(CellSelector cond) => Cells.Select((val, idx) => new { val, idx }).Where(x => cond(x.val, x.idx)).Select(x => x.idx);

        /// <summary>
        /// return an enumeration of cells that have value 0 (empty)
        /// </summary>
        public IEnumerable<int> EmptyCells => Where((value, idx) => value == 0);

        private Random random = new();

        /// <summary>
        /// add a new random value for an empty cell
        /// </summary>
        public void Add()
        {
            var emptyCells = this.EmptyCells.ToArray(); // get the array of indizes of the empty cells
            LastAddedCellIndex = emptyCells[random.Next(emptyCells.Length)]; // choose one randomly
            var value = random.Next(100); // generate a ranom value 0..99
            Cells[LastAddedCellIndex] = value > 89 ? 4 : 2; // create a new value with 90% chance for a two and 10% chance for a four
            ++MovesCounter;
        }
    }
}
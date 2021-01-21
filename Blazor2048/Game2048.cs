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
        public Game2048(int Size=4)
        {
            this.Size = Size;
            Cells = new int[Size * Size];
            if (!NoAutoAdd) Add();
        }

        /// <summary>
        /// the size of the game, default is 4
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// if set to true (default is false) after a move has been performed the Add function is not being called
        /// </summary>
        public bool NoAutoAdd { get; set; } = false;

        /// <summary>
        /// the cells. for various reasons (e.g. for better performance) a 1d array is used instead of a 2d grid
        /// </summary>
        public int[] Cells { get; init; }

        /// <summary>
        /// the counter of the moves
        /// </summary>
        public int MovesCounter { get; private set; } = 0;

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
        public int LastAddedCellIndex { get; set; }

        /// <summary>
        /// cell indexer
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public int this[int row, int column]
        {
            get { return Cells[(row * Size) + column]; }
            private set { Cells[(row * Size) + column] = value; }
        }

        /// <summary>
        /// iterate one dimension of the grid (rows or columns)
        /// </summary>
        /// <param name="move">a delegate the performs a move and return if something happend</param>
        /// <returns>if an move execution change the game</returns>
        private bool Iterate(Func<int, bool> move)
        {
            bool anyMove = false;
            for (int index = 0; index < Size; ++index)
            {
                anyMove |= move(index);
            }
            if (anyMove)
            {
                // if there was a move then call Add()
                if (!NoAutoAdd) Add();
            }
            return anyMove;
        }

        /// <summary>
        /// performs a vertical move operation
        /// </summary>
        /// <param name="otherRow">the index of another row to tested</param>
        /// <param name="column">the current column</param>
        /// <param name="currentRow">the idex of the current row</param>
        /// <param name="moved">set to true if a move occured</param>
        /// <returns>if the current cell has change and further processing is required</returns>
        private bool DoVerticalMove(int otherRow, int column, ref int currentRow, ref bool moved) {
            // other row value is the same a the current row value (for the given column)
            if (this[otherRow, column] == this[currentRow, column])
            {
                moved = true; // we move the current cell
                this[otherRow, column] += this[currentRow, column]; // by joining
                this[currentRow, column] = 0; // and setting the original cell to empty
                return false; // we don't need to work on this cell anymore
            }
            // other row value is 0 (for the given column)
            else if (this[otherRow, column] == 0)
            {
                moved = true; // we move the current cell
                this[otherRow, column] = this[currentRow, column]; // set the empty to the current cell
                this[currentRow, column] = 0; // clear the current cell
                currentRow = otherRow; // we must continue with the new cell!
                return true;
            }
            else
            {
                // no move possible
                return false;
            }
        }

        /// <summary>
        /// performs a horizontal move operation
        /// </summary>
        /// <param name="otherRow">the index of another row to tested</param>
        /// <param name="column">the current column</param>
        /// <param name="currentRow">the idex of the current row</param>
        /// <param name="moved">set to true if a move occured</param>
        /// <returns>if the current cell has change and further processing is required</returns>
        private bool DoHorizontalMove(int row, int otherColumn, ref int currentColumn, ref bool moved)
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

        /// <summary>
        /// peform a down move
        /// </summary>
        /// <returns>if any cell changed</returns>
        public bool Down()
        {
            // forech columns
            return Iterate((column) =>
            {
                bool anyMove = false;
                // process all rows but the last
                for (int row = Size - 2; row >= 0; --row)
                {
                    if (this[row, column] == 0) continue; // the cell is empty, we are done
                    int curentRow = row;
                    // check all rows above the current row
                    for (int otherRow = row + 1; otherRow < Size; otherRow++)
                    {
                        if (!DoVerticalMove(otherRow, column, ref curentRow, ref anyMove)) break;
                    }
                }
                return anyMove;
            });
        }

        /// <summary>
        /// peform an Up move
        /// </summary>
        /// <returns>if any cell changed</returns>
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

        /// <summary>
        /// perform a Left move
        /// </summary>
        /// <returns>if any cell changed</returns>
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

        /// <summary>
        /// performs a Right move
        /// </summary>
        /// <returns>if any cell changed</returns>
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
        public IEnumerable<int> EmptyCells => Where((value, _) => value == 0);

        private readonly Random random = new();

        /// <summary>
        /// add a new random value for an empty cell
        /// </summary>
        public void Add()
        {
            var emptyCellsIdxArray = this.EmptyCells.ToArray(); // get the array of indizes of the empty cells
            LastAddedCellIndex = emptyCellsIdxArray[random.Next(emptyCellsIdxArray.Length)]; // choose one randomly
            Cells[LastAddedCellIndex] = random.Next(100) > 89 ? 4 : 2; // create a new value with 90% chance for a two and 10% chance for a four
            ++MovesCounter;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor2048
{

    public class Game2048
    {
        public int Size { get; }
        public bool NoAutoAdd { get; set; } = false;
        public int[] Cells { get; set; }
        public int MoveCounter { get; set; } = 0;

        public Game2048()
        {
            Size = 4;
            Cells = new int[Size * Size];
            if (!NoAutoAdd) Add();
        }

        public int Total => Cells.Sum();
        public int Max => Cells.Max();

        public int LastAddedCellIndex { get; private set; }

        public string CellText(int row, int col)
        {
            var val = this[row, col];
            return val == 0 ? string.Empty : val.ToString();
        }

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

         private Random random = new();
        private void Add()
        {
            var emptyCells = Cells.Where(x=>x==0).Select((val, idx)=>idx).ToArray();
            LastAddedCellIndex = random.Next(emptyCells.Length);
            var value = random.Next(100);
            // create a new value with 90% change for a two and 10% change for a four
            Cells[LastAddedCellIndex] = value > 89 ? 4 : 2;
            ++MoveCounter;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blazor2048
{
    public record CellRefernce(int row, int col);

    public class Game2048
    {
        public int Size { get; }
        public bool NoAutoAdd { get; set; } = false;

        private int[] cells;
        public int[] Cells
        {
            get { return cells; }
            set
            {
                this.cells = value;
            }
        }
        public int MoveCounter { get; set; } = 0;

        public Game2048()
        {
            Size = 4;
            cells = new int[Size * Size];
            if (!NoAutoAdd) Add();
        }

        public int Total => cells.Sum();
        public int Max => cells.Max();

        public string CellText(int row, int col)
        {
            var val = this[row, col];
            return val == 0 ? string.Empty : val.ToString();
        }

        public int this[int row, int col]
        {
            get { return cells[row * Size + col]; }
            set { cells[row * Size + col] = value; }
        }

        public bool VerticalMove(Func<int, bool> processRows)
        {
            bool anyMove = false;
            for (int column = 0; column < Size; ++column)
            {
                anyMove |= processRows(column);
            }
            if (anyMove)
            {
                if (!NoAutoAdd) Add();
            }
            return anyMove;
        }

        public bool HorizontalMove(Func<int, bool> processColumns)
        {
            bool anyMove = false;
            for (int row = 0; row < Size; ++row)
            {
                anyMove |= processColumns(row);
            }
            if (anyMove)
            {
                if (!NoAutoAdd) Add();
            }
            return anyMove;
        }

        bool DoVerticalMove(int otherRow, int column, ref int currentColumn, ref bool moved) {
            if (this[otherRow, column] == this[currentColumn, column])
            {
                moved = true;
                this[otherRow, column] += this[currentColumn, column];
                this[currentColumn, column] = 0;
                return false;
            }
            else if (this[otherRow, column] == 0)
            {
                moved = true;
                this[otherRow, column] = this[currentColumn, column];
                this[currentColumn, column] = 0;
                currentColumn = otherRow;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Down()
        {
            return VerticalMove((column) =>
            {
                bool anyMove = false;
                for (int row = Size - 1; row >= 0; --row)
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
            return VerticalMove((column) =>
            {
                bool anyMove = false;
                for (int row = 1; row < Size; ++row)
                {
                    if (this[row, column] == 0) continue;
                    int x = row;
                    for (int r2 = row - 1; r2 >= 0; r2--)
                    {
                        if (!DoVerticalMove(r2, column, ref x, ref anyMove)) break;
                    }
                }
                return anyMove;
            });
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
        public bool Left()
        {
            return HorizontalMove((row) =>
            {
                bool anyMove = false;
                for (int column = 0; column <= Size-1; ++column)
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

            return HorizontalMove((row) =>
            {
                bool anyMove = false;
                for (int column = Size - 1; column >= 0; --column)
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

        public IEnumerable<CellRefernce> CellReferences(Func<int, int, int, bool> filter)
        {
            for (int r = 0; r < Size; ++r)
            {
                for (int c = 0; c < Size; ++c)
                {
                    if (filter(r, c, this[r, c]))
                    {
                        yield return new CellRefernce(r, c);
                    }
                }
            }
        }

        private Random random = new();
        private void Add()
        {
            var emptyCells = CellReferences((r, c, v) => v == 0).ToArray();
            var i = random.Next(emptyCells.Count());
            var cellRef = emptyCells[i];
            var value = random.Next(100);
            // create a new value with 90% change for a two and 10% change for a four
            this[cellRef.row, cellRef.col] = value > 89 ? 4 : 2;
            ++MoveCounter;
        }
    }
}
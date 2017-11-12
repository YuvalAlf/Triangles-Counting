using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EphemeralTriangleCounting
{
    public sealed class EphemeralMatrix
    {
        private LinkedList<Cell>[] Cols { get; }

        public EphemeralMatrix(LinkedList<Cell>[] cols)
        {
            Cols = cols;
        }

        // O(size)
        public static EphemeralMatrix Empty(int size)
        {
            var array = new LinkedList<Cell>[size];
            for (int i = 0; i < size; i++)
                array[i] = new LinkedList<Cell>();
            return new EphemeralMatrix(array);
        }

        // O(d_max)
        public int GetValue(int row, int col)
        {
            var cell = Cols[col].First;
            while (true)
            {
                if (cell == null || cell.Value.Row > row)
                    return 0;
                if (cell.Value.Row == row)
                    return cell.Value.Row;
                cell = cell.Next;
            }

        }

        // O(d_max)
        public void SetValue(int row, int col, int value)
        {
            if (value == 0)
                ZeroValue(row, col);
            var cell = Cols[col].First;
            while (true)
            {
                if (cell == null)
                {
                    Cols[col].AddLast(new Cell(row, value));
                    return;
                }
                if (cell.Value.Row > row)
                {
                    cell.List.AddBefore(cell, new Cell(row, value));
                    return;
                }
                if (cell.Value.Row == row)
                {
                    cell.Value = cell.Value.WithValue(value);
                    return;
                }
                    
                cell = cell.Next;
            }
        }

        // O(d_max)
        private void ZeroValue(int row, int col)
        {
            var cell = Cols[col].First;
            while (true)
            {
                if (cell == null)
                    return;
                if (cell.Value.Row == row)
                {
                    cell.List.Remove(cell);
                    return;
                }
                cell = cell.Next;
            }
        }

        public void AddCol(int colNum, LinkedList<Cell> col)
        {
            var thisCell = Cols[colNum].First;
            var otherCell = col.First;

            while (true)
            {
                if (otherCell == null)
                    return;
                if (thisCell == null)
                {
                    while (otherCell != null)
                    {
                        Cols[colNum].AddLast(otherCell.Value);
                        otherCell = otherCell.Next;
                    }
                    return;
                }
                if (otherCell.Value.Row < thisCell.Value.Row)
                {
                    thisCell.List.AddBefore(thisCell, otherCell.Value);
                    otherCell = otherCell.Next;
                }
                else if (otherCell.Value.Row > thisCell.Value.Row)
                    thisCell = thisCell.Next;
                else
                {
                    thisCell.Value = thisCell.Value.AddValue(otherCell.Value.Value);
                    thisCell = thisCell.Next;
                    otherCell = otherCell.Next;
                }
            }
        }

        public struct Cell
        {
            public int Row { get; }
            public int Value { get; }

            public Cell(int row, int value)
            {
                Row = row;
                Value = value;
            }

            public Cell WithValue(int newValue) => new Cell(Row, newValue);
            public Cell AddValue(int valueToAdd) => new Cell(Row, Value + valueToAdd);
        }
    }
}

using System;
using System.Collections;
using System.Windows.Forms;

namespace Xml2WinForms.Utils
{
    internal sealed class ListViewItemSorter : IComparer
    {
        int _columnIndex;
        bool _sortAscending = true;

        public int ColumnIndex
        {
            set
            {
                if (_columnIndex == value)
                    _sortAscending = !_sortAscending;
                else
                {
                    _columnIndex = value;
                    _sortAscending = true;
                }
            }
            get { return _columnIndex; }
        }

        public int Compare(object left, object right)
        {
            var leftItem = (ListViewItem) left;
            var rightItem = (ListViewItem)right;

            if (null == left || null == right)
                throw new InvalidOperationException("null == left || null == right");

            if (leftItem.SubItems.Count <= _columnIndex ||
                rightItem.SubItems.Count <= _columnIndex)
                return 0;

            var leftComparable = leftItem.SubItems[_columnIndex].Tag as IComparable;
            var rightComparable = rightItem.SubItems[_columnIndex].Tag as IComparable;

            int result;

            if (null == leftComparable)
                result = -1;
            else if (null == rightComparable)
                result = 1;
            else
                result = leftComparable.CompareTo(rightComparable);

            return result * (_sortAscending ? 1 : -1);
        }
    }
}
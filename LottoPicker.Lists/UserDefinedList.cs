using System;
using System.Text;
using LottoPicker.Common;

namespace LottoPicker.Lists
{
    public class UserDefinedList<T> : IUserDefinedList<T>
    {
        // Constant...
        private const int DefaultCapacity = 4;

        // Fields...
        private T[] _items;
        private T[,] _itemMatrix;
        private int _count;

        // Constructors...
        public UserDefinedList(int capacity = DefaultCapacity)
        {
            _items = new T[capacity];
            ListVersion = 1;
        }

        // Properties...
        public int Count
        {
            get { return _count; }
        }
        public int Capacity
        {
            get
            {
                return _items.Length;
            }
            set
            {
                if (value < _count) value = _count;
                if (value != _items.Length)
                {
                    T[] newItems = new T[value];
                    Array.Copy(_items, 0, newItems, 0, _count);
                    _items = newItems;
                }
            }
        }

        // Indexer...
        public T this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                _items[index] = value;
                OnChanged();
            }
        }

        // A two-dimensional indexer, just to show that an object can have more than one indexer
        public T this[int i, int j]
        {
            get { return _itemMatrix[i, j]; }
            set
            {
                _itemMatrix[i, j] = value;
                OnChanged();
            }
        }

        // Methods...
        public void Add(T item)
        {
            if (_count == Capacity) Capacity = _count * 2;
            _items[_count] = item;
            _count++;
            OnChanged();
        }

        public void Add(T[] numberArray)
        {
            foreach (var i in numberArray)
            {
                Add(i);
            }
        }

        protected virtual void OnChanged()
        {
            if (Changed != null) Changed(this, EventArgs.Empty);
        }

        public override bool Equals(object other)
        {
            return Equals(this, other as UserDefinedList<T>);
        }

        static bool Equals(UserDefinedList<T> a, UserDefinedList<T> b)
        {
            if (a is null) return b is null;
            if (b is null || a._count != b._count) return false;
            for (int i = 0; i < a._count; i++)
            {
                if (!object.Equals(a._items[i], b._items[i]))
                {
                    return false;
                }
            }
            return true;
        }


        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i = 0; i < _count-1; i++)
            {
                sb.Append(_items[i]).Append(", ");
            }

            sb.Append(_items[_count - 1]);

            return sb.ToString();
        }


        // Event...
        public event EventHandler Changed;

        public int ListVersion { get; set; }

        // Operators...
        public static bool operator ==(UserDefinedList<T> a, UserDefinedList<T> b)
        {
            return Equals(a, b);
        }
        public static bool operator !=(UserDefinedList<T> a, UserDefinedList<T> b)
        {
            return !Equals(a, b);
        }
    }
}

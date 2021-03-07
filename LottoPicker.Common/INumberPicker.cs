using System;

namespace LottoPicker.Common
{
    public interface INumberPicker
    {
        IUserDefinedList<int> Pick649();
        IUserDefinedList<int> PickLottoMax();
        IUserDefinedList<int> PickNumbers(string sourceKey);
    }

    public interface IUserDefinedList<T>
    {
        int Count { get; }
        int Capacity { get; set; }
        int ListVersion { get; set; }
        T this[int index] { get; set; }
        T this[int i, int j] { get; set; }
        void Add(T item);
        void Add(T[] numberArray);
        bool Equals(object other);

        /// <inheritdoc />
        string ToString();

        event EventHandler Changed;
    }
}

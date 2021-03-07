using System;

namespace LottoPicker.Common
{
    public interface IArraySort<in T> where T : IComparable
    {
        void Sort(T[] list, int first, int last);
    }

}

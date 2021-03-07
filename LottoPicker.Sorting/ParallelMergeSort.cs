using System;
using System.Threading.Tasks;
using LottoPicker.Common;

namespace LottoPicker.Sorting
{
    public class ParallelMergeArraySort<T> : IArraySort<T> where T : IComparable
    {
        public void Sort(T[] list, int first, int last)
        {
            var sortedList = SplitAndMerge(list, first, last);
            sortedList.CopyTo(list, 0);
        }

        private T[] SplitAndMerge(T[] list, int first, int last)
        {
            if (last <= first)
            {
                var item = new T[1];
                item[0] = list[first];
                return item;
            }

            var twain = (first + last) / 2;
            var t1 = Task.Factory.StartNew(() => SplitAndMerge(list, first, twain));
            var t2 = Task.Factory.StartNew(() => SplitAndMerge(list, twain + 1, last));

            var left = t1.Result;
            var right = t2.Result;

            var sortedList = Task.Factory.StartNew(() => Merge(left, right)).Result;

            return sortedList;
        }

        private T[] Merge(T[] leftList, T[] rightList)
        {
            int leftSize = leftList.Length, rightSize = rightList.Length;
            int leftRunner = 0, rightRunner = 0, fullRunner = 0;
            var mergedList = new T[leftSize + rightSize];

            while (rightRunner < rightSize && leftRunner < leftSize)
            {
                mergedList[fullRunner++] = leftList[leftRunner].CompareTo(rightList[rightRunner]) < 0
                    ? leftList[leftRunner++]
                    : rightList[rightRunner++];
            }
            while (leftRunner < leftSize)
            {
                mergedList[fullRunner++] = leftList[leftRunner++];
            }
            while (rightRunner < rightSize)
            {
                mergedList[fullRunner++] = rightList[rightRunner++];
            }

            return mergedList;

            //for (fullRunner = first; fullRunner <= last; fullRunner++)
            //{
            //    if (leftRunner < middle && (rightRunner > last || leftList[leftRunner].CompareTo(leftList[rightRunner]) < 1))
            //        rightList[fullRunner] = leftList[leftRunner++];
            //    else
            //        rightList[fullRunner] = leftList[rightRunner++];
            //}

            //return rightList;
            //for (var fullRunner = first; fullRunner <= last; fullRunner++)
            //{
            //    list[fullRunner] = mergedList[fullRunner];
            //}

        }

    }
}

using System;
using System.Collections.Generic;
using LottoPicker.Common;

//using FunWithCodeCore.Library.Arrays;

namespace LottoPicker.Lists
{
    public class LotteryNumbers : INumberPicker
    {
        private readonly IArraySort<int> _sorter;

        public LotteryNumbers(IArraySort<int> sorter)
        {
            _sorter = sorter;
        }

        private Dictionary<string, PickList> _sources;

        public IUserDefinedList<int> Pick649()
        {
            return PickNumbers("649");
        }

        public IUserDefinedList<int> PickLottoMax()
        {
            return PickNumbers("max");
        }

        //public int[] PickNumbers(string sourceKey)
        public IUserDefinedList<int> PickNumbers(string sourceKey)
        {
            Initialize();

            var source = _sources[sourceKey];
            var length = source.DrawSize;
            var numbers = source.Numbers;

            var rnd = new Random();
            var numberArray = new int[length];
            var numberList = new UserDefinedList<int>();
            var n = 0;

            while (n < length)
            {
                var next = rnd.Next(0, source.RangeSize);
                if (numbers[next] == 0) continue;
                numberArray[n] = numbers[next];
                numbers[next] = 0;
                n++;
            }

            _sorter.Sort(numberArray, 0, source.DrawSize-1);

            numberList.Add(numberArray);
            //return numberArray;
            return numberList;


        }

        private void Initialize()
        {
            _sources = new Dictionary<string, PickList>()
            {
                {
                    "max", new PickList
                    {
                        DrawSize = 7, SetSize = 50, Numbers = new[]
                        {
                            1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                            11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                            21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
                            31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
                            41, 42, 43, 44, 45, 46, 47, 48, 49, 50
                        }

                    }
                },
                {
                    "649", new PickList
                    {
                        DrawSize = 6, SetSize = 49, Numbers = new[]
                        {
                            1, 2, 3, 4, 5, 6, 7, 8, 9, 10,
                            11, 12, 13, 14, 15, 16, 17, 18, 19, 20,
                            21, 22, 23, 24, 25, 26, 27, 28, 29, 30,
                            31, 32, 33, 34, 35, 36, 37, 38, 39, 40,
                            41, 42, 43, 44, 45, 46, 47, 48, 49
                        }
                    }
                }
            };

        }

        private class PickList
        {
            public int DrawSize { get; set; }
            public int SetSize { get; set; }
            public int[] Numbers { get; set; }

            public int RangeSize => Numbers.Length;
        }
    }
}

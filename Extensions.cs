using System;
using System.Collections.Generic;
using System.Linq;

namespace Sudoku
{
    public static class Extensions
    {
        public static int FindMissingNumber(this IEnumerable<int> list)
        {
            var fullSum = 0;
            var array = list.ToArray();
            for (var i = 1; i <= array.Length; i++)
            {
                fullSum += i;
            }
            return fullSum - array.Sum();
        }

        public static List<List<int>> Clone(this IEnumerable<List<int>> originalList)
        {
            return originalList.Select(innerList => innerList.ToList()).ToList();
        }

        public static IEnumerable<int> Fill(int from, int to)
        {
            if (from > to)
            {
                var temp = from;
                from = to;
                to = temp;
            }
            var list = new List<int>();
            for (var i = from; i <= to; i++)
            {
                list.Add(i);
            }
            return list;
        }

        private static List<List<int>> Split(this IList<int> list, int value)
        {
            if (!list.Contains(value)) throw new ArgumentException();

            var count = list.Count - 1;
            for (var i = 0; i < count; i++)
            {
                if (list[i] == value) return new List<List<int>> { list.Take(i).ToList(), list.Skip(i + 1).ToList() };
            }
            return new List<List<int>> { list.Take(count).ToList(), list.Skip(count + 1).ToList() };
        }

        public static IEnumerable<int> Sequence(this Random random, int from, int to)
        {
            if (from > to)
            {
                var temp = from;
                from = to;
                to = temp;
            }
            var list = new List<int>();
            var remainingValueLists = new List<List<int>> { Fill(from, to - 1).ToList() };
            for (var i = from; i < to; i++)
            {
                int index;
                List<int> listToGenerateFrom;
                do
                {
                    index = random.Next(0, remainingValueLists.Count);
                    listToGenerateFrom = remainingValueLists[index];
                } while (!listToGenerateFrom.Any());
                var value = random.Next(listToGenerateFrom.First(), listToGenerateFrom.Last() + 1);
                var splitList = listToGenerateFrom.Split(value);
                remainingValueLists[index] = splitList.First();
                remainingValueLists.Add(splitList.Last());
                list.Add(value);
            }
            return list;
        }
    }
}

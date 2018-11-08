using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public static class DelegateTest
    {
        public enum SortType
        {
            Ascending,
            Descending
        }

        public static void BubbleSort(int[] items)
        {
            int temp;
            if (items == null)
            {
                return;
            }

            for (var i = items.Length - 1; i >= 0; i--)
            {
                for (var j = 1; j <= i; j++)
                {
                    if (!(items[j - 1] > items[j])) continue;
                    temp = items[j - 1];
                    items[j - 1] = items[j];
                    items[j] = temp;
                }
            }
        }

        public static void BubbleSort(int[] items, SortType sortOrder)
        {
            int temp;
            if (items == null)
            {
                return;
            }

            for (var i = items.Length - 1; i >= 0; i--)
            {
                for (var j = 1; j <= i; j++)
                {
                    bool swap = false;
                    switch (sortOrder)
                    {
                        case SortType.Ascending:
                            swap = items[j - 1] > items[j];
                            break;

                        case SortType.Descending:
                            swap = items[j - 1] < items[j];
                            break;
                    }
                    if (!swap) continue;
                    temp = items[j - 1];
                    items[j - 1] = items[j];
                    items[j] = temp;
                }
            }
        }

        public delegate bool ComparisonHandler(int first, int second);

        public static void BubbleSort(int[] items, ComparisonHandler comparisonHandler)
        {
            if (comparisonHandler == null)
            {
                throw new ArgumentException("comparisonMethod");
            }

            int temp;
            if (items == null)
            {
                return;
            }

            for (var i = items.Length - 1; i >= 0; i--)
            {
                for (var j = 1; j <= i; j++)
                {
                    if (comparisonHandler(items[j - 1], items[j]))
                    {
                        temp = items[j - 1];
                        items[j - 1] = items[j];
                        items[j] = temp;
                    }
                }
            }
        }

        public static bool GreaterThan(int first, int second)
        {
            return first > second;
        }
    }
}
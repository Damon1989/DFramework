using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdapterSample
{
    public interface IScoreOperation
    {
        int[] Sort(int[] array);
        int Search(int[] array, int key);
    }

    public class BinarySearchClass
    {
        public int BinarySearch(int[] array, int key)
        {
            int low = 0;
            int high = array.Length - 1;
            while (low <= high)
            {
                int mid = (low + high) / 2;
                int midVal = array[mid];
                if (midVal < key)
                {
                    low = mid + 1;
                }else if (midVal > key)
                {
                    high = mid - 1;
                }
                else
                {
                    return 1;
                }
            }

            return -1;
        }
    }

    public class QuickSortClass
    {
        public void Swap(int[] a, int i, int j)
        {
            int t = a[i];
            a[i] = a[j];
            a[j] = t;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ConsoleApp
{
    internal class GenericTest
    {
    }

    public static class MatchEx
    {
        public static T Max<T>(T first, params T[] values)
        where T : IComparable<T>
        {
            T maxnum = first;
            foreach (var item in values)
            {
                if (item.CompareTo(maxnum) > 0)
                {
                    maxnum = item;
                }
            }
            return maxnum;
        }

        public static T Min<T>(T first, params T[] values)
            where T : IComparable<T>
        {
            T minnum = first;
            foreach (var item in values)
            {
                if (item.CompareTo(minnum) < 0)
                {
                    minnum = item;
                }
            }

            return minnum;
        }
    }

    public class MethodOverloads
    {
        public void Foo<T>(T obj)
        {
            WriteLine($"Foo<T>(T obj),obj type :{obj.GetType().Name}");
        }

        public void Foo(int x)
        {
            WriteLine($"Foo(int x)");
        }

        public void Bar<T>(T obj)
        {
            Foo(obj);
        }
    }
}
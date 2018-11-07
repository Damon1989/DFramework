using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //ArrayTest();
            ReadLine();
        }

        public static void ListTest()
        {
            var list1=new LinkedList();
            list1.AddLast(2);
            list1.AddLast(4);
            list1.AddLast(6);
        }

        public static void ArrayTest()
        {
            int[] myArray;

            myArray=new int[4];

            int[] myArray1=new int[4];

            int[] myArray2 = new int[4] {1, 3, 5, 6};

            int[] myArray3 = new[] {1, 2, 3, 4};

            int[] myArray4 = {1, 2, 3, 4};

            

            for (int i = 0; i < myArray4.Length; i++)
            {
                WriteLine(myArray4[i]);
            }

            foreach (var item in myArray4)
            {
                WriteLine(item);
            }

            

            WriteLine("-----------------------------");

            Array intArray1 = Array.CreateInstance(typeof(int), 5);
            for (int i = 0; i < 5; i++)
            {
                intArray1.SetValue(33,i);
            }

            for (int i = 0; i < intArray1.Length; i++)
            {
                WriteLine(intArray1.GetValue(i));
            }


            WriteLine("-----------------------------");

            string[] names = {"B", "D", "C", "A"};
            foreach (var name in names)
            {
                WriteLine(name);
            }
            Array.Sort(names);
            foreach (var name in names)
            {
                WriteLine(name);
            }

            Person[] myPersons =
            {
                new Person("c", "b"),
                new Person("c","a"),
                new Person("b","c"),
                new Person("b","a"),
                new Person("b","b"),
                new Person("a","c"),
                new Person("a","b"),
                new Person("a","a"),
            };
            foreach (var myPerson in myPersons)
            {
                WriteLine(myPerson);
            }
            WriteLine("--------------------");
            Array.Sort(myPersons,new PersonComparer(PersonCompareType.FirstName));
            foreach (var myPerson in myPersons)
            {
                WriteLine(myPerson);
            }

            WriteLine("--------------------");
            Array.Sort(myPersons, new PersonComparer(PersonCompareType.LastName));
            foreach (var myPerson in myPersons)
            {
                WriteLine(myPerson);
            }

            int[] ar1 = {1, 4, 5, 11, 13, 18};
            int[] ar2 = {3, 4, 5, 18, 21, 27, 33};

            var segments=new ArraySegment<int>[]
            {
                new ArraySegment<int>(ar1,0,3),
                new ArraySegment<int>(ar2,3,3)
            };
            WriteLine(SumOfSegments(segments));

            WriteLine("111111111111111111111");
            foreach (var person in myPersons)
            {
                WriteLine(person);
            }
            WriteLine("111111111111111111111");
            var enumerator=myPersons.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Person p = (Person)enumerator.Current;
                WriteLine(p);
            }

            var helloCollection=new HelloCollection();
            foreach (var s in helloCollection)
            {
                WriteLine(s);
            }

            var titles=new MusicTitles();
            foreach (var title in titles)
            {
                WriteLine(title);
            }

            WriteLine();

            WriteLine("reverse");

        }

        static int SumOfSegments(ArraySegment<int>[] segments)
        {
            int sum = 0;
            foreach (var segment in segments)
            {
                for (int i = segment.Offset; i < segment.Offset+segments.Length; i++)
                {
                    if (segment.Array != null) sum += segment.Array[i];
                }
            }

            return sum;
        }
    }


    public class Person:IComparable<Person>,IEquatable<Person>
    {

        public int Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Person(string firstName, string lastName)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
        }

        public int CompareTo(Person other)
        {
            if (other==null)
            {
                return 1;
            }

            int result = string.CompareOrdinal(this.LastName, other.LastName);
            if (result==0)
            {
                result = string.CompareOrdinal(this.FirstName, other.FirstName);
            }

            return result;
        }

        public override string ToString()
        {
            return $"{Id} {FirstName} {LastName}";
        }

        public bool Equals(Person other)
        {
            //if (ReferenceEquals(null, other)) return false;
            //if (ReferenceEquals(this, other)) return true;
            //return string.Equals(FirstName, other.FirstName) && string.Equals(LastName, other.LastName);

            if (other==null)
            {
                return base.Equals(other);
            }
            else
            {
                return this.Id == other.Id && this.FirstName == other.FirstName && this.LastName == other.LastName;
            }
        }

        public override bool Equals(object obj)
        {
            //if (ReferenceEquals(null, obj)) return false;
            //if (ReferenceEquals(this, obj)) return true;
            //if (obj.GetType() != this.GetType()) return false;
            //return Equals((Person) obj);

            if (obj==null)
            {
                return base.Equals(obj);
            }

            return Equals((obj) as Person);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }

    public enum PersonCompareType
    {
        FirstName,
        LastName
    }

    public class PersonComparer : IComparer<Person>
    {
        private PersonCompareType _compareType;

        public PersonComparer(PersonCompareType compareType)
        {
            _compareType = compareType;
        }

        public int Compare(Person x, Person y)
        {
            if (x==null&&y==null)
            {
                return 0;
            }

            if (x == null) return 1;
            if (y == null) return -1;

            switch (_compareType)
            {
                case PersonCompareType.FirstName:
                {
                    var result= string.CompareOrdinal(x.FirstName, y.FirstName);
                    if (result==0)
                    {
                        result = string.CompareOrdinal(x.LastName, y.LastName);
                    }
                    return result;
                }
                case PersonCompareType.LastName:
                {
                    var result = string.CompareOrdinal(x.LastName, y.LastName);
                    if (result == 0)
                    {
                        result = string.CompareOrdinal(x.FirstName, y.FirstName);
                    }
                    return result;
                    }
                default:
                    throw new ArgumentException("unexpected compare type");
            }
        }
    }

    public class HelloCollection
    {
        //public IEnumerator<string> GetEnumerator()
        //{
        //    yield return "Hello";
        //    yield return "World";
        //}

        public IEnumerator GetEnumerator()
        {
            return new Enumerator(0);
        }

        public class Enumerator:IEnumerator<string>,IEnumerator,IDisposable
        {
            private int state;
            private string current;

            public Enumerator(int state)
            {
                this.state = state;
            }

            public void Dispose()
            {
                throw new NotImplementedException();
            }

            public bool MoveNext()
            {
                switch (state)
                {
                    case 0:
                        current = "Hello";
                        state = 1;
                        return true;
                    case 1:
                        current = "World";
                        state = 2;
                        return true;
                    case 2:
                        break;
                }

                return false;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            public string Current => current;

            object IEnumerator.Current => Current;
        }
    }

    public class MusicTitles
    {
        public string[] names = {"A", "B", "C", "D"};

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var t in names)
            {
                yield return t;
            }
        }

        public IEnumerator<string> Reverse()
        {
            for (int i = 3; i >= 0; i--)
            {
                yield return names[i];
            }
        }

        public IEnumerator<string> Subset(int index, int length)
        {
            for (int i = index; i < index+length; i++)
            {
                yield return names[i];
            }
        }
    }

    public class LinkedListNode
    {
        public object Value { get; set; }

        public LinkedListNode(object value)
        {
            Value = value;
        }

        public LinkedListNode Next { get; set; }
        public LinkedListNode Prev { get; set; }
    }


    public class LinkedListNode<T>
    {
        public T Value { get; set; }

        public LinkedListNode(T value)
        {
            Value = value;
        }

        public LinkedListNode<T> Next { get; set; }
        public LinkedListNode<T> Prev { get; set; }
    }

    public class LinkedList : IEnumerable
    {
        public LinkedListNode First { get; set; }
        public LinkedListNode Last { get; set; }

        public LinkedListNode AddLast(object value)
        {
            var newNode=new LinkedListNode(value);
            if (First==null)
            {
                First = newNode;
                Last = First;
            }
            else
            {
                LinkedListNode previous = Last;
                Last.Next = newNode;
                Last = newNode;
                Last.Prev = previous;
            }

            return newNode;
        }

        public IEnumerator GetEnumerator()
        {
            LinkedListNode current = First;
            while (current!=null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }
    }

    public class LinkedList<T> : IEnumerable<T>
    {
        public LinkedListNode<T> First { get; set; }
        public LinkedListNode<T> Last { get; set; }

        public LinkedListNode<T> AddLast(T value)
        {
            var newNode = new LinkedListNode<T>(value);
            if (First == null)
            {
                First = newNode;
                Last = First;
            }
            else
            {
                LinkedListNode<T> previous = Last;
                Last.Next = newNode;
                Last = newNode;
                Last.Prev = previous;
            }

            return newNode;
        }

        public IEnumerator<T> GetEnumerator()
        {
            LinkedListNode<T> current = First;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

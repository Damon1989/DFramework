using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DFramework.Infrastructure;
using Quartz;
using Quartz.Impl;
using StackExchange.Redis;
using static System.Console;
using Configuration = DFramework.Config.Configuration;

namespace ConsoleApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var url = "name=damon&age=29&gender=m";
            var a=url.GetQueryString();
            for (int i = 0; i < a.Keys.Count; i++)
            {
                Console.WriteLine(a.Get(i));
            }
            foreach (var item in a)
            {
                Console.WriteLine(a.Keys);
            }

            Console.Read();
            //ArrayTest();
            //Utility.YHSJ(6);

            //var result = MatchEx.Max(1, 2, 3, 4);
            //WriteLine(result);
            //result = MatchEx.Min(1, 2, 3, 4);
            //WriteLine(result);
            //List<int> array = new List<int>() { 1, 2, 3, 4, 5 };
            //var enumerator = array.GetEnumerator();

            //while (enumerator.MoveNext())
            //{
            //    Console.WriteLine(enumerator.Current);
            //}

            //WriteLine("------------------------");

            //Stack<int> stack = new Stack<int>();
            //stack.Push(1);
            //stack.Push(2);
            //stack.Push(3);
            //stack.Push(4);
            //stack.Push(5);
            //stack.Push(6);
            //var stackEnumerator = stack.GetEnumerator();

            //while (stackEnumerator.MoveNext())
            //{
            //    WriteLine(stackEnumerator.Current);
            //}

            //var test=new MethodOverloads();
            //test.Foo(33);
            //test.Foo("abc");
            //test.Bar(44);
            //ReadLine();

            //var list = new List<int> {1, 2, 3, 4, 5};
            //var list1=list.Where(r => r > 1);

            var list = new List<int>();
            list.Add(true, 1);
            list.Add(false, 2);
            list.Add(true, 3);
            list.Add(true, 4);
            list.Add(false, 5);
            WriteLine(string.Join(",",list));

            return;

            var database = RedisManager.Instance.GetDatabase();
            //database.StringSet("00000000", "123", TimeSpan.FromSeconds(1));
            //WriteLine(database.KeyExists("00000000"));
            //Thread.Sleep(2000);
            //WriteLine(database.KeyExists("00000000"));
            //for (int i = 0; i < 10000; i++)
            //{
            //    database.StringSet($"key{i}",i);
            //}
            var redisAccount=new RedisAccount()
            {
                Name = "qbadmin11",
                LastOnlineIP = "127.0.0.1",
                Password = "123456",
                Salt = "123",
                Ticket = "345",
                UserDisplayName = "11",
                UserId = "22"
            };

            Task.Factory.StartNew(() =>
            {
                database.HashSetAsync($"account_{redisAccount.Name}",
                    new HashEntry[]
                    {
                        new HashEntry(nameof(redisAccount.Name),redisAccount.Name),
                        new HashEntry(nameof(redisAccount.LastOnlineIP),redisAccount.LastOnlineIP),
                        new HashEntry(nameof(redisAccount.Password),redisAccount.Password),
                        new HashEntry(nameof(redisAccount.Salt),redisAccount.Salt),
                        new HashEntry(nameof(redisAccount.Ticket),redisAccount.Ticket),
                        new HashEntry(nameof(redisAccount.UserId),redisAccount.UserId),
                        new HashEntry(nameof(redisAccount.UserDisplayName),redisAccount.UserDisplayName),
                    }).ConfigureAwait(false);
            });
            
            //database.HashSet($"account_{redisAccount.Name}",
            //    new HashEntry[]
            //    {
            //        new HashEntry(nameof(redisAccount.Name),redisAccount.Name),
            //        new HashEntry(nameof(redisAccount.LastOnlineIP),redisAccount.LastOnlineIP),
            //        new HashEntry(nameof(redisAccount.Password),redisAccount.Password),
            //        new HashEntry(nameof(redisAccount.Salt),redisAccount.Salt),
            //        new HashEntry(nameof(redisAccount.Ticket),redisAccount.Ticket),
            //        new HashEntry(nameof(redisAccount.UserId),redisAccount.UserId),
            //        new HashEntry(nameof(redisAccount.UserDisplayName),redisAccount.UserDisplayName),
            //    });

            //WriteLine(database.HashGet($"account_{redisAccount.Name}", nameof(redisAccount.Name)));

            var hashlist=database.HashGetAll($"account_{redisAccount.Name}");
            
            WriteLine($"{nameof(redisAccount.Name)}:{hashlist.FirstOrDefault(c=>c.Name==nameof(redisAccount.Name)).Value}");
            WriteLine($"{nameof(redisAccount.LastOnlineIP)}:{hashlist.FirstOrDefault(c => c.Name == nameof(redisAccount.LastOnlineIP)).Value}");
            WriteLine($"{nameof(redisAccount.Password)}:{hashlist.FirstOrDefault(c => c.Name == nameof(redisAccount.Password)).Value}");
            WriteLine($"{nameof(redisAccount.Salt)}:{hashlist.FirstOrDefault(c => c.Name == nameof(redisAccount.Salt)).Value}");
            WriteLine($"{nameof(redisAccount.Ticket)}:{hashlist.FirstOrDefault(c => c.Name == nameof(redisAccount.Ticket)).Value}");
            WriteLine($"{nameof(redisAccount.UserId)}:{hashlist.FirstOrDefault(c => c.Name == nameof(redisAccount.UserId)).Value}");
            WriteLine($"{nameof(redisAccount.UserDisplayName)}:{hashlist.FirstOrDefault(c => c.Name == nameof(redisAccount.UserDisplayName)).Value}");
            
            

            //hashlist.ForEach(item =>
            //{
            //    WriteLine(item.Name);
            //    WriteLine(item.Value);
            //});

            //var keys=RedisManager.Server.Keys();
            //keys.ForEach(item => { WriteLine(item); });

            return ;
            Permission permission = Permission.Create | Permission.Read | Permission.Update | Permission.Delete;
            WriteLine("1、枚举创建，并赋值");
            WriteLine(permission.ToString());//Create, Read, Update, Delete
            WriteLine((int)permission);//15

            permission=(Permission) Enum.Parse(typeof(Permission), "5");
            WriteLine("2、通过数字字符串转换......");
            WriteLine(permission.ToString());//Create, Update
            WriteLine((int)permission);//5

            permission = (Permission) Enum.Parse(typeof(Permission), "update,delete,read", true);
            WriteLine("3、通过枚举名称字符串转换......");
            WriteLine(permission.ToString());//Read, Update, Delete
            WriteLine((int)permission);//14

            permission = (Permission)7;
            WriteLine("4、直接用数字强制转换......");
            WriteLine(permission.ToString());//Create, Read, Update
            WriteLine((int)permission);//7

            permission = permission & ~Permission.Read;
            WriteLine("5、去掉一个枚举项");
            WriteLine(permission.ToString());//Create, Update
            WriteLine((int)permission);//5

            permission = permission | Permission.Delete;
            WriteLine("6、加上一个枚举项");
            WriteLine(permission.ToString());//Create, Update, Delete
            WriteLine((int)permission);//13

            WriteLine(permission.HasFlag(Permission.Delete));//True

            if (permission.HasFlag(Permission.Delete))
            {
                WriteLine("!!!!!!!!");
            }

            WriteLine(permission.GetHashCode());
            WriteLine(permission.GetTypeCode());
            
            return;

            string fullPath = @"~\\WebSite1\\Default.aspx";

            Console.WriteLine(fullPath.GetFileName());
            Console.WriteLine(fullPath.GetFileExtension());
            Console.WriteLine(fullPath.GetFileNameWithoutExtension());
            Console.WriteLine(fullPath.GetDirectoryName());

            var myClass=new MyClass();
            Console.WriteLine(nameof(MyClass));
            Console.WriteLine(nameof(myClass));
            Console.WriteLine(nameof(myClass.Name).Trim().ToLower());

            return;

            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteServerSchedulerClient";


            //设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            //远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "556";
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp";

            var schedulerFactory = new StdSchedulerFactory(properties);
            var scheduler = schedulerFactory.GetScheduler();

            var job = JobBuilder.Create<PrintMessageJob>()
                .WithIdentity(nameof(PrintMessageJob), "group1")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("myJobTrigger", "group1")
                .StartNow()
                .WithCronSchedule("/2 * * ? * *")
                .Build();

            scheduler.ScheduleJob(job, trigger);
            scheduler.Start();
        }
        public class  RedisAccount
        {
            public string Name { get; set; }
            public string Password { get; set; }
            public string Salt { get; set; }
            public string Ticket { get; set; }
            public string LastOnlineIP { get; set; }
            public string UserId { get; set; }
            public string UserDisplayName { get; set; }

        }
        public class MyClass
        {
            public string Name { get; set; }

        }

        [Flags]
        public enum  Permission
        {
            Create=1,
            Read=2,
            Update=4,
            Delete=8
        }


        public class  PrintMessageJob:IJob
        {
            public  void Execute(IJobExecutionContext context)
            {

                Console.WriteLine("Hello!");
                Console.WriteLine(DateTime.Now.ToLongTimeString());
                //await Task.FromResult(Task.Factory.StartNew(() => { Console.WriteLine("Hello!"); }));
            }
        }

        public static void ListTest()
        {
            var list1 = new LinkedList();
            list1.AddLast(2);
            list1.AddLast(4);
            list1.AddLast(6);
        }

        public static void ArrayTest()
        {
            int[] myArray;

            myArray = new int[4];

            int[] myArray1 = new int[4];

            int[] myArray2 = new int[4] { 1, 3, 5, 6 };

            int[] myArray3 = new[] { 1, 2, 3, 4 };

            int[] myArray4 = { 1, 2, 3, 4 };

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
                intArray1.SetValue(33, i);
            }

            for (int i = 0; i < intArray1.Length; i++)
            {
                WriteLine(intArray1.GetValue(i));
            }

            WriteLine("-----------------------------");

            string[] names = { "B", "D", "C", "A" };
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
            Array.Sort(myPersons, new PersonComparer(PersonCompareType.FirstName));
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

            int[] ar1 = { 1, 4, 5, 11, 13, 18 };
            int[] ar2 = { 3, 4, 5, 18, 21, 27, 33 };

            var segments = new ArraySegment<int>[]
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
            var enumerator = myPersons.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Person p = (Person)enumerator.Current;
                WriteLine(p);
            }

            var helloCollection = new HelloCollection();
            foreach (var s in helloCollection)
            {
                WriteLine(s);
            }

            var titles = new MusicTitles();
            foreach (var title in titles)
            {
                WriteLine(title);
            }

            WriteLine();

            WriteLine("reverse");
        }

        private static int SumOfSegments(ArraySegment<int>[] segments)
        {
            int sum = 0;
            foreach (var segment in segments)
            {
                for (int i = segment.Offset; i < segment.Offset + segments.Length; i++)
                {
                    if (segment.Array != null) sum += segment.Array[i];
                }
            }

            return sum;
        }
    }

    public class Person : IComparable<Person>, IEquatable<Person>
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
            if (other == null)
            {
                return 1;
            }

            int result = string.CompareOrdinal(this.LastName, other.LastName);
            if (result == 0)
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

            if (other == null)
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

            if (obj == null)
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
            if (x == null && y == null)
            {
                return 0;
            }

            if (x == null) return 1;
            if (y == null) return -1;

            switch (_compareType)
            {
                case PersonCompareType.FirstName:
                    {
                        var result = string.CompareOrdinal(x.FirstName, y.FirstName);
                        if (result == 0)
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

        public class Enumerator : IEnumerator<string>, IEnumerator, IDisposable
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
        public string[] names = { "A", "B", "C", "D" };

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
            for (int i = index; i < index + length; i++)
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
            var newNode = new LinkedListNode(value);
            if (First == null)
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
            while (current != null)
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

    public static class Utility
    {
        /// <summary>
        /// 阶乘 n!=1*2*3*...(n-1)*n
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static long Factorial(int num)
        {
            if (num == 1)
            {
                return 1;
            }

            return Factorial(num - 1) * num;
        }

        public static void YHSJ(int num)
        {
            int[][] a = new int[num][];
            a[0] = new int[1]; //a[0][0]=1;
            a[1] = new int[2];
            for (int i = 0; i < num; i++)
            {
                a[i] = new int[i + 1];
                a[i][0] = 1;
                a[i][i] = 1;
                if (i > 1)//求出中间的数据
                {
                    for (int j = 1; j < i; j++)
                    {
                        a[i][j] = a[i - 1][j - 1] + a[i - 1][j];
                    }
                }
            }
            for (int i = 0; i < a.Length; i++)
            {
                for (int k = 0; k < a.Length - 1 - i; k++)
                {
                    Console.Write(" ");
                }
                for (int j = 0; j < a[i].Length; j++)
                {
                    Console.Write(a[i][j] + " ");
                }
                Console.WriteLine();
            }
        }

        public static int CountZero(int num)
        {
            var result = 0;
            var numStr = num.ToString();
            for (var i = numStr.Length - 1; i >= 0; i--)
            {
                if (numStr[i] == '0')
                {
                    result++;
                }
                else
                {
                    break;
                }
            }

            return result;
        }
    }
}
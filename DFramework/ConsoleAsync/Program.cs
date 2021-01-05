using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

namespace ConsoleAsync
{
    class Program
    {
        static void Main(string[] args)
        {
            Expression<Func<int, int, bool>> expression;
            expression = (x, y) => x > y;
            WriteLine($"-------------------{expression}--------------");

            PrintNode(expression.Body,0);

            WriteLine();
            WriteLine();

            expression = (x, y) => x * y > x + y;
            WriteLine($"-------------------{expression}--------------");
            PrintNode(expression.Body,0);

            //var model = new PersonModel {ID = "1", Name = "damon"};
            //var result = GetPropertyValue(model, l => l.Name);
            //Debug.WriteLine($"显示名称：{result.Item1}--值：{result.Item2}");
            Console.ReadLine();

            //var al=new ArrayList();
            //al.Add(1);
            //al.Add(2);
            //al.Insert(1,3);
            //foreach (var o in al)
            //{
            // Console.WriteLine(o);   
            //}

            //al.Sort();
            //foreach (var o in al)
            //{
            //    Console.WriteLine(o);
            //}
            //Console.WriteLine(al.Capacity);


            //Console.WriteLine("----------------------------");
            //Console.WriteLine("Hashtable");
            //var ht=new Hashtable();
            //ht.Add("001", "Zara Ali");
            //ht.Add("002", "Abida Rehman");
            //ht.Add("003", "Joe Holzner");
            //ht.Add("004", "Mausam Benazir Nur");
            //ht.Add("005", "M. Amlan");
            //ht.Add("006", "M. Arif");
            //ht.Add("007", "Ritesh Saikia");

            //if (ht.ContainsValue("Nuha Ali"))
            //{
            //    Console.WriteLine("This student name is already in the list");
            //}
            //else
            //{
            //    ht.Add("008", "Nuha Ali");
            //}
            //// 获取键的集合
            //ICollection key = ht.Keys;

            //foreach (string k in key)
            //{
            //    Console.WriteLine(k + ": " + ht[k]);
            //}




            //Console.WriteLine("----------------------------");
            //Console.WriteLine("SortedList");


            //var sl = new SortedList();

            //sl.Add("001", "Zara Ali");
            //sl.Add("002", "Abida Rehman");
            //sl.Add("003", "Joe Holzner");
            //sl.Add("004", "Mausam Benazir Nur");
            //sl.Add("005", "M. Amlan");
            //sl.Add("006", "M. Arif");
            //sl.Add("007", "Ritesh Saikia");

            //if (sl.ContainsValue("Nuha Ali"))
            //{
            //    Console.WriteLine("This student name is already in the list");
            //}
            //else
            //{
            //    sl.Add("008", "Nuha Ali");
            //}

            //// 获取键的集合
            // key = sl.Keys;

            //foreach (string k in key)
            //{
            //    Console.WriteLine(k + ": " + sl[k]);
            //}


            //Console.WriteLine("----------------------------");
            //Console.WriteLine("Stack");

            //var st=new Stack();
            //st.Push('A');
            //st.Push('M');
            //st.Push('G');
            //st.Push('W');

            //WriteLine("Current stack:");
            //foreach (var c in st)
            //{
            //    Write(c+" ");
            //}
            //WriteLine();
            //st.Push('V');
            //st.Push('H');
            //Console.WriteLine("The next poppable value in stack: {0}",
            //    st.Peek());
            //Console.WriteLine("Current stack: ");
            //foreach (char c in st)
            //{
            //    Console.Write(c + " ");
            //}
            //Console.WriteLine();

            //Console.WriteLine("Removing values ");
            //st.Pop();
            //st.Pop();
            //st.Pop();

            //Console.WriteLine("Current stack: ");
            //foreach (char c in st)
            //{
            //    Console.Write(c + " ");
            //}

            //var list=new List<string>();

            //Console.WriteLine("----------------------------");
            //Console.WriteLine("泛型");

            //var byteArray = new Byte[] {5, 1, 4, 2, 3};
            //Array.Sort<Byte>(byteArray);

            //var i = Array.BinarySearch<Byte>(byteArray, 2);
            //    WriteLine(i);

            //var dictionary=new ConcurrentDictionary<int,string>();
            //var newValue = dictionary.AddOrUpdate(0, key => "Zero", (key, oldValue) => "Zero");
            //string currentValue;
            //bool keyExists = dictionary.TryGetValue(0, out currentValue);

            //string removeValue;
            //bool keyExisted = dictionary.TryRemove(0, out removeValue);


            //var stack = ImmutableStack<int>.Empty;
            //stack = stack.Push(13);

            //var biggerStack = stack.Push(7);

            //foreach (var item in biggerStack)
            //{
            //    Trace.WriteLine(item);
            //}

            //foreach (var item in stack)
            //{
            //    Trace.WriteLine(item);
            //}

            //var stack = ImmutableStack<int>.Empty;
            //stack=stack.Push(13);
            //stack=stack.Push(7);

            //foreach (var item in stack)
            //{
            //    Trace.WriteLine(item);
            //}

            //int lastItem;
            //stack = stack.Pop(out lastItem);
            //Trace.WriteLine(lastItem);

            //var t=new Thread(Go);
            //t.Start();
            //t.Join();
            //Console.WriteLine("Thread t is end");
            Console.Read();
        }

        static void Go()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.Write("y");
            }
        }

        private const int NUM_ASC_KEYS = 10000;
        private static ConcurrentQueue<string> _keysQueue;

        private static void ParallelPartitionGenerateAESKeys()
        {
            var sw = Stopwatch.StartNew();
            Parallel.ForEach(Partitioner.Create(1, NUM_ASC_KEYS + 1), range =>
            {
                var aesM = new AesManaged();
                Debug.WriteLine(
                    $"AES Range ({range.Item1},{range.Item2}, TimeOfDay before inner loop starts:{DateTime.Now.TimeOfDay})");
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    aesM.GenerateKey();
                    byte[] result = aesM.Key;
                    string hexString = result.ToString();
                    _keysQueue.Enqueue(hexString);
                }
            });
            Debug.WriteLine($"AES:{sw.Elapsed.ToString()}");
        }

        /// <summary>
        /// 通过Linq表达式获取成员属性
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Tuple<string, string> GetPropertyValue<T>(T instance, Expression<Func<T, string>> expression)
        {
            MemberExpression memberExpression=expression.Body as MemberExpression;
            string propertyName = memberExpression.Member.Name;
            string attributeName = (memberExpression.Member.GetCustomAttributes(false)[0] as DescriptionAttribute).Description;
            var property = typeof(T).GetProperties().Where(l => l.Name == propertyName).First();
            return new Tuple<string, string>(attributeName,property.GetValue(instance).ToString());
        }

        public DataTable GetSum<T>(IQueryable<T> collection, Expression<Func<T, string>> groupby,
            params Expression<Func<T, double>>[] expressions)
        {
            DataTable table=new DataTable();

            MemberExpression memberExpression=groupby.Body as MemberExpression;
            var displayName = (memberExpression.Member.GetCustomAttributes(false)[0] as DescriptionAttribute)
                .Description;
            table.Columns.Add(new DataColumn(displayName));

            foreach (var expression in expressions)
            {
                memberExpression=expression.Body as MemberExpression;
                displayName = (memberExpression.Member.GetCustomAttributes(false)[0] as DescriptionAttribute)
                    .Description;
                table.Columns.Add(new DataColumn(displayName));
            }

            var groups = collection.GroupBy(groupby);

            foreach (var group in groups)
            {
                DataRow dataRow = table.NewRow();
                dataRow[0] = group.Key;
                for (int i = 0; i < expressions.Length; i++)
                {
                    var expression = expressions[i];
                    Func<T, double> fn = expression.Compile();
                    dataRow[i + 1] = group.Sum(fn);
                }

                table.Rows.Add(dataRow);
            }

            return table;
        }


        public static void PrintNode(Expression expression, int indent)
        {
            if (expression is BinaryExpression)
            {
                PrintNode(expression as BinaryExpression, indent);
            }
            else
            {
                PrintSingle(expression,indent);
            }
        }

        private static void PrintNode(BinaryExpression expression, int indent)
        {
            PrintNode(expression.Left,indent+1);
            PrintSingle(expression,indent);
            PrintNode(expression.Right,indent+1);
        }

        private static void PrintSingle(Expression expression, int indent)
        {
            WriteLine("{0,"+indent*5+"}{1}","",NodeToString(expression));
        }

        private static string NodeToString(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Multiply:
                    return "*";
                case ExpressionType.Add:
                    return "+";
                case ExpressionType.Divide:
                    return "/";
                case ExpressionType.Subtract:
                    return "-";
                case ExpressionType.GreaterThan:
                    return ">";
                case ExpressionType.LessThan:
                    return "<";
                default:
                    return expression.ToString() + " (" + expression.NodeType.ToString() + ")";
            }
        }
    }

    class ThreadSafe
    {
        private static bool _done;

        static readonly object _locker=new object();

        static void Main1()
        {
            new Thread(Go).Start();
            Go();
        }

        static void Go()
        {
            lock (_locker)
            {
                if (!_done)
                {
                    Console.WriteLine("Done");
                    _done = true;
                }
            }
        }
    }

    [Description("唯一标识")]
    class PersonModel
    {
        [Description("唯一标识")] public string ID { get; set; }

        [Description("名称")] public string Name { get; set; }

        [Description("值")] public double Value { get; set; }

        [Description("年齡")] public double Age { get; set; }

        [Description("收入")] public double InCome { get; set; }

        [Description("支出")] public double Pay { get; set; }
    }
}

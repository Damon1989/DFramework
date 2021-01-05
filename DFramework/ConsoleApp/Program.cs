using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using DFramework.Infrastructure;
using Microsoft.Web.Administration;
using Quartz;
using Quartz.Impl;
using static System.Console;
using DCommon;

namespace ConsoleApp
{
    using Autofac;
    using Autofac.Extras.DynamicProxy;
    using Castle.DynamicProxy;
    public class CustomComparer : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            string s1 = (string)x;
            string s2 = (string)y;
            if (s1.Length > s2.Length) return 1;
            if (s1.Length < s2.Length) return -1;
            for (int i = 0; i < s1.Length; i++)
            {
                if (s1[i] > s2[i]) return 1;
                if (s1[i] < s2[i]) return -1;
            }
            return 0;
        }
    }
    internal class Program
    {
        private static void DifferentDataLinkedList()
        {
            Node head=new TypedNode<Char>(',');
            head=new TypedNode<DateTime>(DateTime.Now,head);
            head=new TypedNode<string>("Today is ",head);
            Console.WriteLine(head.ToString());
        }

        [Flags]
        public enum Permission
        {
            Create = 1,
            Read = 2,
            Update = 4,
            Delete = 8
        }

        public static class MyInfoClass
        {
            public static string GetInfo(out string msg)
            {
                msg = "2";
                return "123";
            }
        }

        public static string SortCsvColumns(string csv_data)
        {
            if (string.IsNullOrEmpty(csv_data))
            {
                return string.Empty;
            }
            var array = csv_data.Split('\n');
            if (array.Length == 0)
            {
                return string.Empty;
            }
            var dictionary=new Dictionary<string,int>();
            var titleArray = array[0].Split(',');
            for (int i = 0; i < titleArray.Length; i++)
            {
                dictionary.Add(titleArray[i],i);
            }
            
            var resultArray = new ArrayList();
            for (int j = 0; j < titleArray.Length; j++)
            {
                titleArray[j] = new System.Text.RegularExpressions.Regex("[\\s]+").Replace(titleArray[j].Trim(), " ");
            }

            titleArray = titleArray.OrderBy(p => p).ToArray();
            


            var title = string.Join(",", titleArray);
            resultArray.Add(title);

            for (int i = 1; i < array.Length; i++)
            {
                var contentArray=new ArrayList();
                var content = array[i].Split(',');
                foreach (var t in titleArray)
                {
                    var key = dictionary[t];
                    contentArray.Add(content[key]);
                }

                resultArray.Add(string.Join(",", contentArray.ToArray()));
            }

            return string.Join("\n", resultArray.ToArray());
        }

        public static int Find(int[] items)
        {
            var itemLength = items.Length;
            var first = true;
            
            for (int i = 1; i < itemLength; i++)
            {
                if (items[0] < items[i])
                {
                    first=false;
                    break;
                }
            }
            if (first) return 0;

            for (int i = 1; i < itemLength-1; i++)
            {
                if (items[i] >= items[i - 1])
                {
                    var check = true;
                    for (int j = i+1; j < itemLength; j++)
                    {
                        if (items[i] >= items[j])
                        {
                            check = false;
                            break;
                        }
                    }
                    if (check) return i;
                }
            }


            
            var last = true;
            for (int i = 0; i < itemLength - 1; i++)
            {
                if (items[i] > items[itemLength - 1])
                {
                    last = false;
                    break;
                }
            }

            if (last) return itemLength - 1;

            return -1;
        }

        private static void Main(string[] args)
        {
            DifferentDataLinkedList();
            Console.ReadLine();
            //var a = new int[] { 4, 2, 2, 3, 1, 8, 7, 8, 6, 9 };
            //int nIndex = Find(a);
            // Console.WriteLine(nIndex);
            // Console.ReadLine();
            //string content = "<script></script>";

            //content= Regex.Replace(content, "[<]", "&lt;", RegexOptions.IgnoreCase);
            //Console.WriteLine(content);
            //Console.Read();
            //ICat icat = new Cat();
            //var catProxy = new CatProxy(icat);
            //catProxy.Eat();
            //Console.Read();

            //var builder = new ContainerBuilder();
            //builder.RegisterType<CatInterceptor>();//注册拦截器
            //builder.RegisterType<Cat>().As<ICat>().InterceptedBy(typeof(CatInterceptor)).EnableInterfaceInterceptors();//注册Cat并为其添加拦截器

            //var container = builder.Build();
            //var cat= container.Resolve<ICat>();
            //cat.Eat();
            //cat.Sleep();
            //Console.Read();

            //var builder = new ContainerBuilder();
            //builder.RegisterType<CatInterceptor>();//注册拦截器
            //builder.RegisterType<Cat>().As<ICat>();//注册Cat
            //builder.RegisterType<CatOwner>().InterceptedBy(typeof(CatInterceptor)).EnableClassInterceptors(
            //    ProxyGenerationOptions.Default,
            //    additionalInterfaces: typeof(ICat));//注册CatOwner并为其添加拦截器和接口

            //var container = builder.Build();

            //var cat = container.Resolve<CatOwner>();//获取CatOwner的代理类
            //cat.GetType().GetMethod("Eat").Invoke(cat, null);
            //cat.GetType().GetMethod("Sleep").Invoke(cat, null);

            //Console.Read();

        }

        //private static void Main(string[] args)
        //{
        //    var info = MyInfoClass.GetInfo(out var msg);
        //    Console.WriteLine(info);
        //    Console.WriteLine(msg);
        //    Console.ReadLine();
        //    //var list = new List<string>() { "1", "2", "3" };
        //    //Console.WriteLine(string.Join(list,"'",""));

        //    //ServerManager serverManager = new ServerManager();
        //    //foreach (var pool in serverManager.ApplicationPools)
        //    //{
        //    //    if (pool.State==ObjectState.Stopped)
        //    //    {
        //    //        pool.Start();
        //    //    }
        //    //    LoggerHelper.WriteLine($"{pool.Name}");
        //    //}



        //    //ReadLine();
        //    //var bytes = Encoding.Default.GetBytes("damon");
        //    //WriteLine(ToHexString(bytes));
        //    //WriteLine(DateTime.Now.ToString("MMddHHmmss"));
        //    //WriteLine(EncryptWithMD5("damon", "123456"));
        //    //Read();

        //    //var url = "name=damon&age=29&gender=m";
        //    //var a=url.GetQueryString();
        //    //for (int i = 0; i < a.Keys.Count; i++)
        //    //{
        //    //    Console.WriteLine(a.Get(i));
        //    //}
        //    //foreach (var item in a)
        //    //{
        //    //    Console.WriteLine(a.Keys);
        //    //}

        //    //Console.Read();
        //    //ArrayTest();
        //    //Utility.YHSJ(6);

        //    //var result = MatchEx.Max(1, 2, 3, 4);
        //    //WriteLine(result);
        //    //result = MatchEx.Min(1, 2, 3, 4);
        //    //WriteLine(result);
        //    //List<int> array = new List<int>() { 1, 2, 3, 4, 5 };
        //    //var enumerator = array.GetEnumerator();

        //    //while (enumerator.MoveNext())
        //    //{
        //    //    Console.WriteLine(enumerator.Current);
        //    //}

        //    //WriteLine("------------------------");

        //    //Stack<int> stack = new Stack<int>();
        //    //stack.Push(1);
        //    //stack.Push(2);
        //    //stack.Push(3);
        //    //stack.Push(4);
        //    //stack.Push(5);
        //    //stack.Push(6);
        //    //var stackEnumerator = stack.GetEnumerator();

        //    //while (stackEnumerator.MoveNext())
        //    //{
        //    //    WriteLine(stackEnumerator.Current);
        //    //}

        //    //var test=new MethodOverloads();
        //    //test.Foo(33);
        //    //test.Foo("abc");
        //    //test.Bar(44);
        //    //ReadLine();

        //    //var list = new List<int> {1, 2, 3, 4, 5};
        //    //var list1=list.Where(r => r > 1);

        //    //var list = new List<int>();
        //    //list.Add(true, 1);
        //    //list.Add(false, 2);
        //    //list.Add(true, 3);
        //    //list.Add(true, 4);
        //    //list.Add(false, 5);
        //    //WriteLine(string.Join(",",list));

        //    //return;

        //    //var database = RedisManager.Instance.GetDatabase();
        //    //database.StringSet("00000000", "123", TimeSpan.FromSeconds(1));
        //    //WriteLine(database.KeyExists("00000000"));
        //    //Thread.Sleep(2000);
        //    //WriteLine(database.KeyExists("00000000"));
        //    //for (int i = 0; i < 10000; i++)
        //    //{
        //    //    database.StringSet($"key{i}",i);
        //    //}
        //    //var redisAccount=new RedisAccount()
        //    //{
        //    //    Name = "qbadmin11",
        //    //    LastOnlineIP = "127.0.0.1",
        //    //    Password = "123456",
        //    //    Salt = "123",
        //    //    Ticket = "345",
        //    //    UserDisplayName = "11",
        //    //    UserId = "22"
        //    //};

        //    //Task.Factory.StartNew(() =>
        //    //{
        //    //    database.HashSetAsync($"account_{redisAccount.Name}",
        //    //        new HashEntry[]
        //    //        {
        //    //            new HashEntry(nameof(redisAccount.Name),redisAccount.Name),
        //    //            new HashEntry(nameof(redisAccount.LastOnlineIP),redisAccount.LastOnlineIP),
        //    //            new HashEntry(nameof(redisAccount.Password),redisAccount.Password),
        //    //            new HashEntry(nameof(redisAccount.Salt),redisAccount.Salt),
        //    //            new HashEntry(nameof(redisAccount.Ticket),redisAccount.Ticket),
        //    //            new HashEntry(nameof(redisAccount.UserId),redisAccount.UserId),
        //    //            new HashEntry(nameof(redisAccount.UserDisplayName),redisAccount.UserDisplayName),
        //    //        }).ConfigureAwait(false);
        //    //});

        //    //database.HashSet($"account_{redisAccount.Name}",
        //    //    new HashEntry[]
        //    //    {
        //    //        new HashEntry(nameof(redisAccount.Name),redisAccount.Name),
        //    //        new HashEntry(nameof(redisAccount.LastOnlineIP),redisAccount.LastOnlineIP),
        //    //        new HashEntry(nameof(redisAccount.Password),redisAccount.Password),
        //    //        new HashEntry(nameof(redisAccount.Salt),redisAccount.Salt),
        //    //        new HashEntry(nameof(redisAccount.Ticket),redisAccount.Ticket),
        //    //        new HashEntry(nameof(redisAccount.UserId),redisAccount.UserId),
        //    //        new HashEntry(nameof(redisAccount.UserDisplayName),redisAccount.UserDisplayName),
        //    //    });

        //    //WriteLine(database.HashGet($"account_{redisAccount.Name}", nameof(redisAccount.Name)));

        //    //var hashlist=database.HashGetAll($"account_{redisAccount.Name}");

        //    //WriteLine($"{nameof(redisAccount.Name)}:{hashlist.FirstOrDefault(c=>c.Name==nameof(redisAccount.Name)).Value}");
        //    //WriteLine($"{nameof(redisAccount.LastOnlineIP)}:{hashlist.FirstOrDefault(c => c.Name == nameof(redisAccount.LastOnlineIP)).Value}");
        //    //WriteLine($"{nameof(redisAccount.Password)}:{hashlist.FirstOrDefault(c => c.Name == nameof(redisAccount.Password)).Value}");
        //    //WriteLine($"{nameof(redisAccount.Salt)}:{hashlist.FirstOrDefault(c => c.Name == nameof(redisAccount.Salt)).Value}");
        //    //WriteLine($"{nameof(redisAccount.Ticket)}:{hashlist.FirstOrDefault(c => c.Name == nameof(redisAccount.Ticket)).Value}");
        //    //WriteLine($"{nameof(redisAccount.UserId)}:{hashlist.FirstOrDefault(c => c.Name == nameof(redisAccount.UserId)).Value}");
        //    //WriteLine($"{nameof(redisAccount.UserDisplayName)}:{hashlist.FirstOrDefault(c => c.Name == nameof(redisAccount.UserDisplayName)).Value}");


        //    //hashlist.ForEach(item =>
        //    //{
        //    //    WriteLine(item.Name);
        //    //    WriteLine(item.Value);
        //    //});

        //    //var keys=RedisManager.Server.Keys();
        //    //keys.ForEach(item => { WriteLine(item); });

        //    return;
        //    var permission = Permission.Create | Permission.Read | Permission.Update | Permission.Delete;
        //    WriteLine("1、枚举创建，并赋值");
        //    WriteLine(permission.ToString()); //Create, Read, Update, Delete
        //    WriteLine((int) permission); //15

        //    permission = (Permission) Enum.Parse(typeof(Permission), "5");
        //    WriteLine("2、通过数字字符串转换......");
        //    WriteLine(permission.ToString()); //Create, Update
        //    WriteLine((int) permission); //5

        //    permission = (Permission) Enum.Parse(typeof(Permission), "update,delete,read", true);
        //    WriteLine("3、通过枚举名称字符串转换......");
        //    WriteLine(permission.ToString()); //Read, Update, Delete
        //    WriteLine((int) permission); //14

        //    permission = (Permission) 7;
        //    WriteLine("4、直接用数字强制转换......");
        //    WriteLine(permission.ToString()); //Create, Read, Update
        //    WriteLine((int) permission); //7

        //    permission = permission & ~Permission.Read;
        //    WriteLine("5、去掉一个枚举项");
        //    WriteLine(permission.ToString()); //Create, Update
        //    WriteLine((int) permission); //5

        //    permission = permission | Permission.Delete;
        //    WriteLine("6、加上一个枚举项");
        //    WriteLine(permission.ToString()); //Create, Update, Delete
        //    WriteLine((int) permission); //13

        //    WriteLine(permission.HasFlag(Permission.Delete)); //True

        //    if (permission.HasFlag(Permission.Delete)) WriteLine("!!!!!!!!");

        //    WriteLine(permission.GetHashCode());
        //    WriteLine(permission.GetTypeCode());

        //    return;

        //    var fullPath = @"~\\WebSite1\\Default.aspx";

        //    WriteLine(fullPath.GetFileName());
        //    WriteLine(fullPath.GetFileExtension());
        //    WriteLine(fullPath.GetFileNameWithoutExtension());
        //    WriteLine(fullPath.GetDirectoryName());

        //    var myClass = new MyClass();
        //    WriteLine(nameof(MyClass));
        //    WriteLine(nameof(myClass));
        //    WriteLine(nameof(myClass.Name).Trim().ToLower());

        //    return;

        //    var properties = new NameValueCollection();
        //    properties["quartz.scheduler.instanceName"] = "RemoteServerSchedulerClient";


        //    //设置线程池
        //    properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
        //    properties["quartz.threadPool.threadCount"] = "5";
        //    properties["quartz.threadPool.threadPriority"] = "Normal";

        //    //远程输出配置
        //    properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
        //    properties["quartz.scheduler.exporter.port"] = "556";
        //    properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
        //    properties["quartz.scheduler.exporter.channelType"] = "tcp";

        //    var schedulerFactory = new StdSchedulerFactory(properties);
        //    var scheduler = schedulerFactory.GetScheduler();

        //    var job = JobBuilder.Create<PrintMessageJob>()
        //        .WithIdentity(nameof(PrintMessageJob), "group1")
        //        .Build();

        //    var trigger = TriggerBuilder.Create()
        //        .WithIdentity("myJobTrigger", "group1")
        //        .StartNow()
        //        .WithCronSchedule("/2 * * ? * *")
        //        .Build();

        //    scheduler.ScheduleJob(job, trigger);
        //    scheduler.Start();
        //}

        public static string EncryptWithMD5(string userName, string password)
        {
            var md5Hasher = new MD5CryptoServiceProvider();
            var bytes = Encoding.Default.GetBytes($"{userName}{password}{DateTime.Now.ToString("MMddHHmmss")}");
            //var bytes = Encoding.Default.GetBytes($"{userName}{password}{DateTime.Parse("2019-06-24 17:09:27").ToString("MMddHHmmss")}");
            bytes = md5Hasher.ComputeHash(bytes);
            return ToHexString(bytes).ToLower();
        }

        public static string ToHexString(byte[] bytes)
        {
            var hexString = string.Empty;
            if (bytes != null)
            {
                var sb = new StringBuilder();
                for (var i = 0; i < bytes.Length; i++) sb.Append(bytes[i].ToString("X2"));

                hexString = sb.ToString();
            }

            return hexString;
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

            var myArray1 = new int[4];

            var myArray2 = new int[4] {1, 3, 5, 6};

            int[] myArray3 = {1, 2, 3, 4};

            int[] myArray4 = {1, 2, 3, 4};

            for (var i = 0; i < myArray4.Length; i++) WriteLine(myArray4[i]);

            foreach (var item in myArray4) WriteLine(item);

            WriteLine("-----------------------------");

            var intArray1 = Array.CreateInstance(typeof(int), 5);
            for (var i = 0; i < 5; i++) intArray1.SetValue(33, i);

            for (var i = 0; i < intArray1.Length; i++) WriteLine(intArray1.GetValue(i));

            WriteLine("-----------------------------");

            string[] names = {"B", "D", "C", "A"};
            foreach (var name in names) WriteLine(name);
            Array.Sort(names);
            foreach (var name in names) WriteLine(name);

            Person[] myPersons =
            {
                new Person("c", "b"),
                new Person("c", "a"),
                new Person("b", "c"),
                new Person("b", "a"),
                new Person("b", "b"),
                new Person("a", "c"),
                new Person("a", "b"),
                new Person("a", "a")
            };
            foreach (var myPerson in myPersons) WriteLine(myPerson);
            WriteLine("--------------------");
            Array.Sort(myPersons, new PersonComparer(PersonCompareType.FirstName));
            foreach (var myPerson in myPersons) WriteLine(myPerson);

            WriteLine("--------------------");
            Array.Sort(myPersons, new PersonComparer(PersonCompareType.LastName));
            foreach (var myPerson in myPersons) WriteLine(myPerson);

            int[] ar1 = {1, 4, 5, 11, 13, 18};
            int[] ar2 = {3, 4, 5, 18, 21, 27, 33};

            var segments = new[]
            {
                new ArraySegment<int>(ar1, 0, 3),
                new ArraySegment<int>(ar2, 3, 3)
            };
            WriteLine(SumOfSegments(segments));

            WriteLine("111111111111111111111");
            foreach (var person in myPersons) WriteLine(person);
            WriteLine("111111111111111111111");
            var enumerator = myPersons.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var p = (Person) enumerator.Current;
                WriteLine(p);
            }

            var helloCollection = new HelloCollection();
            foreach (var s in helloCollection) WriteLine(s);

            var titles = new MusicTitles();
            foreach (var title in titles) WriteLine(title);

            WriteLine();

            WriteLine("reverse");
        }

        private static int SumOfSegments(ArraySegment<int>[] segments)
        {
            var sum = 0;
            foreach (var segment in segments)
                for (var i = segment.Offset; i < segment.Offset + segments.Length; i++)
                    if (segment.Array != null)
                        sum += segment.Array[i];

            return sum;
        }

        public class RedisAccount
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


        public class PrintMessageJob : IJob
        {
            public void Execute(IJobExecutionContext context)
            {
                WriteLine("Hello!");
                WriteLine(DateTime.Now.ToLongTimeString());
                //await Task.FromResult(Task.Factory.StartNew(() => { Console.WriteLine("Hello!"); }));
            }
        }
    }

    public static class LoggerHelper
    {
        public static void WriteLine(string msg)
        {
            WriteLine(msg, true);
        }

        public static void WriteLine(string msg, bool recordIp)
        {
            WriteLine(msg, recordIp, "C");
        }

        public static void WriteLine(string msg, string disk)
        {
            WriteLine(msg, true, disk);
        }

        public static void WriteLine(string msg, bool recordIp, string disk = "C")
        {
            try
            {
                //if (ConfigurationManager.AppSettings["WriteLog"]
                //    .Equals("False", StringComparison.InvariantCultureIgnoreCase))
                //{
                //    return;
                //}
            }
            catch (Exception e)
            {

            }
            var now = DateTime.Now;
            var directory = $"{disk}://log//{now.Year}//{now.Month}//{now.Day}";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var fileCount = Directory.GetFiles(directory).Length;
            var filePath = fileCount == 0 ? $"{directory}//{fileCount}.txt" : $"{directory}//{fileCount - 1}.txt";

            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
            }
            else
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Length / 1024 / 1024 > 5)
                {
                    File.Create(filePath.Replace($"{Path.GetFileName(filePath)}", $"{fileCount}.txt")).Close();
                    WriteLine(msg, recordIp, disk);
                }
            }

            var ip = recordIp ;//? GetHostAddress() : "";
            using (var fileStream = new FileStream(filePath, FileMode.Append))
            {
                using (var writer = new StreamWriter(fileStream))
                {
                    writer.WriteLine($"{now}--{ip}--{msg}");
                    writer.WriteLine();
                }
            }
        }

        //private static string GetHostAddress()
        //{
        //    try
        //    {
        //        var userHostAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
        //        if (string.IsNullOrEmpty(userHostAddress))
        //        {
        //            userHostAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        //        }
        //        //最后判断获取是否成功，并检查IP地址的格式（检查其格式非常重要）
        //        if (!string.IsNullOrEmpty(userHostAddress) && IsIp(userHostAddress))
        //        {
        //            return userHostAddress;
        //        }
        //        return "127.0.0.1";
        //    }
        //    catch
        //    {
        //        return "127.0.0.1";
        //    }
        //}

        private static bool IsIp(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
    }

    public class Person : IComparable<Person>, IEquatable<Person>
    {
        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public int Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int CompareTo(Person other)
        {
            if (other == null) return 1;

            var result = string.CompareOrdinal(LastName, other.LastName);
            if (result == 0) result = string.CompareOrdinal(FirstName, other.FirstName);

            return result;
        }

        public bool Equals(Person other)
        {
            //if (ReferenceEquals(null, other)) return false;
            //if (ReferenceEquals(this, other)) return true;
            //return string.Equals(FirstName, other.FirstName) && string.Equals(LastName, other.LastName);

            if (other == null)
                return base.Equals(other);
            return Id == other.Id && FirstName == other.FirstName && LastName == other.LastName;
        }

        public override string ToString()
        {
            return $"{Id} {FirstName} {LastName}";
        }

        public override bool Equals(object obj)
        {
            //if (ReferenceEquals(null, obj)) return false;
            //if (ReferenceEquals(this, obj)) return true;
            //if (obj.GetType() != this.GetType()) return false;
            //return Equals((Person) obj);

            if (obj == null) return base.Equals(obj);

            return Equals(obj as Person);
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
        private readonly PersonCompareType _compareType;

        public PersonComparer(PersonCompareType compareType)
        {
            _compareType = compareType;
        }

        public int Compare(Person x, Person y)
        {
            if (x == null && y == null) return 0;

            if (x == null) return 1;
            if (y == null) return -1;

            switch (_compareType)
            {
                case PersonCompareType.FirstName:
                {
                    var result = string.CompareOrdinal(x.FirstName, y.FirstName);
                    if (result == 0) result = string.CompareOrdinal(x.LastName, y.LastName);
                    return result;
                }
                case PersonCompareType.LastName:
                {
                    var result = string.CompareOrdinal(x.LastName, y.LastName);
                    if (result == 0) result = string.CompareOrdinal(x.FirstName, y.FirstName);
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
                        Current = "Hello";
                        state = 1;
                        return true;

                    case 1:
                        Current = "World";
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

            public string Current { get; private set; }

            object IEnumerator.Current => Current;
        }
    }

    public class MusicTitles
    {
        public string[] names = {"A", "B", "C", "D"};

        public IEnumerator<string> GetEnumerator()
        {
            foreach (var t in names) yield return t;
        }

        public IEnumerator<string> Reverse()
        {
            for (var i = 3; i >= 0; i--) yield return names[i];
        }

        public IEnumerator<string> Subset(int index, int length)
        {
            for (var i = index; i < index + length; i++) yield return names[i];
        }
    }

    public class LinkedListNode
    {
        public LinkedListNode(object value)
        {
            Value = value;
        }

        public object Value { get; set; }

        public LinkedListNode Next { get; set; }
        public LinkedListNode Prev { get; set; }
    }

    public class LinkedListNode<T>
    {
        public LinkedListNode(T value)
        {
            Value = value;
        }

        public T Value { get; set; }

        public LinkedListNode<T> Next { get; set; }
        public LinkedListNode<T> Prev { get; set; }
    }

    public class LinkedList : IEnumerable
    {
        public LinkedListNode First { get; set; }
        public LinkedListNode Last { get; set; }

        public IEnumerator GetEnumerator()
        {
            var current = First;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

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
                var previous = Last;
                Last.Next = newNode;
                Last = newNode;
                Last.Prev = previous;
            }

            return newNode;
        }
    }

    public class LinkedList<T> : IEnumerable<T>
    {
        public LinkedListNode<T> First { get; set; }
        public LinkedListNode<T> Last { get; set; }

        public IEnumerator<T> GetEnumerator()
        {
            var current = First;
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
                var previous = Last;
                Last.Next = newNode;
                Last = newNode;
                Last.Prev = previous;
            }

            return newNode;
        }
    }

    public static class Utility
    {
        /// <summary>
        ///     阶乘 n!=1*2*3*...(n-1)*n
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static long Factorial(int num)
        {
            if (num == 1) return 1;

            return Factorial(num - 1) * num;
        }

        public static void YHSJ(int num)
        {
            var a = new int[num][];
            a[0] = new int[1]; //a[0][0]=1;
            a[1] = new int[2];
            for (var i = 0; i < num; i++)
            {
                a[i] = new int[i + 1];
                a[i][0] = 1;
                a[i][i] = 1;
                if (i > 1) //求出中间的数据
                    for (var j = 1; j < i; j++)
                        a[i][j] = a[i - 1][j - 1] + a[i - 1][j];
            }

            for (var i = 0; i < a.Length; i++)
            {
                for (var k = 0; k < a.Length - 1 - i; k++) Write(" ");
                for (var j = 0; j < a[i].Length; j++) Write(a[i][j] + " ");
                WriteLine();
            }
        }

        public static int CountZero(int num)
        {
            var result = 0;
            var numStr = num.ToString();
            for (var i = numStr.Length - 1; i >= 0; i--)
                if (numStr[i] == '0')
                    result++;
                else
                    break;

            return result;
        }
    }

    public interface ICat
    {
        string Eat();

        string Sleep();
    }

    public class Cat:ICat
    {
        public string Eat()
        {
            Console.WriteLine("猫在吃东西");
            return "Eat";
        }

        public string Sleep()
        {
            Console.WriteLine("猫在吃睡觉");
            return "Sleep";
        }
    }

    //public class CatProxy : ICat
    //{
    //    private readonly ICat _cat;

    //    public CatProxy(ICat cat)
    //    {
    //        this._cat = cat;
    //    }
    //    public string Eat()
    //    {
    //        Console.WriteLine("猫吃东西之前");
    //        _cat.Eat();
    //        Console.WriteLine("猫吃东西之后");
    //        return "Eat";
    //    }

    //    public string Sleep()
    //    {
    //        Console.WriteLine("猫睡觉之前");
    //        _cat.Eat();
    //        Console.WriteLine("猫睡觉之后");
    //        return "Sleep";
    //    }
    //}

    //public class CatInterceptor:IInterceptor
    //{
    //    public void Intercept(IInvocation invocation)
    //    {
    //        Console.WriteLine("猫吃东西之前");
    //        invocation.Proceed();
    //        Console.WriteLine("猫吃东西之后");
    //    }
    //}

    public class CatOwner
    {

    }

    public class CatInterceptor : IInterceptor
    {
        private readonly ICat _cat;

        public CatInterceptor(ICat cat)
        {
            this._cat = cat;
        }

        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("喂猫吃东西");

            invocation.Method.Invoke(_cat, invocation.Arguments);

        }
    }

    internal class Node
    {
        protected Node m_next;

        public Node(Node next)
        {
            m_next = next;
        }
    }

    internal sealed class TypedNode<T> : Node
    {
        public T m_data;

        public TypedNode(T data):this(data,null)
        {
            
        }
        public TypedNode(T data, Node next) : base(next)
        {
            m_data = data;
        }

        public override string ToString()
        {
            return m_data.ToString() + ((m_next != null) ? m_next.ToString() : string.Empty);
        }
    }
}
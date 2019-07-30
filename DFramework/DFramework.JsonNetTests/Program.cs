using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DFramework.Config;
using DFramework.Infrastructure;
using DFramework.IoC;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static System.Console;

namespace DFramework.JsonNetTests
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Configuration.Instance.UseAutofacContainer().RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
                .UseJsonNet();

            var account = new Account("james@example.com")
            {
                //Email = "james@example.com",
                Active = true,
                CreatedDate = new DateTime(2013, 1, 20, 0, 0, 0, DateTimeKind.Utc),
                Roles = new List<string>
                {
                    "User",
                    "Admin"
                }
            };

            var iJsonConvert = IoCFactory.Instance.CurrentContainer.Resolve<IJsonConvert>();
            var json = iJsonConvert.SerializeObject(account);
            WriteLine(json);

            //            json = JsonConvert.SerializeObject(account, Formatting.Indented);
            //            List<string> videogames = new List<string>
            //            {
            //                "Starcraft",
            //                "Halo",
            //                "Legend of Zelda"
            //            };
            //            json = JsonConvert.SerializeObject(videogames);

            //            Dictionary<string, int> points = new Dictionary<string, int>
            //            {
            //                { "James", 9001 },
            //                { "Jo", 3474 },
            //                { "Jess", 11926 }
            //            };

            //            json = JsonConvert.SerializeObject(points, Formatting.Indented);
            //            Movie movie = new Movie
            //            {
            //                Name = "Bad Boys",
            //                Year = 1995
            //            };
            //            File.WriteAllText(@"c:\movie.json", JsonConvert.SerializeObject(movie));

            //            using (StreamWriter file = File.CreateText(@"c:\movie1.json"))
            //            {
            //                JsonSerializer serializer = new JsonSerializer();
            //                serializer.Serialize(file, movie);
            //            }

            //            List<StringComparison> stringComparisons = new List<StringComparison>
            //            {
            //                StringComparison.CurrentCulture,
            //                StringComparison.Ordinal
            //            };
            //            string jsonWithoutConverter = JsonConvert.SerializeObject(stringComparisons);

            //            WriteLine(jsonWithoutConverter);

            //            //jsonWithoutConverter = JsonConvert.SerializeObject(stringComparisons, new StringEnumConverter());
            //            //WriteLine(jsonWithoutConverter);

            //            List<StringComparison> newStringComparsions = JsonConvert.DeserializeObject<List<StringComparison>>(
            //                jsonWithoutConverter, new StringEnumConverter());

            //            WriteLine(newStringComparsions);

            //            Console.WriteLine(string.Join(", ", newStringComparsions.Select(c => c.ToString()).ToArray()));

            //            WriteLine(json);

            //            DataSet dataSet = new DataSet("dataSet");
            //            dataSet.Namespace = "NetFrameWork";
            //            DataTable table = new DataTable();
            //            table.TableName = "tab1";
            //            DataColumn idColumn = new DataColumn("id", typeof(int));
            //            idColumn.AutoIncrement = true;

            //            DataColumn itemColumn = new DataColumn("item");
            //            table.Columns.Add(idColumn);
            //            table.Columns.Add(itemColumn);
            //            dataSet.Tables.Add(table);

            //            for (int i = 0; i < 2; i++)
            //            {
            //                DataRow newRow = table.NewRow();
            //                newRow["item"] = "item " + i;
            //                table.Rows.Add(newRow);
            //            }

            //            dataSet.AcceptChanges();

            //            json = JsonConvert.SerializeObject(dataSet, Formatting.Indented);

            //            WriteLine(json);

            //            json = @"{'Url':'http://www.google.com'}";

            //            try
            //            {
            //                JsonConvert.DeserializeObject<Website>(json);
            //            }
            //            catch (Exception ex)
            //            {
            //                Console.WriteLine(ex.Message);
            //                // Value cannot be null.
            //                // Parameter name: website
            //            }

            //            Website website = JsonConvert.DeserializeObject<Website>(json, new JsonSerializerSettings()
            //            {
            //                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            //            });
            //            WriteLine(website.Url);

            //            json = @"{
            //  'Name': 'James',
            //  'Offices': [
            //    'Auckland11111',
            //    'Wellington',
            //    'Christchurch'
            //  ]
            //}";

            //            UserViewModel model1 = JsonConvert.DeserializeObject<UserViewModel>(json);

            //            //foreach (string office in model1.Offices)
            //            //{
            //            //    Console.WriteLine(office);
            //            //}

            //            UserViewModel model2 = JsonConvert.DeserializeObject<UserViewModel>(json, new JsonSerializerSettings
            //            {
            //                ObjectCreationHandling = ObjectCreationHandling.Replace
            //            });

            //            foreach (string office in model2.Offices)
            //            {
            //                Console.WriteLine(office);
            //            }

            //            Person person = new Person();

            //            string jsonIncludeDefaultValues = JsonConvert.SerializeObject(person, Formatting.Indented);

            //            Console.WriteLine(jsonIncludeDefaultValues);

            //            string jsonIgnoreDefaultValues = JsonConvert.SerializeObject(person, Formatting.Indented, new JsonSerializerSettings
            //            {
            //                DefaultValueHandling = DefaultValueHandling.Ignore
            //            });

            //            Console.WriteLine(jsonIgnoreDefaultValues);

            //            jsonIgnoreDefaultValues = JsonConvert.SerializeObject(person, Formatting.Indented);
            //            Console.WriteLine(jsonIgnoreDefaultValues);

            //            string json = @"{
            //  'FullName': 'Dan Deleted',
            //  'Deleted': true,
            //  'DeletedDate': '2013-01-20T00:00:00'
            //}";

            //            try
            //            {
            //                var objectData = JsonConvert.DeserializeObject<Account>(json);
            //                WriteLine(objectData.FullName);
            //                JsonConvert.DeserializeObject<Account>(json, new JsonSerializerSettings
            //                {
            //                    MissingMemberHandling = MissingMemberHandling.Error,
            //                    NullValueHandling = NullValueHandling.Include
            //                });
            //            }
            //            catch (JsonSerializationException ex)
            //            {
            //                Console.WriteLine(ex.Message);
            //                // Could not find member 'DeletedDate' on object of type 'Account'. Path 'DeletedDate', line 4, position 23.
            //            }

            //Person person = new Person
            //{
            //    Name = "Nigal Newborn",
            //    Age = 1
            //};

            //string jsonIncludeNullValues = JsonConvert.SerializeObject(person, Formatting.Indented);

            //Console.WriteLine(jsonIncludeNullValues);
            //// {
            ////   "Name": "Nigal Newborn",
            ////   "Age": 1,
            ////   "Partner": null,
            ////   "Salary": null
            //// }

            //string jsonIgnoreNullValues = JsonConvert.SerializeObject(person, Formatting.Indented, new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Include       ,
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            //    DateFormatHandling = DateFormatHandling.IsoDateFormat

            //});

            //Console.WriteLine(jsonIgnoreNullValues);
            //// {
            ////   "Name": "Nigal Newborn",
            ////   "Age": 1
            //// }
            ///
            ///

            //            string json = @"{
            //  'FullName': 'Dan Deleted',
            //  'Deleted': true,
            //  'DeletedDate': '2013-01-20T00:00:00'
            //}";

            //            MemoryTraceWriter traceWriter = new MemoryTraceWriter();

            //            Account account = JsonConvert.DeserializeObject<Account>(json, new JsonSerializerSettings
            //            {
            //                TraceWriter = traceWriter
            //            });

            //            Console.WriteLine(traceWriter.ToString());

            //            string json = @"{
            //  ""UserName"": ""domain\\username"",
            //  ""Enabled"": true
            //}";

            //            User user = JsonConvert.DeserializeObject<User>(json);

            //            Console.WriteLine(user.UserName + "|||");

            //User user = new User
            //{
            //    UserName = @"domain\username"
            //};

            //string json = JsonConvert.SerializeObject(user, Formatting.Indented);

            //Console.WriteLine(json);

            //User user = new User
            //{
            //    UserName = @"domain\username",
            //    Status = UserStatus.Deleted
            //};

            //string json = JsonConvert.SerializeObject(user, Formatting.Indented);

            //Console.WriteLine(json);
            //Directory directory = new Directory
            //{
            //    Name = "My Documents",
            //    Files =
            //    {
            //        "ImportantLegalDocuments.docx",
            //        "WiseFinancalAdvice.xlsx"
            //    }
            //};

            //string json = JsonConvert.SerializeObject(directory, Formatting.Indented);

            //Console.WriteLine(json);

            //User user = new User
            //{
            //    FirstName = "Tom",
            //    LastName = "Riddle",
            //    SnakeRating = 10
            //};

            //string json = JsonConvert.SerializeObject(user, Formatting.Indented);

            //Console.WriteLine(json);

            ReadLine();
        }

        public class Account
        {
            public Account(string email)
            {
                Email = email;
            }

            [JsonProperty] private string Email { get; set; }

            public bool Active { get; set; }
            public DateTime CreatedDate { get; set; }
            public IList<string> Roles { get; set; }
        }

        [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
        public class User
        {
            [JsonProperty(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
            public string FirstName { get; set; }

            [JsonProperty(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
            public string LastName { get; set; }

            [JsonProperty(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
            public int SnakeRating { get; set; }
        }

        [JsonObject]
        public class Directory : IEnumerable<string>
        {
            public Directory()
            {
                Files = new List<string>();
            }

            public string Name { get; set; }
            public IList<string> Files { get; set; }

            public IEnumerator<string> GetEnumerator()
            {
                return Files.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        //public class Account
        //{
        //    public string FullName { get; set; }
        //    public bool Deleted { get; set; }
        //}

        //public class Person
        //{
        //    public string Name { get; set; }
        //    public int Age { get; set; }
        //    public Person Partner { get; set; }
        //    public decimal? Salary { get; set; }
        //}

        //public enum UserStatus
        //{
        //    NotConfirmed,
        //    Active,
        //    Deleted
        //}

        //public class User
        //{
        //    public string UserName { get; set; }

        //    [JsonConverter(typeof(StringEnumConverter))]
        //    public UserStatus Status { get; set; }
        //}

        //[JsonObject(MemberSerialization.Fields)]
        //public class File
        //{
        //    public File(Guid id)
        //    {
        //        Id = id;
        //    }

        //    // excluded from serialization
        //    // does not have JsonPropertyAttribute
        //    //[JsonProperty]
        //    private Guid Id { get; set; }

        //    //[JsonProperty]
        //    public string Name { get; set; }

        //    [JsonProperty]
        //    public int Size { get; set; }
        //}

        //public class User
        //{
        //    public string UserName { get; private set; }
        //    public bool Enabled { get; private set; }

        //    public User()
        //    {
        //        var i = 0;
        //    }

        //    [JsonConstructor]
        //    public User(string userName, bool enabled)
        //    {
        //        var j = 0;
        //        UserName = userName;
        //        Enabled = enabled;
        //    }
        //}

        //public class UserConverter : JsonConverter
        //{
        //    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        //    {
        //        User user = (User)value;

        //        writer.WriteValue(user.UserName);
        //    }

        //    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        //    {
        //        User user = new User();
        //        user.UserName = (string)reader.Value;

        //        return user;
        //    }

        //    public override bool CanConvert(Type objectType)
        //    {
        //        return objectType == typeof(User);
        //    }
        //}

        //[JsonConverter(typeof(UserConverter))]
        //public class User
        //{
        //    public string UserName { get; set; }
        //}

        //public class Account
        //{
        //    public Account(List<string> roles)
        //    {
        //        Roles = roles;
        //    }

        //    public string Email { get; set; }
        //    public bool Active { get; set; }
        //    public DateTime CreatedDate { get; set; }

        //    [JsonProperty("RoleList")]
        //    private IList<string> Roles { get; set; }
        //}

        //public class Movie
        //{
        //    public string Name { get; set; }
        //    public int Year { get; set; }
        //}

        //public class Website
        //{
        //    public string Url { get; set; }

        //    private Website()
        //    {
        //        var i = 0;
        //    }

        //    public Website(Website website)
        //    {
        //        if (website == null)
        //        {
        //            throw new ArgumentNullException(nameof(website));
        //        }

        //        Url = website.Url;
        //    }
        //}

        //public class UserViewModel
        //{
        //    public string Name { get; set; }
        //    public IList<string> Offices { get; private set; }

        //    public UserViewModel()
        //    {
        //        Offices = new List<string>
        //        {
        //            "Auckland",
        //            "Wellington",
        //            "Christchurch"
        //        };
        //    }
        //}

        //public class Person
        //{
        //    public string Name { get; set; }
        //    public int Age { get; set; }
        //    public Person Partner { get; set; }
        //    public decimal? Salary { get; set; }
        //}
    }
}
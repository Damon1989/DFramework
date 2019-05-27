using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMvcTest.Helper;

namespace MyMvcUnitTest
{
    [TestClass]
    public class RedisTest
    {
        private readonly IRedisService redisService;

        public RedisTest()
        {
            redisService = new RedisService();
        }

        [TestMethod]
        public void Set()
        {
            redisService.Set(new CacheValue("mykey", "myinfo"));
            var result = redisService.Get("mykey");
            Assert.AreEqual("myinfo", result);
        }

        [TestMethod]
        public async Task SetAsync()
        {
            await redisService.SetAsync(new CacheValue("mykeyasync", "myinfoasync"));
            var result = redisService.Get("mykeyasync");
            Assert.AreEqual("myinfoasync", result);
        }

        [TestMethod]
        public void SetPerson()
        {
            var person = new Person()
            {
                Name = "damon",
                Age = 29
            };
            var key = "myperson";
            redisService.Set(new CacheValue<Person>(key, person));
            var redisPerson = redisService.Get<Person>(key);
            Assert.AreEqual(person.Name, redisPerson.Name);
            Assert.AreEqual(person.Age, redisPerson.Age);
        }

        [TestMethod]
        public void SetFunc()
        {
            var key1 = "mykey1";
            var key2 = "mykey2";
            redisService.Set(key1, () => 1);
            var result = redisService.Get<int>(key1);
            Assert.AreEqual(result, 1);

            redisService.Set(key2, () => 2,60);
            result = redisService.Get<int>(key2);
            Assert.AreEqual(result, 2);
        }

        [TestMethod]
        public void HashSet()
        {
            var person = new Person()
            {
                Name = "damon",
                Age = 29
            };
            var key = "myHash";
            redisService.HashSet<Person>(new CacheValue<Person>(key, person));
        }

        [TestMethod]
        public async Task HashSetAsync()
        {
            var person = new Person()
            {
                Name = "damon",
                Age = 29
            };
            var key = "myHash1";
            await redisService.HashSetAsync<Person>(new CacheValue<Person>(key, person));
        }

        [TestMethod]
        public void HashGet()
        {
            var result= redisService.HashGet("myHash", "Name");
            Assert.AreEqual(result, "damon");
        }

        [TestMethod]
        public async Task HashGetAsync()
        {
            var result =await redisService.HashGetAsync("myHash1", "Name");
            Assert.AreEqual(result, "damon");
        }

        [TestMethod]
        public async Task HashGetIntAsync()
        {
            var result = await redisService.HashGetAsync<int>("myHash1", "Age");
            Assert.AreEqual(result, 29);
        }

        [TestMethod]
        public async Task HashGetObjectAsync()
        {
            var result = await redisService.HashGetAsync<Person>("myHash1");
            Assert.AreEqual(result.Name, "damon");
            Assert.AreEqual(result.Age, 29);
        }

        [TestMethod]
        public void Remove()
        {
            redisService.Remove("myHash1");
            redisService.Remove("myHash");
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }

    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMvcTest.Helper;

namespace MyMvcUnitTest
{
    [TestClass]
    public class JsonTest
    {
        [TestMethod]
        public void TestString()
        {
            var jsonPerson = new JsonPerson("damon", 29);
            var result = jsonPerson.ToJsonString(false);
            var result1 = jsonPerson.ToJsonString(useCamelCase: true);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result1);
        }
    }

    public class JsonPerson
    {
        public JsonPerson()
        {
        }

        public JsonPerson(string name, int age)
        {
            Name = name;
            Age = age;
            Phone = "15201864775";
        }

        public string Name { get; set; }

        public int Age { get; set; }

        private string Phone { get; }
    }
}
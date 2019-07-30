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
            var result = jsonPerson.ToJsonString(true,useStringEnumConvert:false);
            var result1 = jsonPerson.ToJsonString(useCamelCase: true,ignoreNullValue:true);

            var jsonPerson1 = result.ToJsonObject<JsonPerson>(false);
            var jsonPerson2 = result1.ToJsonObject<JsonPerson>();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result1);
            Assert.IsNotNull(jsonPerson1);
            Assert.IsNotNull(jsonPerson2);
        }
    }

    public class JsonPerson
    {
        public JsonPerson()
        {
        }

        public JsonPerson(string name, int age)
        {
            Name = null;
            Age = age;
            Phone = "15201864775";
            Gener = GenerEnum.Male;
        }

        public string Name { get; set; }

        public int Age { get; set; }

        private string Phone { get; set; }

        public GenerEnum Gener { get; set; }
    }

    public enum GenerEnum
    {
        Male,
        Female
    }
}
using DCommon;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyMvcTest.Helper;
using MyMvcTest.Test;

namespace MyMvcUnitTest
{
    [TestClass]
    public class ConcurrentTest
    {
        [TestMethod]
        public void ConcurrentDictionaryTest()
        {
            var concurrentTestHelper = new ConcurrentTestHelper();
            var result = concurrentTestHelper.ConcurrentDictionaryTest();
            LoggerHelper.WriteLine(result);
            Assert.IsNotNull(result);
        }
    }
}
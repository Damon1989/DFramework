using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace ConsoleApp
{
    [TestFixture]
    public class CalculatorTests
    {
        private IUSD_RMB_ExchangeRateFeed prvGetMockExchangeRateFeed()
        {
            Mock<IUSD_RMB_ExchangeRateFeed> mockObject = new Mock<IUSD_RMB_ExchangeRateFeed>();
            mockObject.Setup(m => m.GetActualUSDValue()).Returns(500);
            return mockObject.Object;
        }

        [Test(Description = "Divide 9 by 3. Expected result is 3.")]
        public void TC1_Divide9By3()
        {
            IUSD_RMB_ExchangeRateFeed feed = this.prvGetMockExchangeRateFeed();
            ICalculator calculator = new Calculator(feed);
            int actualResult = calculator.Divide(9, 3);
            int expectedResult = 3;
            Assert.AreEqual(expectedResult, actualResult);
        }

        [Test(Description = "Convert 2 USD to RMB. Expected result is 1000")]
        public void TC3_ConvertUSDtoRMBTest()
        {
            IUSD_RMB_ExchangeRateFeed feed = this.prvGetMockExchangeRateFeed();
            ICalculator calculator = new Calculator(feed);
            int actualResult = calculator.ConvertUSDtoRMB(2);
            int expectedResult = 1000;
            Assert.AreEqual(expectedResult, actualResult);
        }
    }
}
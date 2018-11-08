using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Calculator : ICalculator
    {
        private readonly IUSD_RMB_ExchangeRateFeed _feed;

        public Calculator(IUSD_RMB_ExchangeRateFeed feed)
        {
            this._feed = feed;
        }

        public int Add(int param1, int param2)
        {
            return param1 + param1;
        }

        public int Subtract(int param1, int param2)
        {
            return param1 - param2;
        }

        public int Multipy(int param1, int param2)
        {
            return param1 * param2;
        }

        public int Divide(int param1, int param2)
        {
            return param1 / param2;
        }

        public int ConvertUSDtoRMB(int unit)
        {
            return unit * this._feed.GetActualUSDValue();
        }
    }
}
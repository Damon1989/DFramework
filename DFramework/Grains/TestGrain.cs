using Interfaces;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grains
{
    public class TestGrain : Grain, ITest
    {
        private int num = 1;

        public Task AddCount(string taskName)
        {
            Console.WriteLine(taskName + "----" + num);
            num++;
            return Task.CompletedTask;
        }
    }
}
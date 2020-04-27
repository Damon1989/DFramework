using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using DCommon;
using MyMvcTest.Helper;

namespace MyMvcTest.Test
{
    public class ConcurrentTestHelper
    {
        private const string Item = "Dictionary item";
        public static string CurrentItem;

        public string ConcurrentDictionaryTest()
        {
            var sb = new StringBuilder();

            var concurrentDictionary = new ConcurrentDictionary<int, string>();
            var dictionary = new Dictionary<int, string>();

            var sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < 10000000; i++)
            {
                lock (dictionary)
                {
                    dictionary[i] = Item;
                }
            }

            sw.Stop();

            sb.Append($"Writing to dictionary with a lock :{sw.ElapsedMilliseconds}.  ");

            sw.Restart();

            for (int i = 0; i < 10000000; i++)
            {
                concurrentDictionary[i] = Item;
            }

            sw.Stop();

            sb.Append($"Writing to a concurrent dictionary:{sw.ElapsedMilliseconds}.  ");

            sw.Restart();

            for (int i = 0; i < 10000000; i++)
            {
                lock (dictionary)
                {
                    CurrentItem = dictionary[i];
                }
            }

            sw.Stop();

            sb.Append($"Reading from dictionary with a lock:{sw.ElapsedMilliseconds}.  ");

            sw.Restart();
            for (int i = 0; i < 10000000; i++)
            {
                CurrentItem = concurrentDictionary[i];
            }

            sw.Stop();

            sb.Append($"Reading from a concurrent dictionary:{sw.ElapsedMilliseconds}.");

            return sb.ToString();
        }


        //public string ConcurrentQueueTest()
        //{
        //    var taskQueue = new ConcurrentQueue<CustomTask>();

        //    var cts = new CancellationTokenSource();

            
        //}

        private async Task TaskProducer(ConcurrentQueue<CustomTask> queue)
        {
            for (int i = 0; i < 20; i++)
            {
                await Task.Delay(50);
                var workItem = new CustomTask {Id = i};
                queue.Enqueue(workItem);
                LoggerHelper.WriteLine($" Task {workItem.Id} has been posted");
            }
        }

        private async Task TaskProcessor(ConcurrentQueue<CustomTask> queue, string name, CancellationToken token)
        {

        }
    }

    public class CustomTask
    {
        public int Id { get; set; }
    }
}
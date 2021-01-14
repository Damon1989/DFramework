using System;

namespace SimpleFactorySample
{
    public class HistogramChart:IChart
    {
        public HistogramChart()
        {
            Console.WriteLine("创建柱状图！");
        }
        public void Display()
        {
            Console.WriteLine("显示柱状图！");
        }
    }
}
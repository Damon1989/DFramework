using System;

namespace AbstractFactorySample
{
    class SummerTextField : ITextField
    {
        public void Display()
        {
            Console.WriteLine("显示蓝色边框文本框。");
        }
    }
}
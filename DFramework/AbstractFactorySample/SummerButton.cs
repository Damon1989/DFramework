using System;

namespace AbstractFactorySample
{
    class SummerButton : IButton
    {
        public void Display()
        {
            Console.WriteLine("显示浅蓝色按钮。");
        }
    }
}
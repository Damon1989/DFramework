using System;

namespace AbstractFactorySample
{
    class SummerComboBox : IComboBox
    {
        public void Display()
        {
            Console.WriteLine("显示蓝色边框组合框。");
        }
    }
}
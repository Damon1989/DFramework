using System;

namespace AbstractFactorySample
{
    class SpringComboBox : IComboBox
    {
        public void Display()
        {
            Console.WriteLine("显示绿色边框组合框。");
        }
    }
}
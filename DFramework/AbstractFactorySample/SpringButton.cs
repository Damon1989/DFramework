using System;

namespace AbstractFactorySample
{
    class SpringButton : IButton
    {
        public void Display()
        {
            Console.WriteLine("显示浅绿色按钮。");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeSample
{
    public abstract class Component
    {
        public abstract void Add(Component c);//增加成员
        public abstract void Remove(Component c);//删除成员
        public abstract Component GetChild(int i);//获取成员
        public abstract void Operation();//业务方法
    }

    public class Leaf : Component
    {
        public override void Add(Component c)
        {
            //异常处理或错误提示
        }

        public override void Remove(Component c)
        {
            //异常处理或错误提示
        }

        public override Component GetChild(int i)
        {
            //异常处理或错误提示
            return null;
        }

        public override void Operation()
        {
            //叶子构件具体业务方法的实现
        }
    }


    public class Composite:Component
    {
        private List<Component> list=new List<Component>();
        public override void Add(Component c)
        {
            list.Add(c);
        }

        public override void Remove(Component c)
        {
            list.Remove(c);
        }

        public override Component GetChild(int i)
        {
            return list[i];
        }

        public override void Operation()
        {
            foreach (var obj in list)
            {
                obj.Operation();
            }
        }
    }
}

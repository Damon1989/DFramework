using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrototypeSample
{
    abstract class Prototype
    {
        public abstract Prototype Clone();
    }

    class ConcretePrototype:Prototype
    {
        public Member Member { get; set; }
        public string Attr { get; set; }
        public override Prototype Clone()
        {
            var prototype=new ConcretePrototype()
            {
                Member = new Member()
            };
            return prototype;
        }
    }

    public class Member
    {

    }

    public class ConcretePrototypeA
    {
        public Member Member { get; set; }
        public string Attr { get; set; }

        public ConcretePrototypeA Clone()
        {
            return (ConcretePrototypeA)this.MemberwiseClone();
        }
    }

    class ConcretePrototypeB : ICloneable
    {
        public Member Member { get; set; }

        public object Clone()
        {
            ConcretePrototypeB copy = (ConcretePrototypeB)this.MemberwiseClone();
            Member newMember = new Member();
            copy.Member = newMember;
            return copy;
        }
    }

    public class Demo
    {
        
    }
}

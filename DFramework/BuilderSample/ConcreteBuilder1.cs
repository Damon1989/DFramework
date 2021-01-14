namespace BuilderSample
{
    class ConcreteBuilder1 : Builder
    {
        public override void BuildPartA()
        {
            product.PartA = "A1";
        }

        public override void BuildPartB()
        {
            product.PartB = "B1";
        }

        public override void BuildPartC()
        {
            product.PartC = "C1";
        }
    }
}
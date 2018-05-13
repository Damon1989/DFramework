namespace DFramework.IoC
{
    public class Parameter
    {
        public Parameter(string parameterName, object parameterValue)
        {
            Name = parameterName;
            Value = parameterValue;
        }

        public string Name { get; set; }
        public object Value { get; set; }
    }
}
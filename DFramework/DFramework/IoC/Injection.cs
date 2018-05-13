namespace DFramework.IoC
{
    public interface Injection
    {
    }

    public class ConstructInjection : Injection
    {
        public ConstructInjection(params ParameterInjection[] parameters)
        {
            Parameters = parameters;
        }

        public ParameterInjection[] Parameters { get; set; }
    }

    public class ParameterInjection : Injection
    {
        public ParameterInjection(string propertyName)
        {
            ParameterName = propertyName;
        }

        public ParameterInjection(string propertyName, object propertyValue)
            : this(propertyName)
        {
            ParameterValue = propertyValue;
        }

        public string ParameterName { get; set; }
        public object ParameterValue { get; set; }
    }
}
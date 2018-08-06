namespace JohanPolosn.UnityInjector.Internals
{
    using System.Reflection;

    public class InjectorCache
    {
        public MethodInfo MethodInfo { get; private set; }
        public ParameterInfo[] Parameters { get; private set; }

        public InjectorCache(MethodInfo methodInfo , ParameterInfo[] parameters)
        {
            this.MethodInfo = methodInfo;
            this.Parameters = parameters;
        }
    }
}

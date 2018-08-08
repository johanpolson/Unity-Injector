namespace JohanPolosn.UnityInjector.Internals
{
    using System.Reflection;

    public class InjectorCache
    {
        private enum OpenalCache : byte { NotTested, Yes, No }

        private readonly OpenalCache[] openalParameters;

        public MethodInfo MethodInfo { get; private set; }

        public ParameterInfo[] Parameters { get; private set; }

        public InjectorCache(MethodInfo methodInfo, ParameterInfo[] parameters)
        {
            this.MethodInfo = methodInfo;
            this.Parameters = parameters;
            this.openalParameters = new OpenalCache[this.Parameters.Length];
        }

        public bool IsParameterOpenal(int index)
        {
            var cache = this.openalParameters[index];
            if (cache == OpenalCache.NotTested)
            {
                cache = this.openalParameters[index] =
                    this.Parameters[index].GetCustomAttributes(typeof(OpenalAttribute), false).Length != 0
                        ? OpenalCache.Yes : OpenalCache.No;
            }

            return cache == OpenalCache.Yes;
        }
    }
}

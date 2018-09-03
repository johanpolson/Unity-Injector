namespace JohanPolosn.UnityInjector.Internals
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Collections.Generic;

    [Serializable]
    public class DependencyMissingException : Exception
    {
        public DependencyMissingException(Type type, List<ParameterInfo> missingParameters)
            : base(GetMessage(type, missingParameters))
        {
        }

        public DependencyMissingException(string message)
            : base(message)
        {
        }

        private static string GetMessage(Type type, List<ParameterInfo> missingParameters)
        {
            return string.Format("type: ({0}), count {1}, {{{2}}}",
                type.FullName,
                missingParameters.Count,
                string.Join(", ", missingParameters.Select(p => p.ParameterType.FullName + " " + p.Name).ToArray())
                );
        }
    }
}
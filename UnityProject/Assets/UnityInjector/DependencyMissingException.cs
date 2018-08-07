namespace JohanPolosn.UnityInjector
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;

    [Serializable]
    public class DependencyMissingException : Exception
    {
        public DependencyMissingException(Type type, List<ParameterInfo> missingParameters)
            : base(GetMessage(type, missingParameters))
        {
        }

        private static string GetMessage(Type type, List<ParameterInfo> missingParameters)
        {
            var message = type.FullName + "\n";
            foreach (var parameter in missingParameters)
            {
                message += parameter.ParameterType.FullName + " " + parameter.Name + "\n";
            }

            return message;
        }
    }
}
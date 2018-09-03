namespace JohanPolosn.UnityInjector.Internals
{
    using System;

    [Serializable]
    public class InjectOnAwakeException : Exception
    {
        public InjectOnAwakeException(string message)
            : base(message)
        {

        }
    }
    
}
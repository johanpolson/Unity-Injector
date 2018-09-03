namespace JohanPolosn.UnityInjector.Internals
{
    using System;

    [Serializable]
    public class InjectOnFirstOnEnableException : Exception
    {
        public InjectOnFirstOnEnableException(string message)
            : base(message)
        {

        }
    }
    
}
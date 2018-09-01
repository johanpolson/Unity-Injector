namespace JohanPolosn.UnityInjector
{
    using System;

    [Serializable]
    public class InjectOnStartException : Exception
    {
        public InjectOnStartException(string message)
            :base(message)
        {

        }

    }
}
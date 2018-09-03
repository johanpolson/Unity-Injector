namespace JohanPolosn.UnityInjector
{
    using System;
    using JohanPolosn.UnityInjector.Internals;
    using UnityEngine;

    [AddComponentMenu("Unity Injector/Inject On First OnEnable")]
    public class InjectOnFirstOnEnable : OnEventInjector
    {
        public bool alreadyExecuted;
        private void OnEnable()
        {
            if (!alreadyExecuted)
            {
                this.alreadyExecuted = true;
                this.InjectComponents();
            }
        }

        protected override Exception GetException(string message)
        {
            return new InjectOnFirstOnEnableException(message);
        }
    }

}

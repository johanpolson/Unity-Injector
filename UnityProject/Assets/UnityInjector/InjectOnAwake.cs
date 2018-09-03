namespace JohanPolosn.UnityInjector
{
    using System;
    using JohanPolosn.UnityInjector.Internals;
    using UnityEngine;

    [AddComponentMenu("Unity Injector/Inject On Awake")]
    public class InjectOnAwake : OnEventInjector
    {
        private void Awake()
        {
            this.InjectComponents();
        }

        protected override Exception GetException(string message)
        {
            return new InjectOnAwakeException(message);
        }
    }

}

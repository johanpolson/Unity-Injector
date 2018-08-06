﻿namespace JohanPolosn.UnityInjector.Internals
{
    using System;

    public class Dependency
    {
        public object Singelton
        {
            get; private set;
        }

        public Func<IDependencyInjector, object> Creator
        {
            get; private set;
        }

        public bool IsSingelton
        {
            get
            {
                return this.Singelton != null;
            }
        }

        public Dependency(object singelton)
        {
            this.Singelton = singelton;
        }

        public Dependency(Func<IDependencyInjector, object> creator)
        {
            this.Creator = creator;
        }
    }
}

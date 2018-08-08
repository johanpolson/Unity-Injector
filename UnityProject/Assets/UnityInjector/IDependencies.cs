﻿namespace JohanPolosn.UnityInjector
{
    using JohanPolosn.UnityInjector.Internals;
    using System;
    using System.Collections.Generic;

    public interface IDependencies : IEnumerable<KeyValuePair<Type, Dependency>>
    {
        int Count { get; }

        void AddSingleton(object singleton);

        void AddSingleton<T>(T singleton);

        void AddCreator<T>(Func<IDependencyInjector, object> creator);

        void Remove(Type key);

        void Remove(object @object);

        bool TryGet(Type key, out Dependency obj);

        void Clear();
    }
}

namespace JohanPolosn.UnityInjector
{
    using System;
    using UnityEngine;

    public interface IDependencyInjector
    {
        GameObject GetGameObject(string key);

        T Get<T>();

        object Get(Type type);

        bool TryGet<T>(out T obj);

        bool TryGet(Type type, out object obj);

        object Inject(object target);

        object Inject(GameObject gameObject);

        object Inject(GameObject gameObject, bool includeInactive);
    }
}
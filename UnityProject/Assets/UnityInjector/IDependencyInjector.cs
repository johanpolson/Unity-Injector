namespace JohanPolosn.UnityInjector
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IDependencyInjector
    {
        GameObject GetGameObject(string key);

        T Get<T>();

        object Get(Type type);

        bool TryGet<T>(out T obj);

        bool TryGet(Type type, out object obj);

        object Inject(object target);

        object Inject(object target, IDictionary<Type, object> tempDependencys);

        object Inject(object target, IDictionary<Type, object> tempDependencys, IDictionary<string, GameObject> tempGameObjectDependencys);

        object Inject(GameObject gameObject);

        object Inject(GameObject gameObject, bool includeInactive);

        object Inject(GameObject gameObject, bool includeInactive, IDictionary<Type, object> tempDependencys);

        object Inject(GameObject gameObject, bool includeInactive, IDictionary<Type, object> tempDependencys, IDictionary<string, GameObject> tempGameObjectDependencys);
    }
}
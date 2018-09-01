namespace JohanPolosn.UnityInjector
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public interface IDependencyInjector
    {
        GameObject GetGameObject(string key);

        T Get<T>() where T : class;

        T Inject<T>(T target) where T : class;

        T Inject<T>(T target, IDictionary<Type, object> tempDependencys) where T : class;

        T Inject<T>(T target, IDictionary<Type, object> tempDependencys, IDictionary<string, GameObject> tempGameObjectDependencys) where T : class;

        object Inject(GameObject gameObject);

        object Inject(GameObject gameObject, bool includeInactive);

        object Inject(GameObject gameObject, bool includeInactive, IDictionary<Type, object> tempDependencys);

        object Inject(GameObject gameObject, bool includeInactive, IDictionary<Type, object> tempDependencys, IDictionary<string, GameObject> tempGameObjectDependencys);
    }
}
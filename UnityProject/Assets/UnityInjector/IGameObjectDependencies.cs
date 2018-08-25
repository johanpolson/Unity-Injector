namespace JohanPolosn.UnityInjector
{
    using System.Collections.Generic;
    using UnityEngine;

    public interface IGameObjectDependencies : IEnumerable<KeyValuePair<string, GameObject>>
    {
        int Count { get; }

        void Add(string key, GameObject gameObject);

        void Remove(GameObject gameObject);

        void Remove(string key);

        bool TryGet(string key, out GameObject gameObject);

        void Clear();
    }
}

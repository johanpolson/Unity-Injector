namespace JohanPolosn.UnityInjector.Internals
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class GameObjectDependencies : IGameObjectDependencies
    {
        private readonly Dictionary<string, GameObject> dependencys = new Dictionary<string, GameObject>();

        public int Count
        {
            get
            {
                return this.dependencys.Count;
            }
        }

        public void Add(string key, GameObject gameObject)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new Exception("key is null or empty");
            }

            if (gameObject == null)
            {
                throw new ArgumentNullException("gameObject");
            }


            this.dependencys[key] = gameObject;
        }

        public IEnumerator<KeyValuePair<string, GameObject>> GetEnumerator()
        {
            return this.dependencys.GetEnumerator();
        }

        public void Remove(GameObject gameObject)
        {
            var removeItems = this.dependencys
            .Where(x => ReferenceEquals(x.Value, gameObject))
            .ToArray();

            foreach (var item in removeItems)
            {
                this.dependencys.Remove(item.Key);
            }
        }

        public void Remove(string key)
        {
            this.dependencys.Remove(key);
        }

        public bool TryGet(string key, out GameObject gameObject)
        {
            return this.dependencys.TryGetValue(key, out gameObject);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.dependencys.GetEnumerator();
        }

        public void Clear()
        {
            this.dependencys.Clear();
        }
    }
}

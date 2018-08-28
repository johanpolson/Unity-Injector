namespace JohanPolosn.UnityInjector.Internals
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class Dependencies : IDependencies
    {
        private readonly Dictionary<Type, Dependency> dependencys = new Dictionary<Type, Dependency>();

        public int Count
        {
            get
            {
                return this.dependencys.Count;
            }
        }

        public void AddFactory<T>(Func<IDependencyInjector, object> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("creator");
            }

            this.dependencys[typeof(T)] = new Dependency(factory);
        }

        public void AddSingleton(object singleton)
        {
            if (singleton == null)
            {
                throw new ArgumentNullException("singleton");
            }

            this.dependencys[singleton.GetType()] = new Dependency(singleton);
        }

        public void AddSingleton<T>(T singleton)
        {
            if (singleton == null)
            {
                throw new ArgumentNullException("singleton");
            }

            this.dependencys[typeof(T)] = new Dependency(singleton);
        }

        public void Clear()
        {
            this.dependencys.Clear();
        }

        public IEnumerator<KeyValuePair<Type, Dependency>> GetEnumerator()
        {
            return this.dependencys.GetEnumerator();
        }

        public void Remove(Type key)
        {
            this.dependencys.Remove(key);
        }

        public void Remove(object @object)
        {
            var removeItems = this.dependencys
              .Where(x => x.Value.IsSingelton && x.Value.Singelton == @object)
              .ToArray();

            foreach (var item in removeItems)
            {
                this.dependencys.Remove(item.Key);
            }
        }

        public bool TryGet(Type key, out Dependency obj)
        {
            return this.dependencys.TryGetValue(key, out obj);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.dependencys.GetEnumerator();
        }
    }
}

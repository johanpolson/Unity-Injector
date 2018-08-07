namespace JohanPolosn.UnityInjector
{
    using System;
    using System.Reflection;
    using System.Collections.Generic;
    using UnityEngine;
    using JohanPolosn.UnityInjector.Internals;

    public class DependencyInjector : IDependencyInjector
    {
        private readonly Dictionary<Type, InjectorCache> injectorCache;

        public DependencyInjector()
        {
            this.injectorCache = new Dictionary<Type, InjectorCache>();
            this.Dependencys = new Dependencies(this);
            this.GameObjectDependencys = new GameObjectDependencies(this);

            this.Dependencys.AddSingleton<IDependencyInjector>(this);
            this.Dependencys.AddSingleton<IDependencies>(this.Dependencys);
            this.Dependencys.AddSingleton<IGameObjectDependencies>(this.GameObjectDependencys);
        }

        public Dependencies Dependencys { get; private set; }
        public GameObjectDependencies GameObjectDependencys { get; private set; }

        public object Inject(object target)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            this.InjectObject(target);

            return target;
        }

        public object Inject(GameObject gameObject)
        {
            return this.Inject(gameObject, true);
        }

        public object Inject(GameObject gameObject, bool includeInactive)
        {
            if (gameObject == null)
            {
                throw new ArgumentNullException("target");
            }

            this.InjectGameObject(gameObject, includeInactive);

            return gameObject;
        }

        public GameObject GetGameObject(string key)
        {
            GameObject gameObject;
            this.GameObjectDependencys.TryGet(key, out gameObject);
            return gameObject;
        }

        public T Get<T>()
        {
            return (T)this.Get(typeof(T));
        }

        public object Get(Type type)
        {
            Dependency dependency;
            if (this.Dependencys.TryGet(type, out dependency))
            {
                return dependency.IsSingelton ?
                    dependency.Singelton : dependency.Creator(this);
            }

            return null;
        }

        public bool TryGet<T>(out T obj)
        {
            Dependency dependency;
            if (this.Dependencys.TryGet(typeof(T), out dependency))
            {
                obj = (T)(dependency.IsSingelton ?
                    dependency.Singelton : dependency.Creator(this));
                return true;
            }

            obj = default(T);
            return false;
        }

        public bool TryGet(Type type, out object obj)
        {
            Dependency dependency;
            if (this.Dependencys.TryGet(type, out dependency))
            {
                obj = dependency.IsSingelton ?
                    dependency.Singelton : dependency.Creator(this);
                return true;
            }

            obj = null;
            return false;
        }

        private void InjectGameObject(GameObject gameObject, bool includeInactive)
        {
            var components = gameObject.GetComponentsInChildren<Component>(includeInactive);
            foreach (var component in components)
            {
                if (component != null)
                {
                    this.InjectObject(component);
                }
            }
        }

        private void InjectObject(object targe)
        {
            InjectorCache cache;
            var type = targe.GetType();

            if (!injectorCache.TryGetValue(type, out cache))
            {
                var methodInfo = type.GetMethod("Construct", BindingFlags.Public | BindingFlags.Instance);
                ParameterInfo[] parameters = null;
                if (methodInfo != null && (parameters = methodInfo.GetParameters()).Length != 0)
                {
                    cache = new InjectorCache(methodInfo, parameters);
                }

                injectorCache.Add(type, cache);
            }

            if (cache == null)
            {
                return;
            }

            var args = new object[cache.Parameters.Length];
            var missingParameters = new List<ParameterInfo>(cache.Parameters.Length);

            for (int i = 0; i < args.Length; i++)
            {
                var parameter = cache.Parameters[i];

                args[i] = parameter.ParameterType == typeof(GameObject) ?
                    this.GetGameObject(parameter.Name) :
                    this.Get(parameter.ParameterType);

                if (args[i] == null || !cache.IsParameterOpenal(i))
                {
                    missingParameters.Add(parameter);
                }
            }

            if (missingParameters.Count != 0)
            {
                throw new DependencyMissingException(type, missingParameters);
            }

            cache.MethodInfo.Invoke(targe, args);
        }
    }
}

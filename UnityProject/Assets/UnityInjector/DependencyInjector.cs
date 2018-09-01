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
            this.Dependencys = new Dependencies();
            this.GameObjectDependencys = new GameObjectDependencies();

            this.Dependencys.AddSingleton<IDependencyInjector>(this);
            this.Dependencys.AddSingleton<IDependencies>(this.Dependencys);
            this.Dependencys.AddSingleton<IGameObjectDependencies>(this.GameObjectDependencys);
        }

        public Dependencies Dependencys { get; private set; }
        public GameObjectDependencies GameObjectDependencys { get; private set; }

        public T Inject<T>(T target) where T : class
        {
            return this.Inject(target, null, null);
        }

        public T Inject<T>(T target, IDictionary<Type, object> tempDependencys) where T : class
        {
            return this.Inject(target, tempDependencys, null);
        }

        public T Inject<T>(T target, IDictionary<Type, object> tempDependencys, IDictionary<string, GameObject> tempGameObjectDependencys) where T : class
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            this.InjectObject(target, tempDependencys, tempGameObjectDependencys);

            return target;
        }

        public object Inject(GameObject gameObject)
        {
            return this.Inject(gameObject, true);
        }

        public object Inject(GameObject gameObject, bool includeInactive)
        {
            return this.Inject(gameObject, includeInactive, null, null);
        }

        public object Inject(GameObject gameObject, bool includeInactive, IDictionary<Type, object> tempDependencys)
        {
            return this.Inject(gameObject, includeInactive, tempDependencys, null);
        }

        public object Inject(GameObject gameObject, bool includeInactive, IDictionary<Type, object> tempDependencys, IDictionary<string, GameObject> tempGameObjectDependencys)
        {
            if (gameObject == null)
            {
                throw new ArgumentNullException("target");
            }

            this.InjectGameObject(gameObject, includeInactive, tempDependencys, tempGameObjectDependencys);

            return gameObject;
        }

        public GameObject GetGameObject(string key)
        {
            GameObject gameObject;
            this.GameObjectDependencys.TryGet(key, out gameObject);
            return gameObject;
        }

        public T Get<T>() where T : class
        {
            Dependency dependency;
            if (this.Dependencys.TryGet(typeof(T), out dependency))
            {
                return (T)(dependency.IsSingelton ?
                    dependency.Singelton : dependency.Factory(this));
            }

            return null;
        }

        private object Get(Type type)
        {
            Dependency dependency;
            if (this.Dependencys.TryGet(type, out dependency))
            {
                return dependency.IsSingelton ?
                    dependency.Singelton : dependency.Factory(this);
            }

            return null;
        }


        private void InjectGameObject(GameObject gameObject, bool includeInactive, IDictionary<Type, object> tempDependencys, IDictionary<string, GameObject> tempGameObjectDependencys)
        {
            var components = gameObject.GetComponentsInChildren<Component>(includeInactive);
            foreach (var component in components)
            {
                if (component != null)
                {
                    this.InjectObject(component, tempDependencys, tempGameObjectDependencys);
                }
            }
        }

        private void InjectObject(object targe, IDictionary<Type, object> tempDependencys, IDictionary<string, GameObject> tempGameObjectDependencys)
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

                var isGameObject = parameter.ParameterType == typeof(GameObject);

                args[i] = isGameObject ?
                    this.GetGameObject(parameter.Name) :
                    this.Get(parameter.ParameterType);

                if (args[i] != null)
                {
                    continue;
                }

                if (isGameObject && tempGameObjectDependencys != null)
                {
                    GameObject temp = null;
                    if (tempGameObjectDependencys.TryGetValue(parameter.Name, out temp))
                    {
                        args[i] = temp;
                        continue;
                    }
                }
                else if (tempDependencys != null)
                {
                    object temp = null;
                    if (tempDependencys.TryGetValue(parameter.ParameterType, out temp))
                    {
                        args[i] = temp;
                        continue;
                    }
                }

                if (!cache.IsParameterOpenal(i))
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

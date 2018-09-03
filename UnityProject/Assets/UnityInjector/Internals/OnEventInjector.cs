namespace JohanPolosn.UnityInjector.Internals
{
    using System;
    using UnityEngine;

    public abstract class OnEventInjector : MonoBehaviour
    {
        [SerializeField]
        protected Component[] components;

        [SerializeField]
        protected bool includeInactive;

        protected abstract Exception GetException(string message);

        protected void InjectComponents()
        {
            if (this.components.Length == 0)
            {
                try
                {
                    GlobalInjector.singleton.Inject(this.gameObject, this.includeInactive);
                }
                catch (DependencyMissingException ex)
                {
                    var message = this.GetType().Name + ": (" + this.gameObject.name + "), " + ex.Message;
                    throw this.GetException(message);
                }
            }
            else
            {
                for (int i = 0; i < this.components.Length; i++)
                {
                    this.InjectComponent(i);
                }
            }
        }

        private void InjectComponent(int i)
        {
            var component = this.components[i];

            if (component == null)
            {
                var message = string.Format("Null in components array on index: {0}, gameObject: ({1})", i, this.gameObject.name);
                throw this.GetException(message);
            }

            try
            {
                GlobalInjector.singleton.Inject(component);
            }
            catch (DependencyMissingException ex)
            {
                var message = string.Format("Dependency Missing Exception in component on index: {0}, gameObject: ({1}), component gameObject: ({2}), DependencyMissingException: {{{3}}}",
                    i,
                    this.gameObject.name,
                    component.gameObject.name,
                    ex.Message);

                throw this.GetException(message);
            }
        }
    }
}

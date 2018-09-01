namespace JohanPolosn.UnityInjector
{
    using UnityEngine;

    [AddComponentMenu("Unity Injector/Inject On Start")]
    public class InjectOnStart : MonoBehaviour
    {
        public Component[] components;
        public bool includeInactive;

        private void Start()
        {
            if (this.components.Length == 0)
            {
                try
                {
                    GlobalInjector.singleton.Inject(this.gameObject, includeInactive);
                }
                catch (DependencyMissingException ex)
                {
                    var message = "InjectOnStart: (" + this.gameObject.name + "), " + ex.Message;
                    throw new InjectOnStartException(message);
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
                throw new InjectOnStartException(message);
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

                throw new InjectOnStartException(message);
            }
        }
    }
}
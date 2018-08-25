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
                GlobalInjector.singleton.Inject(this.gameObject, includeInactive);
            }
            else
            {
                for (int i = 0; i < this.components.Length; i++)
                {
                    GlobalInjector.singleton.Inject(this.components[i]);
                }
            }
        }
    }
}
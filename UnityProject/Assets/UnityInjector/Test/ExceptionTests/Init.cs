namespace JohanPolosn.UnityInjector.Test.ExceptionTests
{
    using JohanPolosn.UnityInjector;
    using UnityEngine;

    public class Init : MonoBehaviour
    {

        private void Awake()
        {
            GlobalInjector.singleton = new DependencyInjector();
        }
    }
}
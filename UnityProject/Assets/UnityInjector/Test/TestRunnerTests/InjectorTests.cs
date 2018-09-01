namespace JohanPolosn.UnityInjector.Test.TestRunnerTests
{
    using UnityEngine;
    class InjectorTests
    {
        public class TestClass { }

        public class Compnent : MonoBehaviour
        {
            public GameObject test;
            public TestClass testClass;

            public void Construct(GameObject test, [Openal] TestClass testClass)
            {
                this.test = test;
                this.testClass = testClass;
            }
        }
        
        private readonly DependencyInjector injector = new DependencyInjector();

        private readonly GameObject gameObject = new GameObject("Test");

        private readonly Compnent compnent = new Compnent();

        private readonly TestClass testClass = new TestClass();

        private void GetGameObject()
        {
            injector.GameObjectDependencys.Add("test", gameObject);

            TestRunner.ReferenceEquals(gameObject, injector.GetGameObject("test"));
        }

        private void InjectGameObject()
        {
            injector.GameObjectDependencys.Add("test", gameObject);

            injector.Inject(compnent);

            TestRunner.ReferenceEquals(gameObject, compnent.test);
            TestRunner.ReferenceEquals(null, compnent.testClass);
        }

        private void InjectTestClass1()
        {
            injector.Dependencys.AddSingleton(testClass);

            injector.GameObjectDependencys.Add("test", gameObject);

            injector.Inject(compnent);

            TestRunner.ReferenceEquals(gameObject, compnent.test);
            TestRunner.ReferenceEquals(testClass, compnent.testClass);
        }
    }
}

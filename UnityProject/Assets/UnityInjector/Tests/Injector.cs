namespace JohanPolosn.UnityInjector.Tests
{
    using UnityEngine;
    class Injector
    {
        public class Test1Compnent : MonoBehaviour
        {
            public bool constructRun;
            public GameObject test;
            public TestClass1 testClass1;

            public void Construct(GameObject test,[Openal] TestClass1 testClass1)
            {
                this.constructRun = true;
                this.test = test;
                this.testClass1 = testClass1;
            }
        }

        public class TestClass1
        {

        }

        private DependencyInjector di = new DependencyInjector();

        private void GetGameObject()
        {
            var gameObjectTest = new GameObject("Test");      
            di.GameObjectDependencys.Add("test", gameObjectTest);

            TestRunner.ReferenceEquals(gameObjectTest, di.GetGameObject("test"));
        }

        private void InjectGameObject()
        {
            var gameObjectTest = new GameObject("Test");
            di.GameObjectDependencys.Add("test", gameObjectTest);

            var dependency1 = new Test1Compnent();

            di.Inject(dependency1);

            TestRunner.ReferenceEquals(gameObjectTest, dependency1.test);
        }

        private void InjectTestClass1()
        {
            var testClass1 = new TestClass1();
            di.Dependencys.AddSingleton(testClass1);

            di.GameObjectDependencys.Add("test", new GameObject("Test"));

            var dependency1 = new Test1Compnent();

            di.Inject(dependency1);
            
            TestRunner.ReferenceEquals(testClass1, dependency1.testClass1);
        }
    }
}
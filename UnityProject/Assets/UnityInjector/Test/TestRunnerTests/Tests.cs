namespace JohanPolosn.UnityInjector.Test.TestRunnerTests
{
    using UnityEngine;

    public class Tests
    {
        private DependencyInjector di = new DependencyInjector();

        public class Test1Compnent : MonoBehaviour
        {
            public bool constructRun;

            public void Construct(IDependencyInjector injector)
            {
                this.constructRun = true;
            }
        }

        private void AddSingletonAndGet()
        {
            var dependency = new Test1Compnent();

            di.Dependencys.AddSingleton<MonoBehaviour>(dependency);
            di.Dependencys.AddSingleton(dependency);

            TestRunner.ReferenceEquals(dependency, di.Get<MonoBehaviour>());
            TestRunner.ReferenceEquals(dependency, di.Get<Test1Compnent>());
        }

        private void AddGameObject()
        {
            var newGameObj = new GameObject("GameObject");

            di.GameObjectDependencys.Add("test", newGameObj);

            TestRunner.ReferenceEquals(newGameObj, di.GetGameObject("test"));
        }

        private void DestroyGameObject()
        {
            var newGameObj = new GameObject("GameObject");

            di.GameObjectDependencys.Add("test", newGameObj);

            GameObject.Destroy(newGameObj);

            TestRunner.ReferenceEquals(newGameObj, di.GetGameObject("test"));

            TestRunner.Equals(1, this.di.GameObjectDependencys.Count);

            di.GameObjectDependencys.Remove(newGameObj);

            TestRunner.Equals(0, this.di.GameObjectDependencys.Count);
        }

        private void ConstructOnMonoBehaviourViaGameObject()
        {
            var newGameObj = new GameObject("GameObject");
            var comp = newGameObj.AddComponent<Test1Compnent>();

            di.GameObjectDependencys.Add("test", newGameObj);
            di.Inject(newGameObj);

            TestRunner.Equals(true, comp.constructRun);
        }

        private void ConstructOnGameObjectChildren()
        {
            var newGameObj1 = new GameObject("GameObject1");
            var newGameObj2 = new GameObject("GameObject2");

            newGameObj2.transform.parent = newGameObj1.transform;

            var comp = newGameObj2.AddComponent<Test1Compnent>();
            
            di.GameObjectDependencys.Add("test", newGameObj1);
            di.Inject(newGameObj1);

            TestRunner.Equals(true, comp.constructRun);
        }

        private void ConstructOnGameObjectNotChildren()
        {
            var newGameObj1 = new GameObject("GameObject1");
            var newGameObj2 = new GameObject("GameObject2");

            newGameObj2.transform.parent = newGameObj1.transform;
            newGameObj2.gameObject.SetActive(false);

            var comp = newGameObj2.AddComponent<Test1Compnent>();

            di.GameObjectDependencys.Add("test", newGameObj1);

            TestRunner.Equals(false, comp.constructRun);
        }

    }
}

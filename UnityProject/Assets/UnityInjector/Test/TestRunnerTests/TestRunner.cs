namespace JohanPolosn.UnityInjector.Test.TestRunnerTests
{
    using System;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;
    using System.Reflection;

    public class TestRunner : MonoBehaviour
    {

        public Text uiText;

        private void Start()
        {
            this.uiText.text = "";

            Debug.ClearDeveloperConsole();

            this.Log("TestRunner Start");

            var camera = GameObject.FindObjectOfType<Camera>();

            var type = this.GetType();
            var myNamespace = type.Namespace;

            var testTypes = type.Assembly
                .GetTypes()
                .Where(x => x != type && !x.IsNested && x.Namespace.StartsWith(myNamespace));

            foreach (var item in testTypes)
            {
                this.RunTestOnType(item, camera);
            }

            this.Log("--- Exception Tests");

            Application.logMessageReceivedThreaded += Application_logMessageReceived;
            SceneManager.LoadScene("ExceptionTests", LoadSceneMode.Additive);

        }

        private void Application_logMessageReceived(string condition, string stackTrace, LogType type)
        {
            if (type == LogType.Exception)
            {
                Log(condition + "\n");
            }
        }

        private void RunTestOnType(Type type, Camera camera)
        {
            var methods = type
               .GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
               .Where(x => x.DeclaringType == type)
               .ToArray();

            var className = PascalCasingToNormal(type.Name);

            this.Log("--- Runing Test On " + className+ " (" + methods.Length + ")");
            Debug.Log("--- Runing Test On " + className + " (" + methods.Length + ")");

            foreach (var method in methods)
            {
                Debug.Log("--- Runing " + method.Name );

                var methodName = PascalCasingToNormal(method.Name);
                var testScene = SceneManager.CreateScene(methodName);
                SceneManager.SetActiveScene(testScene);
                try
                {
                    var target = System.Activator.CreateInstance(type);
                    method.Invoke(target, null);
                    Log("Ok -" + methodName + "-");
                }
                catch (Exception ex)
                {
                    camera.backgroundColor = Color.red;
                    Debug.LogError(methodName + "- : " + ex.InnerException);
                    Log("Error -" + methodName + "- : " + ex.InnerException);
                }
            }

        }

        private void Log(string text)
        {
            this.uiText.text += text + "\n";
        }

        public static void ReferenceEquals(object expected, object actual, string message = null)
        {
            if (!object.ReferenceEquals(expected, actual))
            {
                throw GetException("ReferenceEquals", expected, actual, message);
            }
        }

        public static void Equals(object expected, object actual, string message = null)
        {
            if (!object.Equals(expected, actual))
            {
                throw GetException("Equals", expected, actual, message);
            }
        }

        public static string PascalCasingToNormal(string text)
        {
            var ret = "";
            for (int i = 0; i < text.Length; i++)
            {
                if (i == 0)
                {
                    ret += char.ToUpper(text[i]);
                }
                else if (char.IsUpper(text[i]))
                {
                    ret += " " + text[i];
                }
                else
                {
                    ret += text[i];
                }
            }
            return ret;
        }

        private static Exception GetException(string type, object expected, object actual, string message)
        {
            var ExceptionMessage = type + ": ";

            if (expected == null)
            {
                expected = "NULL";
            }

            ExceptionMessage += "expected \"" + expected + "\" != ";

            if (actual == null)
            {
                actual = "NULL";
            }

            ExceptionMessage += "actual \"" + actual + "\"";

            if (message == null)
            {
                ExceptionMessage += " : " + message;
            }

            return new Exception(ExceptionMessage);
        }
    }
}
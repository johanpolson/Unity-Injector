## Dependency Injection library for Unity3D

A simple but powerful full dependency Injection library for Unity3D

### <a id="features"></a>Features
* Injection
    * Supports for normal C# classes , GameObject and MonoBehaviours
    * Singleton and factory
* Optional dependencies

### <a id="setUp"></a>How to set up
```csharp
using UnityEngine;
using JohanPolosn.UnityInjector;

static class AppInit
{
    [RuntimeInitializeOnLoadMethod]
    static private void Init()
    {
        var injector = new DependencyInjector(); // the new Global Injector
        var dependencys = injector.Dependencys;
        
        // in order to use InjectOnStart , see "How to injet a MonoBehaviour in a scene or in a prefab"
        GlobalInjector.singleton = injector;

        dependencys.AddSingleton(new ApiService());

        dependencys.AddSingleton(injector.Inject(new GameService()));
    }
}

public class ApiService
{
}

public class GameService
{
    private ApiService api;
    public void Construct(ApiService api)
    {
        this.api = api;
    }
}
```
### How to injet a MonoBehaviour in a scene or in a prefab
Example MonoBehaviour to inject.
```csharp
public class Player : MonoBehaviour
{
    private ApiService api;
    public void Construct(ApiService api)
    {
        this.api = api;
    }
}
```
* Add "Player" to a GameObject in scene or in a prefab.
* Add a "InjectOnStart" to the same GameObject , ComponentMenu -> "Unity Injector/Inject On Start"

And that's it ! Construct on Player will be called at the same time as Start

# UnityUtilities

Just some various utility / classes that I have created/found whilst developing that could be used in any project.

## Usage
Go into the package.json and add `"com.js.unityutility": "https://github.com/JamesStudd/UnityUtilities.git",`

## Scene Loader

### Usage
Go to `HomidaUtility` in the top bar, click `SceneLoader`

### Description
Gives a window available in the Editor to load single/multiple scenes and the option to start them in playmode or not. Alleviates the problem of having to find each scene in the project, as well as loading multiple scenes in additive and generally making some development quicker.
![Example of Scene Loader Window](https://i.imgur.com/5DeFe4G.png)

---

## Async Error Handler

### Usage
If you find yourself wanting to call an `async Task` function from a non-async function, call the function normally, and attach `.FireAndForgetSafeAsync()` onto the end. Pass in an AsyncErrorHandler object created in the Start/Awake.

### Description
From a great article on using [Async in Unity](http://www.stevevermeulen.com/index.php/2017/09/using-async-await-in-unity3d-2017/), error handling in Unity using Async can have unforeseen circumstances, creating an `async Task` method means that any error returned will be captured by the Task object. Therefore, not using `await` when calling this method will mean the error is lost in the aether. However, sometimes one may want to call an async method but not really care about the result. You may ask, why not use `async void`? Well there are [numerous](https://haacked.com/archive/2014/11/11/async-void-methods/) reasons against such a thing. So, the other solution is to use a handy method called FireAndForgetSafeAsync.

The wrong way to do it: (The error is lost and never displayed in the console)
```cs
private void CalledFromButton()
{
  DoThing();
}

private async Task DoThing()
{
  throw new Exception();
}
```

The right way to do it: (The error is returned and dealt with)
```cs
private void CalledFromButton()
{
  DoThing().FireAndForgetSafeAsync(_asyncErrorHandler);
}

private async Task DoThing()
{
  throw new Exception();
}
```

---

## Async Terminator

### Usage
Drag the AsyncTerminator prefab into any scene, the OnApplicationQuit of this GameObject will handle cleaning up Async methods. This object does not persist through scenes but could do by just making it a singleton.

### Description
Async code continues to run to completion even if Unity stops playing. This leads to some annoying patterns, such as having to check `if (Application.isPlaying)`, otherwise some code may have unintended side-effects, for example:

```cs
private void CalledFromButton()
{
  DoThing().FireAndForgetSafeAsync(_asyncErrorHandler);
}

private async Task DoThing()
{
  await Task.Delay(3000);
  GameObject.CreatePrimitive(PrimitiveType.Sphere);
}
```
Running this project, then stopping the project before 3 seconds is up, will mean the `GameObject.CreatePrimitive(PrimitiveType.Sphere)` is still ran, and a gameobject will be created after the project has stopped. One solution is to check if the application is still playing right after waiting 3 seconds (or in a more real circumstance, making a web request / reading a file etc), but this can become annoyng. Another solution is the **ASYNC TERMINATOR/ANNIHILATOR/DESTROYER/(any cool sounding word)**. In all honesty, I'm not sure exactly how this works, I grabbed it from a [Unity Forum post](https://forum.unity.com/threads/non-stopping-async-method-after-in-editor-game-is-stopped.558283/), from what I understand the *SynchronizationContext* is the location that async code returns too after completion, and by setting a new *SynchronizationContext* OnApplicationQuit, the async code stops.

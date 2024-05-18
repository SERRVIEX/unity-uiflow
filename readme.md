## Requirements
[![Unity 2023.2.0f1+](https://img.shields.io/badge/unity-2023.2.0f1+-black.svg?style=flat&logo=unity&cacheSeconds=2592000)](https://unity3d.com/get-unity/download/archive)

![.NET 4.x Scripting Runtime](https://img.shields.io/badge/.NET-4.x-blueviolet.svg?style=flat&cacheSeconds=2592000)

## Description
UIFlow simplifies the process of creating navigation controllers between screens in Unity projects. With UIFlow, users can easily set up efficient navigation reducing development time and complexity. This asset provides a user-friendly interface for managing navigation flows, allowing developers to focus on creating engaging user experiences without getting bogged down in technical details.

## Integration

### StoryboardEnvironment

Flow system is controlled by one manager and everything is created under the object ```StoryboardEnvironnment```.

To create this object simple call ```GameObject/UI/Storyboard```. This will generate all the necessary components to enable UIFlow functionality.

It is advisable to make ```StoryboardEnvironment``` a singleton. This ensures that the environment persists across all scenes, eliminating the need to create a new ```StoryboardEnvironment``` for each scene.

### Storyboard
```Storyboard``` is the manager within the ```StoryboardEnvironment``` responsible for controlling screens.

#### Layers
It contains layers where a new ```ViewController``` can be presented. This functionality is particularly useful when a menu needs to be displayed on top of other view controllers. Here are the different layers:

-  ```Under``` - The lowest layer, suitable for elements like Play Controller UI or screens without backgrounds.

-  ```Base``` - The default layer, commonly used for menus.

-  ```Extra``` - An additional higher layer above the base, often used for additional menu elements.

-  ```Context``` - Reserved for context menus positioned above screens, such as dropdowns.

-  ```Alert``` - An important layer designed to block all interactions except for alerts.

-  ```Over``` - A layer that blocks everything, including alerts, typically used for loading screens covering the entire display area.

> These comments are only advices how to make UI better.

### View Controllers
For each screen/menu a new view controller is required. Creating a view controller can be done in next way:

```csharp
using  UIFlow;
public  class  MenuViewController : ViewController {}
```

They are the same like ```MonoBehaviour``` but with additional properties and methods. Here you can add what you want:

```csharp
using  UIFlow;

public  class  MenuViewController : ViewController
{
    [SerializedField] private  string  _name;
    public  int  Coins;
}

```

How this menu can be presented? Simple, you just need to have a reference to it or just present it by the its type.

- Here is an example with a reference:
```csharp
public  class  MyScript: MonoBehaviour
{
    [SerializeField] private  MenuViewController  _menuViewController;
    public  void  ShowMenu()
    {
        // Here we have menu instance 'controller ' and we can do what we want.
        MenuViewController  controller = Storyboard.Present(_menuViewController);
    }
}
```

- Here is an example by the type:
```csharp
public  class  MyScript: MonoBehaviour
{
    public  void  ShowMenu()
    {
        // Here we have menu instance 'controller ' and we can do what we want.
        MenuViewController  controller = Storyboard.Present<MenuViewController>();
    }
}
```

How this can be dismissed? Of course only instances should be dismissed, not prefabs.

```csharp
public  class  MyScript: MonoBehaviour
{
    [SerializeField] private  MenuViewController  _menuViewController;
    public  void  ShowAndDismissMenu()
    {
        MenuViewController  controller = Storyboard.Present(_menuViewController);
        controller.Dismiss();
    }
}
```
#### View Controller Lifecycle

View controllers possess their own life cycle.

```csharp
// Called before animation on Present.
public  virtual  void  OnWillAppear() { }
// Called after animation completes on Present.
public  virtual  void  OnDidAppear() { }

// Called before animation on Dismiss.
public  virtual  void  OnDidDisappear() { }
// Called after animation completes on Dismiss.
public  virtual  void  OnWillDisappear() { }

// Called on present.
public  virtual  void  OnAppearTransition() { }
// Called on dismiss.
public  virtual  void  OnDisappearTransition() { }
```

#### View Controller Transitions
Transitions can be made manually for each view controller on present and on dismiss. Also it is available to create a transition class and use it in every view controller. Use ```DOTween``` to animate for easy.

## Examples

Inside the project are examples that shows how to use the ```UIFlow```.
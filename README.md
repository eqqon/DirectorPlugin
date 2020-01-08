# How to write a plugin for Director with .NET

## Start a new .NET Framework project like this:
![Create a new project in VS](DirectorPlugin/Images/CreateNewProject.JPG)

Note: make sure it is a **WPF User Control Library** and the Framework is **.NET Framework 4.5.2**

## Download [Eqqon.Director.Plugin.dll](https://github.com/eqqon/DirectorPlugin/blob/master/Releases/1.0.0.0/Eqqon.Director.Plugin.dll) and reference it in your project.

## Create a Plugin Controller

Create a class that derives from `Eqqon.Director.Plugin.PluginController`. The name of the class as well as the namespace it resides in is completely up to you:

```C#
namespace TestPlugin
{
    public class TestPlugin : Eqqon.Director.Plugin.PluginController
    {

    }
}
```

Now you are ready to add your own functionality by overriding any virtual method of the base class. The typical methods to override are:

| Method | Effekt |
| ------ | ------ |
| DisposeHook() | Free resources here. |
| GetCommands() | List of command names which will let other components invoke functions in the plugin via **EventBinding** |
| GetEvents() | List of events names which the plugin can use to interact with other components via **EventBinding** |
| GetPropertiesHook() | List of config settings or runtime values to show up in the property dialog of the plugin or can be bound against  other components with **Binding** |
| GetViewHook() | Create and return a WPF user element that will serve as the user interface of the plugin. Note that Director may call this more than once if the plugin is displayed in more than one location and expects a new instance every time. You **must not** return return the same view multiple times. |
| Initialize0() | Set up your plugin's internals here and allocate resources |
| Initialize1() | Set up everything that depends on anything in Director, will be called after `Initialize0()` |
| InvokeCommand(string name, Dictionary<string, object> data) | Implement this to react to commands called by other Director components via EventBinding |
| OnPropertyValueChanged(PluginProperty p, object old_value, object new_value) | **Overload** this to react to property changes. Override only if base behavior is not sufficient |
| UpdateView(FrameworkElement view) | Override to implement the plugin GUI's reaction to changes in the properties |

Here is an example of how the overridden GetPropertiesHook() method could look like:


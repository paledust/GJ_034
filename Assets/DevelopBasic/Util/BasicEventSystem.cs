using System;

//A basic C# Event System
public static class EventHandler
{
#region Basic Game
    public static event Action E_BeforeUnloadScene;
    public static void Call_BeforeUnloadScene(){E_BeforeUnloadScene?.Invoke();}
    public static event Action E_AfterLoadScene;
    public static void Call_AfterLoadScene(){E_AfterLoadScene?.Invoke();}
    public static event Action E_OnBeginSave;
    public static void Call_OnBeginSave()=>E_OnBeginSave?.Invoke();
    public static event Action E_OnCompleteSave;
    public static void Call_OnCompleteSave()=>E_OnCompleteSave?.Invoke();
#endregion

#region Interaction
    public static event Action<Target> E_OnHitCircle;
    public static void Call_OnHitCircle(Target circle)=>E_OnHitCircle?.Invoke(circle);
    public static event Action<Window> E_OnEnterWindow;
    public static void Call_OnEnterWindow(Window window) => E_OnEnterWindow?.Invoke(window);
#endregion
}
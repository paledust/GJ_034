using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private Color[] windowColorSheet;
    [SerializeField] private List<Window> windowPool;

    private int windowIndex;
    private Window activeWindow;

    void OnEnable()
    {
        EventHandler.E_OnHitCircle += OnHitCircleHandler;
        EventHandler.E_OnEnterWindow += OnEnterWindowHandler;
    }
    void OnDisable()
    {
        EventHandler.E_OnHitCircle -= OnHitCircleHandler;
        EventHandler.E_OnEnterWindow -= OnEnterWindowHandler;
    }
    void OnHitCircleHandler(Target circle)
    {

    }
    void OnEnterWindowHandler(Window window)
    {
        activeWindow = window;
    }
    GameObject CreateWindow()
    {
        return null;
    }
    void RecycleWindow()
    {

    }
}
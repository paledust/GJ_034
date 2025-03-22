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
        EventHandler.E_OnCheckTarget += OnCheckTarget;
    }
    void OnDisable()
    {
        EventHandler.E_OnHitCircle -= OnHitCircleHandler;
        EventHandler.E_OnEnterWindow -= OnEnterWindowHandler;
        EventHandler.E_OnCheckTarget -= OnCheckTarget;
    }
    void Start()
    {
        windowIndex = windowPool.Count;
    }
    void OnCheckTarget()
    {
        if(activeWindow!=null)
        {
            if(activeWindow.CheckTarget())
            {

            }
        }
    }
    void OnHitCircleHandler(Target circle)
    {
        
    }
    void OnEnterWindowHandler(Window window)
    {
        if(activeWindow==null)
        {
            activeWindow = window;
            activeWindow.ActivateWindow();
            PlayerManager.Instance.MatchWindowBoundry(activeWindow);
        }
        foreach(var go in windowPool)
        {
            go.DisableHitbox();
        }
    }
    void OnExitWindowHandler()
    {

    }
    GameObject CreateWindow()
    {
        return null;
    }
    void RecycleWindow()
    {

    }
}
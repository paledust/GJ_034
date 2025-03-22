using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Color[] windowColorSheet;
    [SerializeField] private List<Window> windowPool;
    [SerializeField] private Marker[] winMarker;
    [SerializeField] private ParticleSystem p_explode;

    private int windowIndex;
    private Window activeWindow;

    void OnEnable()
    {
        EventHandler.E_OnEnterWindow += OnEnterWindowHandler;
        EventHandler.E_OnCheckTarget += OnCheckTarget;
        EventHandler.E_OnWindowExplode += OnWindowExplodeHandler;
    }
    void OnDisable()
    {
        EventHandler.E_OnEnterWindow -= OnEnterWindowHandler;
        EventHandler.E_OnCheckTarget -= OnCheckTarget;
        EventHandler.E_OnWindowExplode -= OnWindowExplodeHandler;
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
                activeWindow.DefeatWindow();
                foreach(var go in windowPool)
                {
                    if(go!=activeWindow)
                        go.EnableHitbox();
                }
                activeWindow = null;

                PlayerManager.Instance.DefeatTarget();
            }
        }
    }
    void OnWindowExplodeHandler(Window window)
    {
        cameraManager.ShakeScreen(0.2f, 5);

        foreach(var go in windowPool)
        {
            if(go!=activeWindow)
                go.EnableHitbox();
        }
        activeWindow = null;

        p_explode.transform.position = window.transform.position;
        var shapeModule = p_explode.shape;
        shapeModule.scale = window.transform.localScale;
        p_explode.Play();

        PlayerManager.Instance.ResetPlayer();
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
    GameObject CreateWindow()
    {
        return null;
    }
    void RecycleWindow()
    {

    }
}
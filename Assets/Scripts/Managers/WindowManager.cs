using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private Color[] windowColorSheet;
    [SerializeField] private List<Window> windowPool;
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
                Debug.Log("Has Target");
            }
        }
    }
    void OnWindowExplodeHandler(Window window)
    {
        window.gameObject.SetActive(false);

        PlayerManager.Instance.ResetBoundry();
        PlayerManager.Instance.currentPlayer.ActiveRender();
        cameraManager.ShakeScreen(0.2f, 5);

        p_explode.transform.position = window.transform.position;
        var shapeModule = p_explode.shape;
        shapeModule.scale = window.transform.localScale;
        p_explode.Play();
    //暂停玩家移动，稍后恢复

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
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public PlayerController currentPlayer{get; private set;}
    public Vector4 currentBounds;

    protected override void Awake(){
        base.Awake();
        
        FindPlayer();
        currentBounds = new Vector4(40, -40, 30, -30);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventHandler.E_AfterLoadScene += FindPlayer;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventHandler.E_AfterLoadScene -= FindPlayer;
    }
    public void MatchWindowBoundry(Window window)
    {
        currentBounds = window.GetBoundry();
        currentPlayer.DeactiveRender();
    }
    public void ResetBoundry()
    {
        currentBounds = new Vector4(40, -40, 30, -30);
    }
    void FindPlayer(){
        currentPlayer = FindFirstObjectByType<PlayerController>();
    }
}
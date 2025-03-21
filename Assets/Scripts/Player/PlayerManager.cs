using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    private bool IsInTransition;
    private PlayerController currentPlayer;
    
    public bool m_canControl => !IsInTransition;

    protected override void Awake(){
        base.Awake();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventHandler.E_AfterLoadScene += FindPlayer;
    }
    void Start(){
        FindPlayer();
    }
    void Update(){
        if(currentPlayer!=null){
        }
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventHandler.E_AfterLoadScene -= FindPlayer;
    }
    void FindPlayer(){
        currentPlayer = FindFirstObjectByType<PlayerController>();
    }
}
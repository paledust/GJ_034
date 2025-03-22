using UnityEngine;

public class SubWorld : MonoBehaviour
{
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private Target target;
    [SerializeField] private PlayerDummy playerDummy;

    private bool isActivated = false;

    void Start()
    {
        playerDummy.gameObject.SetActive(false);
    }
    public void Activate()
    {
        if(!isActivated)
        {
            isActivated = true;
            playerDummy.gameObject.SetActive(true);
            target.ActivateTarget();
        }
    }
    public bool CheckTarget()
    {
        if(target.hasPlayer)
        {
            EventHandler.Call_OnHitCircle(target);
        }
        return target.hasPlayer;
    }
}

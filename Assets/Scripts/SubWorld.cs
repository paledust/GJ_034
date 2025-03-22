using UnityEngine;

public class SubWorld : MonoBehaviour
{
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private Target target;
    [SerializeField] private PlayerDummy playerDummy;
    private Window window;

    private bool isActivated = false;

    void Start()
    {
        playerDummy.gameObject.SetActive(false);
        target.LinkWorld(this);
    }
    public void LinkWindow(Window window)
    {
        this.window = window;
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
            target.Defeat();
        }
        return target.hasPlayer;
    }
    public void Explode()
    {
        window.Explode();
    }
}
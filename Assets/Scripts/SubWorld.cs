using DG.Tweening;
using UnityEngine;

public class SubWorld : MonoBehaviour
{
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private Target target;
    [SerializeField] private PlayerDummy playerDummy;
    [SerializeField] private Window window;
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
    public void Deactivate()
    {
        isActivated = false;
        playerDummy.gameObject.SetActive(false);
        target.ResetTarget();
    }
    public bool CheckTarget()=>target.hasPlayer;
    public void Explode()
    {
        window.ExplodeWindow();
        Deactivate();
    }
    public void OnDefeat()
    {
        background.DOColor(Color.white, 0.1f);
        playerDummy.DefeatTarget();
        target.Defeat();
    }
    public void ResetSubWorld(Color color){
        background.color = color;
        Deactivate();
    }
    public float GetCost()=>target.currentTime;
    public Vector4 GetBounds()=>window.GetBoundry();
}
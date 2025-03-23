using UnityEngine;
using DG.Tweening;
using System.Collections;

public class Window : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SubWorld subWorld;
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private float scaleTime = 0.1f;

    private int sortIndex;
    private Vector3 originScale;
    private Collider2D m_collider;
    public int m_sortIndex=>sortIndex;
    void Awake()
    {
        m_collider = GetComponent<Collider2D>();
    }
    private void Start()
    {
        originScale = transform.localScale;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Service.PLAYER_TAG)
        {
            EventHandler.Call_OnEnterWindow(this);
        }
    }
    public void EnableHitbox()=>m_collider.enabled = true;
    public void DisableHitbox()=>m_collider.enabled = false;
    public void ActivateWindow()
    {
        transform.DOKill();
        transform.DOScale(originScale*scaleFactor, scaleTime).SetEase(Ease.OutQuad);
        subWorld.Activate();
        spriteRenderer.sortingOrder = 6;
    }
    public bool CheckTarget()=>subWorld.CheckTarget();
    public Vector4 GetBoundry()
    {
        Vector4 boundry = Vector4.zero;
        boundry.x = transform.localPosition.x + transform.localScale.x*0.5f*scaleFactor;
        boundry.y = transform.localPosition.x - transform.localScale.x*0.5f*scaleFactor;
        boundry.z = transform.localPosition.y + transform.localScale.y*0.5f*scaleFactor;
        boundry.w = transform.localPosition.y - transform.localScale.y*0.5f*scaleFactor;
        return boundry;
    }
    public void ExplodeWindow()
    {
        gameObject.SetActive(false);
        EventHandler.Call_OnWindowExplode(this);
    }
    public void DefeatWindow()
    {
        subWorld.OnDefeat();
        StartCoroutine(coroutineDefeatWindow());
    }
    public float GetCost()=>subWorld.GetCost();
    public void CompleteReset(Color backColor, int sort)
    {
        originScale = transform.localScale;
        subWorld.ResetSubWorld(backColor);
        spriteRenderer.sortingOrder = sort;
        StartCoroutine(CommonCoroutine.delayAction(()=>EnableHitbox(), 0.1f));
    }
    public void DecrementSort()
    {
        spriteRenderer.sortingOrder -= 1;
    }
    IEnumerator coroutineDefeatWindow()
    {
        yield return new WaitForSeconds(0.25f);
        Vector3 startScale = transform.localScale;
        Vector3 endSccale = startScale;
        endSccale.x *= 2;
        endSccale.y = 0; 
        yield return new WaitForLoop(0.1f, (t)=>{
            transform.localScale = Vector3.Lerp(startScale,endSccale,EasingFunc.Easing.CircEaseIn(t));
        });
        transform.localScale = endSccale;
        gameObject.SetActive(false); 
    }
}

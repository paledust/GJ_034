using UnityEngine;
using DG.Tweening;

public class Window : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SubWorld subWorld;
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private float scaleTime = 0.1f;
    private Vector3 originScale;
    private Collider2D m_collider;
    void Awake()
    {
        m_collider = GetComponent<Collider2D>();
    }
    private void Start()=>originScale = transform.localScale;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Service.PLAYER_TAG)
        {
            EventHandler.Call_OnEnterWindow(this);
        }
    }
    public void DisableHitbox()
    {
        m_collider.enabled = false;
    }
    public void ActivateWindow()
    {
        transform.DOKill();
        transform.DOScale(originScale*scaleFactor, scaleTime).SetEase(Ease.OutQuad);
        subWorld.Activate();
        spriteRenderer.sortingOrder = 6;
    }
    public Vector4 GetBoundry()
    {
        Vector4 boundry = Vector4.zero;
        boundry.x = transform.localPosition.x + transform.localScale.x*0.5f*scaleFactor;
        boundry.y = transform.localPosition.x - transform.localScale.x*0.5f*scaleFactor;
        boundry.z = transform.localPosition.y + transform.localScale.y*0.5f*scaleFactor;
        boundry.w = transform.localPosition.y - transform.localScale.y*0.5f*scaleFactor;
        return boundry;
    }
}

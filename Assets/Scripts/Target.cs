using UnityEngine;
using DG.Tweening;

public class Target : MonoBehaviour
{
    public enum TargetState
    {
        Idle,
        Activate,
        Defeat,
        Explode
    }
    [SerializeField] private SpriteRenderer m_renderer;
    [SerializeField] private Animator m_animator;
    [SerializeField] private TargetState state = TargetState.Idle;
    [SerializeField] private SubWorld subWorld;

    private float stateTimer = 0;
    private Vector3 startScale;
    private Color startColor;
    private Collider2D m_collider;

    public bool hasPlayer{get; private set;} = false;
    public float currentTime => stateTimer;
    
    private const string IDLE_TRIGGER = "Idle";
    private const string ACTIVE_TRIGGER = "Active";
    private const string DEFEATE_TRIGGER = "Defeat";
    private const string EXPLODE_TRIGGER = "EXPLODE";

    void Awake(){
        m_collider = GetComponent<Collider2D>();
        startColor = m_renderer.color;
    }
    void Start()=>startScale = transform.localScale;
    void Update()
    {
        switch(state)
        {
            case TargetState.Activate:
                if(stateTimer >= Service.MAX_GAME_TIME)
                {
                    m_collider.enabled = false;
                    ChangeState(TargetState.Explode);
                    return;
                }
                stateTimer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, Mathf.Clamp01(stateTimer/Service.MAX_GAME_TIME));
                return;
        }
    }
    void ChangeState(TargetState newState)
    {
        if(state != newState)
        {
            state = newState;
            switch(newState)
            {
                case TargetState.Idle:
                    m_animator.SetTrigger(IDLE_TRIGGER);
                    break;
                case TargetState.Activate:
                    m_animator.SetTrigger(ACTIVE_TRIGGER);
                    break;
                case TargetState.Defeat:
                    transform.DOScale(startScale, 0.1f).SetEase(Ease.OutBack);
                    m_animator.SetTrigger(DEFEATE_TRIGGER);
                    break;
                case TargetState.Explode:
                    subWorld.Explode();
                    m_animator.SetTrigger(EXPLODE_TRIGGER);
                    break;
            }
            stateTimer = 0;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == Service.PLAYER_TAG)
        {
            hasPlayer = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {   
        if(other.tag == Service.PLAYER_TAG)
        {
            hasPlayer = false;
        }
    }
    public void ResetTarget(){
        m_renderer.color = startColor;
        transform.localScale = startScale;
        Vector4 bound = subWorld.GetBounds();
        float width = bound.x - bound.y;
        float height = bound.z - bound.w;
        transform.localPosition = new Vector2(Random.Range(bound.y+width*0.25f, bound.x-width*0.25f), Random.Range(bound.w+height*0.25f, bound.z-height*0.25f));
        m_collider.enabled = true;
        ChangeState(TargetState.Idle);
    }
    public void Defeat()=>ChangeState(TargetState.Defeat);
    public void ActivateTarget()=>ChangeState(TargetState.Activate);
}
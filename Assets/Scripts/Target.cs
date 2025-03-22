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

    public bool hasPlayer{get; private set;} = false;
    private SubWorld subWorld;
    private float stateTimer = 0;
    private Vector3 startScale;
    private Collider2D m_collider;

    private const string ACTIVE_TRIGGER = "Active";
    private const string DEFEATE_TRIGGER = "Defeat";
    private const string EXPLODE_TRIGGER = "EXPLODE";

    void Awake()=>m_collider = GetComponent<Collider2D>();
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
        stateTimer = 0;
        state = newState;
        switch(newState)
        {
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
    public void Defeat()=>ChangeState(TargetState.Defeat);
    public void ActivateTarget()=>ChangeState(TargetState.Activate);
    public void LinkWorld(SubWorld subWorld)=>this.subWorld = subWorld;
}
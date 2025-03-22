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
[Header("Hit Feedback")]
    [SerializeField] private Color hitColor;
    [SerializeField] private float hitDuration = 0.25f;

    private float stateTimer = 0;
    private Vector3 startScale;
    private Collider2D m_collider;

    private const string ACTIVE_TRIGGER = "Active";
    private const string DEFEATE_TRIGGER = "Defeat";
    private const string EXPLODE_TRIGGER = "EXPLODE";

    void Awake()=>m_collider = GetComponent<Collider2D>();
    void Start()=>startScale = transform.localScale;
    public void ActivateTarget()
    {
        ChangeState(TargetState.Activate);
    }
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
                m_animator.SetTrigger(DEFEATE_TRIGGER);
                break;
            case TargetState.Explode:
                m_animator.SetTrigger(EXPLODE_TRIGGER);
                break;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(state == TargetState.Activate && other.tag == Service.PLAYER_TAG)
        {
            m_collider.enabled = false;

            ChangeState(TargetState.Defeat);
        
            m_renderer.DOColor(hitColor, hitDuration);
            EventHandler.Call_OnHitCircle(this);
        }
    }
}

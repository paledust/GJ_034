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

    private float floatFreq = 2;
    private float floatPhase = 2;
    private float floatAmp = 1;

    private float stateTimer = 0;
    private Vector3 startScale = Vector3.one;
    private Vector3 idlePos;
    private Vector4 bound;
    private Color startColor;
    private Collider2D m_collider;

    public bool hasPlayer{get; private set;} = false;
    public float currentTime => stateTimer;

    private const string IDLE_TRIGGER = "Idle";
    private const string ACTIVE_TRIGGER = "Active";
    private const string DEFEATE_TRIGGER = "Defeat";

    void Awake(){
        m_collider = GetComponent<Collider2D>();
        startColor = m_renderer.color;
        idlePos = transform.localPosition;
    }
    void Start()=>startScale = Vector3.one*2.5f;
    void Update()
    {
        switch(state)
        {
            case TargetState.Idle:
                transform.localPosition = idlePos + Vector3.up*Mathf.Sin(Mathf.PI*Time.time*floatFreq + floatPhase)*floatAmp;
                break;
            case TargetState.Activate:
                if(stateTimer >= Service.MAX_GAME_TIME)
                {
                    m_collider.enabled = false;
                    ChangeState(TargetState.Explode);
                    return;
                }
                stateTimer += Time.deltaTime;
                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, Mathf.Clamp01(stateTimer/Service.MAX_GAME_TIME));
                break;
        }
        transform.localPosition = Service.ConstraintInBoundry(transform.localPosition, bound, 1);
        
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
        floatFreq = Random.Range(0.2f,0.4f);
        floatPhase = Random.Range(0, Mathf.PI);
        floatAmp = Random.Range(0.5f,1.5f);
        m_renderer.color = startColor;
        transform.localScale = Vector3.one*2.5f;
        bound = subWorld.GetBounds();
        float width = bound.x - bound.y;
        float height = bound.z - bound.w;
        transform.localPosition = new Vector2(Random.Range(bound.y+width*0.25f, bound.x-width*0.25f), Random.Range(bound.w+height*0.25f, bound.z-height*0.25f));
        m_collider.enabled = true;
        idlePos = transform.localPosition;
        ChangeState(TargetState.Idle);
    }
    public void Defeat()
    {
        ChangeState(TargetState.Defeat);
        var behavior = GetComponent<TargetBehavior>();
        if(behavior!=null)
        {
            Destroy(behavior);
        }
    }
    public void ActivateTarget()=>ChangeState(TargetState.Activate);
}
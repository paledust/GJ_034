using System.Collections;
using SimpleAudioSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
[Header("Control")]
    [SerializeField] private Animation m_anime;
    [SerializeField] private PlayerInput input;
    [SerializeField] private float speed = 2;
    [SerializeField] private float lerpSpeed = 10;
    [SerializeField] private SpriteRenderer playerRender;
    
[Header("Hit feedback")]
    [SerializeField] private Color hitColor;
    [SerializeField] private Color originalColor;
    [SerializeField] private ParticleSystem p_droplet;
    [SerializeField] private float hitCooldown = 0.05f;
[Header("Audio")]
    [SerializeField] private AudioSource playerAudio;
    [SerializeField] private string click_clip;

    private Vector2 direction;
    private Vector2 velocity;
    private Vector2 pointerDelta;
    private bool cooling;

    public bool IsCooling=>cooling;
    public Vector2 m_velocity=>velocity;
    private const float BOUND_EXTEND = 1;

    void Start()
    {
        direction = Vector2.down;
        velocity  = Vector2.zero;
        pointerDelta = Vector2.zero;
        speed *= (Screen.width+0f)/800f;
    }
    // Update is called once per frame
    void Update()
    {
        velocity = Vector2.Lerp(velocity, pointerDelta * (cooling?0:speed), Time.deltaTime * lerpSpeed);
        if(velocity.sqrMagnitude!=0)
        {
            direction = Vector2.Lerp(direction, velocity.normalized, Time.deltaTime * lerpSpeed).normalized;
        }
        transform.rotation = Quaternion.Euler(0, 0, -Vector2.SignedAngle(direction, Vector2.down));
        Vector2 pos = (Vector2)transform.position + velocity * Time.deltaTime;
        transform.position = Service.ConstraintInBoundry(pos, PlayerManager.Instance.currentBounds, BOUND_EXTEND);
    }
    public void DeactiveRender()=>playerRender.enabled = false;
    public void ActiveRender()=>playerRender.enabled = true;
    public void ResetPlayer()
    {
        input.enabled = false;
        transform.position = Vector3.zero;
        StartCoroutine(coroutineResetPlayer());
    }
    public void DefeatTarget()
    {
        input.enabled = false;
        StartCoroutine(coroutineStunPlayer());
    }

#region Player Input
    void OnPointerMove(InputValue value)
    {
        pointerDelta = value.Get<Vector2>();
    }
    void OnFire(InputValue value)
    {
        if(!cooling)
        {
            AudioManager.Instance.PlaySoundEffect(playerAudio, click_clip, 1.0f);
            pointerDelta = Vector2.zero;
            EventHandler.Call_OnCheckTarget();
            StartCoroutine(coroutineHit());
        }
    }
#endregion
    IEnumerator coroutineHit()
    {
        cooling = true;
        playerRender.color = hitColor;
        yield return new WaitForSeconds(hitCooldown);
        playerRender.color = originalColor;
        cooling = false;
    }
    IEnumerator coroutineResetPlayer()
    {
        yield return new WaitForSeconds(1.5f);
        ActiveRender();
        m_anime.Play();
        yield return new WaitForSeconds(0.4f);
        input.enabled = true;
    }
    IEnumerator coroutineStunPlayer()
    {
        ActiveRender();
        yield return new WaitForSeconds(0.1f);
        p_droplet.Play();
        input.enabled = true;
    }
}

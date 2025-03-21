using System.Collections;
using System.Collections.Generic;
using SimpleAudioSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
[Header("Control")]
    [SerializeField] private PlayerInput input;
    [SerializeField] private float speed = 2;
    [SerializeField] private float lerpSpeed = 10;
[Header("Audio")]
    [SerializeField] private AudioSource playerAudio;

    private Vector2 direction;
    private Vector2 velocity;
    private Vector2 pointerDelta;
    void Start()
    {
        direction = Vector2.down;
        velocity  = Vector2.zero;
        pointerDelta = Vector2.zero;
    }
    // Update is called once per frame
    void Update()
    {
        velocity = Vector2.Lerp(velocity, pointerDelta * speed, Time.deltaTime * lerpSpeed);
        if(velocity.sqrMagnitude!=0)
        {
            direction = Vector2.Lerp(direction, velocity.normalized, Time.deltaTime * lerpSpeed).normalized;
        }
        transform.rotation = Quaternion.Euler(0, 0, -Vector2.SignedAngle(direction, Vector2.down));
        transform.position = (Vector2)transform.position + velocity * Time.deltaTime;
    }

    #region Handle Interactable
    public void CheckControllable(){
        if(PlayerManager.Instance.m_canControl){
            input.ActivateInput();
            this.enabled = true;
        }
        else{
            this.enabled = false;
            input.DeactivateInput();
        }
    }
#endregion

#region Player Input
    void OnPointerMove(InputValue value){
        pointerDelta = value.Get<Vector2>();
    }
    void OnFire(InputValue value){}
#endregion
}

using UnityEngine;

public class TargetDodge : TargetBehavior
{
    private float maxSpeed = 20;
    private Vector2 targetVel = Vector2.zero;
    private Vector2 velocity;
    private PlayerController player;

    void Start()
    {
        velocity = targetVel = Vector2.zero;
        player = PlayerManager.Instance.currentPlayer;
    }

    void Update()
    {
        targetVel = Vector2.ClampMagnitude(Quaternion.Euler(0,0,30)*player.m_velocity, maxSpeed);
        velocity = Vector2.Lerp(velocity, targetVel, Time.deltaTime*4);
        transform.localPosition += (Vector3)velocity * Time.deltaTime;
    }
}

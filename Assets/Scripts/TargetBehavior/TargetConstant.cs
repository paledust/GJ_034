using UnityEngine;

public class TargetConstant : TargetBehavior
{
    private float maxSpeed = 15;
    private Vector2 targetVel = Vector2.zero;
    private Vector2 velocity;
    void Start()
    {
        velocity = targetVel = Vector2.zero;
        targetVel = Random.insideUnitCircle.normalized * maxSpeed;
    }

    void Update()
    {
        velocity = Vector2.Lerp(velocity, targetVel, Time.deltaTime*4);
        transform.localPosition += (Vector3)velocity * Time.deltaTime;
    }
}

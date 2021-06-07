using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LimitVelocity : MonoBehaviour
{
    private Rigidbody2D _rigidBody;

    public float MaxVelocityX = float.PositiveInfinity;
    public float MaxVelocityY = float.PositiveInfinity;
    public float MaxAngularVelocity = float.PositiveInfinity;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Mathf.Abs(_rigidBody.angularVelocity) > MaxAngularVelocity)
            _rigidBody.angularVelocity = (_rigidBody.angularVelocity > 0 ? MaxAngularVelocity : -MaxAngularVelocity); 

        var velocity = _rigidBody.velocity;
        if (Mathf.Abs(velocity.x) > MaxVelocityX)
            velocity.x = (velocity.x > 0 ? MaxVelocityX : -MaxVelocityX);

        if (Mathf.Abs(velocity.y) > MaxVelocityY)
            velocity.y = (velocity.y > 0 ? MaxVelocityY : -MaxVelocityY);
            
        _rigidBody.velocity = velocity;
    }
}

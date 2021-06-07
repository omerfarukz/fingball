using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class AnimateWithVelocityBehavior : MonoBehaviour
{
    private Rigidbody2D _rigidBody2d;
    private Animator _animator;

    private void Start()
    {
        _rigidBody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();

        _rigidBody2d.freezeRotation = true;
    }

    void Update()
    {
        _animator.speed = _rigidBody2d.velocity.magnitude / 2f;
        var angle = Mathf.Atan2(_rigidBody2d.velocity.y, _rigidBody2d.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

}

using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InstansiateObjectOnCollision : MonoBehaviour
{
    public GameObject Object;
    public string Tag = "Ground";
    public float MinrRlativeVelocity = 2f;
    public float Frequency = 0.2f;

    private float _nextAvailableTime = 0f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tag))
        {
            if (collision.contacts != null && collision.relativeVelocity.magnitude > MinrRlativeVelocity)
            {
                if (Time.time > _nextAvailableTime)
                {
                    var dust = Instantiate(Object);
                    dust.transform.position = collision.contacts[0].point;
                    _nextAvailableTime = Time.time + Frequency;
                }
            }
        }
    }
}
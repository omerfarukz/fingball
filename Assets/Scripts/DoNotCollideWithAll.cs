using UnityEngine;

public class DoNotCollideWithAll : MonoBehaviour
{
    public GameObject Target;

    void Start()
    {
        IgnoreCollision();
    }

    private void OnEnable()
    {
        IgnoreCollision();
    }

    private void IgnoreCollision()
    {
        if (Target != null)
        {
            var colliders = GetComponents<Collider2D>();
            if (colliders != null)
            {
                foreach (var thisCollider in colliders)
                {
                    foreach (var targetCollider in Target.GetComponents<Collider2D>())
                    {
                        Physics2D.IgnoreCollision(thisCollider, targetCollider, true);
                    }
                }
            }
        }
    }
}

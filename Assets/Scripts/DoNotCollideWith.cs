using UnityEngine;

public class DoNotCollideWith : MonoBehaviour
{
    public Collider2D[] Gamebjects;

    void Awake()
    {
        if (Gamebjects != null)
        {
            var thisCollider = GetComponent<Collider2D>();
            foreach (var item in Gamebjects)
            {
                var _collider = item.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(thisCollider, _collider);
            }
        }
    }
}

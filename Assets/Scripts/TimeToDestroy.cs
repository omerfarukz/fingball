using UnityEngine;

public class TimeToDestroy : MonoBehaviour
{
    public float Time = 1f;

    void Start()
    {
        Destroy(gameObject, this.Time);
    }
}

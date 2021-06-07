using UnityEngine;

public class BallShadowBehavior : MonoBehaviour
{
    public Transform Ball;
    public Transform Light;
    public float SpriteRotation = 0f;

    void Update()
    {
        var lPosDifference = transform.position - Light.position;
        var angle = (Mathf.Atan2(lPosDifference.y, lPosDifference.x) + SpriteRotation) * Mathf.Rad2Deg;
        transform.position = Ball.position + new Vector3(0.01f, 0.01f);
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
}
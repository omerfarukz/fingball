using UnityEngine;

public class ComputerController : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private Vector2 _startPosition;

    public Rigidbody2D Ball;
    public Transform Center;
    public SettingsController SettingsController;

    public bool Rule1;
    public bool Rule2;
    public bool Rule3;

    [Range(2f, 5f)]
    public float Rule2_Distance = 4f;

    [Range(2f, 15f)]
    public float Speed = 3f;

    private GameController _gameController;

    void Start()
    {
        _gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
        _rigidBody = GetComponent<Rigidbody2D>();
        _startPosition = transform.position;
    }

    void Update()
    {
        var ballPosition = Ball.position;

        Vector2 vector = Vector2.zero;

        if (_gameController.BallInPlay && !_gameController.UIController.IsActiveAny())
        {
            if (Rule1 && _gameController.BallInPlay)
            {
#if DEBUG
                var v0 = new Vector3(ballPosition.x, ballPosition.y, 0f);
                var v1 = new Vector3(ballPosition.x, ballPosition.y + 15f, 0f);

                Debug.DrawLine(v0, v1, Color.blue);
#endif
                var xDifference = ballPosition - (Vector2)transform.position;
                if (Mathf.Abs(xDifference.x) > 0.5f)
                    vector += new Vector2(xDifference.x / 5f, 0f);

            }

            if (Rule2)
            {
#if DEBUG
                var v0 = new Vector3(-5f, transform.position.y - Rule2_Distance, 0f);
                var v1 = new Vector3(5f, transform.position.y - Rule2_Distance, 0f);

                Debug.DrawLine(v0, v1, Color.red);
#endif

                // Kural 2 - Topa hizla yaklas
                var differenceToBall = ballPosition - (Vector2)transform.position;
                var differenceToCenter = ballPosition.y - transform.position.y;

                if (differenceToBall.magnitude < Rule2_Distance && ballPosition.y > Center.position.y)
                {
                    var normalized = differenceToBall.normalized;
                    if (Mathf.Abs(normalized.y) < 0.1f)
                        normalized.y += 0.2f;

                    vector += normalized;
                }
            }

            if (Rule3 && ballPosition.y < Center.position.y)
            {
                var difference2 = _startPosition - (Vector2)transform.position;
                if (difference2.magnitude > 0.5f)
                    vector += difference2.normalized / 5f;
            }
        }
        else
        {
            var difference2 = _startPosition - (Vector2)transform.position;
            if (difference2.magnitude > 0.5f)
                vector += difference2.normalized / 5f;
        }
    

        vector *= (Speed + (SettingsController.Difficulty * 3f));
        vector *= Time.deltaTime;

        var pos = Vector2.Lerp(transform.position, (Vector2)transform.position + vector, 1f);

        _rigidBody.MovePosition(pos);
    }
}

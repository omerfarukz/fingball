using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidBody2D;
    private Vector2 _touchObjectOffset;
    private bool _moving;

    public Transform TouchObject;
    public float MinDistance = 1f;

    public int? FingerId = -1;
    public PlayerController OtherPlayer;

    void Start()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _touchObjectOffset = transform.position - TouchObject.position;
    }


    private void FixedUpdate()
    {

        var targetPos = Vector2.zero;

//#if UNITY_EDITOR
        if (Input.touchSupported && Input.touchCount > 0)
        {
            if (!_moving)
            {
                foreach (var currentTouch in Input.touches)
                {
                    var touchPos = Camera.main.ScreenToWorldPoint(currentTouch.position);
                    if (
                        (
                            currentTouch.phase == TouchPhase.Began // || currentTouch.phase == TouchPhase.Moved
                        ) &&
                        Vector2.Distance(TouchObject.position, touchPos) <= MinDistance
                    )
                    {    
                            _moving = true;
                            FingerId = currentTouch.fingerId;

                    }
                }
            }
            else
            {
                if (Input.touches.Any(f => f.fingerId == FingerId.Value))
                {
                    var touch = Input.touches.Single(f => f.fingerId == FingerId.Value);
                    if (touch.phase == TouchPhase.Moved)
                        targetPos = Camera.main.ScreenToWorldPoint(touch.position);
                    else
                        targetPos = TouchObject.position;
                }
                else
                {
                    _moving = false;
                    FingerId = null;
                }

            }
        }
        else if(Input.GetMouseButton(0)) 
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (!_moving && Vector2.Distance(TouchObject.position, mousePos) <= MinDistance)
            {
                _moving = true;
            }

            targetPos = mousePos;
        }
        else
        {
            _moving = false;
        }

        if (_moving && targetPos != Vector2.zero)
        {
            _rigidBody2D.MovePosition(targetPos + _touchObjectOffset);
        }

        _rigidBody2D.velocity = Vector2.zero;
    }

    // TODO: optimize
    private Vector2 InputPosition()
    {
        Vector2 position = Vector2.zero;
        if (Input.touchSupported && Input.touchCount > 0)
        {
            foreach (var currentTouch in Input.touches)
            {
                if (currentTouch.phase == TouchPhase.Moved)
                {
                    if (Vector2.Distance(TouchObject.position, Camera.main.ScreenToWorldPoint(currentTouch.position)) <= MinDistance)
                    {
                        position = Camera.main.ScreenToWorldPoint(currentTouch.position);
                    }
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (Vector2.Distance(TouchObject.position, Camera.main.ScreenToWorldPoint(Input.mousePosition)) <= MinDistance)
            {
                position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        return position;
    }
}

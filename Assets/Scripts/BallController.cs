using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BallController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlayerGoal")
        {
            var gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            gameController.GoalFrom(false, true);
        }
        else if (collision.gameObject.name == "ComputerGoal")
        {
            var gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            gameController.GoalFrom(true, false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            var rigidBody2D = GetComponent<Rigidbody2D>();
            var velocity = rigidBody2D.velocity;
            if (velocity.y > 0.5f && velocity.x < 0.05f)
            {
                var forceX = (transform.position.x < 0) ? 1f : -1f;
                rigidBody2D.AddForce(new Vector2(forceX, 0f), ForceMode2D.Force);
                //Debug.Log("Ball x velocity randomized");
            }
            rigidBody2D.velocity = velocity;
        }
    }
}
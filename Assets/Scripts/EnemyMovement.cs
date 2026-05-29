using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float enemyMoveSpeed = 2f;
    Rigidbody2D enemy;

    void Start()
    {
        enemy = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        enemy.linearVelocity = new Vector2(enemyMoveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        enemyMoveSpeed = -enemyMoveSpeed;
        FlipDirection();
    }

    void FlipDirection()
    {
        transform.localScale = new Vector2(-Mathf.Sign(enemy.linearVelocityX), 1f);
    }
}

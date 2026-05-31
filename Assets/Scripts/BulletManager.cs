using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;

    Rigidbody2D bulletBody, player;
    private float xSpeed;

    void Start()
    {
        bulletBody = GetComponent<Rigidbody2D>();
        player = PlayerMovement.instance.GetPlayer();
        xSpeed = player.transform.localScale.x * bulletSpeed;
    }

    void Update()
    {
        bulletBody.linearVelocity = new Vector2(xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject, 0.3f);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0.4f);
    }
}

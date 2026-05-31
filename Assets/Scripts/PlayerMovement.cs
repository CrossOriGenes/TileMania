using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    [SerializeField] float playerMoveSpeed = 9.55f;
    [SerializeField] float jumpSpeed = 23f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Vector2 moveInput;
    Rigidbody2D player;
    Animator animator;
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    float gravity;
    bool isAlive = true, hasTouchedWater = false;
    int waterContacts = 0;

    void Start()
    {
        instance = this;
        player = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        gravity = player.gravityScale;
    }

    void Update()
    {
        if (!isAlive) return;
        Run();
        FlipPlayer();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) return;
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        if (value.isPressed)
        {
            player.linearVelocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnAttack(InputValue value)
    {
        if (!isAlive) return;
        Instantiate(bullet, gun.position, transform.rotation);
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * playerMoveSpeed, player.linearVelocityY);
        player.linearVelocity = playerVelocity;
        bool hasPlayerSpeed = Mathf.Abs(player.linearVelocityX) > Mathf.Epsilon;
        animator.SetBool("isRunning", hasPlayerSpeed);
    }

    void FlipPlayer()
    {
        bool hasHorizontalSpeed = Mathf.Abs(player.linearVelocityX) > Mathf.Epsilon;
        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(player.linearVelocityX), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            player.gravityScale = gravity;
            animator.SetBool("isClimbing", false);
            return;
        }
        player.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(player.linearVelocityX, moveInput.y * climbSpeed);
        player.linearVelocity = climbVelocity;
        bool hasVerticalSpeed = Mathf.Abs(player.linearVelocityY) > Mathf.Epsilon;
        animator.SetBool("isClimbing", hasVerticalSpeed);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Waterbody")) return;

        waterContacts++;

        if (!hasTouchedWater)
        {
            hasTouchedWater = true;
            PlayerHealthManager.instance.TakeDamage();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Waterbody")) return;

        waterContacts--;

        if (waterContacts <= 0)
        {
            waterContacts = 0;
            hasTouchedWater = false;
        }
    }

    void Die()
    {
        if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")) ||
        playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")) ||
        playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");
            Invoke(nameof(ShowGameOver), 0.5f);
        }
    }

    void ShowGameOver()
    {
        GameManager.instance.GameOver();
    }

    // Setters
    public bool IsPlayerAlive()
    {
        return isAlive;
    }
    public void SetPlayerAlive(bool flag)
    {
        isAlive = flag;
    }

    // Getters
    public Animator GetAnimator()
    {
        return animator;
    }
    public Rigidbody2D GetPlayer()
    {
        return player;
    }
}

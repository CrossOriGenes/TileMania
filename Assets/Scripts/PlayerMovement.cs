using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float playerMoveSpeed = 9.55f;
    [SerializeField] float jumpSpeed = 23f;
    [SerializeField] float climbSpeed = 3f;

    Vector2 moveInput;
    Rigidbody2D player;
    Animator animator;
    CapsuleCollider2D playerCollider;
    float gravity;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        gravity = player.gravityScale;
    }

    void Update()
    {
        Run();
        FlipPlayer();
        ClimbLadder();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        if (value.isPressed)
        {
            player.linearVelocity += new Vector2(0f, jumpSpeed);
        }
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
        if (!playerCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            player.gravityScale = gravity;
            return;  
        } 
        player.gravityScale = 0f;
        Vector2 climbVelocity = new Vector2(player.linearVelocityX, moveInput.y * climbSpeed);
        player.linearVelocity = climbVelocity;
        bool hasVerticalSpeed = Mathf.Abs(player.linearVelocityY) > Mathf.Epsilon;
        animator.SetBool("isClimbing", hasVerticalSpeed);
    }
}

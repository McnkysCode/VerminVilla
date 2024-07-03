using UnityEngine;
using System.Collections;
public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float dashDuration = 0.15f;
    public float dashSpeed = 10f;
    public float dashCooldown = 5f;
    private bool isDashing;
    private float maxDashDistance = 5f;
    private bool canDash = true;

    private Rigidbody2D playerRb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector3 movementDirection;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        movementDirection = new Vector3(horizontalInput, verticalInput, 0f).normalized;
        

        if (horizontalInput != 0f || verticalInput != 0f)
        {
            animator.SetBool("Walking", true);
            animator.SetFloat("Horizontal", horizontalInput);

            if (horizontalInput > 0f)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        playerRb.velocity = movementDirection * moveSpeed;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravityScale = playerRb.gravityScale;
        playerRb.gravityScale = 0f;

        Vector2 startPos = playerRb.position;
        Vector2 endPos = startPos + (Vector2)movementDirection * maxDashDistance;
        Vector2 dashDirection = (endPos - startPos).normalized;

        playerRb.velocity = dashDirection * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        playerRb.velocity = Vector2.zero;
        playerRb.gravityScale = originalGravityScale;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;

    }


}
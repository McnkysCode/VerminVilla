using UnityEngine;

public class JumpBehavior : StateMachineBehaviour
{
    public float Timer;
    private int Rand;

    public float minTime = 2;
    public float maxTime = 4;

    private Transform Player;
    private PlayerHealth playerHealth;
    public float speed = 5;
    public int damageAmount = 10;
    public float attackRange = 1.5f;

    private Transform bossTransform;
    private SpriteRenderer spriteRenderer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Rand = Random.Range(0, 2);
        Timer = Random.Range(minTime, maxTime);

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerHealth = Player.GetComponent<PlayerHealth>();

        bossTransform = animator.transform;
        spriteRenderer = animator.GetComponent<SpriteRenderer>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Timer <= 0)
        {
            if (Rand == 0)
            {
                animator.SetTrigger("Idle1");
            }
            else
            {
                animator.SetTrigger("Run1");
            }
        }
        else
        {
            Timer -= Time.deltaTime;
        }

        Vector2 target = new Vector2(Player.position.x, Player.position.y);
        Vector2 newPosition = Vector2.MoveTowards(bossTransform.position, target, speed * Time.deltaTime);

        bossTransform.position = newPosition;

        // Flip sprite based on movement direction
        if (Player.position.x > bossTransform.position.x)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }

        // Check for collision with player to apply damage
        if (Vector2.Distance(bossTransform.position, Player.position) < attackRange)
        {
            playerHealth.TakeDamage(damageAmount);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
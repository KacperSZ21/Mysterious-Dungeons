using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float wanderRadius = 2f;
    public LayerMask groundLayer;

    [Header("Wander Delay Range")]
    public float minWanderDelay = 1f;
    public float maxWanderDelay = 3f;
    public float stuckTime = 1f;
    [Header("Enemy Attack")]
    public float stoppingDistance;
    public float attackRange = 1.2f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;
    public DamageType damageType = DamageType.Physical;

    private float lastAttackTime;
    private Rigidbody2D rb;
    private Transform player;
    private Vector2 targetPosition;
    private Animator animator;
    private EnemyHealth health;
    private bool isChasing = false;
    private float wanderTimer;
    private float moveTimer = 0f;
    private Vector2 lastPosition;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
        health = gameObject.GetComponent<EnemyHealth>();
        ResetWanderTimer();
    }

    void Update()
    {
        if (isChasing && player != null)
        {
            targetPosition = player.position;
        }
        else
        {
            HandleWander();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleWander()
    {
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0f)
        {
            for (int i = 0; i < 10; i++)
            {
                Vector2 randomDirection = Random.insideUnitCircle * wanderRadius;
                Vector2 candidate = (Vector2)transform.position + randomDirection;

                if (IsOnGround(candidate))
                {
                    targetPosition = candidate;
                    break;
                }
            }

            ResetWanderTimer();
        }
    }

    bool IsOnGround(Vector2 position)
    {
        return Physics2D.OverlapPoint(position, groundLayer) != null;
    }

    void ResetWanderTimer()
    {
        wanderTimer = Random.Range(minWanderDelay, maxWanderDelay);
    }

    void Move()
    {
        moveTimer += Time.deltaTime;
        Vector2 currentPosition = rb.position;
        Vector2 target = targetPosition;

        float distanceToTarget = Vector2.Distance(currentPosition, target);

        if (isChasing && player != null)
        {
            float distanceToPlayer = Vector2.Distance(currentPosition, player.position);

            if (distanceToPlayer <= attackRange)
            {
                animator.SetBool("1_Move", false);
                TryAttack();
                rb.linearVelocity = Vector2.zero;
                return;
            }
        }

        Vector2 newPosition = Vector2.MoveTowards(currentPosition, target, moveSpeed * Time.fixedDeltaTime);

        if (IsOnGround(newPosition))
        {
            rb.MovePosition(newPosition);
        }

        float movedDistance = Vector2.Distance(rb.position, lastPosition);

        if (movedDistance < 0.01f)
        {
            moveTimer += Time.deltaTime;
        }
        else
        {
            moveTimer = 0f;
        }

        if (moveTimer >= stuckTime)
        {
            // choose new point
            Vector2 randomOffset = Random.insideUnitCircle * 1.5f;
            targetPosition = rb.position + randomOffset;

            moveTimer = 0f;
        }

        float distance = Vector2.Distance(currentPosition, target);
        animator.SetBool("1_Move", distance > 0.05f);
        rb.linearVelocity = Vector2.zero;

        // Rotation
        float directionX = target.x - transform.position.x;

        if (directionX < -0.01f)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if (directionX > 0.01f)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        lastPosition = rb.position;
    }

    void TryAttack()
    {
        if (health != null && health.IsDead()) return;

        if (player == null) return;

        // Rotation to player
        float directionX = player.position.x - transform.position.x;
        if (directionX < 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);

        // cooldown
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        if (animator != null)
            animator.SetTrigger("2_Attack");

        rb.linearVelocity = Vector2.zero;

        if (player.TryGetComponent<ClickToMove2D>(out var playerScript))
        {
            playerScript.TakeDamage(attackDamage, damageType);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            isChasing = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;
            isChasing = false;
            ResetWanderTimer();
        }
    }
}
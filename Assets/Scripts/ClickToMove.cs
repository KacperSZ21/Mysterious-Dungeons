using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickToMove2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public LayerMask groundLayer;
    public LayerMask enemyLayer;
    public LayerMask caveLayer;

    [Header("Camera")]
    public Transform cameraTransform;
    public Vector3 cameraOffset = new(0, 0, 0);
    public float cameraSmoothSpeed = 5f;

    [Header("Health")]
    public int baseHealth = 100;
    [HideInInspector]
    public int bonusHealth = 0;
    [HideInInspector]
    public int armor = 0;
    [HideInInspector]
    public ArmorType armorType = ArmorType.Physical;
    private int currentHealth;

    [Header("Attack Parameters")]
    public float attackRange = 1.5f;
    public int baseDamage = 10;
    public DamageType currentDamageType = DamageType.Physical;
    [HideInInspector]
    public int bonusDamage = 0;
    public float attackCooldown = 1f;
    public EnemyUI enemyUI;

    [Header("UI")]
    public UnityEngine.UI.Image healthbarFill;
    public TMP_Text armorText;
    public TMP_Text damageText;
    public GameObject deathMenu;

    [Header("Visuals")]
    public PlayerVisuals visuals;

    private float lastAttackTime;
    private Transform currentTargetEnemy;

    private Vector2 targetPosition;
    private bool isMovingToClick = false;

    private Rigidbody2D rb;
    private Animator animator;
    private PlayerAttackBuff attackBuff;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponentInChildren<Animator>();
        attackBuff = GetComponent<PlayerAttackBuff>();

        currentHealth = GetMaxHealth();
        UpdateStatsUI();
    }

    void Update()
    {
        HandleMouseInput();
    }

    void FixedUpdate()
    {
        HandleKeyboardMovement(); // WASD
        HandleMovement(); // mouse usage
    }

    void LateUpdate()
    {
        HandleCamera();
    }

    void HandleMouseInput()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new(mouseWorld.x, mouseWorld.y);

            // ENEMY
            Collider2D enemy = Physics2D.OverlapCircle(mousePos2D, 0.3f, enemyLayer);

            if (enemy != null)
            {
                currentTargetEnemy = enemy.transform;
                HandleAutoAttack();
                return;
            }
            else
            {
                currentTargetEnemy = null;
                enemyUI.ClearTarget();
                rb.linearVelocity = Vector2.zero;
            }

            // CAVE
            Collider2D caveCollider = Physics2D.OverlapCircle(mousePos2D, 0.3f, caveLayer);

            if (caveCollider != null)
            {
                if (caveCollider.TryGetComponent<CaveEntrance>(out var cave))
                {
                    cave.EnterCave(gameObject);

                    currentTargetEnemy = null;
                    isMovingToClick = false;

                    rb.linearVelocity = Vector2.zero;
                    animator.SetBool("1_Move", false);

                    return;
                }


                if (caveCollider.TryGetComponent<SceneEntrance>(out var entrance))
                {
                    entrance.TryUse(gameObject);
                    return;
                }
            }

            // GROUND CHECK
            if (IsOnGround(mousePos2D))
            {
                targetPosition = mousePos2D;
                isMovingToClick = true;
                animator.SetBool("1_Move", true);

                SetRotation(targetPosition.x - transform.position.x);
            }
        }
    }

    void HandleKeyboardMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 movement = new Vector2(h, v).normalized;

        if (movement.magnitude > 0.1f)
        {
            isMovingToClick = false;

            Vector2 newPosition = rb.position + moveSpeed * Time.fixedDeltaTime * movement;
            animator.SetBool("1_Move", true);

            if (IsOnGround(newPosition))
            {
                rb.MovePosition(newPosition);
            }

            SetRotation(movement.x);

            currentTargetEnemy = null;
            enemyUI.ClearTarget();
        }
        else
        {
            animator.SetBool("1_Move", false);
        }
    }

    void HandleMovement()
    {
        if (!isMovingToClick)
        {
            return;
        }

        Vector2 direction = (targetPosition - rb.position).normalized;
        Vector2 newPos = rb.position + moveSpeed * Time.fixedDeltaTime * direction;

        if (IsOnGround(newPos))
        {
            rb.MovePosition(newPos);
            animator.SetBool("1_Move", true);
        }
        else
        {
            isMovingToClick = false;
            animator.SetBool("1_Move", false);
            rb.linearVelocity = Vector2.zero;
        }

        if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            isMovingToClick = false;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void HandleAutoAttack()
    {
        if (currentTargetEnemy == null) return;

        float distance = Vector2.Distance(transform.position, currentTargetEnemy.position);

        SetRotation(currentTargetEnemy.position.x - transform.position.x);

        EnemyHealth enemyHealth = currentTargetEnemy.GetComponentInParent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyUI.SetTarget(enemyHealth);
        }

        if (distance > attackRange)
        {
            targetPosition = currentTargetEnemy.position;
            isMovingToClick = true;
            return;
        }

        isMovingToClick = false;
        rb.linearVelocity = Vector2.zero;

        // cooldown
        if (Time.time < lastAttackTime + attackCooldown)
            return;

        lastAttackTime = Time.time;

        if (animator != null)
        {
            animator.SetBool("1_Move", false);
            animator.SetTrigger("2_Attack");
        }

        // DAMAGE
        if (enemyHealth != null)
        {
            AttackModifier modifier = attackBuff != null ? attackBuff.Consume() : new AttackModifier();

            int finalDamage = baseDamage + modifier.bonusDamage;

            DamageType finalDamageType = modifier.damageType ?? currentDamageType;

            if (modifier.particlePrefab != null)
            {
                Instantiate(modifier.particlePrefab, enemyHealth.transform.position, Quaternion.identity);
            }

            // Tutaj później można dodać crit
            enemyHealth.TakeDamage(finalDamage, finalDamageType);
        }
    }

    void HandleCamera()
    {
        if (cameraTransform == null) return;

        Vector3 desiredPosition = transform.position + cameraOffset;

        cameraTransform.position = Vector3.Lerp(
            cameraTransform.position,
            desiredPosition,
            cameraSmoothSpeed * Time.deltaTime
        );
    }

    void SetRotation(float directionX)
    {
        if (directionX < 0)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if (directionX > 0)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    bool IsOnGround(Vector2 position)
    {
        return Physics2D.OverlapCircle(position, 0.2f, groundLayer);
    }

    public void TakeDamage(int incomingDamage, DamageType damageType)
    {
        int finalDamage = incomingDamage;

        switch (armorType)
        {
            case ArmorType.Magical:
                if (damageType == DamageType.Physical)
                {
                    finalDamage = Mathf.RoundToInt(incomingDamage * 0.9f);
                }

                else if (damageType == DamageType.Magical)
                {
                    float reduction = armor * 0.01f;

                    reduction = Mathf.Clamp(reduction, 0f, 0.8f);

                    finalDamage = Mathf.RoundToInt(incomingDamage * (1f - reduction));
                }

                break;

            case ArmorType.Physical:
                if (damageType == DamageType.Physical)
                {
                    float reduction = armor * 0.01f;

                    reduction = Mathf.Clamp(reduction, 0f, 0.8f);

                    finalDamage = Mathf.RoundToInt(incomingDamage * (1f - reduction));
                }

                break;
        }

        currentHealth -= finalDamage;

        UpdateStatsUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("4_Death");
        gameObject.SetActive(false);
        deathMenu.SetActive(true);
    }

    public int GetDamage()
    {
        return baseDamage + bonusDamage;
    }

    public int GetMaxHealth()
    {
        return baseHealth + bonusHealth;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void SetCurrentHealth(int value)
    {
        currentHealth = value;
        UpdateStatsUI();
    }

    public void UpdateStatsUI()
    {
        // Armor text
        if (armorText != null)
        {
            armorText.text = armor.ToString();
            armorText.color = armorType == ArmorType.Magical ? Color.cyan : Color.white;
        }

        if (damageText != null)
        {
            damageText.text = GetDamage().ToString();
        }

        // heatlh bar
        if (healthbarFill != null)
        {
            healthbarFill.fillAmount = (float)currentHealth / GetMaxHealth();
        }
    }
}
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public float baseHitPoints;
    public float baseAttackPoints;
    public float baseDefense;
    public float baseSpeed;

    public float maxHitPoints;
    public float maxAttackPoints;
    public float maxDefense;
    public float maxSpeed;

    public int level = 1;

    public float currentHitPoints;
    public float currentAttackPoints;
    public float currentDefense;
    public float currentSpeed;

    public abstract void HitPointScaling();
    public abstract void AttackPointScaling();
    public abstract void DefenseScaling();
    public abstract void SpeedScaling();
    

    public void LevelUp()
    {
        level++;
        HitPointScaling();
        AttackPointScaling();
        DefenseScaling();
        SpeedScaling();
        RecoverAllStats();
    }

    public void NormalAttack(Unit other)
    {
        other.currentHitPoints = other.currentHitPoints - currentAttackPoints;
    }

    private void RecoverAllStats()
    {
        currentHitPoints = maxHitPoints;
        currentAttackPoints = maxAttackPoints;
        currentDefense = maxDefense;
        currentSpeed = maxSpeed;
    }

    protected virtual void Update()
    {
        // Example: Check if the unit's hit points have dropped to zero or lower
        if (currentHitPoints <= 0)
        {
            DestroyUnit();
        }
    }

    protected virtual void DestroyUnit()
    {
        // Implement any logic you want before destroying the unit
        Debug.Log($"{gameObject.name} has been destroyed!");

        // Destroy the GameObject
        Destroy(gameObject);
    }
}

public class Monster : Unit
{
    private Rigidbody rb;
    private Transform player;  // Reference to the player's transform
    private Animator animator;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints.None;
        rb.freezeRotation = true;

        // Assuming the player has the "Player" tag, find the player by tag
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    void FixedUpdate()
    {
        // Check if the player is within a 10 units radius
        if (player != null && Vector3.Distance(transform.position, player.position) <= 5.0f &&
            Vector3.Distance(transform.position, player.position) >= 1.0f)
        {
            // Move towards the player
            MoveTowardsPlayer();
            animator.SetBool("IsRunning", true);
            animator.SetBool("IsAttacking", false); // Ensure IsAttacking is set to false when moving
        }
        else if (player != null && Vector3.Distance(transform.position, player.position) < 2.0f)
        {
            // Stop moving by setting the velocity to zero
            rb.velocity = Vector3.zero;
            animator.SetBool("IsRunning", false);
            
            // Check if not already attacking
            if (!animator.GetBool("IsAttacking"))
            {
                // Set IsAttacking to true to trigger the attack animation
                animator.SetBool("IsAttacking", true);
                NormalAttack(player.GetComponent<Unit>());
            }
        }
        else
        {
            // Stop moving by setting the velocity to zero
            rb.velocity = Vector3.zero;
            animator.SetBool("IsRunning", false);
            animator.SetBool("IsAttacking", false); // Ensure IsAttacking is set to false when not attacking
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction to the player
        animator.SetBool("IsRunning", true);
        Vector3 directionToPlayer = player.position - transform.position;

        // Normalize the direction to get a unit vector
        directionToPlayer.Normalize();

        // Calculate the target position to move towards the player
        Vector3 targetPosition = transform.position + directionToPlayer * currentSpeed * Time.deltaTime;

        // Move the Monster towards the player
        rb.MovePosition(targetPosition);

        // Rotate the Monster to face the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        rb.MoveRotation(targetRotation);
    }

    public override void AttackPointScaling()
    {
        maxAttackPoints = baseAttackPoints * level;
    }
    public override void HitPointScaling()
    {
        maxHitPoints = baseHitPoints * level;
    }
    public override void DefenseScaling()
    {
        maxDefense = baseDefense * level;
    }
    public override void SpeedScaling()
    {
        maxSpeed = baseSpeed * level;
    }

}

public class Hero : Unit
{
    public Equipment[] equippedItems = new Equipment[4];

    public void EquipItem(Equipment item)
    {
        // Implement equip logic for heroes
    }

    public void UnequipItem(string category)
    {
        // Implement unequip logic for heroes
    }

    public override void HitPointScaling()
    {
        // Implement scaling logic for heroes
    }

    public override void AttackPointScaling()
    {
        // Implement scaling logic for heroes
    }

    public override void DefenseScaling()
    {
        // Implement scaling logic for heroes
    }

    public override void SpeedScaling()
    {
        // Implement scaling logic for heroes
    }

    public override string ToString()
    {
        return $"HP: {currentHitPoints}\nATK: {currentAttackPoints}\nDEF: {currentDefense}\nSPD: {currentSpeed}\nLVL: {level}";
    }
}

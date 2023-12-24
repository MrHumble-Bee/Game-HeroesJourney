using UnityEngine;
using System;
using System.Collections;

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
    public float currentExperience = 0;
    private float baseExpReq = 10;
    private float baseExpGrowth = 1.5f;
    public float nextLevelExperience;

    public float currentHitPoints;
    public float currentAttackPoints;
    public float currentDefense;
    public float currentSpeed;

    protected bool isAttacking;
    protected bool isJumping;

    // public HealthBar healthBar;
    public GameObject helmet;
    public GameObject weapon;
    public GameObject bodyarmor;
    public GameObject boots;

    protected abstract void HitPointScaling();
    protected abstract void AttackPointScaling();
    protected abstract void DefenseScaling();
    protected abstract void SpeedScaling();

    public bool isDead = false;
    
    protected virtual void Start()
    {
        RecoverAllStats();
        nextLevelExperience = GetExperienceNeededForLevel();
    }

    // protected virtual void Update()
    // {
    //     if (currentHitPoints <= 0)
    //     {
    //         gameObject.tag = "Untagged";
    //     }
    //     if (currentExperience > nextLevelExperience)
    //     {
    //         currentExperience = currentExperience % nextLevelExperience;
    //         LevelUp();
    //         nextLevelExperience = GetExperienceNeededForLevel(baseExpReq, baseExpGrowth, level);
    //     }
    // }

    protected float GetExperienceNeededForLevel()
    {
        return baseExpReq * (float)Math.Pow(baseExpGrowth, level);
    }

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

    protected virtual void DestroyUnit()
    {
        Destroy(gameObject);
    }
}

public abstract class Monster : Unit
{
    protected Rigidbody rb;
    protected Animator animator;
    public AudioSource attackingSound;

    protected Transform playerTransform;

    private bool canAttack;
    private float timeSinceLastAttack;
    private float walkingSpeedFactor = .33f;
    protected float visionRange;
    protected float attackRange;
    protected float attackCooldown;

    // protected GameObject equipedWeapon;
    private Vector3 randomDirection = new Vector3(0f,0f,0f);
    private float timeSinceLastWander;
    private float wanderCooldown = 3f;


    protected override void Start()
    {
        base.Start();

        // Animation Elements
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        rb.freezeRotation = true;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // equipedWeapon = weapon.
        
        if (playerTransform == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }

    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {
        HandleAiMovement();
    }

    protected virtual void HandleAiMovement()
    {
        if (currentHitPoints <= 0)
        {
            Dying();
        }
        else
        {
            if (playerTransform != null && 
                Vector3.Distance(transform.position, playerTransform.position) <= visionRange &&
                Vector3.Distance(transform.position, playerTransform.position) >= attackRange - 1)
            {
                Vector3 directionToPlayer = playerTransform.position - transform.position;
                Running(directionToPlayer);
            }
            else if (playerTransform != null && 
                     Vector3.Distance(transform.position, playerTransform.position) < attackRange)
            {
                Hero hero = playerTransform.GetComponent<Hero>();
                Attacking(hero);
            }
            else
            {
                // Wander
                Wondering();
            }

        }
    }

    protected void Wondering()
    {
        if (Time.time - timeSinceLastWander >= wanderCooldown)
        {
            randomDirection = UnityEngine.Random.insideUnitSphere * 10.0f;
            timeSinceLastWander = Time.time; // Reset the timer
        }
        else
        {
            Walking(randomDirection);
        }
    }

    protected void Walking(Vector3 direction)
    {
        animator.SetBool("IsWalking", true);
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttacking", false);

        direction.Normalize();
        Vector3 targetPosition = transform.position + direction * currentSpeed * walkingSpeedFactor * Time.deltaTime;
        rb.MovePosition(targetPosition);

        // Rotate the Monster to face the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(targetRotation);
    }

    protected void Running(Vector3 direction)
    {
        animator.SetBool("IsRunning", true);
        animator.SetBool("IsAttacking", false);

        // Normalize the direction to get a unit vector
        direction.Normalize();

        // Calculate the target position to move towards the player
        Vector3 targetPosition = transform.position + direction * currentSpeed * Time.deltaTime;

        // Move the Monster towards the player
        rb.MovePosition(targetPosition);

        // Rotate the Monster to face the player
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        rb.MoveRotation(targetRotation);

    }

    protected void Attacking(Hero hero)
    {
        rb.velocity = Vector3.zero;
        animator.SetBool("IsRunning", false);

        // Check if not already attacking and cooldown has expired
        if (!animator.GetBool("IsAttacking") && canAttack)
        {
            attackingSound.Play();
            // rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0f,45f,0f)));

            // Set IsAttacking to true to trigger the attack animation
            animator.SetBool("IsAttacking", true);

            // Perform a normal attack on the player
            StartCoroutine(AttackCoroutine(hero, 1f));

            // Set the cooldown timer
            canAttack = false;
            timeSinceLastAttack = 0f;
        }

        if (!canAttack)
        {
            timeSinceLastAttack += Time.deltaTime;

            // Check if the cooldown period has elapsed
            if (timeSinceLastAttack >= attackCooldown)
            {
                canAttack = true;
                animator.SetBool("IsAttacking", false);
                rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0f,-45f,0f)));
            }
        }
    }

    IEnumerator AttackCoroutine(Hero hero, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (hero.currentHitPoints > 0)
        {
            NormalAttack(hero);
            attackingSound.Play();
        }

        // You can yield return to wait for a specific duration or the next frame if needed
        yield return null;
    }

    protected void Idling()
    {
        
    }

    protected void Dying()
    {
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsAttacking", false);
        if (isDead == false) {
            animator.SetTrigger("Dead");
            Invoke("DestroyUnit", 3.0f);
            isDead = true;
        }
        rb.velocity = Vector3.zero;
    }


    protected override void AttackPointScaling()
    {
        maxAttackPoints = baseAttackPoints * level;
    }
    protected override void HitPointScaling()
    {
        maxHitPoints = baseHitPoints * level;
    }
    protected override void DefenseScaling()
    {
        maxDefense = baseDefense * level;
    }
    protected override void SpeedScaling()
    {
        maxSpeed = baseSpeed;
    }

}


public class Hero : Unit
{
    public int coins = 0;
    public Equipment[] equippedItems = new Equipment[4];
    private bool isEquipped = false;

    public void EquipItem(GameObject item)
    {
        if (isEquipped)
        {
        }
        else
        {
            // Implement equip logic for heroes
            GameObject weapon1 = Instantiate(item, weapon.transform.position, Quaternion.identity);
            // weapon1.parent = weapon;
            weapon1.transform.SetParent(weapon.transform);
            weapon1.transform.rotation = weapon.transform.rotation;
            isEquipped = true;
        }
    }

    public void UnequipItem(string category)
    {
        // Implement unequip logic for heroes
    }
    protected override void AttackPointScaling()
    {
        maxAttackPoints = baseAttackPoints * level;
    }
    protected override void HitPointScaling()
    {
        maxHitPoints = baseHitPoints * level;
    }
    protected override void DefenseScaling()
    {
        maxDefense = baseDefense * level;
    }
    protected override void SpeedScaling()
    {
        maxSpeed = baseSpeed;
    }
}

public class Mage : Hero
{
    protected void FireBall(Vector3 direction)
    {
        // Cast in a direction

    }

    protected void KineticEnergy(Monster monster)
    {
        // Auto aim
        // Makes the nearest monster increase in y space

    }

    protected void ChainLightning(Monster monster)
    {
        // Auto aim
        // hits the nearest monster after attacking the monster, dealing damage
        

    }

    protected void Golem()
    {
        // Summon Golem near
    }


    protected override void AttackPointScaling()
    {
        maxAttackPoints = baseAttackPoints * level * 1.1f;
    }
    protected override void HitPointScaling()
    {
        maxHitPoints = baseHitPoints * level * 0.8f;
    }
    protected override void DefenseScaling()
    {
        maxDefense = baseDefense * level * 0.85f;
    }
    protected override void SpeedScaling()
    {
        maxSpeed = baseSpeed;
    }
}

// public class Shadow : Hero
// {
//     protected void DashStab()
//     {

//     }

//     protected void ShadowLeap()
//     {

//     }

//     protected void Enhance(float damageMultiplier)
//     {
        

//     }

//     protected void SummonShadow(int numShadow)
//     {
//         // Summon Shadows partners that kill monsters
//     }


//     protected override void AttackPointScaling()
//     {
//         maxAttackPoints = baseAttackPoints * level * 1.15;
//     }
//     protected override void HitPointScaling()
//     {
//         maxHitPoints = baseHitPoints * level * 0.9;
//     }
//     protected override void DefenseScaling()
//     {
//         maxDefense = baseDefense * level * 0.8;
//     }
//     protected override void SpeedScaling()
//     {
//         maxSpeed = baseSpeed;
//     }
// }

public class Archer : Hero
{
    protected void TripleShot(Vector3 direction)
    {

    }

    protected void TumbleShot(Monster monster)
    {
        // auto aim
    }

    protected void RainShot(Vector3 direction)
    {
        

    }

    protected void PenatratingShot(Vector3 direction)
    {
    
    }


    protected override void AttackPointScaling()
    {
        maxAttackPoints = baseAttackPoints * level * 1.1f;
    }
    protected override void HitPointScaling()
    {
        maxHitPoints = baseHitPoints * level * 0.9f;
    }
    protected override void DefenseScaling()
    {
        maxDefense = baseDefense * level * 0.8f;
    }
    protected override void SpeedScaling()
    {
        maxSpeed = baseSpeed;
    }
}

public class Warrior : Hero
{
    protected void Defend(Vector3 direction)
    {

    }

    protected void Charge(Monster monster)
    {
        // auto aim
    }

    protected void GroundPound(Vector3 direction)
    {
        

    }

    protected void Evolve(Vector3 direction)
    {
    
    }

    protected override void AttackPointScaling()
    {
        maxAttackPoints = baseAttackPoints * level * 1.05f;
    }
    protected override void HitPointScaling()
    {
        maxHitPoints = baseHitPoints * level * 1.2f;
    }
    protected override void DefenseScaling()
    {
        maxDefense = baseDefense * level * 1.1f;
    }
    protected override void SpeedScaling()
    {
        maxSpeed = baseSpeed;
    }
}
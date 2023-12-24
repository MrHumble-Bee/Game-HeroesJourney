// using UnityEngine;
// using System;

// public abstract class Unit : MonoBehaviour
// {
//     public float baseHitPoints;
//     public float baseAttackPoints;
//     public float baseDefense;
//     public float baseSpeed;

//     public float maxHitPoints;
//     public float maxAttackPoints;
//     public float maxDefense;
//     public float maxSpeed;

//     public int level = 1;
//     public float currentExperience = 0;
//     private float baseExpReq = 10;
//     private float baseExpGrowth = 1.5f;
//     public float nextLevelExperience;

//     public float currentHitPoints;
//     public float currentAttackPoints;
//     public float currentDefense;
//     public float currentSpeed;

//     public bool isAttacking;
//     public bool isJumping;

//     // public HealthBar healthBar;
//     public GameObject helmet;
//     public GameObject weapon;
//     public GameObject bodyarmor;
//     public GameObject boots;

//     public abstract void HitPointScaling();
//     public abstract void AttackPointScaling();
//     public abstract void DefenseScaling();
//     public abstract void SpeedScaling();

//     public bool isDead = false;
    
//     protected virtual void Start()
//     {
//         nextLevelExperience = GetExperienceNeededForLevel(baseExpReq, baseExpGrowth, level);
//     }

//     protected virtual void Update()
//     {
//         if (currentHitPoints <= 0)
//         {
//             // animator.SetTrigger("Dead");
//             // isDead = true;
//             gameObject.tag = "Untagged";
//             // Invoke("DestroyUnit", 3.0f);
//         }
//         if (currentExperience > nextLevelExperience)
//         {
//             currentExperience = currentExperience % nextLevelExperience;
//             LevelUp();
//             nextLevelExperience = GetExperienceNeededForLevel(baseExpReq, baseExpGrowth, level);
//         }
//     }

//     private float GetExperienceNeededForLevel(float baseExpReq, float baseExpGrowth, int level)
//     {
//         return baseExpReq * (float)Math.Pow(baseExpGrowth, level);
//     }

//     public void LevelUp()
//     {
//         level++;
//         HitPointScaling();
//         AttackPointScaling();
//         DefenseScaling();
//         SpeedScaling();
//         RecoverAllStats();
//     }

//     public void NormalAttack(Unit other)
//     {
//         other.currentHitPoints = other.currentHitPoints - currentAttackPoints;
//     }

//     public void RecoverAllStats()
//     {
//         currentHitPoints = maxHitPoints;
//         currentAttackPoints = maxAttackPoints;
//         currentDefense = maxDefense;
//         currentSpeed = maxSpeed;
//     }

//     protected virtual void DestroyUnit()
//     {
//         // Implement any logic you want before destroying the unit
//         Debug.Log($"{gameObject.name} has been destroyed!");

//         // Destroy the GameObject
//         // gameObject.tag = "Untagged";
//         Destroy(gameObject);
//     }
// }

// public class Monster : Unit
// {

//     protected Rigidbody rb;
//     private Transform player;  // Reference to the player's transform
//     protected Animator animator;
//     private AudioSource audioSource;

//     private bool canAttack = true;  // Flag to track if the unit can attack
//     private float attackCooldown = 2.67f;  // Cooldown period in seconds
//     private float timeSinceLastAttack = 0f;  // Time elapsed since the last attack

//     private float wanderCooldown = 3.0f; // Cooldown period for wandering
//     private float timeSinceLastWander = 0.0f; // Time elapsed since the last wander
//     private Vector3 randomDirection;


//     void Start()
//     {
//         base.Start();
//         rb = GetComponent<Rigidbody>();
//         animator = GetComponent<Animator>();
//         audioSource = GetComponent<AudioSource>();
//         rb.constraints = RigidbodyConstraints.None;
//         rb.freezeRotation = true;

//         // Assuming the player has the "Player" tag, find the player by tag
//         player = GameObject.FindGameObjectWithTag("Player").transform;

//         if (player == null)
//         {
//             Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
//         }
//     }

//     void FixedUpdate()
//     {
//         if (currentHitPoints <= 0)
//         {
//             animator.SetBool("IsRunning", false);
//             animator.SetBool("IsAttacking", false);
//             if (isDead == false) {
//                 animator.SetTrigger("Dead");
//                 Invoke("DestroyUnit", 3.0f);
//                 isDead = true;
//             }
//             rb.velocity = Vector3.zero;
//         }
//         else
//         {
//             // Check if the player is within a 10 units radius
//             if (player != null && Vector3.Distance(transform.position, player.position) <= 5.0f &&
//                 Vector3.Distance(transform.position, player.position) >= 1.0f)
//             {
//                 // Move towards the player
//                 MoveTowardsPlayer();
//                 animator.SetBool("IsRunning", true);
//                 animator.SetBool("IsAttacking", false); // Ensure IsAttacking is set to false when moving
//             }





//             else if (player != null && Vector3.Distance(transform.position, player.position) < 2.0f)
//             {
//                 // Stop moving by setting the velocity to zero
//                 rb.velocity = Vector3.zero;
//                 animator.SetBool("IsRunning", false);

//                 // Check if not already attacking and cooldown has expired
//                 if (!animator.GetBool("IsAttacking") && canAttack)
//                 {
//                     rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0f,45f,0f)));
//                     // Set IsAttacking to true to trigger the attack animation
//                     animator.SetBool("IsAttacking", true);

//                     // Perform a normal attack on the player
//                     // NormalAttack(player.GetComponent<Unit>());
//                     Invoke("PlayHitSound", 1.0f);

//                     // Set the cooldown timer
//                     canAttack = false;
//                     timeSinceLastAttack = 0f;
//                 }
//             }




//             else
//             {
                
//                 if (Time.time - timeSinceLastWander >= wanderCooldown)
//                 {
//                     randomDirection = UnityEngine.Random.insideUnitSphere * 10.0f;
//                     timeSinceLastWander = Time.time; // Reset the timer
//                 }
//                 else
//                 {
//                     Wander(randomDirection);
//                 }
//                 // animator.SetBool("IsRunning", false);
//                 animator.SetBool("IsAttacking", false); // Ensure IsAttacking is set to false when not attacking
//             }



            

//             // Update the cooldown timer
//             if (!canAttack)
//             {
//                 timeSinceLastAttack += Time.deltaTime;

//                 // Check if the cooldown period has elapsed
//                 if (timeSinceLastAttack >= attackCooldown)
//                 {
//                     canAttack = true;
//                     animator.SetBool("IsAttacking", false);
//                     rb.MoveRotation(Quaternion.Euler(rb.rotation.eulerAngles + new Vector3(0f,-45f,0f)));
//                 }
//             }
//         }
//     }


//     void MoveTowardsPlayer()
//     {
//         // Calculate the direction to the player
//         animator.SetBool("IsRunning", true);
        
//         Vector3 directionToPlayer = player.position - transform.position;

//         // Normalize the direction to get a unit vector
//         directionToPlayer.Normalize();

//         // Calculate the target position to move towards the player
//         Vector3 targetPosition = transform.position + directionToPlayer * currentSpeed * Time.deltaTime;

//         // Move the Monster towards the player
//         rb.MovePosition(targetPosition);

//         // Rotate the Monster to face the player
//         Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
//         rb.MoveRotation(targetRotation);
//     }

//     protected virtual void Wander(Vector3 randomDirection)
//     {
//         animator.SetBool("IsRunning", true);
//         randomDirection.y = 0; // Keep the direction in the horizontal plane

//         // Calculate the target position to move towards in the random direction
//         Vector3 targetPosition = transform.position + randomDirection;

//         // Move the Monster towards the random direction
//         rb.MovePosition(Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime));

//         // Rotate the Monster to face the random direction
//         Quaternion targetRotation = Quaternion.LookRotation(randomDirection);
//         rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, 180.0f * Time.deltaTime));
//     }


//     void PlayHitSound()
//     {
//         if (player.GetComponent<Unit>().currentHitPoints > 0)
//         {
//             NormalAttack(player.GetComponent<Unit>());
//             audioSource.Play();
//         }
//     }

//     public override void AttackPointScaling()
//     {
//         maxAttackPoints = baseAttackPoints * level;
//     }
//     public override void HitPointScaling()
//     {
//         maxHitPoints = baseHitPoints * level;
//     }
//     public override void DefenseScaling()
//     {
//         maxDefense = baseDefense * level;
//     }
//     public override void SpeedScaling()
//     {
//         maxSpeed = baseSpeed * level;
//     }

// }

// public class Hero : Unit
// {
//     public int coins = 0;
//     public Equipment[] equippedItems = new Equipment[4];
//     private bool isEquipped = false;

//     public void EquipItem(GameObject item)
//     {
//         if (isEquipped)
//         {
//         }
//         else
//         {
//             // Implement equip logic for heroes
//             GameObject weapon1 = Instantiate(item, weapon.transform.position, Quaternion.identity);
//             // weapon1.parent = weapon;
//             weapon1.transform.SetParent(weapon.transform);
//             weapon1.transform.rotation = weapon.transform.rotation;
//             isEquipped = true;
//         }
//     }

//     public void UnequipItem(string category)
//     {
//         // Implement unequip logic for heroes
//     }

//     public override void HitPointScaling()
//     {
//         // Implement scaling logic for heroes
//     }

//     public override void AttackPointScaling()
//     {
//         // Implement scaling logic for heroes
//     }

//     public override void DefenseScaling()
//     {
//         // Implement scaling logic for heroes
//     }

//     public override void SpeedScaling()
//     {
//         // Implement scaling logic for heroes
//     }

//     public override string ToString()
//     {
//         return $"HP: {currentHitPoints}\nATK: {currentAttackPoints}\nDEF: {currentDefense}\nSPD: {currentSpeed}\nLVL: {level}";
//     }


// }

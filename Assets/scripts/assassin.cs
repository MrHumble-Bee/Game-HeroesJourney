using UnityEngine;

public class Assassin : Hero
{
    public Animator animator;
    private AudioSource audioSource;
    // public bool is_moving;
    public Rigidbody rb;

    void Start()
    {
        // Assuming you have an Animator component attached to the GameObject
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None; // Adjust as needed
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        // Set the animation to loop
        animator.SetBool("IsRunning", false);
        animator.SetBool("IsJumping", false);

    }


    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Jumping");
            animator.SetBool("IsJumping", true);
            // Add a vertical force to simulate jumping
            rb.AddForce(Vector3.up * currentSpeed * 100);
        }
        // Handle movement
        HandleMovement();

        // Handle attack
        HandleAttack();
    }

    void HandleMovement()
    {
        // Get input values for horizontal and vertical movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement direction
        Vector3 movement = new Vector3(-horizontalInput, 0f, -verticalInput);
        
        // Check if there's any movement
        if (movement.magnitude > 0f)
        {
            // Set the rotation to face the movement direction
            transform.LookAt(transform.position + movement.normalized);

            // Use Rigidbody for physics-based movement
            // Rigidbody rb = GetComponent<Rigidbody>();
            
            // Calculate the target position based on the movement input
            Vector3 targetPosition = transform.position + movement * currentSpeed * Time.deltaTime;
            
            // Move the Assassin using Rigidbody's MovePosition
            rb.MovePosition(targetPosition);

            // Set the running animation parameter
            animator.SetBool("IsRunning", true);

            PlayRunningSound(true);
        }
        else
        {
            // Set the running animation parameter to false when there's no movement
            animator.SetBool("IsRunning", false);
            // animator.SetBool("IsJumping", false);

            PlayRunningSound(false);
        }
        // Check if the space bar is pressed

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collider is tagged as "Ground"
        if (collision.collider.CompareTag("ground"))
        {
            // Set the "IsJumping" parameter to false in the Animator
            animator.SetBool("IsJumping", false);
        }
    }

    void HandleAttack()
    {
        // Check for attack input (space key)
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // Perform the attack
            PerformAttack();
        }
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            LevelUp();
            Debug.Log(ToString());
        }
    }

    void PerformAttack()
    {
        // Log the attack as a print statement
        Debug.Log("Assassin performed an attack!");
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

    public void PlayRunningSound(bool shouldPlay)
    {
        // float originalPitch = audioSource.pitch;
        // audioSource.pitch = originalPitch * slowdownFactor;
        // Check if the AudioSource and AudioClip are set
        if (audioSource != null && audioSource.clip != null)
        {

            if (shouldPlay && !audioSource.isPlaying)
            {
                // Play the sound if it's not already playing
                audioSource.Play();
            }
            else if (!shouldPlay && audioSource.isPlaying)
            {
                // Stop the sound if it's playing and should not be playing
                audioSource.Stop();
            }
        }
    }
}

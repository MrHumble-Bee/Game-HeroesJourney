using UnityEngine;
public class Assassin : Hero
{
    public Animator animator;
    public AudioSource audioSource;
    public AudioSource swordSwingSource;
    // public bool is_moving;
    public Rigidbody rb;

    public int attackRadius = 3;

    private bool isClimbing = false;

    public GameObject sword;

    protected override void Start()
    {
        base.Start();
        // Assuming you have an Animator component attached to the GameObject
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None; // Adjust as needed
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        // Set the animation to loop
        animator.SetBool("IsRunning", false);

    }

    // protected override void Update()
    protected void Update()
    {
        // base.Update();

        if (Input.GetKey(KeyCode.C) && isClimbing)
        {
            Debug.Log("C and climbing");
            // Move the character up on the y-axis
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 climbDirection = new Vector3(0f, currentSpeed, 0f).normalized;
            Vector3 climbVelocity = climbDirection * currentSpeed / 2 * Time.deltaTime;
            transform.Translate(climbVelocity);
            animator.SetBool("IsClimbing", true);

            // Add any additional climbing logic here, such as playing climbing animation
        }


        if (Input.GetKeyDown(KeyCode.X) && isJumping == false)
        {
            isJumping = true;
            Debug.Log("Jumping");
            animator.SetTrigger("PerformJump");
            // Add a vertical force to simulate jumping
            rb.AddForce(Vector3.up * currentSpeed * 65);
        }
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            EquipItem(sword);
        }

        // Handle attack
        HandleAttack();
    }


    void FixedUpdate()
    {
        // Handle movement
        HandleMovement();

    }
    void HandleClimbing()
    {
        // Implement climbing logic here
        Debug.Log("Climbing!");
        // Add code to move the player up the ladder, play climbing animation, etc.
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

            PlayRunningSound(false);
        }
        // Check if the space bar is pressed

    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collider is tagged as "Ground"
        if (collision.collider.CompareTag("ground"))
        {
            // Can jump logic
            isJumping = false;
        }
    }

    void HandleAttack()
    {
        // Check for attack input (space key)
        if (Input.GetKeyDown(KeyCode.Z) && isAttacking == false)
        {
            // Perform the attack
            isAttacking = true;
            animator.SetTrigger("PerformAttack");
            PlaySwordSwingSound();

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius);
            foreach (Collider col in hitColliders)
            {
                if (col.CompareTag("monster"))
                {
                    // Perform NormalAttack on the nearest 'monster'
                    NormalAttack(col.GetComponent<Unit>());
                    if (col.GetComponent<Unit>().currentHitPoints <= 0)
                    {
                        currentExperience = currentExperience + (col.GetComponent<Unit>().level * 10);
                    }
                    break;
                }
            }

            Invoke("ResetAttack", 0.5f);

        }
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            LevelUp();
            Debug.Log(ToString());
        }
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

    public void PlaySwordSwingSound()
    {
        swordSwingSource.Play();
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player is in contact with a climbable object
        if (other.CompareTag("climbable"))
        {
            Debug.Log("climbing");
            isClimbing = true;

            // Disable gravity while climbing
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player is no longer in contact with a climbable object
        if (other.CompareTag("climbable"))
        {
            Debug.Log("unclimbing");
            isClimbing = false;
            animator.SetBool("IsClimbing", false);
            // Re-enable gravity when not climbing
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.useGravity = true;
        }
    }
}

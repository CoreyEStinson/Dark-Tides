using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // Public variables for configuring the enemy behavior
    public NavMeshAgent navAgent; // Navigation agent for pathfinding
    public Transform player; // Reference to the player
    public LayerMask groundLayer, playerLayer; // Layers for ground and player detection
    public float health; // Enemy health
    public float walkPointRange; // Range for random walk points
    public float timeBetweenAttacks; // Time interval between attacks
    public float sightRange; // Range within which the enemy can see the player
    public float attackRange; // Range within which the enemy can attack the player
    public int damage; // Damage dealt to the player
    public Animator animator; // Animator for controlling animations
    public ParticleSystem hitEffect; // Particle effect for when the enemy takes damage

    // Private variables for internal state management
    private Vector3 walkPoint; // Current walk point
    private bool walkPointSet; // Whether a walk point is set
    private bool alreadyAttacked; // Whether the enemy has already attacked
    private bool takeDamage; // Whether the enemy is taking damage

    // Called when the script instance is being loaded
    private void Awake()
    {   
        //animator = GetComponent<Animator>(); // Get the Animator component
        player = GameObject.Find("XR Origin (XR Rig)").transform; // Find the player object
        //navAgent = GetComponent<NavMeshAgent>(); // Get the NavMeshAgent component
    }

    // Called once per frame
    private void Update()
    {
        // Check if the player is within sight or attack range
        bool playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
        bool playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);

        // Determine the enemy's behavior based on the player's position
        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        else if (playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        else if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }
        else if (!playerInSightRange && takeDamage)
        {
            ChasePlayer();
        }
    }

    // Enemy patrolling behavior
    private void Patroling()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if (walkPointSet)
        {
            navAgent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        animator.SetFloat("Velocity", 0.2f); // Set walking animation

        // Reset walk point if the enemy has reached it
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    // Search for a random walk point within the specified range
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Check if the walk point is on the ground
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
        {
            walkPointSet = true;
        }
    }

    // Enemy chasing the player behavior
    private void ChasePlayer()
    {
        navAgent.SetDestination(player.position); // Set destination to the player's position
        animator.SetFloat("Velocity", 0.6f); // Set running animation
        navAgent.isStopped = false; // Ensure the nav agent is not stopped
    }

    // Enemy attacking the player behavior
    private void AttackPlayer()
    {
        navAgent.SetDestination(transform.position); // Stop moving

        if (!alreadyAttacked)
        {
            transform.LookAt(player.position); // Face the player
            alreadyAttacked = true;
            animator.SetBool("Attack", true); // Trigger attack animation
            Invoke(nameof(ResetAttack), timeBetweenAttacks); // Reset attack after a delay

            // Perform a raycast to check if the player is hit
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
            {
                /*
                    YOU CAN USE THIS TO GET THE PLAYER HUD AND CALL THE TAKE DAMAGE FUNCTION

                PlayerHUD playerHUD = hit.transform.GetComponent<PlayerHUD>();
                if (playerHUD != null)
                {
                   playerHUD.takeDamage(damage);
                }
                 */
            }
        }
    }

    // Reset the attack state
    private void ResetAttack()
    {
        alreadyAttacked = false;
        animator.SetBool("Attack", false); // Reset attack animation
    }

    // Method to handle taking damage
    public void TakeDamage(float damage)
    {
        health -= damage; // Reduce health
        hitEffect.Play(); // Play hit effect
        StartCoroutine(TakeDamageCoroutine()); // Start damage coroutine

        // Destroy the enemy if health is depleted
        if (health <= 0)
        {
            Invoke(nameof(DestroyEnemy), 0.5f);
        }
    }

    // Coroutine to handle taking damage state
    private IEnumerator TakeDamageCoroutine()
    {
        takeDamage = true;
        yield return new WaitForSeconds(2f); // Wait for 2 seconds
        takeDamage = false;
    }

    // Method to destroy the enemy
    private void DestroyEnemy()
    {
        StartCoroutine(DestroyEnemyCoroutine());
    }

    // Coroutine to handle enemy destruction
    private IEnumerator DestroyEnemyCoroutine()
    {
        animator.SetBool("Dead", true); // Trigger death animation
        yield return new WaitForSeconds(1.8f); // Wait for animation to finish
        Destroy(gameObject); // Destroy the enemy object
    }

    // Draw gizmos in the editor for debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Draw attack range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange); // Draw sight range
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody enemyRigidbody;
    private Animator enemyAnimator;
    private bool isDead = false;

    private float initialYPosition; // Store the initial Y position to keep it grounded
    public float walkSpeed = 1.5f;
    public float runSpeed = 3.5f;
    public float rotationSpeed = 2.0f; // Speed for rotating towards the player
    private float stopDistance = 5f; // Distance to stop before reaching the player

    private Transform playerTransform; // Reference to the player's position

    // Start is called before the first frame update
    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>(); // Get Rigidbody component
        enemyAnimator = GetComponent<Animator>(); // Get Animator component

        // Find the player's transform (assuming the player is tagged "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player is tagged 'Player'.");
        }

        // Ensure the Rigidbody is kinematic for manual movement control
        enemyRigidbody.isKinematic = true;
        initialYPosition = transform.position.y; // Store initial Y position
    }

    void FixedUpdate()
    {
        if (playerTransform != null && !isDead)
        {
            // Continuously run towards the player
            //runTowardsPlayer(playerTransform.position);
            turnTowardsPlayer(playerTransform.position);
        }
    }

    public void showDeath()
    {
        isDead = true;
        enemyAnimator.SetBool("isDead", true);
    }

    public void StartShooting()
    {
        enemyAnimator.SetBool("isRunning", false);
        enemyAnimator.SetBool("isShooting", true);
        enemyAnimator.SetBool("isIdle", false);
    }

    public void runTowardsPlayer(Vector3 playerPos)
    {
        if (isDead) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerPos);

        // Stop if the enemy is within the stopping range

        Debug.Log("Distance to Player: " + distanceToPlayer);

        if (distanceToPlayer <= stopDistance)
        {
            // Fix the position of the enemy to prevent it from moving closer to the player
            // Stop the enemy a certain distance away from the player
            Vector3 directionToPlayer = (playerPos - transform.position).normalized;
            Vector3 stopPosition = playerPos - directionToPlayer * stopDistance;

            // Set the enemy's position
            transform.position = new Vector3(stopPosition.x, initialYPosition, stopPosition.z);

            StartShooting();
        }
        else
        {
            // Calculate direction towards the player
            Vector3 directionToPlayer = (playerPos - transform.position).normalized;
            directionToPlayer.y = 0; // Keep the y-axis unchanged to avoid tilting

            // Smoothly rotate towards the player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Move towards the player while grounded
            Vector3 movePosition = transform.position + directionToPlayer * runSpeed * Time.deltaTime;
            transform.position = new Vector3(movePosition.x, initialYPosition, movePosition.z);

            // Trigger running animation

            enemyAnimator.SetBool("isRunning", true);

        }
    }

    public void turnTowardsPlayer(Vector3 playerPos)
    {
        // Calculate direction towards the player
        Vector3 directionToPlayer = (playerPos - transform.position).normalized;
        directionToPlayer.y = 0; // Keep the y-axis unchanged to avoid tilting

        // Smoothly rotate towards the player
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        enemyAnimator.SetBool("isShooting", true);
    }
}

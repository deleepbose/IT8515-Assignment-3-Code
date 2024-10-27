using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionStateManager : MonoBehaviour
{
    [SerializeField] float lookDistance = 30, fov = 120;
    [SerializeField] Transform enemyEyes;
    Transform playerHead;
    GameManager gameManager;
    EnemyMovement enemyMovement;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        enemyMovement = FindObjectOfType<EnemyMovement>();
        playerHead = gameManager.playerHead;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (PlayerSeen())
        {
            //Debug.Log("Player Seen");
            //enemyMovement.runTowardsPlayer(playerHead.position);
            enemyMovement.turnTowardsPlayer(playerHead.position);
        }
        else
        {
            //Debug.Log("Player Not Seen");
        }
    }

    public bool PlayerSeen()
    {
        // Calculate the distance from the enemy to the player
        if (Vector3.Distance(enemyEyes.position, playerHead.position) > lookDistance)
            return false;

        // Calculate the angle between the enemy's forward vector and the vector to the player
        Vector3 dirToPlayer = (playerHead.position - enemyEyes.position).normalized;

        float angleToPlayer = Vector3.Angle(enemyEyes.parent.forward, dirToPlayer);

        if (angleToPlayer > (fov / 2)) return false;

        //Debug.Log("Angle to Player: " + angleToPlayer);

        enemyEyes.LookAt(playerHead);

        RaycastHit hit;

        if (Physics.Raycast(enemyEyes.position, enemyEyes.forward, out hit, lookDistance))
        {
            //Debug.Log("Should look at " + playerHead.root.name);

            if (hit.transform == null) return false;

            if (hit.transform.name == playerHead.name)
            {
                Debug.DrawLine(enemyEyes.position, hit.point, Color.green);
                return true;
            }
        }

        return false;
    }
}

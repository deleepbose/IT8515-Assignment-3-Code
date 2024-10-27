using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    //RagdollManager ragdollManager;
    EnemyMovement enemyMovement;

    [HideInInspector] public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        enemyMovement = GetComponent<EnemyMovement>();
    }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;

            //Debug.Log("Enemy Health: " + health);

            if (health <= 0)
            {
                EnemyDeath();
            }
            else
            {
                //Debug.Log("Enemy Damaged");
            }
        }
    }

    public void EnemyDeath()
    {
        //ragdollManager.TriggerRagdoll();
        enemyMovement.showDeath();
        //Debug.Log("Enemy is dead");
    }
}

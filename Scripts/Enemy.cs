using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	[Header("Enemy Health and Damage")]
	private float enemyHealth = 120f;
	private float presentHealth;
	public float giveDamage = 5f;

	[Header("Enemy Things")]
	public NavMeshAgent enemy;
	//public Transform LookPoint;
	//public Camera ShootingRaycastArea;
	public Transform playerBody;
	public LayerMask playerLayer;

	[Header("Enemy Guarding Var")]
	public GameObject[] walkPoints;
	int currentEnemyPosition = 0;
	public float enemySpeed;
	float walkingPointRadius = 2;

	[Header("Enemy mood/situation")]
	public float visionRadius;
	public float shootingRadius;
	public bool playerInvisionRadius;
	public bool playerInshootingRadius;

	private void Awake()
	{
		presentHealth = enemyHealth;
		playerBody = GameObject.Find("Player").transform;
		enemy = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		playerInvisionRadius = Physics.CheckSphere(transform.position, visionRadius, playerLayer);
		playerInshootingRadius = Physics.CheckSphere(transform.position, shootingRadius, playerLayer);

		if (!playerInvisionRadius && !playerInshootingRadius)
		{
			Guard();
		}

		//if (playerInvisionRadius && !playerInshootingRadius)
		//{
		//	PursuePlayer();
		//}

		//if (playerInvisionRadius && playerInshootingRadius)
		//{
		//	ShootPlayer();
		//}

	}

	private void Guard()
	{
		if (Vector3.Distance(walkPoints[currentEnemyPosition].transform.position, transform.position) < walkingPointRadius)
		{
			currentEnemyPosition = Random.Range(0, walkPoints.Length);
			if (currentEnemyPosition >= walkPoints.Length)
			{
				currentEnemyPosition = 0;
			}
		}

		transform.position = Vector3.MoveTowards(transform.position, walkPoints[currentEnemyPosition].transform.position, Time.deltaTime * enemySpeed);
		transform.LookAt(walkPoints[currentEnemyPosition].transform.position);
	}

}

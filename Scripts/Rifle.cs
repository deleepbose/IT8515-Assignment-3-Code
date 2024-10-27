using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : MonoBehaviour
{
	[Header("Rifle Things")]
	public Camera Camera;
	public float giveDamageOf = 10f;
	public float shootingRange = 100f;
	public float fireCharge = 15f;
	public Animator animator;
	public PlayerScript player;

	// Update is called once per frame
	[Header("Rifle Ammunition and Shooting")]
	private int maximumAmmunition = 20;
	private int mag = 15;
	private int presentAmmunition;
	public float reloadingTime = 1.3f;
	private bool setReloading = false;
	private float nextTimeShoot = 0f;
	[Header("Rifle Effects")]
	public ParticleSystem muzzleSpark;
	public GameObject impactEffect;

	//[Header("Sounds and UI")]
	private void Awake()
	{
		presentAmmunition = maximumAmmunition;
	}

	// Update is called once per frame
	void Update()
	{
		if (setReloading)
			return;

		if (presentAmmunition <= 0)
		{
			StartCoroutine(Reload());
			return;
		}
		if (Input.GetButton("Fire1") && Time.time >= nextTimeShoot)
		{
			animator.SetBool("Fire", true);
			animator.SetBool("Idle", false);
			nextTimeShoot = Time.time + 1f / fireCharge;
			Shoot();
		}
		else if (Input.GetButton("Fire1") && Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
		{
			animator.SetBool("Idle", false);
			animator.SetBool("IdleAim", true);
			animator.SetBool("FireWalk", true);
			animator.SetBool("Walk", true);
			animator.SetBool("Reloading", false);
		}
		else if (Input.GetButton("Fire2") && Input.GetButton("Fire1"))
		{
			animator.SetBool("Idle", false);
			animator.SetBool("IdleAim", true);
			animator.SetBool("FireWalk", true);
			animator.SetBool("Walk", true);
			animator.SetBool("Reloading", false);
		}
		else
		{
			animator.SetBool("Fire", false);
			animator.SetBool("Idle", true);
			animator.SetBool("FireWalk", false);
			animator.SetBool("Reloading", false);
		}
	}

	void Shoot()
	{
		if (mag == 0)
		{
			// UI Text
			return;
		}

		presentAmmunition--;

		if (presentAmmunition == 0)
		{
			mag--;
		}
		muzzleSpark.Play();
		//muzzleSpark.SetActive(true);
		RaycastHit hitInfo;

		if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out hitInfo, shootingRange))
		{
			//Debug.Log(hitInfo.transform.name);
			//Debug.Log("Shot at " + hitInfo.transform.root.name);
			Objects objects = hitInfo.transform.GetComponent<Objects>();
			//Enemy enemy = hitInfo.transform.GetComponent<Enemy>();

			//Check if hitInfo.transform has tag "Enemy"

			if (hitInfo.transform.root.tag == "Enemy")
			{
				// Get the root object (BossEnemy)
				GameObject bossEnemy = hitInfo.transform.root.gameObject;

				// Get the EnemyHealth component
				EnemyHealth enemyHealth = bossEnemy.GetComponent<EnemyHealth>();
				if (enemyHealth != null)
				{
					// Invoke TakeDamage method or other methods as needed
					enemyHealth.TakeDamage((int)giveDamageOf);
				}



				//Debug.Log("Boss Enemy Hit");
			}

			if (objects != null)
			{
				//Check

				objects.objectHitDamage(giveDamageOf);
				GameObject impactGO = Instantiate(impactEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
				Destroy(impactGO, 5f);
			}
			//else if (enemy != null)
			//{
			//	Debug.Log(enemy.name);
			//	enemy.enemyHitDamage(giveDamageOf);
			//}
		}
	}

	IEnumerator Reload()
	{
		player.playerSpeed = 0f;
		player.sprint = 0f;
		setReloading = true;
		Debug.Log("Reloading...");
		animator.SetBool("Reloading", true);
		yield return new WaitForSeconds(reloadingTime);
		animator.SetBool("Reloading", false);
		presentAmmunition = maximumAmmunition;
		player.playerSpeed = 1.9f;
		player.sprint = 3f;
		setReloading = false;
	}
}

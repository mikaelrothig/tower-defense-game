using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyBasic : MonoBehaviour , IEnemy {

	private Transform target;
	private NavMeshAgent agent;

	public float enemyHealth = 3f;
	private float startHealth;

	public float range = 10f;
	public float fireRate = 1f;
	private float fireCountdown = 0f;

	public string enemyTag = "Turret";
	private Transform turretTarget;
	public GameObject bullet;

	public GameObject deathEffect;

	public Image healthBar;

	private Transform closestTurret;
	
	bool attack = false;

	public void Start () {

		agent = GetComponent<NavMeshAgent>();
		target = GameObject.FindGameObjectWithTag("Finish").transform;
		startHealth = enemyHealth;

		InvokeRepeating("UpdateTarget", 0f, 0.5f);	
		InvokeRepeating("ClosestTurret", 0f, 0.5f);
	}
	
	public void Update () {
		
		if(!attack)
		{
			float distance = Vector3.Distance(transform.position, target.position);

			if(closestTurret.position != null)
			{
				agent.destination = closestTurret.position;
			}
				
			
			/*if(distance < 1)
			{
				Destroy(gameObject);
				PlayerStats.lives--;
				WaveSpawner.EnemiesAlive--;
			}*/
		}
		else
		{
			agent.destination = transform.position;
			
			if(turretTarget == null)
			return;
		
			if(fireCountdown <= 0)
			{
				Shoot();
				fireCountdown = 1f / fireRate;
			}

			fireCountdown -= Time.deltaTime;
		}
	}

	public void ClosestTurret()
	{
		GameObject[] turrets = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestTurret = null;

		foreach (GameObject turret in turrets)
		{
			float distanceToTurret = Vector3.Distance(transform.position, turret.transform.position);

			if(distanceToTurret < shortestDistance)
			{
				shortestDistance = distanceToTurret;
				nearestTurret = turret;
			}
		}

		if(nearestTurret != null)
		{
			closestTurret = nearestTurret.transform;
		}
		else
		{
			closestTurret = null;
		}	
	}

	public void TakeDamage(float bulletDamage)
	{
		enemyHealth -= bulletDamage;
		healthBar.fillAmount  = enemyHealth / startHealth;

		if(enemyHealth  <= 0)
		{
			Death();
		}
	}

	public void Death()
	{
		GameObject deathEff = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(deathEff, 3f);
		Destroy(gameObject);

		PlayerStats.money += 40;
		WaveSpawner.EnemiesAlive--;
	}


	public void Shoot()
	{
		GameObject bulletFired = Instantiate(bullet, new Vector3(transform.position.x, transform.position.y+0.6f, transform.position.z), transform.rotation);
		EnemyBullet bulletScript = bulletFired.GetComponent<EnemyBullet>();
		
		if(bulletScript != null)
		{
			bulletScript.Chase(turretTarget);
		}
	}

	public void UpdateTarget()
	{
		GameObject[] turrets = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestTurret = null;

		foreach (GameObject turret in turrets)
		{
			float distanceToTurret = Vector3.Distance(transform.position, turret.transform.position);

			if(distanceToTurret < shortestDistance)
			{
				shortestDistance = distanceToTurret;
				nearestTurret = turret;
			}
		}

		if(nearestTurret != null && shortestDistance <= range)
		{
			turretTarget = nearestTurret.transform;
			attack = true;
		}
		else
		{
			turretTarget = null;
			attack = false;
		}
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}

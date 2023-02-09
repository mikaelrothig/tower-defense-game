using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyCommander : MonoBehaviour, IEnemy {
	
	private Transform target;
	private Transform allie;
	private NavMeshAgent agent;

	public float enemyHealth = 3f;
	private float startHealth;

	public float range = 10f;
	public float fireRate = 1f;
	private float fireCountdown = 0f;

	public float allieSearchRadius;
	public string allieTag = "Enemy";
	private Transform allieTarget;

	public string enemyTag = "Turret";
	private Transform turretTarget;
	public GameObject bullet;

	public GameObject deathEffect;

	public Image healthBar;
	
	bool attack = false;

	public Transform closestTurret;
	public float retreatRange;

	Vector3 startPos;

	private Animator anim;

	public void Start () {

		startPos = transform.position;

		agent = GetComponent<NavMeshAgent>();
		target = GameObject.FindGameObjectWithTag("Finish").transform;
		anim = gameObject.GetComponent<Animator>();
		allie = null;
		startHealth = enemyHealth;

		InvokeRepeating("UpdateTarget", 0f, 0.5f);
		InvokeRepeating("UpdateAllieTarget", 0f, 0.5f);
		InvokeRepeating("ClosestTurret", 0f, 0.5f);
	}
	
	public void Update () {
		
		if(allieTarget != null)
		{
			agent.destination = allieTarget.position;
			anim.enabled = true;
		}
		else
		{
			if (Vector3.Distance(this.transform.position, closestTurret.transform.position) <= retreatRange)
			{
				agent.destination = startPos;
				transform.LookAt(startPos);
				anim.enabled = true;
			}
			else if (Vector3.Distance(this.transform.position, closestTurret.transform.position) > retreatRange && Vector3.Distance(this.transform.position, closestTurret.transform.position) < retreatRange+2)
			{
				agent.destination = transform.position;
				transform.LookAt(target.position);
				anim.enabled = false;
			}
			else if (Vector3.Distance(this.transform.position, closestTurret.transform.position) > retreatRange+2)
			{
				agent.destination = closestTurret.transform.position;
				transform.LookAt(closestTurret.transform.position);
				anim.enabled = true;
			}
		}	

		if(!attack)
		{
			float distance = Vector3.Distance(transform.position, target.position);
			
			/*if(distance < 1)
			{
				Destroy(gameObject);
				PlayerStats.lives--;
				WaveSpawner.EnemiesAlive--;
			}*/
		}
		else
		{
			transform.LookAt(turretTarget.position);

			if(turretTarget == null)
			return;
		
			if(fireCountdown <= 0)
			{
				Shoot();
				fireCountdown = 1f / fireRate;
			}

			fireCountdown -= Time.deltaTime;
		}

		if(WaveSpawner.EnemiesAlive == 1)
		{
			agent.destination = target.position;
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
		AudioManager.explosion.Play();
		GameObject deathEff = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(deathEff, 3f);
		Destroy(gameObject);

		PlayerStats.money += 40;
		WaveSpawner.EnemiesAlive--;
	}

	public void Shoot()
	{
		AudioManager.enemyCommanderFire.Play();
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

	public void UpdateAllieTarget()
	{
		GameObject[] allies = GameObject.FindGameObjectsWithTag(allieTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestAllie = null;

		foreach (GameObject allie in allies)
		{
			float distanceToAllie = Vector3.Distance(transform.position, allie.transform.position);

			if(distanceToAllie < shortestDistance && allie.transform.position != this.transform.position)
			{
				shortestDistance = distanceToAllie;
				nearestAllie = allie;
			}
		}

		if(nearestAllie != null && shortestDistance <= allieSearchRadius)
		{
			allieTarget = nearestAllie.transform;
		}
		else
		{
			allieTarget = null;
		}
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}

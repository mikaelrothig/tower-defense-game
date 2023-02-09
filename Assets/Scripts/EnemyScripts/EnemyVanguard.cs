using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyVanguard : MonoBehaviour, IEnemy {
	private Transform target;
	private NavMeshAgent agent;

	public float enemyHealth = 3f;
	private float startHealth;

	public float range = 10f;
	public float fireRate = 1f;
	private float fireCountdown = 0f;

	public float manaPoints;
	private float manaPointsTotal;
	public float manaRate;
	private float chargePortal = 5f;
	public GameObject portalChargeEffect;

	public string enemyTag = "Turret";
	private Transform turretTarget;
	public GameObject bullet;

	public GameObject deathEffect;

	public Image healthBar;

	private Transform closestTurret;
	
	private bool attack = false;
	private bool canMove = true;

	public Transform[] portals;
	public float spawnRandomiser;

	private Animator anim;

	public void Start () {

		agent = GetComponent<NavMeshAgent>();
		target = GameObject.FindGameObjectWithTag("Finish").transform;
		anim = gameObject.GetComponent<Animator>();
		startHealth = enemyHealth;

		manaPointsTotal = manaPoints;
		manaPoints = 0;

		InvokeRepeating("UpdateTarget", 0f, 0.5f);	
		InvokeRepeating("ClosestTurret", 0f, 0.5f);	
	}
	
	public void Update () {

		//transform.LookAt(closestTurret.position);

		if(manaPoints < manaPointsTotal)
		{
			manaPoints = manaPoints + Time.deltaTime;
		}

		if(manaPoints < manaPointsTotal && manaPoints > manaPointsTotal - 2)
			AudioManager.enemyVanguardPowerup.Play();	

		if(manaPoints > manaPointsTotal)
		{
			canMove = false;

			chargePortal = chargePortal - Time.deltaTime;
			print("Charging Portal");
			anim.enabled = false;
			portalChargeEffect.SetActive(true);				

			if (chargePortal <= 0)
			{
				print("Use Portal");
				anim.enabled = true;
				portalChargeEffect.SetActive(false);
				Teleport();
			}
		}

		if(!attack && canMove)
		{
			float distance = Vector3.Distance(transform.position, target.position);

			agent.destination = closestTurret.position;
			
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

	void Teleport()
	{
		AudioManager.enemyVanguardPowerup.Stop();
		int randomPortal = Random.Range(0,4);

		if(portals[randomPortal].position != ClosestPortal().position)
		{
			AudioManager.enemyVanguardTeleport.Play();
			Vector3 newPosition = new Vector3(portals[randomPortal].position.x + Random.Range(-spawnRandomiser,spawnRandomiser), portals[randomPortal].position.y, portals[randomPortal].position.z + Random.Range(-spawnRandomiser,spawnRandomiser));
			transform.position = newPosition;
			manaPoints = 0;
			chargePortal = 5f;
			canMove = true;
		}
		else
		{
			Teleport();
		}
	}

	Transform ClosestPortal()
	{
		float shortestDistance = Mathf.Infinity;
		Transform nearestPortal = null;

		foreach (Transform portal in portals)
		{
			float distanceToPortal = Vector3.Distance(transform.position, portal.position);

			if(distanceToPortal < shortestDistance)
			{
				shortestDistance = distanceToPortal;
				nearestPortal = portal;
			}
		}

		if(nearestPortal != null)
		{
			return nearestPortal.transform;
		}
		else
		{
			return null;
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
		AudioManager.enemyVanguardPowerup.Stop();
		AudioManager.explosion.Play();
		GameObject deathEff = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(deathEff, 3f);
		Destroy(gameObject);

		PlayerStats.money += 40;
		WaveSpawner.EnemiesAlive--;
	}

	public void Shoot()
	{
		AudioManager.enemyVanguardFire.Play();
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

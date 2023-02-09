using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyWizard : MonoBehaviour , IEnemy {

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
	
	bool attack = false;

	private Animator anim;

	public void Start () {

		agent = GetComponent<NavMeshAgent>();
		target = GameObject.FindGameObjectWithTag("Finish").transform;
		anim = gameObject.GetComponent<Animator>();
		startHealth = enemyHealth;

		InvokeRepeating("UpdateTarget", 0f, 0.5f);		
	}
	
	public void Update () 
	{
		if(!attack)
		{
			float distance = Vector3.Distance(transform.position, target.position);
			agent.destination = target.position;
			transform.LookAt(target.position);
			anim.enabled = true;
		}
		else
		{
			if(turretTarget == null)
			return;

			agent.destination = transform.position;
			transform.LookAt(turretTarget.position);
			anim.enabled = false;
		
			if(fireCountdown <= 0)
			{
				Shoot();
				fireCountdown = 1f / fireRate;
			}

			fireCountdown -= Time.deltaTime;
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
		AudioManager.enemyWizardFire.Play();
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

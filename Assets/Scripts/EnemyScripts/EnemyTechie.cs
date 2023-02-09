using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyTechie : MonoBehaviour, IEnemy {

	//private Transform target;
	private NavMeshAgent agent;

	public float enemyHealth = 3f;
	private float startHealth;

	public float startSpeed;
	public float sprintSpeed;

	public float sprintRange;
	public float detonationRange;
	public float explosionRadius;
	public float damage;

	public string enemyTag = "Turret";
	private Transform turretTarget;
	public GameObject deathEffect;
	public Image healthBar;
	
	bool attack = false;

	float distance;

	private Animator anim;

	public void Start () {

		agent = GetComponent<NavMeshAgent>();
		turretTarget = GameObject.FindGameObjectWithTag("Finish").transform;
		anim = gameObject.GetComponent<Animator>();
		startHealth = enemyHealth;

		InvokeRepeating("UpdateTarget", 0f, 0.5f);	
	}
	
	public void Update () {

		transform.LookAt(turretTarget.position);

		if(turretTarget != null)
		{
			distance = Vector3.Distance(transform.position, turretTarget.position);
		}
		else
		{
			turretTarget = GameObject.FindGameObjectWithTag("Finish").transform;
		}

		if(attack)
		{
			if(turretTarget != null)
			{
				agent.destination = turretTarget.position;
				agent.speed = sprintSpeed;
				anim.speed = 2f;
			}
			else
			{
				anim.speed = 1f;
				attack = false;
				return;
			}
		} 
		else if(!attack)
		{
			agent.destination = turretTarget.position;
			agent.speed = startSpeed;
			
			/*if(distance < detonationRange)
			{
				Death();
				PlayerStats.lives--;
			}*/
		}

		if(distance < detonationRange || enemyHealth <= 0)
		{
			Death();
		}
	}

	public void TakeDamage(float bulletDamage)
	{
		enemyHealth -= bulletDamage;
		healthBar.fillAmount  = enemyHealth / startHealth;
	}

	public void Death()
	{
		AudioManager.explosion.Play();
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

		foreach(Collider collider in colliders)
		{
			if(collider.tag == "Enemy" || collider.tag == "Turret")
			{
				collider.enabled = false;
				Damage(collider.transform);
				collider.enabled = true;
			}
		}

		GameObject deathEff = Instantiate(deathEffect, transform.position, Quaternion.identity);
		Destroy(deathEff, 3f);
		Destroy(gameObject);

		PlayerStats.money += 40;
		WaveSpawner.EnemiesAlive--;
	}

	public void Damage(Transform collider)
	{
		if(collider.tag == "Turret")
		{
			TurretController tur = collider.GetComponent<TurretController>();
			tur.TakeDamage(damage);
		}
		
		if (collider.tag == "Enemy")
		{
			IEnemy enim = collider.GetComponent<IEnemy>();
			enim.TakeDamage(damage);
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

		if(nearestTurret != null && shortestDistance <= sprintRange)
		{
			turretTarget = nearestTurret.transform;
			attack = true;
		}
		else
		{
			attack = false;
		}
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}

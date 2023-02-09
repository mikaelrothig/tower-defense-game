using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TurretController : MonoBehaviour, ITurret {

	[Header("Attributes")]
	public float range = 10f;
	public float turnSpeed = 10f;
	public float fireRate = 1f;
	private float fireCountdown = 0f;
	public float turretHealth = 3;
	private float startHealth;

	
	[Header("Misc")]
	public string enemyTag = "Enemy";
	private Transform target;
	public Transform partToRotate;
	public GameObject bullet;
	public Transform firePoint;
	public Image healthBar;

	public GameObject deathEffect;
	public string turretType;
	
	// Use this for initialization
	void Start () {
		InvokeRepeating("UpdateTarget", 0f, 0.5f);
		startHealth = turretHealth;
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null)
			return;

		//Target Lock-on
		Vector3 dir = target.position - transform.position;
		Quaternion lookRotation = Quaternion.LookRotation(dir);
		Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
		partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

		if(fireCountdown <= 0)
		{
			Shoot();
			fireCountdown = 1f / fireRate;
		}

		fireCountdown -= Time.deltaTime;
	}

	public void Shoot()
	{
		if(turretType.Equals("Basic"))
			AudioManager.standardTurretFire.Play();
		if(turretType.Equals("Missile"))
			AudioManager.missileTurretFire.Play();
		GameObject bulletFired = Instantiate(bullet, firePoint.position, firePoint.rotation);
		TurretBullet bulletScript = bulletFired.GetComponent<TurretBullet>();
		
		if(bulletScript != null)
		{
			bulletScript.Chase(target);
		}
	}

	public void UpdateTarget()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity;
		GameObject nearestEnemy = null;

		foreach (GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

			if(distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy;
			}
		}

		if(nearestEnemy != null && shortestDistance <= range)
		{
			target = nearestEnemy.transform;
		}
		else
		{
			target = null;
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}

	public void TakeDamage(float bulletDamage)
	{
		turretHealth -= bulletDamage;
		healthBar.fillAmount  = turretHealth / startHealth;

		if(turretHealth  <= 0)
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
	}
}

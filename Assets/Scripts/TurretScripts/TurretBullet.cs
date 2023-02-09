using UnityEngine;

public class TurretBullet : MonoBehaviour {

	private Transform target;
	public float speed = 70f;
	public float explosionRadius = 0f;
	public float damage = 1f;

	public GameObject impactEffect;
	
	public void Chase (Transform _target)
	{
		target = _target;
	}
	
	// Update is called once per frame
	void Update () {
		if(target == null)
		{
			Destroy(gameObject);
			return;
		}

		Vector3 dir = target.position - transform.position;
		float distanceThisFrame = speed * Time.deltaTime;

		if(dir.magnitude <= distanceThisFrame)
		{
			HitTarget();
			return;
		}

		transform.Translate(dir.normalized * distanceThisFrame, Space.World);
		transform.LookAt(target);
	}

	void HitTarget()
	{
		GameObject effectInstance = Instantiate(impactEffect, transform.position, transform.rotation);
		Destroy(effectInstance, 1f);

		if(explosionRadius > 0f)
		{
			Explode();
		}
		else
		{
			Damage(target);
		}

		if(explosionRadius > 0f)
			AudioManager.explosion.Play();

		Destroy(gameObject);
	}

	void Damage(Transform enemy)
	{
		//EnemyBasic enim = enemy.GetComponent<EnemyBasic>();
		IEnemy enim = enemy.GetComponent<IEnemy>();
		enim.TakeDamage(damage);
	}

	void Explode()
	{
		Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

		foreach(Collider collider in colliders)
		{
			if(collider.tag == "Enemy")
			{
				Damage(collider.transform);
			}
		}
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface ITurret{
	
	void Shoot();
	void UpdateTarget();
	void TakeDamage(float bulletDamage);
	void Death();
}

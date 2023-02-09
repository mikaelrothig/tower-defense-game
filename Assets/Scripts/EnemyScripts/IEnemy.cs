using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IEnemy
{
	void TakeDamage(float bulletDamage);
	void Death();
	void UpdateTarget();
}

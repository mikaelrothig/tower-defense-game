using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyFrankenstein : MonoBehaviour {

	private Transform target;
	private NavMeshAgent agent;
	private Transform closestTurret;
	private Transform turretTarget;

	public float enemyHealth = 3f;
	private float startHealth;
	public float range = 10f;
	public float fireRate = 1f;
	private float fireCountdown = 0f;

	public string enemyTag = "Turret";

	public GameObject bullet;
	public GameObject deathEffect;
	public Image healthBar;

	private bool attack = false;
	private bool canMove = true;

	//Vanguard
	public Transform[] portals;
	public float spawnRandomiser;
	public float manaPoints;
	private float manaPointsTotal;
	public float manaRate;
	private float chargePortal = 5f;
	public GameObject portalChargeEffect;

	//Techie
	public float startSpeed;
	public float sprintSpeed;

	//Scout
	private bool visibility = true;
	public Material invisible;
	private Material visible;
	public GameObject glow;

	//Commander
	private Transform allie;
	public float allieSearchRadius;
	public string allieTag = "Enemy";
	private Transform allieTarget;
	public float retreatRange;
	Vector3 startPos;

	int enemyTypeOne;
	int enemyTypeTwo;

	public void Start () {

		enemyTypeOne = Random.Range(0,5);
		enemyTypeTwo = Random.Range(0,5);
		
		while( enemyTypeOne == enemyTypeTwo)
			enemyTypeTwo = Random.Range(0,5);

		SelectEnemyType(enemyTypeOne);
		SelectEnemyType(enemyTypeTwo);
	}

	public void SelectEnemyType(int enemyType)
	{
		switch(enemyType)
		{
			case 0:	//Commander
				gameObject.GetComponent<EnemyCommander>().enabled = true;
				break;
			
			case 1:	//Vanguard
				gameObject.GetComponent<EnemyVanguard>().enabled = true;
				break;

			case 2: //Scout
				gameObject.GetComponent<EnemyScout>().enabled = true;
				break;

			case 3: //Techie
				gameObject.GetComponent<EnemyTechie>().enabled = true;
				break;

			case 4: //Wizard
				gameObject.GetComponent<EnemyWizard>().enabled = true;
				break;
		}

		print(enemyType);
	}
	
	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, range);
	}
}

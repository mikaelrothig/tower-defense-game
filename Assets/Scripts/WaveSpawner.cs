using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class WaveSpawner : MonoBehaviour {
	
	public static int EnemiesAlive = 0;

	public Transform spawnPointOne;
	public Transform spawnPointTwo;
	public Transform spawnPointThree;
	public Transform spawnPointFour;

	public float spawnRate;
	public float timeBetweenWaves = 10f;
	public Text waveCountdownText;
	public Text EnemiesRemaining;
	public Text wavesCompleted;

	private float countDown = 2f;
	private int waveIndex = -1;
	private int enemyIndex = 0;
	
	public Wave[] waves;
	public static bool LevelCompleted;

	void Start()
	{
		LevelCompleted = false;
		countDown = timeBetweenWaves;
		EnemiesAlive = 0;
	}

	void Update()
	{
		EnemiesRemaining.text = "" + EnemiesAlive;
		wavesCompleted.text = waveIndex + 1 + "/10";

		if(EnemiesAlive > 0)
		{
			return;
		}	

		if(countDown <= 0)
		{
			waveIndex++;
			StartCoroutine(SpawnWave(waves[waveIndex]));
			countDown = timeBetweenWaves;
			return;
		}

		if(waveIndex >= waves.Length-1 && EnemiesAlive <= 0)
		{
			LevelCompleted = true;
			this.enabled = false;
		}

		if(EnemiesAlive <= 0)
		{
			countDown -= Time.deltaTime;
			countDown = Mathf.Clamp (countDown, 0f, Mathf.Infinity);
			waveCountdownText.text = string.Format("{0:00.00}", countDown);
		}
	}

	IEnumerator SpawnWave(Wave wave)
	{
		Debug.Log("Wave Incoming!");
		
		EnemiesAlive = 0;
		PlayerStats.rounds++;

		int temp = wave.loop;

		while(wave.loop > 0)
		{
			for (int j = 0; j < wave.enemyTypes.Length; j++)
			{			
				if(wave.enemyTypes[j].laneOne)
					EnemiesAlive++;

				if(wave.enemyTypes[j].laneTwo)
					EnemiesAlive++;
				
				if(wave.enemyTypes[j].laneThree)	
					EnemiesAlive++;

				if(wave.enemyTypes[j].laneFour)	
					EnemiesAlive++;	
			}
			wave.loop--;
		}	

		wave.loop = temp;
		
		while(wave.loop > 0)
		{
			for (int j = 0; j < wave.enemyTypes.Length; j++)
			{			
				if(wave.enemyTypes[j].laneOne)
					SpawnEnemy(spawnPointOne, wave.enemyTypes[j].enemyType);

				if(wave.enemyTypes[j].laneTwo)
					SpawnEnemy(spawnPointTwo, wave.enemyTypes[j].enemyType);
				
				if(wave.enemyTypes[j].laneThree)	
					SpawnEnemy(spawnPointThree, wave.enemyTypes[j].enemyType);

				if(wave.enemyTypes[j].laneFour)	
					SpawnEnemy(spawnPointFour, wave.enemyTypes[j].enemyType);

				yield return new WaitForSeconds(1/spawnRate);	
			}
			wave.loop--;
		}	
	}

	void SpawnEnemy(Transform spawnPoint, GameObject enemy)
	{
		Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
	}
}

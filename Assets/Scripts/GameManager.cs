using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	//public Text livesCounter;
	public static bool gameOver;

	public GameObject gameOverUI;
	public GameObject levelCompletedUI;

	void Start() {
		gameOver = false;
	}
	
	void Update () {
		//livesCounter.text = PlayerStats.lives + "	Lives";

		if(PlayerStats.lives <= 0)
		{
			EndGame();
		}

		if(gameOver)
			return;

		if(WaveSpawner.LevelCompleted)
		{
			levelCompleted();
		}

		if(Input.GetKeyUp(KeyCode.P))
		{
			if(Time.timeScale < 1f)
				Time.timeScale = 1f;
			else
				Time.timeScale = 0f;
		}
	}

	void EndGame()
	{
		gameOver = true;
		gameOverUI.SetActive(true);
	}

	void levelCompleted()
	{
		gameOver = true;
		levelCompletedUI.SetActive(true);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public GameObject PauseMenuUI;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Toggle();
		}
	}

	public void Toggle()
	{
		PauseMenuUI.SetActive(!PauseMenuUI.activeSelf);	

		if(PauseMenuUI.activeSelf)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void Retry()
	{
		Toggle();
		SceneManager.LoadScene(1);
	}

	public void Menu()
	{
		Toggle();
		SceneManager.LoadScene(0);
	}
}

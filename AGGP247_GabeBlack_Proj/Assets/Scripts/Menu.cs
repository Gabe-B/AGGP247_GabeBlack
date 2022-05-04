using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
	public void Quit()
	{
		Application.Quit();
	}

	public void MainMenu()
	{
		SceneManager.LoadScene(0);
	}

	public void StartGame()
	{
		SceneManager.LoadScene(1);
	}

	public void LoadGame()
	{
		SceneManager.LoadScene(2);
	}
}

using UnityEngine;
using System.Collections;

public class InGameMenuScript : MonoBehaviour {

	public string toMainMenu = "";

	public bool isPaused = false;

	public GameObject pauseMenu = null;
	
	// Update is called once per frame
	void Update () {
		if (isPaused) {
			pauseMenu.SetActive (true);
			Time.timeScale = 0;
		} else {
			pauseMenu.SetActive (false);
			Time.timeScale = 1;
		}

		if (Input.GetKeyDown(KeyCode.Escape) == true) {
			isPaused = !isPaused;
		}
	}

	public void Resume() {
		isPaused = false;
	}

	public void Quit() {
		Application.LoadLevel (toMainMenu);
	}
}

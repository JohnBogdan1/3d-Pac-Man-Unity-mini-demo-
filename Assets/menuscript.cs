using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class menuscript : MonoBehaviour {

    //public Canvas startMenu;
    public Button start;
    public Button quit;

	// Use this for initialization
	public void Start () {
        //startMenu = startMenu.GetComponent<Canvas>();
        start = start.GetComponent<Button> ();
        quit = quit.GetComponent<Button> ();
        //startMenu.enabled = false;
	}
	
	// Update is called once per frame
	public void ExitPress () {
        //startMenu.enabled = true;
        start.enabled = false;
        quit.enabled = false;
	}

    public void NoPress ()
    {
        //startMenu.enabled = false;
        start.enabled = true;
        quit.enabled = true;
    }

    public void StartLevel ()
    {
        Application.LoadLevel(1);
    }

    public void ExitGame ()
    {
        Application.Quit ();
    }
}

using UnityEngine;
using System.Collections;

public class PauseMenuScript : MonoBehaviour {

	public LoverScript playerScript;

	public void Show()
	{
		gameObject.SetActive(true);
		Vector3 pos = transform.position;
		pos.x = Camera.main.transform.position.x;
		transform.position = pos;
	}

	public void ButtonClicked(string name)
	{
		switch(name)
		{
		case "Resume":
			playerScript.TogglePause();
			break;
		case "Quit":
			playerScript.TogglePause();
			Application.LoadLevel(0);
			break;
		}
	}
}

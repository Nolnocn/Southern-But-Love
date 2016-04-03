using UnityEngine;
using System.Collections;

public class GameOverScript : MonoBehaviour {

	public bool gameOver = false;
	public bool win = false;
	public Font sketch;
	public GUISkin skin;
	private int score;
	private string playerName = "Put Yer Name";
	private bool submitted = false;

	public void Show(int pScore)
	{
		score = pScore;
		gameObject.SetActive(true);
		Vector3 pos = transform.position;
		pos.x = Camera.main.transform.position.x;
		transform.position = pos;
	}

	void OnGUI()
	{
		GUI.skin = skin;

		if(gameOver)
		{
			GUI.skin.label.fontSize = Mathf.RoundToInt(Screen.height * 0.1f);

			if(!win)
			{
				GUI.Label(new Rect(Screen.width/2 - 250, Screen.height/2 - Screen.height * 0.2f, 500, Screen.height * 0.2f), "Game Over");
			}
			else
			{
				GUI.Label(new Rect(Screen.width/2 - 250, Screen.height/2 - Screen.height * 0.2f, 500, Screen.height * 0.2f), "Yee Haw!");
			}

			GUI.skin.label.fontSize = Mathf.RoundToInt(Screen.height * 0.05f);

			GUI.Label(new Rect(Screen.width/2 - 250,Screen.height/2 - Screen.height * 0.085f, 500, Screen.height * 0.1f), "Score:" + " " + score);

			GUI.SetNextControlName ("playerName");
			GUI.skin.textField.fontSize = Mathf.RoundToInt(Screen.height * 0.04f);
			float width = Mathf.Max(Screen.width * 0.2f, 225);
			playerName = GUI.TextField(new Rect(Screen.width/2 - width * 0.5f, Screen.height/2, width, Screen.height * 0.06f), playerName, 12);
			
			if (UnityEngine.Event.current.type == EventType.Repaint)
			{
				if (GUI.GetNameOfFocusedControl () == "playerName")
				{
					if (playerName == "Put Yer Name") playerName = "";
				}
				else
				{
					if (playerName == "") playerName = "Put Yer Name";
				}
			}
		}
	}
	
	public void ButtonClicked(string id)
	{
		switch(id)
		{
		case "Replay":
			SubmitScore();
			Application.LoadLevel(1);
			break;
		case "Submit":
		case "Quit":
			SubmitScore();
			Application.LoadLevel(0);
			break;
		}
	}

	public void SubmitScore()
	{
		if(playerName == "Put Yer Name" || submitted)
		{
			return;
		}

		int tmpScore;
		string tmpName;
		for(int i = 0; i < 10; i++)
		{
			if(PlayerPrefs.HasKey("score" + i))
			{
				if(PlayerPrefs.GetInt("score" + i) < score)
				{
					tmpScore = PlayerPrefs.GetInt("score" + i);
					tmpName = PlayerPrefs.GetString("name" + i);
					PlayerPrefs.SetInt("score" + i, score);
					PlayerPrefs.SetString("name" + i, playerName);
					
					for(int j = i + 1; j < 10; j++)
					{
						string tmpName2 = tmpName;
						int tmpScore2 = tmpScore;
						tmpScore = PlayerPrefs.GetInt("score" + j);
						tmpName = PlayerPrefs.GetString("name" + j);
						PlayerPrefs.SetInt("score" + j, tmpScore2);
						PlayerPrefs.SetString("name" + j, tmpName2);
					}
					
					return;
				}
			}
			else
			{
				PlayerPrefs.SetString("name" + i, playerName);
				PlayerPrefs.SetInt("score" + i, score);
			}
		}

		submitted = true;
	}
}

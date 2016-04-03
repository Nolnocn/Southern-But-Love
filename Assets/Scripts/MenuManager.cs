using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour {

	private Camera menuCamera;
	private bool transition;
	private bool scores;
	private bool guide;
	private bool returnMenu;
	private Vector3 treePos;
	private float screenWidth;
	private GameObject guideScreen;
	public List<GameObject> Main = new List<GameObject>();
	public GUISkin skin; 
	public Font font;

	private AsyncOperation sceneLoad;

	// Use this for initialization
	void Start () {

		menuCamera = Camera.main;
		transition = false;
		scores = false;
		guide = false;
		returnMenu = false;

		screenWidth = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 0.0f)).x * 2;

		//Set tree location reletive to screen size
		GameObject menuTree = new GameObject ();
		menuTree.AddComponent<SpriteRenderer> ();
		Sprite treeSprite = Resources.Load<Sprite> ("Menu/MenuTree3");
		menuTree.GetComponent <SpriteRenderer>().sprite = treeSprite;
		treePos = menuCamera.ScreenToWorldPoint (new Vector3(0, Screen.height/2, -menuCamera.transform.position.z));
		//menuCamera.ScreenToWorldPoint (new Vector3(treeSprite.texture.width * 0.5f, Screen.height/2, -menuCamera.transform.position.z));
		menuTree.transform.position = treePos;
		Main.Add (menuTree);

		//Play Button
		GameObject playButton = new GameObject ();
		playButton.AddComponent<SpriteRenderer> ();
		Button playBTN = playButton.AddComponent<Button>();
		Vector3 playPos = treePos + new Vector3(3.3f, 0.2f, 0);
		//menuCamera.ScreenToWorldPoint (new Vector3(Screen.width/5.3f, Screen.height/1.95f, -menuCamera.transform.position.z));
		playPos.z = -1.0f;
		playButton.transform.position = playPos;
		playButton.transform.parent = menuTree.transform;
		playBTN.SetName("PlayButton",this);

		//Guide Button
		GameObject guideButton = new GameObject ();
		guideButton.AddComponent<SpriteRenderer> ();
		Button guideBTN = guideButton.AddComponent<Button>();
		Vector3 guidePos = treePos + new Vector3(3.3f, -0.5f, 0);
		//menuCamera.ScreenToWorldPoint (new Vector3(Screen.width/5.3f, Screen.height/2.25f, -menuCamera.transform.position.z));
		guidePos.z = -1.0f;
		guideButton.transform.position = guidePos;
		guideButton.transform.parent = menuTree.transform;
		guideBTN.SetName("GuideButton",this);

		//Scores Button
		GameObject scoreButton = new GameObject ();
		scoreButton.AddComponent<SpriteRenderer> ();
		Button scoreBTN = scoreButton.AddComponent<Button>();
		Vector3 scorePos =  treePos + new Vector3(3.3f, -1.2f, 0);
		//menuCamera.ScreenToWorldPoint (new Vector3(Screen.width/5.3f, Screen.height/2.7f, -menuCamera.transform.position.z));
		scorePos.z = -1.0f;
		scoreButton.transform.position = scorePos;
		scoreButton.transform.parent = menuTree.transform;
		scoreBTN.SetName("ScoresButton",this);

		//Quit Button
		if(Application.platform != RuntimePlatform.OSXWebPlayer && Application.platform != RuntimePlatform.WindowsWebPlayer)
		{
			GameObject quitButton = new GameObject ();
			quitButton.AddComponent<SpriteRenderer> ();
			Button quitBTN = quitButton.AddComponent<Button>();
			Vector3 quitPos =  menuCamera.ScreenToWorldPoint (new Vector3(Screen.width - Screen.height * .1f, Screen.height * .1f, -menuCamera.transform.position.z));
			quitPos.z = -1.0f;
			quitButton.transform.position = quitPos;
			quitButton.transform.parent = menuTree.transform;
			quitBTN.SetName("QuitButton",this);
		}

		//Scorescreen
		GameObject scoreScreen = new GameObject ();
		scoreScreen.AddComponent<SpriteRenderer> ();
		Sprite scoreScreenSprite = Resources.Load<Sprite> ("Menu/StoneMenu_Cracked");
		scoreScreen.GetComponent<SpriteRenderer> ().sprite = scoreScreenSprite;
		Vector3 scoreScreenPos = new Vector3 (treePos.x - screenWidth * 0.5f, 0.0f, -1.0f);
		scoreScreen.transform.position = scoreScreenPos;
		Main.Add (scoreScreen);

		//Guidescreen
		guideScreen = new GameObject ();
		guideScreen.AddComponent<SpriteRenderer> ();
		Sprite guideScreenSprite = Resources.Load<Sprite> ("Menu/StoneGuide_Cracked");
		guideScreen.GetComponent<SpriteRenderer> ().sprite = guideScreenSprite;
		Vector3 guideScreenPos = new Vector3 (treePos.x - screenWidth * 0.5f, 0.0f, -1.0f);
		guideScreen.transform.position = guideScreenPos;
		Main.Add (guideScreen);

		//Back Button for score screen
		GameObject backButton_s = new GameObject ();
		backButton_s.AddComponent<SpriteRenderer> ();
		Button backBTN = backButton_s.AddComponent<Button>();
		Vector3 backButtonPos = guideScreenPos + new Vector3 (-4.0f, -3.0f, -1.0f);
		backButton_s.transform.position = backButtonPos;
		backBTN.SetName("BackButton",this);
		backButton_s.transform.parent = menuTree.transform;

		if(!PlayerPrefs.HasKey("score0"))
		{
			PlayerPrefs.SetString("name0", "Ma");
			PlayerPrefs.SetInt("score0", 10000);
			PlayerPrefs.SetString("name1", "Bucky");
			PlayerPrefs.SetInt("score1", 9000);
			PlayerPrefs.SetString("name2", "Cletus");
			PlayerPrefs.SetInt("score2", 8000);
			PlayerPrefs.SetString("name3", "Jebediah");
			PlayerPrefs.SetInt("score3", 7000);
			PlayerPrefs.SetString("name4", "Daisy Marie");
			PlayerPrefs.SetInt("score4", 6000);
			PlayerPrefs.SetString("name5", "Billy Ray");
			PlayerPrefs.SetInt("score5", 5000);
			PlayerPrefs.SetString("name6", "Billy Bob");
			PlayerPrefs.SetInt("score6", 4000);
			PlayerPrefs.SetString("name7", "Billy Joe");
			PlayerPrefs.SetInt("score7", 3000);
			PlayerPrefs.SetString("name8", "Jezzebelle");
			PlayerPrefs.SetInt("score8", 2000);
			PlayerPrefs.SetString("name9", "Col. Sanders");
			PlayerPrefs.SetInt("score9", 1000);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Handle Menu Movement
		if (scores && !returnMenu) 
		{
			MoveObject (Main [0].transform.position, new Vector3 (treePos.x + screenWidth, Main [0].transform.position.y, Main [0].transform.position.z), Main [0]);
		}
		else if(guide && !returnMenu)
		{
			MoveObject (Main [0].transform.position, new Vector3 (treePos.x + screenWidth, Main [0].transform.position.y, Main [0].transform.position.z), Main [0]);
		}
		else if(returnMenu)
		{
			MoveObject (Main [0].transform.position, treePos, Main [0]);
		}

		//Check if on orginal menu
		if(Main[0].transform.position == treePos)
		{
			Main[1].transform.parent = null;
			Main[2].transform.parent = null;

			guide = false;
			scores = false;
			returnMenu= false;
			transition = false;
		}
		else if(Main[0].transform.position ==  new Vector3 (treePos.x + screenWidth, Main [0].transform.position.y, Main [0].transform.position.z))
		{
			transition = false;
		}
	}

	void OnGUI()
	{
		GUI.skin = skin;
		GUI.skin.font = font;

		if(scores && !transition)
		{
			for(int i = 0; i < 10; i++)
			{
				if(PlayerPrefs.HasKey("score" + i))
				{
					int fontSize = Mathf.RoundToInt(Screen.height * 0.05f);
					GUI.skin.label.fontSize = fontSize;
					int hiScore = PlayerPrefs.GetInt("score" + i);
					string hiName = PlayerPrefs.GetString("name" + i);
					
					GUI.Label(new Rect(Screen.width/2 - Screen.width * 0.25f, Screen.height/2 - Screen.height * 0.25f + (i*fontSize),
					                   Screen.width/2, 400), "" + (i + 1) + ": " + hiName + " - " + hiScore);
				}
				else
				{
					break;
				}
			}
		}
	}

	void MoveObject(Vector3 pStartPos, Vector3 pEndPos, GameObject pObject)
	{
		float step = 10.0f * Time.deltaTime;

		pObject.transform.position = Vector3.MoveTowards (pStartPos, pEndPos, step);
	}

	public IEnumerator ButtonClicked(string name)
	{
		if(!transition && sceneLoad == null)
		{
			switch(name)
			{
			case "PlayButton":
				guideScreen.transform.position = new Vector3(0, 0, 0);
				guideScreen.GetComponent<SpriteRenderer>().sortingOrder = 100;
				yield return new WaitForSeconds(0.5f);
				//Application.LoadLevel(1);
				sceneLoad = SceneManager.LoadSceneAsync(1);
				break;
			case "ScoresButton":
				yield return new WaitForSeconds(0.25f);
				scores = true;
				transition = true;
				Main [1].transform.parent = Main [0].transform;
				break;
			case "GuideButton":
				yield return new WaitForSeconds(0.25f);
				guide = true;
				transition = true;
				Main [2].transform.parent = Main [0].transform;
				break;
			case "BackButton":
				yield return new WaitForSeconds(0.25f);
				returnMenu = true;
				transition = true;
				break;
			case "QuitButton":
				yield return new WaitForSeconds(0.25f);
				Application.Quit();
				break;
			}
		}
	}
}

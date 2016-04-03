using UnityEngine;
using System.Collections;

public class ManagerScript : MonoBehaviour {

	public static float upperVertBounds = 7.0f;
	public static float lowerVertBounds = 1.0f;
	public static float halfScreenWidth;
	public Sprite[] waterHighlightSprites;
	public Sprite[] treeSprites;
	public Transform[] waterHighlights;
	public Transform[] shorelines;
	private Transform[] trees;
	public Transform player;

	private GameObject[] dereksLevels;
	private GameObject nicksAmazingLevel;
	public GameObject endArea;
	public GameObject speedZone;

	private AudioSource gameMusic;
	private AudioSource comeSailAway;

	private bool win;
	public Transform credits;
	private Vector3 creditsStart;
	private Vector3 creditsEnd;
	private float t;
	private float timeToReachTarget;

	void Awake ()
	{
		win = false;
		AudioSource[] audioSources = GetComponents<AudioSource>();
		gameMusic = audioSources[0];
		comeSailAway = audioSources[1];
		halfScreenWidth = Camera.main.ViewportToWorldPoint(new Vector3(1.0f, 0.0f, 0.0f)).x;
		CreateWaterHighLights();
		CreateTrees();

		dereksLevels = new GameObject[8];
		for(int i = 0; i < 8; i++)
		{
			dereksLevels[i] = Resources.Load ("Levels/Prefabs/level" + (i+1)) as GameObject;
		}
		nicksAmazingLevel = Resources.Load ("Levels/Prefabs/level" + 9) as GameObject;

		int numLoops = 3;

		for(int i = 0; i < numLoops; i++)
		{
			CreateLoop(i);
		}
		CreateEnd(numLoops);
	}

	void Update ()
	{
		CheckHighlightPos();
		CheckShorePos();
		CheckTreePos();

		if(win)
		{
			t += Time.deltaTime/timeToReachTarget;
			credits.localPosition = Vector3.Lerp(creditsStart, creditsEnd, t);
		}
	}

	public void StartMusic()
	{
		gameMusic.Play();
	}

	private void CreateWaterHighLights()
	{
		waterHighlights = new Transform[20];
		for(int i = 0; i < 20; i++)
		{
			GameObject highlight = new GameObject();
			highlight.AddComponent<SpriteRenderer>();
			SpriteRenderer sr = highlight.GetComponent<SpriteRenderer>();
			sr.sprite = waterHighlightSprites[Random.Range(0, waterHighlightSprites.Length)];
			sr.sortingOrder = -89;
			Vector3 pos = new Vector3(-halfScreenWidth + i + Random.Range(-0.5f, 0.5f),
			                          Random.Range(highlight.transform.localScale.y * 0.5f, 7.0f),
			                          0);
			highlight.transform.position = pos;
			waterHighlights[i] = highlight.transform;
		}
	}

	private void CreateTrees()
	{
		trees = new Transform[50];
		for(int i = 0; i < trees.Length; i++)
		{
			GameObject tree = new GameObject();
			tree.AddComponent<SpriteRenderer>();
			SpriteRenderer sr = tree.GetComponent<SpriteRenderer>();
			sr.sprite = treeSprites[Random.Range(0, treeSprites.Length)];
			sr.sortingOrder = -81;
			Vector3 pos = new Vector3(-halfScreenWidth + i + Random.Range(-0.5f, 0.5f),
			                          9.0f,
			                          0);
			tree.transform.position = pos;
			trees[i] = tree.transform;
		}
	}

	private void CheckHighlightPos()
	{
		for(int i = 0; i < 20; i++)
		{
			Transform highlight = waterHighlights[i];
			if(highlight.position.x < Camera.main.transform.position.x - halfScreenWidth - highlight.localScale.x)
			{
				SpriteRenderer sr = highlight.GetComponent<SpriteRenderer>();
				sr.sprite = waterHighlightSprites[Random.Range(0, waterHighlightSprites.Length)];
				highlight.position = new Vector3(Camera.main.transform.position.x + halfScreenWidth + highlight.localScale.x,
				                                 Random.Range(highlight.localScale.y * 0.5f, 7.0f - highlight.localScale.y * 0.5f), 0);
			}
		}
	}

	private void CheckShorePos()
	{
		for(int i = 0; i < 3; i++)
		{
			Transform shoreline = shorelines[i];
			if(shoreline.position.x < Camera.main.transform.position.x - halfScreenWidth - 17.77778f)
			{
				shoreline.position += new Vector3(17.77778f * 3, 0, 0);
			}
		}
	}

	private void CheckTreePos()
	{
		for(int i = 0; i < trees.Length; i++)
		{
			Transform tree = trees[i];
			if(tree.position.x < Camera.main.transform.position.x - halfScreenWidth - 5)
			{
				tree.position += new Vector3(trees.Length, 0, 0);
			}
		}
	}

	private void CreateLoop(int num)
	{
		bool bridge = false;
		Vector3 pos = new Vector3(24 * num * (dereksLevels.Length), 0, 0);
		Instantiate(nicksAmazingLevel, pos, Quaternion.identity);
		Instantiate(speedZone, pos, Quaternion.identity);
		for(int i = 1; i < dereksLevels.Length; i++)
		{
			pos = new Vector3(24 * i + 24 * num * (dereksLevels.Length), 0, 0);
			if(bridge)
			{
				Instantiate(dereksLevels[Random.Range(0, 4)], pos, Quaternion.identity);
			}
			else
			{
				Instantiate(dereksLevels[Random.Range(4, 8)], pos, Quaternion.identity);
			}

			bridge = !bridge;
		}
	}

	private void CreateEnd(int num)
	{
		Vector3 pos = new Vector3(24 * num * (dereksLevels.Length), 0, 0);
		Instantiate(nicksAmazingLevel, pos, Quaternion.identity);
		pos += new Vector3(24, 0, 0);
		Instantiate(endArea, pos, Quaternion.identity);
	}

	public void Win()
	{
		win = true;
		gameMusic.Stop();
		comeSailAway.Play();

		t = 0;
		timeToReachTarget = 73;
		credits.localPosition = new Vector3(ManagerScript.halfScreenWidth,
		                                    credits.localPosition.y, 5);
		creditsStart = credits.localPosition;
		creditsEnd = creditsStart + new Vector3(0, 2 * Mathf.Abs(creditsStart.y), 0);
		credits.gameObject.SetActive(true);
	}
}
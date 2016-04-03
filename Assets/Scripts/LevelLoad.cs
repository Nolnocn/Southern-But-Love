using UnityEngine;
using System.Collections;
//using UnityEditor;

//[ExecuteInEditMode]

public class LevelLoad : MonoBehaviour {

	public TextAsset level;
	public int numLevels = 8;
	public GameObject[] staticObstacles;
	public GameObject gatorPrefab;
	public GameObject hillBillyPrefab;
	public GameObject bridgePrefab;
	public GameObject collectiblePrefab;
	public bool loadAll = false;
	public int levelToLoad = 0;

	void OnEnable()
	{
		//Debug.Log("Gogogogogo");
		if(loadAll)
		{
			for (int i = 1; i < numLevels + 1; i++)
			{
				level = Resources.Load ("Levels/Level" + i) as TextAsset;
				SplitLevel(level.text, i);
				Debug.Log("Loaded Level " + i);
			}
		}
		else
		{
			level = Resources.Load ("Levels/Level" + levelToLoad) as TextAsset;
			SplitLevel(level.text, levelToLoad);
			Debug.Log("Loaded Level " + levelToLoad);
		}
		this.enabled = false;
	}

	public void SplitLevel(string map, int number)
	{
		GameObject levelSegment = new GameObject ();
		levelSegment.transform.position = Vector3.zero;

		string[] lines = map.Split("\n"[0]);

		for (int i = 0; i < lines.Length; i++)
		{
			string[] line = lines [i].Split ("," [0]);
			for(int j = 0; j < line.Length; j++)
			{
				Vector3 position = new Vector3(j * 1.2f, 6.4f - (i * 1.2f), 0.0f);
				switch(line[j])
				{
				case "R":
					GameObject obstacle = (GameObject)Instantiate(staticObstacles[Random.Range(0, 5)], position, Quaternion.identity);
					obstacle.transform.parent = levelSegment.transform;
					break;
				case "A":
					GameObject gator = (GameObject)Instantiate(gatorPrefab, position, Quaternion.identity);
					gator.transform.parent = levelSegment.transform;
					break;
				case "H":
					GameObject hillBilly = (GameObject)Instantiate(hillBillyPrefab, position, Quaternion.identity);
					hillBilly.transform.parent = levelSegment.transform;
					break;
				case "B":
					if(i == 4)
					{
						GameObject bridge = (GameObject)Instantiate(bridgePrefab, new Vector3(j * 1.2f + .3f, 2.8f, 0.0f), Quaternion.identity);
						bridge.transform.parent = levelSegment.transform;
					}
					break;
				case "C":
					GameObject collectible = (GameObject)Instantiate(collectiblePrefab, position, Quaternion.identity);
					collectible.transform.parent = levelSegment.transform;
					break;
				}
			}
		}
		//PrefabUtility.CreatePrefab ("Assets/Resources/Levels/Prefabs/level" + number + ".prefab", levelSegment);
		DestroyImmediate(levelSegment);
	}
	
}

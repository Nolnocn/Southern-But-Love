using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {

	void Update ()
	{
		if(transform.position.x < Camera.main.transform.position.x - ManagerScript.halfScreenWidth * 2 ||
		   transform.position.x > Camera.main.transform.position.x + ManagerScript.halfScreenWidth * 2)
		{
			Destroy(gameObject);
		}
	}
}

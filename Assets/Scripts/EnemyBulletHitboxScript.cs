using UnityEngine;
using System.Collections;

public class EnemyBulletHitboxScript : MonoBehaviour
{
	private HillBillyScript hillbilly;

	void Start()
	{
		hillbilly = transform.parent.GetComponent<HillBillyScript>();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		hillbilly.HandleBulletHit(col.gameObject);
	}
}

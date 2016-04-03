using UnityEngine;
using System.Collections;

public class BulletHitboxScript : MonoBehaviour
{
	private LoverScript lovers;

	void Start()
	{
		lovers = transform.parent.GetComponent<LoverScript>();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		lovers.HandleBulletHit(col.gameObject);
	}
}

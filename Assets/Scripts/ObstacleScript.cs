using UnityEngine;
using System.Collections;

public class ObstacleScript : MonoBehaviour {

	private SpriteRenderer spriteRenderer;

	void Start ()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		SetSortingOrder();
	}
	
	private void SetSortingOrder()
	{
		spriteRenderer.sortingOrder = Mathf.FloorToInt(100 - transform.position.y * 10);
	}
}
using UnityEngine;
using System.Collections;

public class CollectibleScript : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private AudioSource collectSound;
	
	void Start ()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		collectSound = GetComponent<AudioSource>();
		SetSortingOrder();
	}
	
	private void SetSortingOrder()
	{
		spriteRenderer.sortingOrder = Mathf.FloorToInt(100 - transform.position.y * 10);
	}

	public void Collect()
	{
		collectSound.Play();
		spriteRenderer.enabled = false;
		StartCoroutine(DespawnDelay());
	}

	private IEnumerator DespawnDelay()
	{
		yield return new WaitForSeconds(0.5f);
		Destroy(gameObject);
	}
}

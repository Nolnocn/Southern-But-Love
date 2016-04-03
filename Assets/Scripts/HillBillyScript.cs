using UnityEngine;
using System.Collections;

public class HillBillyScript : MonoBehaviour {
	
	public Transform player;
	public LoverScript playerScript;
	public GameObject buckshotPrefab;
	public Transform shootPos;
	private bool alive;
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private bool shooting;
	private AudioSource shotgunSound;
	private AudioSource splashSound;

	void Start ()
	{
		alive = true;
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		player = GameObject.Find("Lovers").transform;
		playerScript = player.GetComponent<LoverScript>();
		shootPos = transform.GetChild(0).transform;
		shooting = false;
		AudioSource[] audioSources = GetComponents<AudioSource>();
		shotgunSound = audioSources[0];
		splashSound = audioSources[1];
		SetSortingOrder();
	}
	
	void Update ()
	{
		if(!shooting)
		{
			CheckPlayerPos();
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Projectile")
		{
			Die();
			Destroy(col.gameObject);
		}
	}
	
	private void CheckPlayerPos()
	{
		if(playerScript.alive && !playerScript.jump && !shooting)
		{
			if(player.position.x > transform.position.x - ManagerScript.halfScreenWidth) {
				if(player.position.y > transform.position.y - 0.5f && player.position.y < transform.position.y + 0.5f)
				{
					shooting = true;
					animator.SetTrigger("Shoot");
				}
				else
				{
					shooting = false;
				}
			}
			else
			{
				shooting = false;
			}
		}
	}

	private void Fire()
	{
		if(alive)
		{
			GameObject buckshot = Instantiate(buckshotPrefab, shootPos.position, Quaternion.identity) as GameObject;
			buckshot.GetComponent<Rigidbody2D>().velocity = new Vector2(-10, 0);
			buckshot.GetComponent<SpriteRenderer>().sortingOrder = spriteRenderer.sortingOrder;
			shotgunSound.Play();
		}
	}
	
	private void SetSortingOrder()
	{
		spriteRenderer.sortingOrder = Mathf.FloorToInt(100 - transform.position.y * 10);
	}

	public void Die()
	{
		alive = false;
		GetComponent<Collider2D>().enabled = false;
		splashSound.Play();
		animator.SetTrigger("Die");
	}
}

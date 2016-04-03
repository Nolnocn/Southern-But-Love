using UnityEngine;
using System.Collections;

public class GatorScript : MonoBehaviour {

	public Transform player;
	public LoverScript playerScript;
	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private bool mouthOpen;
	private AudioSource chompSound;

	void Start ()
	{
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		player = GameObject.Find("Lovers").transform;
		playerScript = player.GetComponent<LoverScript>();
		chompSound = GetComponent<AudioSource>();
		SetSortingOrder();
	}

	void Update ()
	{
		CheckPlayerPos();
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Player")
		{
			StartCoroutine(Chomp());
		}
	}

	private void CheckPlayerPos()
	{
		if(player.position.x > transform.position.x - ManagerScript.halfScreenWidth) {
			if(!playerScript.jump && player.position.y > transform.position.y - 1 && player.position.y < transform.position.y + 1)
			{
				if(!mouthOpen)
				{
					mouthOpen = true;
					animator.SetTrigger("OpenMouth");
				}
			}
			else if(mouthOpen)
			{
				mouthOpen = false;
				animator.SetTrigger("ShutMouth");
			}
		}
		else if(mouthOpen)
		{
			mouthOpen = false;
			animator.SetTrigger("ShutMouth");
		}
	}

	private IEnumerator Chomp()
	{
		yield return new WaitForSeconds(0.6f);
		chompSound.Play();
		animator.SetTrigger("ShutMouth");
		animator.SetBool("Laugh", true);
	}

	private void SetSortingOrder()
	{
		spriteRenderer.sortingOrder = Mathf.FloorToInt(100 - transform.position.y * 10);
	}
}

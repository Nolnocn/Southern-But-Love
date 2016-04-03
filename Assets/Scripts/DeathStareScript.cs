using UnityEngine;
using System.Collections;

public class DeathStareScript : MonoBehaviour {

	private bool show = false;
	private SpriteRenderer spriteRenderer;
	private Animator animator;

	void Start ()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	void Update ()
	{
		if(show && spriteRenderer.color.a < 0.75f)
		{
			spriteRenderer.color = new Color(1, 1, 1, Mathf.Lerp(spriteRenderer.color.a, 0.75f, 0.5f * Time.deltaTime));
		}
	}

	public void Show()
	{
		spriteRenderer.enabled = true;
		show = true;
		spriteRenderer.color = new Color(1, 1, 1, 0);
	}

	public void Explode()
	{
		animator.SetTrigger("Explode");
	}
}

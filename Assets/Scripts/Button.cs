using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	public Sprite idleSprite;
	public Sprite hoverSprite;
	public Sprite activeSprite;
	public string id;
	private bool clicked;
	private MenuManager menuManager;
	public bool pauseMenu = false;
	public PauseMenuScript pauseScript;
	public bool gameOverMenu = false;
	public GameOverScript gameOverScript;

	void Start()
	{
		gameObject.AddComponent<BoxCollider2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void OnMouseOver()
	{
		if(!clicked)
		{
			spriteRenderer.sprite = hoverSprite;
		}
	}

	void OnMouseExit()
	{
		clicked = false;
		spriteRenderer.sprite = idleSprite;
	}

	void OnMouseDown()
	{
		if(!clicked)
		{
			clicked = true;
			spriteRenderer.sprite = activeSprite;
			if(!pauseMenu && !gameOverMenu)
			{
				StartCoroutine(menuManager.ButtonClicked(id));
			}
			else if(gameOverMenu)
			{
				gameOverScript.ButtonClicked(id);
			}
			else
			{
				pauseScript.ButtonClicked(id);
			}
		}
	}

	void OnMouseUp()
	{
		clicked = false;
	}

	public void SetName(string pName, MenuManager manager)
	{
		id = pName;
		spriteRenderer = GetComponent<SpriteRenderer>();
		menuManager = manager;

		idleSprite = Resources.Load<Sprite>("Menu/ButtonSprites/" + id + "Idle");
		hoverSprite = Resources.Load<Sprite>("Menu/ButtonSprites/" + id + "Hover");
		activeSprite = Resources.Load<Sprite>("Menu/ButtonSprites/" + id + "Active");

		spriteRenderer.sprite = idleSprite;
	}
}

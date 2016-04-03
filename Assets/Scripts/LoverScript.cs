using UnityEngine;
using System.Collections;

public class LoverScript : MonoBehaviour {

	private Animator animator;
	private SpriteRenderer spriteRenderer;
	public bool jump, ram, shield, alive;
	private float vertSpeed, horSpeed;
	private float currSpeed;
	private float yLevel;
	private float shieldMeter;
	private bool win;
	private AudioSource airboatSound;
	private AudioSource explosionSound;
	private AudioSource smoochSound;
	private AudioSource splashSound;
	private AudioSource countDown1;
	private AudioSource countDown2;
	private AudioSource countDown3;
	private AudioSource geterdone;
	public DeathStareScript dss;
	public int score;
	private bool paused;
	private bool sailAway;
	private bool start;
	private int speedLevel;
	public ManagerScript mngr;
	public WhiskeyBarScript barScript;
	public PauseMenuScript pauseScript;
	public GameOverScript gameOverScript;
	
	void Start ()
	{
		start = false;
		speedLevel = 0;
		transform.position = new Vector3(-ManagerScript.halfScreenWidth * 0.5f, transform.position.y, 0);
		sailAway = false;
		win = false;
		paused = false;
		shieldMeter = 0;
		score = 0;
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		alive = true;
		jump = false;
		ram = false;
		shield = false;
		vertSpeed = 5.0f;
		horSpeed = 2.5f;
		currSpeed = 5.0f;
		AudioSource[] audioSources = GetComponents<AudioSource>();
		airboatSound = audioSources[0];
		explosionSound = audioSources[1];
		smoochSound = audioSources[2];
		splashSound = audioSources[3];
		countDown1 = audioSources[4];
		countDown2 = audioSources[5];
		countDown3 = audioSources[6];
		geterdone = audioSources[7];
		barScript.SetAmount(shieldMeter);
		StartCoroutine(Countdown());
	}

	void Update ()
	{
		if(!win && start)
		{
			if(alive)
			{
				score += Mathf.FloorToInt(Time.deltaTime * 100) * 10;
			}

			HandleInput();

			Camera.main.GetComponent<Rigidbody2D>().velocity = new Vector2(currSpeed, 0);
		}
		else if(start)
		{
			if(!sailAway)
			{
				Vector3 pos = new Vector3(Camera.main.transform.position.x - 5, 4, 0);
				transform.position = Vector3.MoveTowards(transform.position, pos,  Time.deltaTime);
			}
			else
			{
				if(transform.position.x > ManagerScript.halfScreenWidth * 2 + Camera.main.transform.position.x)
				{
					GetComponent<Rigidbody2D>().velocity = Vector2.zero;
					if(!gameOverScript.win)
					{
						gameOverScript.win = true;
						gameOverScript.Show(score);
					}
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if(alive)
		{
			if(col.tag == "Obstacle")
			{
				Die();
			}
			else if(col.tag == "Hillbilly")
			{
				if(ram)
				{
					col.gameObject.GetComponent<HillBillyScript>().Die();
					GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, -GetComponent<Rigidbody2D>().velocity.y * 0.15f);
				}
				else
				{
					Die ();
				}
			}
			else if(col.tag == "Collectible")
			{
				col.gameObject.GetComponent<CollectibleScript>().Collect();
				shieldMeter += 1;
				if(shieldMeter > 10)
				{
					shieldMeter = 10;
				}
				barScript.SetAmount(shieldMeter);
			}
			else if(col.tag == "Projectile")
			{
				if(shield)
				{
					col.gameObject.GetComponent<Rigidbody2D>().velocity *= -1.5f;
				}
				else
				{
					Die();
					Destroy(col.gameObject);
				}
			}
			else if(col.tag == "Speed Up")
			{
				currSpeed = 5.0f + speedLevel;
				speedLevel++;
			}
			else if(col.tag == "End Area")
			{
				Win ();
			}
		}
	}

	private IEnumerator Countdown()
	{
		countDown3.Play();
		yield return new WaitForSeconds(1.0f);
		countDown2.Play();
		yield return new WaitForSeconds(.75f);
		countDown1.Play();
		yield return new WaitForSeconds(1.0f);
		geterdone.Play();
		yield return new WaitForSeconds(2.0f);
		start = true;
		airboatSound.Play();
		mngr.StartMusic();
	}

	public void TogglePause()
	{
		paused = !paused;
		Time.timeScale = paused ? 0 : 1;
		if(paused)
		{
			pauseScript.Show();
		}
		else
		{
			pauseScript.gameObject.SetActive(false);
		}
	}

	private void HandleInput()
	{
		if(alive && !jump && !ram)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(currSpeed + horSpeed * Input.GetAxis("Horizontal"), vertSpeed * Input.GetAxis("Vertical"));
			ConstrainVertical();
			ConstrainHorizontal();
			if(Input.GetButtonDown("Ram"))
			{
				Ram();
			}

			if(Input.GetButton("Jump") && !shield)
			{
				StartJump();
			}
			if(Input.GetButtonDown("Shield") && shieldMeter > 0)
			{
				StartShield();
			}
			else if(Input.GetButtonUp("Shield"))
			{
				EndShield();
			}

			if(shield)
			{
				shieldMeter -= Time.deltaTime * 2;
				if(shieldMeter <= 0)
				{
					shieldMeter = 0;
					EndShield();
				}
				barScript.SetAmount(shieldMeter);
			}
		}
		else if(jump)
		{
			CheckJump();
		}
		else if(ram)
		{
			ConstrainVertical();
			ConstrainHorizontal();
		}

		if(!jump)
		{
			SetSortingOrder();
		}

		if(Input.GetButtonDown("Pause") && alive)
		{
			TogglePause();
		}

		if(Input.GetButtonDown("Replay") && !alive)
		{
			Application.LoadLevel(1);
		}
	}

	private void ConstrainVertical()
	{
		if(transform.position.y < ManagerScript.lowerVertBounds + transform.localScale.y * 0.5f)
		{
			transform.position = new Vector3(transform.position.x, ManagerScript.lowerVertBounds + transform.localScale.y * 0.5f, transform.position.z);
		}
		else if(transform.position.y > ManagerScript.upperVertBounds - transform.localScale.y * 0.5f)
		{
			transform.position = new Vector3(transform.position.x, ManagerScript.upperVertBounds - transform.localScale.y * 0.5f, transform.position.z);
		}
	}

	private void ConstrainHorizontal()
	{
		float leftBounds = Camera.main.transform.position.x - ManagerScript.halfScreenWidth + transform.localScale.x * 0.5f;
		float rightBounds = Camera.main.transform.position.x + ManagerScript.halfScreenWidth - transform.localScale.x * 0.5f;
		if(transform.position.x < leftBounds)
		{
			transform.position = new Vector3(leftBounds, transform.position.y, transform.position.z);
		}
		else if(transform.position.x > rightBounds)
		{
			transform.position = new Vector3(rightBounds, transform.position.y, transform.position.z);
		}
	}

	private void SetSortingOrder()
	{
		spriteRenderer.sortingOrder = Mathf.FloorToInt(100 - transform.position.y * 10);
	}

	private void ResetVelocity()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(currSpeed, 0);
	}

	private void StopMoving()
	{
		currSpeed = 0;
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}

	private void Ram()
	{
		float dir = Input.GetAxisRaw("Vertical");
		if(dir != 0)
		{
			ram = true;
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, vertSpeed * 1.5f * dir);
			animator.SetFloat("VertSpeed", dir);
			animator.SetTrigger("Ram");
		}
	}

	private void EndRam()
	{
		ram = false;
	}

	private void StartJump()
	{
		jump = true;
		yLevel = transform.position.y;
		ResetVelocity();
		animator.SetTrigger("StartJump");
	}

	private void Jump()
	{
		if(jump) {
			GetComponent<Collider2D>().enabled = false;
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, 10.0f);
			GetComponent<Rigidbody2D>().gravityScale = 1.0f;
		}
	}

	private void CheckJump()
	{
		float yVel = GetComponent<Rigidbody2D>().velocity.y;
		animator.SetFloat("VertSpeed", yVel);
		if(transform.position.y < yLevel && yVel < 0)
		{
			GetComponent<Collider2D>().enabled = true;
			GetComponent<Rigidbody2D>().gravityScale = 0.0f;
			transform.position = new Vector3(transform.position.x, yLevel, 0);
			GetComponent<Rigidbody2D>().velocity = new Vector2(currSpeed, 0);
			animator.SetTrigger("EndJump");
		}
	}

	private void EndJump()
	{
		splashSound.Play();
		jump = false;
	}

	private void StartShield()
	{
		if(!shield) {
			shield = true;
			animator.SetTrigger("StartShield");
			animator.SetBool("Shield", true);
			currSpeed *= 0.5f;
		}
	}

	private void EndShield()
	{
		if(shield) {
			currSpeed *= 2;
			shield = false;
			animator.SetBool("Shield", false);
		}
	}

	private void Die()
	{
		dss.Show();
		if(jump)
		{
			EndJump();
		}
		StopMoving();
		alive = false;
		animator.SetTrigger("Die");
		smoochSound.Play();
		airboatSound.Stop();
		StartCoroutine(GameOver());
	}

	private IEnumerator GameOver()
	{
		yield return new WaitForSeconds(2.0f);
		gameOverScript.Show(score);
	}

	private void Explosion()
	{
		dss.Explode();
		explosionSound.Play();
	}

	private void Win()
	{
		if(jump)
		{
			EndJump();
		}
		if(shield)
		{
			EndShield();
		}
		win = true;
		GetComponent<Rigidbody2D>().velocity = new Vector2(currSpeed, 0);
		mngr.Win();
		StartCoroutine(TakeOffDelay());
	}

	private IEnumerator TakeOffDelay()
	{
		yield return new WaitForSeconds(74);
		sailAway = true;
		Camera.main.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
	}
}
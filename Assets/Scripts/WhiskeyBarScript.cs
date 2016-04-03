using UnityEngine;
using System.Collections;

public class WhiskeyBarScript : MonoBehaviour {
	
	public GameObject bar;
	public float amount;

	void Start ()
	{
		Vector3 pos = transform.localPosition;
		pos.x = -ManagerScript.halfScreenWidth + transform.localScale.x * 2.5f;
		transform.localPosition = pos;
	}

	void Update ()
	{
		ScaleBar();
	}

	public void SetAmount(float amt)
	{
		amount = amt;
	}

	private void ScaleBar()
	{
		Vector3 scale = bar.transform.localScale;
		scale.x = 3.0f * amount * 0.1f;
		bar.transform.localScale = scale;
		Vector3 pos = bar.transform.localPosition;
		pos.x = -1.5f + scale.x * 0.5f;
		bar.transform.localPosition = pos;
	}
}
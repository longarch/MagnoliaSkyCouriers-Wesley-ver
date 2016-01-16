using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BaseCloud : MonoBehaviour
{
	[SerializeField]
	protected CloudManager cloud;
	protected Vector3 position;
	Vector2 min;
	protected float heightChangeTimer = 1.0f, heightAscent = 0;
	// Use this for initialization

	void Destroy()
	{
		gameObject.SetActive(false);
	}
	void OnDisable()
	{
		CancelInvoke();
	}

	// Update is called once per frame
	void Update ()
	{
		min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));
		if (transform.position.x < min.x) {
			//Debug.Log ("Destroy");
			Invoke ("Destroy", 1f);
		}
	}

	public void Setup (Vector2 startingPos)
	{
		position = startingPos;
		transform.position = position;
	}

	public float heightVariantChange ()
	{
		return UnityEngine.Random.Range (-0.5f, 0.5f);
	}

	public void bobCloud ()
	{
		heightChangeTimer -= Time.deltaTime;

		if (heightChangeTimer <= 0.0f) {
			heightAscent = heightVariantChange ();
			heightChangeTimer = 4.0f; //Hard coded for now
		}

		//transform.DOMoveY (heightAscent, 5.0f, false);
		transform.position = position;
	}
}
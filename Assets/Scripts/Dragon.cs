using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Dragon : MonoBehaviour {

	[SerializeField]
	GameObject player;

	private float heightChangeTimer = 5.0f, speed = 0.05f, heightAscent = 0;
	private Vector3 position;
	private int currentHealth;

	void Awake() {

		player = GameObject.Find("Ship");
		position = player.transform.position;
	}

	// Use this for initialization
	void Start () {
		currentHealth = 100;
	}

	// Update is called once per frame
	void Update () {
		moveDragon ();
	}		

	//Returns random between descending and ascending
	public float heightVariantChange()
	{
		return Random.Range (-1.2f,1.2f);

	}

	public void moveDragon()
	{
		heightChangeTimer -= Time.deltaTime;

		if (heightChangeTimer <= 0.0f) {
			heightAscent = heightVariantChange();
			heightChangeTimer = 5.0f; //Hard coded for now
		}

		position += new Vector3(1, 0,0) * speed;
		transform.DOMoveX (position.x, 5.0f, false);
		transform.DOMoveY (player.transform.position.y + heightAscent, 10.0f, false);
		//transform.DOMoveX(player.transform.position.x, 10.0f, false);
		//transform.DOMove(new Vector3(1,heightAscent,0) * speed, 10.0f, false);
		//Debug.Log("Distance left : " + distance);

	}

	public void dragonTakeDamage(int i ) {
		if (currentHealth > 0) {
			currentHealth -= i;
			//healthSlider.value = currentHealth;
			//Debug.Log(currentHealth/100);
			//healthImage.DOFillAmount (((float)currentHealth) / 100, 0.5f);
			//Debug.Log (healthImage.fillAmount);
			//healthTxt.text = "Health: " + currentHealth;
			///innerCam.DOShakePosition(0.5f, 5.0f, 30);
		}
	}
}

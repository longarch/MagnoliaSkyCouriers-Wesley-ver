﻿// Obselete/ Not In used
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Raider : BaseEnemy {
	
	//[SerializeField]
	//GameObject player;
    
	void Awake() {

		player = GameObject.Find("Ship");
		//position = player.transform.position;
        heightChangeTimer = 10.0f;
        speed = 0.05f;
        heightAscent = 0;
    }

	// Use this for initialization
	void Start () {
		currentHealth = 100;
	}
	
	// Update is called once per frame
	void Update () {
		//moveShip ();
		//Debug.Log (transform.position.x);
	}		

	public void moveShip()
	{
		heightChangeTimer -= Time.deltaTime;

		if (heightChangeTimer <= 0.0f) {
			heightAscent = heightVariantChange();
			heightChangeTimer = 10.0f; //Hard coded for now
		}

		//position += new Vector3(1, 0,0) * speed;
		//transform.DOMoveX (position.x, 5.0f, false);
		transform.DOMoveY (player.transform.position.y + heightAscent, 10.0f, false);
		//transform.DOMoveX(player.transform.position.x, 10.0f, false);
		//transform.DOMove(new Vector3(1,heightAscent,0) * speed, 10.0f, false);
		//Debug.Log("Distance left : " + distance);
	}

	/*public void shipTakeDamage(int i ) {
		if (currentHealth > 0) {
			currentHealth -= i;
			//healthSlider.value = currentHealth;
			//Debug.Log(currentHealth/100);
			//healthImage.DOFillAmount (((float)currentHealth) / 100, 0.5f);
			//Debug.Log (healthImage.fillAmount);
			//healthTxt.text = "Health: " + currentHealth;
			///innerCam.DOShakePosition(0.5f, 5.0f, 30);
		}
	}*/
}

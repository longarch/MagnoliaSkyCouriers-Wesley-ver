using UnityEngine;
using System.Collections;

public class enemyGun : MonoBehaviour {

	public GameObject enemyBullet;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void fireEnemyBullet()
	{
		GameObject playership = GameObject.Find ("Ship");

		if (playership != null) {
			
		}
	}
}

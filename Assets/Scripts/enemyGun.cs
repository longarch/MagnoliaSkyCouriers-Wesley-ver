using UnityEngine;
using System.Collections;

public class enemyGun : MonoBehaviour {

	public GameObject enemyBullet;
	// Use this for initialization
	void Start () {
		Invoke ("fireEnemyBullet", 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void fireEnemyBullet()
	{
		GameObject playership = GameObject.Find ("Ship");

		if (playership != null) {

			GameObject bullet = (GameObject)Instantiate (enemyBullet);

			bullet.transform.position = transform.position;

			Vector2 direction = playership.transform.position - bullet.transform.position;

			bullet.GetComponent<enemyBullet> ().setDirection (direction);
		}
	}
}

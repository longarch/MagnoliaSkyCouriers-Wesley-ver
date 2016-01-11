using UnityEngine;
using System.Collections;

public class enemyGun : MonoBehaviour {

	float eventTime = 10.0f;
	float fixedeventTime;

	public GameObject enemyBullet;
	// Use this for initialization
	void Start () {
		//Invoke ("fireEnemyBullet", 1.0f);
		fixedeventTime = eventTime;
	}
	
	// Update is called once per frame
	void Update () {
		eventTime -= Time.deltaTime;
		if (eventTime <= 0) {
			fireEnemyBullet ();
			eventTime = fixedeventTime;
		}
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

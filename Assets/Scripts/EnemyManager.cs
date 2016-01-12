using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	public List<GameObject> enemy;

	[SerializeField]
	GameObject raider;
	[SerializeField]
	GameObject dragon;
	[SerializeField]
	GameObject playerObject;
	Ship player;

	public GameObject enemies;
	float eventTime = 5.0f, spawnTime;
	int i, currentNo = 0, maxNo = 3;
	// Use this for initialization
	void Start () {
		spawnTime = eventTime;
		enemy = new List<GameObject> ();
		player = new Ship ();
		//InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	// Update is called once per frame
	void Update () {
		eventTime -= Time.deltaTime;
		if (eventTime <= 0) {
			Spawn ();
			eventTime = spawnTime;
		}
	}

	void Spawn()
	{
		if (player.currentHealth <= 0 || currentNo >= maxNo) {
			return;
		} else {
			i = randomType ();
			if (randomType () < 1) {
				enemies = Instantiate (dragon, new Vector3 (playerObject.transform.position.x - 10,
					Random.Range (playerObject.transform.position.y - 10, playerObject.transform.position.y + 10), 0), Quaternion.identity) as GameObject;
				enemy.Add (enemies);
				currentNo++;
				spawnTime = 15f; //change the time between spawns
			} else {
				enemies = Instantiate (raider, new Vector3 (playerObject.transform.position.x - 10,
					Random.Range (playerObject.transform.position.y - 10, playerObject.transform.position.y + 10), 0), Quaternion.identity) as GameObject;
				enemy.Add (enemies);
				currentNo++;
				spawnTime = 10f; //change the time between spawns
			}
		}
		Debug.Log (enemy.Count);
	}

	private int randomType()
	{
		int i = Random.Range (0, 3);

		return i;
	}
}
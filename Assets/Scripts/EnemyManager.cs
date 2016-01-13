using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	public List<BaseEnemy> enemy;

	[SerializeField]
	GameObject raider;
	[SerializeField]
	GameObject dragon;
	[SerializeField]
	GameObject playerObject;
    [SerializeField]
    Ship player;

	public BaseEnemy enemies;
	float eventTime = 5.0f, spawnTime;
	int i, currentNo = 0, maxNo = 3;
	// Use this for initialization
	void Start () {
		spawnTime = eventTime;
		enemy = new List<BaseEnemy> ();
		//player = new Ship ();
		//InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	// Update is called once per frame
	void Update () {

		if (GameManager.Instance.getStatus () == GameManager.STATE.START) {
			return;
		}

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
            //Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Debug.Log("Start Spawn 1 " + playerObject.transform.position.x);
            Debug.Log("Start Spawn 2 " + player.getPosition().x);
            i = randomType ();
			if (randomType () < 1) {
                //Debug.Log(playerObject.transform.position.x);
				enemies = EnemyPool.Instance.SpawnEnemy(new Vector3(playerObject.transform.position.x - 2,
                    Random.Range(player.getPosition().y - 10, player.getPosition().y + 10), 0), EnemyPool.EnemyType.Dragon);

                //Instantiate(dragon, new Vector3 (playerObject.transform.position.x - 10,
					//Random.Range (playerObject.transform.position.y - 10, playerObject.transform.position.y + 10), 0), Quaternion.identity) as GameObject;
				enemy.Add (enemies);
				currentNo++;
				spawnTime = 15f; //change the time between spawns
			} else {
				enemies = EnemyPool.Instance.SpawnEnemy(new Vector3(playerObject.transform.position.x - 2,
                    Random.Range(player.getPosition().y - 10, player.getPosition().y + 10), 0), EnemyPool.EnemyType.Raider);

                //Instantiate (raider, new Vector3 (playerObject.transform.position.x - 10,
                //Random.Range (playerObject.transform.position.y - 10, playerObject.transform.position.y + 10), 0), Quaternion.identity) as GameObject;
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
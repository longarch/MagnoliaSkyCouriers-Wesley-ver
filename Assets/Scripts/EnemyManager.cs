using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
	
	public List<GameObject> enemy;

	[SerializeField]

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void trackEnemySpawn() {
		randomType ();	
		//Instantiate (enemy[0], Vector3 (0, 0, 0), transform.rotation);
	}

	private void randomType()
	{
		int i = Random.Range (0, 1);
		if (i == 0) {
			//GameObject eSpawn = new Enemy (200, Enemy.Type.DRAGON);
			//enemy.Add (eSpawn);
		} else {
			//GameObject eSpawn = new Enemy (100, Enemy.Type.RAIDER);
			//enemy.Add (eSpawn);
		}
	}
}

public class Enemy
{
	public enum Type { RAIDER, DRAGON };
	public int health;
	public Type enemyType;

	public Enemy(int h, Type d)
	{	
		setHealth (h);
		setType (d);
	}

	public int getHealth()
	{
		return health;
	}

	public Type getType()
	{
		return enemyType;
	}
		
	public void setHealth (int h)
	{
		health = h;
	}

	public void setType (Type e)
	{
		enemyType = e;
	}

}

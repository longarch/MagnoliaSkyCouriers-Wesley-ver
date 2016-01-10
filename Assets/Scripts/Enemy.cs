using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	public enum Type { RAIDER, DRAGON };

	public List<GameObject> enemy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void trackEnemySpawn(GameObject obj) {
		enemy.Add (obj);
	}
}

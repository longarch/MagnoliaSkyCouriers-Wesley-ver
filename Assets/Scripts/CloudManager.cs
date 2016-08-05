using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class CloudManager : MonoBehaviour
{
	public float fireTime = .05f;
	//public GameObject bullet;
	//public int pooledAmount = 20;
	List<GameObject> clouds;

	void Start ()
	{		
	}

	void Update()
	{
		fireTime -= Time.deltaTime;
		if (fireTime <= 0) {
			cloudSpawn ();
			fireTime = Random.Range (0.15f, 1.0f);
		}
	}

	void cloudSpawn ()
	{
		Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (1, 0));
		Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));

		GameObject obj = CloudPool.current.GetPooledObject ();
		if (obj == null)
			return;
		obj.transform.position = new Vector2 (min.x + Random.Range (0.0f, 55.0f), Random.Range (min.y, max.y));

		//Randomizes between 0 and 1 so as to make clouds sometimes cover the ship / enemy
		obj.GetComponent<SpriteRenderer> ().sortingOrder = Random.Range (0, 2);
		//obj.transform.rotation = transform.rotation;
		obj.SetActive (true);
	}
}


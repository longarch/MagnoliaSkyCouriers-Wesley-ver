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
		InvokeRepeating ("Fire", fireTime, fireTime);
	}

	void Fire ()
	{
		Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (1, 0));
		Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));

		GameObject obj = CloudPool.current.GetPooledObject ();
		if (obj == null)
			return;
		obj.transform.position = new Vector2 (Random.Range (min.x, max.x+15), Random.Range (min.y, max.y));
		//obj.transform.rotation = transform.rotation;
		obj.SetActive (true);

	}
}


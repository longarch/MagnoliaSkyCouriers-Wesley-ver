using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class CloudPool : MonoBehaviour
{
	public enum CloudType
	{
		None,
		Cloud1,
		Cloud2
	}

	public static CloudPool current;
	public GameObject pooledObject;
	public int pooledAmount = 8;
	public bool willGrow = false;
	List<GameObject> pooledObjects;

	[SerializeField]
	private GameObject _cloud1Prefab, _cloud2Prefab;

	private static CloudPool _cloudInstance;

	public static CloudPool Instance {
		get {
			return _cloudInstance;
		}
	}

	void Awake ()
	{
		// Perform initialization of the Singleton
		if (_cloudInstance != null && _cloudInstance != this) {
			Destroy (gameObject);
		}
		_cloudInstance = this;

		current = this;
	}

	// Use this for initialization
	void Start ()
	{		
		InitPool ();
	}

	public GameObject GetPooledObject()
	{
		for(int i = 0; i < pooledObjects.Count; i++)
		{
			if(!pooledObjects[i].activeInHierarchy)
			{
				return pooledObjects[i];
			}
		}
		if (willGrow)
		{
			GameObject obj = (GameObject)Instantiate(pooledObject);
			pooledObjects.Add(obj);
			return obj;
		}
		return null;
	}

	void InitPool ()
	{		
		pooledObjects = new List<GameObject> ();
		for (int i = 0; i < pooledAmount; i++) {
			if (Random.Range (0, 4) < 2) {
				pooledObject = _cloud1Prefab;
			} else {
				pooledObject = _cloud2Prefab;
			}
			GameObject obj = (GameObject)Instantiate (pooledObject);
			obj.SetActive (false);
			pooledObjects.Add (obj);
		}
	}
}
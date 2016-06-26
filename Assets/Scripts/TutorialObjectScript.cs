using UnityEngine;
using System.Collections;

public class TutorialObjectScript : MonoBehaviour {

	[SerializeField]
	bool _isComplete;



	// Use this for initialization
	void Start () {
	
		_isComplete = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log ("Touched something!");
		if (other.gameObject.layer == LayerMask.NameToLayer("Crew"))
		{
			_isComplete = true;
		}

	}
}

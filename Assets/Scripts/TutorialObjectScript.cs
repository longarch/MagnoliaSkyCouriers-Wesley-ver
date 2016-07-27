using UnityEngine;
using System.Collections;

public class TutorialObjectScript : MonoBehaviour {

	[SerializeField]
	bool _isComplete;

	TutorialGameManager _manager;
	int tutorialIndex = 0;

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
		//Trying to do something that is not current objective
		if (tutorialIndex != _manager.getTutorialIndex () && !_isComplete) {
			if (other.gameObject.layer == LayerMask.NameToLayer ("Crew")) {
				_isComplete = true;
				notifyGameManager ();
			}
		}

	}

	public void setTutorialIndex(int i )
	{
		tutorialIndex = i;
	}

	public void notifyGameManager()
	{

		_manager.GetNotified ();
	}

	public void setGameManager(TutorialGameManager manager)
	{
		_manager = manager;
	}

}

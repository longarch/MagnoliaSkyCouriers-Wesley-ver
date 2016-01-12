using UnityEngine;
using System.Collections;
using DG.Tweening;
public class EventManager : MonoBehaviour {

	[SerializeField]
	Camera innerCam;

	[SerializeField]
	GameObject raider;

	[SerializeField]
	GameObject dragon;

	[SerializeField]
	GameObject player;

	float eventTime = 1.0f;
	float fixedeventTime;
	float percent;

	[SerializeField]
	int eventCap = 2;

	int currentEventNo = 0;
	//Ship mc;
	// Use this for initialization
	void Start () {
		//Ship mc = new Ship ();
		//fixedeventTime = eventTime;
		//raider.gameObject.SetActive (false);
	}
	//gameManager tracks journey/distance to goal
	// convert that into %
	//force an encounter at a certain point

	//every time frame randomise number to get events RNG
	//reset timer after an event occurs

	//spawn a raider chasing after the ship

	// Update is called once per frame
	void Update () {
				//Debug.Log(GameManager.Instance.position);
		/*
		if (GameManager.Instance.position == 5) {

				//raider.gameObject.SetActive (true);
		}*/
	}
}

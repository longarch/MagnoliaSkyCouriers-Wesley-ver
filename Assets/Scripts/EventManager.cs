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
		fixedeventTime = eventTime;
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
		eventTime -= Time.deltaTime;
		if (eventTime <= 0) {
			//innerCam.DOShakePosition(0.5f, 5.0f, 30);
			//GameManager.Instance.takeDamage(5);
			int i = Random.Range(0,2);
			
			if (i == 0 && currentEventNo <= 2) {
				//raider.gameObject.transform.position = new Vector3 (player.transform.position.x - 5, player.transform.position.y, 0);
				int j = Random.Range(0,10);
				if (j < 5) {
					Instantiate (raider, new Vector3 (player.transform.position.x -9,
						Random.Range (player.transform.position.y - 10, player.transform.position.y + 10), 0), Quaternion.identity);
					currentEventNo++;
				} else {
					//Instantiate (raider, new Vector3 (player.transform.position.x - 3,
					//	Random.Range (player.transform.position.y - 10, player.transform.position.y + 10), 0), Quaternion.identity);
					Instantiate(dragon, new Vector3 (player.transform.position.x - 13,
					Random.Range (player.transform.position.y - 10, player.transform.position.y + 10), 0), Quaternion.identity);
					currentEventNo++;
				}	
			}

			eventTime = fixedeventTime;
		}
		//Debug.Log(GameManager.Instance.position);
		/*
		if (GameManager.Instance.position == 5) {

				//raider.gameObject.SetActive (true);
		}*/
	}
}

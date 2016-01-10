using UnityEngine;
using System.Collections;
using DG.Tweening;
public class EventManager : MonoBehaviour {

	[SerializeField]
	Camera innerCam;

	[SerializeField]
	GameObject raider;

	[SerializeField]
	GameObject player;

	float eventTime = 1.0f;
	float fixedeventTime;
	float percent;
	//Ship mc;
	// Use this for initialization
	void Start () {
		//Ship mc = new Ship ();
		fixedeventTime = eventTime;
		raider.gameObject.SetActive (false);
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

			eventTime = fixedeventTime;
		}
		Debug.Log(GameManager.Instance.position);
		if (GameManager.Instance.position >= 5) {			
			if (!raider.gameObject.activeSelf) {
				raider.gameObject.transform.position = new Vector3 (player.transform.position.x - 5, player.transform.position.y, 0);
			}
				raider.gameObject.SetActive (true);
		}
	}
}

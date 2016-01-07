using UnityEngine;
using System.Collections;
using DG.Tweening;
public class EventManager : MonoBehaviour {

	[SerializeField]
	Camera innerCam;

	[SerializeField]
	float eventTime = 1.0f;
	float fixedeventTime;
	// Use this for initialization
	void Start () {

		fixedeventTime = eventTime;
	}
	
	// Update is called once per frame
	void Update () {
	
		eventTime -= Time.deltaTime;
		if (eventTime <= 0) {
			innerCam.DOShakePosition(0.5f, 5.0f, 30);
			eventTime = fixedeventTime;
		}

	}
}

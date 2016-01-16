using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;


public class ResultsMenu : MonoBehaviour {

	
	[SerializeField]
	Transform _transform;

	[SerializeField]
	Image pnl_MainMenu;

	[SerializeField]
	GameObject ship;

	// Use this for initialization
	void Start () {
		if (_transform != null) {
			Camera.main.transform.DOMoveY (_transform.position.y, 2.0f, false).SetEase (Ease.OutCubic).OnComplete(() =>
			                                                                                                      {
				//btn_start.gameObject.SetActive(true);
				//btn_quit.gameObject.SetActive(true);
				//btn_credits.gameObject.SetActive(true);
				//camera.orthographic = !currentMode;
				//_spriteFeedback.SetActive (false);
				//gameObject.SetActive(false);
				ship.transform.DOLocalMove(new Vector3(50,10,0),10.0f,false);
				pnl_MainMenu.gameObject.SetActive (true); 
			});
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void quitGame() {
		Application.Quit ();
	}

	public void loadlevel(string s ) {
		Application.LoadLevel (s);
	}
}

using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
public class MainMenuScript : MonoBehaviour {

	[SerializeField]
	Transform _transform;

	[SerializeField]
	Button btn_start,btn_quit,btn_credits;

	// Use this for initialization
	void Start () {
	
		if (_transform != null) {
			Camera.main.transform.DOMoveY (_transform.position.y, 2.0f, false).SetEase (Ease.OutCubic).OnComplete(() =>
			                                                                                                      {
				btn_start.gameObject.SetActive(true);
				btn_quit.gameObject.SetActive(true);
				btn_credits.gameObject.SetActive(true);
				//camera.orthographic = !currentMode;
				//_spriteFeedback.SetActive (false);
				//gameObject.SetActive(false);
			});
		}

	}


	// Update is called once per frame
	void Update () {
	
	}

	public void loadLevel(string level)
	{
		Application.LoadLevel (level);
	}

	public void quitgame()
	{
		Application.Quit();
	}

}

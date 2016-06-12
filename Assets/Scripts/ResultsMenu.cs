using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;


public class ResultsMenu : MonoBehaviour {

	DataStoresHandler _dataStores;
	
	[SerializeField]
	Transform _transform;

	[SerializeField]
	Image pnl_MainMenu;

	[SerializeField]
	GameObject ship;

	[SerializeField]
	Text _txt_enemyNO,_txt_ShipHP,_txt_CargoHP, _txt_Difficulty,_txt_FinalScore;


	// Use this for initialization
	void Start () {

		_dataStores = FindObjectOfType<DataStoresHandler> ();

		if (_dataStores != null) {
			UpdateUI();
		}

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

		Destroy (_dataStores.gameObject);
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

	void UpdateUI()
	{
		_txt_enemyNO.text = "Enemies defeated: " + _dataStores.enemyCount + " x 100";
		_txt_CargoHP.text = "Cargo Health: " + _dataStores.cargoHP + " x 200";
		_txt_ShipHP.text = "Ship Health: " + _dataStores.shipHP + " x 100";
		_txt_Difficulty.text = "Difficulty Multiplier: " + _dataStores.difficultyMultiplier;
		_txt_FinalScore.text = "Your Score: " + calculateFinalScore().ToString();


	}


	int calculateFinalScore()
	{
		int finalScore = (int)(_dataStores.enemyCount * 100 + _dataStores.cargoHP * 200 + _dataStores.shipHP * 100
			* _dataStores.difficultyMultiplier);

		return finalScore;

	}

}

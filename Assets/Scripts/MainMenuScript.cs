using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
public class MainMenuScript : MonoBehaviour {

	[SerializeField]
	Transform _transform;


	[SerializeField]
	Text txt_description,txt_Difficulty;

	[SerializeField]
	Button btn_start,btn_quit,btn_credits;

	[SerializeField]
	Image img_Character, img_statusPlaceholder;

	[SerializeField]
	Image pnl_LeaderSelection,pnl_MainMenu,pnl_Fader, pnl_Credits, pnl_HowtoPlay, pnl_Facilities, pnl_Controls,pnl_Objectives;



	[SerializeField]
	Sprite humanSpr,elfSpr,wolfSpr,fairySpr;

	[SerializeField]
	Sprite normalPlace,hardPlace;

	[SerializeField]
	List<string> descriptionStrings;

	[SerializeField]
	LevelLoadHandler _levelHandler;

	int currentSelection = 0;
	int currentTutorial = 0;
	int currentDifficulty = 0;

	void Awake() {
		pnl_LeaderSelection.gameObject.SetActive (false);
		pnl_Credits.gameObject.SetActive (false);
		pnl_HowtoPlay.gameObject.SetActive (false);
		//pnl_MainMenu.gameObject.SetActive (false);
	}

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
				pnl_MainMenu.gameObject.SetActive (true); 
			});
		}

	}


	// Update is called once per frame
	void Update () {
	
	}

	public void LoadProperLevel(string s)
	{
		Sequence sequence = DOTween.Sequence();
		sequence.Append(ShrinkLevelSelectPanel());
		sequence.AppendCallback(() =>
		                        {
			Application.LoadLevel (s);
		});
	}
	
	
	public Tween ShrinkLevelSelectPanel()
	{
		
		pnl_Fader.gameObject.SetActive(true);
		
		Camera.main.DOOrthoSize(0.01f, 1.5f).SetEase(Ease.InQuint);
		return pnl_Fader.DOFade(1, 1.5f);
	}

	public void quitgame()
	{
		Application.Quit();
	}




	public void proceedtoSelectLeader()
	{
		pnl_MainMenu.gameObject.SetActive (false); 
		pnl_LeaderSelection.gameObject.SetActive (true);
	}

	public void proceedtoCredits()
	{
		pnl_MainMenu.gameObject.SetActive (false); 
		pnl_Credits.gameObject.SetActive (true);
	}

	public void proceedtoHowtoPlay()
	{
		pnl_MainMenu.gameObject.SetActive (false); 
		pnl_HowtoPlay.gameObject.SetActive (true);
	}

	public void updateTutorialForward()
	{
		currentTutorial++;
		if (currentTutorial >= 3) {
			currentTutorial = 0;
		}
		refreshTutorial (currentTutorial);

	}

	public void updateTutorialBackward()
	{
		currentTutorial--;
		if (currentTutorial < 0) {
			currentTutorial = 2;
		}
		refreshTutorial (currentTutorial);
		
	}



	public void backtoMainMenu()
	{
		pnl_MainMenu.gameObject.SetActive (true); 
		pnl_LeaderSelection.gameObject.SetActive (false);
		pnl_Credits.gameObject.SetActive (false);
		pnl_HowtoPlay.gameObject.SetActive (false);
	}

	public void updateSpriteForward()
	{
		currentSelection++;
		if (currentSelection >= 4) {
			currentSelection = 0;
		}
		refreshSprite (currentSelection);
		_levelHandler.setLeader (currentSelection);
		if (descriptionStrings.Count > 3) {
			txt_description.text = descriptionStrings [currentSelection];
		}
	}



	public void updateSpriteBackwards()
	{
		currentSelection--;
		if (currentSelection < 0) {
			currentSelection = 3;
		}
		refreshSprite (currentSelection);
		_levelHandler.setLeader (currentSelection);
		if (descriptionStrings.Count > 3) {
			txt_description.text = descriptionStrings [currentSelection];
		}
	}

	public void refreshTutorial(int i)
	{
		pnl_Objectives.gameObject.SetActive(false);
		pnl_Facilities.gameObject.SetActive(false);
		pnl_Controls.gameObject.SetActive(false);
		switch (i) {
		case 0:
			pnl_Objectives.gameObject.SetActive(true);

			break;
		case 1:
			pnl_Controls.gameObject.SetActive(true);
			break;
		case 2:
			pnl_Facilities.gameObject.SetActive(true);
			break;
		}
	}

	public void updateDifficultyBackwards()
	{
		currentDifficulty++;
		if (currentDifficulty >= 2) {
			currentDifficulty = 0;
		}
		refreshDifficulty (currentDifficulty);

		
	}

	public void updateDifficultyForward()
	{
		currentDifficulty--;
		if (currentDifficulty < 0) {
			currentDifficulty = 1;
		}
		refreshDifficulty (currentDifficulty);


	}

	public void refreshDifficulty (int i)
	{
		switch (i) {
		case 0:
			img_statusPlaceholder.sprite = normalPlace;
			txt_Difficulty.text = "Normal";
			break;
		case 1:
			img_statusPlaceholder.sprite = hardPlace;
			txt_Difficulty.text = "Hard";
			break;
		case 2:

			break;
		}

	}

	public void refreshSprite(int i)
	{
		switch (i) {
		case 0:
			img_Character.sprite = humanSpr;
			break;
		case 1:
			img_Character.sprite = elfSpr;
			break;
		case 2:
			img_Character.sprite = wolfSpr;
			break;
		case 3:
			img_Character.sprite = fairySpr;
			break;
		}
	}


}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

//detect which scene
public class UIManager : MonoBehaviour {

	//public enum Tutorial1Phase
	//{
	//	PreStart,
	//	Dialog,
	//	Action,
	//	Ending
	//}

	[SerializeField]
	Text txtInstruction1,txtOpeningText,txtStatus;
	[SerializeField]
	Button start_btn, dlogNext_btn, dlogCom_btn,btn_pause;
	[SerializeField]
	Image pausePnl,DialogPnl,openingPnl;
	[SerializeField]
	List<string> Dialogs, Dialogs_2, Dialogs_3;

	string emptySpeech = "";
	private bool paused;
	int current = 0, dialogCount = 0;

	[SerializeField]
	string next_level,main_title;

	private int menuState = 0;

	//public Tutorial1Phase TPhase = new Tutorial1Phase ();

	private static UIManager _instance = null;
	//public CargoManager c;

	bool isDebug = false;

	public static UIManager Instance
	{
		get { return _instance; }
	}

	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		_instance = this;

	}

	// Use this for initialization
	void Start () {

		//TPhase = Tutorial1Phase.PreStart;
		if (pausePnl != null) {
			pausePnl.gameObject.SetActive (false);
		}
		if (dlogCom_btn != null) {
			dlogCom_btn.gameObject.SetActive (false);
		}

		startOpeningSequence ();

		paused = false;
		//DialogManager.Instance.returnDialogs (0);
		//Dialogs.Add(" ");
		//Debug.Log(GameManager.Instance.getStatus ());
		if (GameManager.Instance.getStatus () == GameManager.STATE.TUTORIAL) { //Application.loadedLevelName.Contains ("DialogScene")
			DialogPnl.gameObject.SetActive (true);
			if (Dialogs != null && isDebug) {						
				setUpDialog (DialogManager.Instance.returnDialogs (0).getListofTexts ());
			}
		}
		else {
			DialogPnl.gameObject.SetActive (false);
		}


	}

	// Update is called once per frame
	void Update () {
		if (Application.loadedLevelName.Contains ("DialogScene")) {
			if (Dialogs != null && isDebug) {	
				if (Input.GetButtonDown ("Jump") && current < Dialogs.Count - 1) {
					current += 1;
					StartCoroutine (TypeText (Dialogs [current]));
				}
				txtInstruction1.text = emptySpeech;
			}
		}

		if (GameManager.Instance.getStatus () == GameManager.STATE.CARGOLOST) {
			txtStatus.text = "Ship Lost!";
			startGameOverSequence ();
		} else if (GameManager.Instance.getStatus () == GameManager.STATE.GOAL) {
			txtStatus.text = "Successful delivery!";
			startGameOverSequence ();
		}
	}

	IEnumerator TypeText (string speech) {
		emptySpeech = "";
		foreach (char letter in speech) {
			emptySpeech += letter;
			yield return 0;
			yield return new WaitForEndOfFrame();
		}      
	}

	public void triggerNextDialog()
	{
		txtInstruction1.text = "";
		if (current < Dialogs.Count - 1) {
			current += 1;
			StopAllCoroutines();
			StartCoroutine (TypeText (Dialogs [current]));
		}

		//Last line of dialog!

		if (current == Dialogs.Count - 1) {
			//if (dialogCount < 2) {
			//	dialogCount++;
				//Debug.Log (dialogCount);
				//setUpDialog (dialogCount);
			//} else {				
				dlogCom_btn.gameObject.SetActive (true);
			//}						
		}
	}

	public void startOpeningSequence()
	{

		Sequence sequence = DOTween.Sequence();
		//DOTween.Complete (_spriteFeedback.transform);
		sequence.Insert(0.1f,openingPnl.DOFade(0.5f,3.0f));
		sequence.Insert(0.1f,txtOpeningText.rectTransform.DOAnchorPosY(0,1.0f,false));
		sequence.Append(openingPnl.DOFade(0,1.0f));
		//sequence.Insert(1.1f,txtOpeningText.rectTransform.DOAnchorPosY(800,1.0f,false));
		sequence.OnComplete(() =>
		                    {
			txtOpeningText.gameObject.SetActive(false);
			openingPnl.gameObject.SetActive(false);

			//camera.orthographic = !currentMode;
			//_spriteFeedback.SetActive (false);
			//gameObject.SetActive(false);
		});
	}

	public void startGameOverSequence()
	{
		openingPnl.gameObject.SetActive (true);
		//Time.timeScale = 0.25f;
		btn_pause.gameObject.SetActive (false);

		Sequence sequence = DOTween.Sequence ();
		sequence.Append (openingPnl.DOFade (1.0f, 1.0f));
		sequence.Insert (1.5f,txtStatus.rectTransform.DOAnchorPosY(0.0f,1.0f,false));
		sequence.OnComplete (() =>
		                     {
			
			//txt_Status.gameObject.SetActive(true);
			//txt_Status.DOFade (1.0f,1.0f);
			//txt_Status.DOFade (0.0f,6.0f);
			
			
			StartCoroutine(Wait(5));
			
			//Application.LoadLevel("Level_1");
			//img_Fader.gameObject.SetActive(false);
			//IsGameStart = true;
		});
		
	}

	IEnumerator Wait(float duration)
	{
		//This is a coroutine
		//Debug.Log("Start Wait() function. The time is: "+Time.time);
		//Debug.Log( "Float duration = "+duration);
		yield return new WaitForSeconds(duration);   //Wait

		if (GameManager.Instance.getStatus () == GameManager.STATE.CARGOLOST) {
			Application.LoadLevel (main_title);
		} else {
			Application.LoadLevel (next_level);
		}
		//Debug.Log("End Wait() function and the time is: "+Time.time);
	}


	public void HideDialogBox()
	{
		//TPhase = Tutorial1Phase.Action;
		DialogPnl.gameObject.SetActive (false);
	}

	public void setUpDialog(int i)
	{
		//TPhase = Tutorial1Phase.Dialog;
		DialogPnl.gameObject.SetActive (true);
		dlogCom_btn.gameObject.SetActive (false);
		current = 0;
		Dialogs.Clear ();
		switch (i) {
		case 1:
			for (int j = 0; j < Dialogs_2.Count; j ++) {
				Dialogs.Add (Dialogs_2[j]);
			}
			break;
		case 2:
			for (int j= 0; j < Dialogs_3.Count; j ++) {
				Dialogs.Add (Dialogs_3[j]);
			}
			break;
		}

		StopAllCoroutines();
		StartCoroutine (TypeText (Dialogs [current]));


		//tutNextBtn.onClick.AddListener (() => triggerNextDialog_2());
	}

	public void setUpDialog(List<string> newDialog)
	{
		//TPhase = Tutorial1Phase.Dialog;
		DialogPnl.gameObject.SetActive (true);
		dlogCom_btn.gameObject.SetActive (false);
		current = 0;
		Dialogs.Clear ();

		for (int j = 0; j < newDialog.Count; j ++) {			
			Dialogs.Add (newDialog[j]);
		}

		StopAllCoroutines();
		StartCoroutine(TypeText (Dialogs [current]));

		//tutNextBtn.onClick.AddListener (() => triggerNextDialog_2());
	}

	public void loadLevel(string s)
	{
		Time.timeScale = 1.0f;
		Application.LoadLevel (s);
	}	

	public void startButton ()
	{		
		//start_btn.gameObject.transform.DOMoveX (start_btn.gameObject.transform.position.x - 1.0f, 2.0f, false);
		//loadLevel("Level_1");
	}

	public void pause_btn() //in-game pause button
	{
		if (paused) {
			Sequence sequence = DOTween.Sequence();
			//sequence.Complete();
			sequence.Append(pausePnl.rectTransform.DOLocalMoveY(950, 1.0f, false));

			Time.timeScale = 1.0f;
			paused = false;
			pausePnl.gameObject.SetActive (false);
		} else {			
			pausePnl.gameObject.SetActive (true);
			Sequence sequence = DOTween.Sequence();
			//sequence.Complete ();
			sequence.SetUpdate (true);
			//DOTween.Complete (pausePnl.rectTransform);
			sequence.Append(pausePnl.rectTransform.DOLocalMoveY(0, 1.0f, false));

			Time.timeScale = 0.0f;
			paused = true;
			//Debug.Log (pausePnl.gameObject.transform.position.y);
		}				
	}

	public void restart_btn()
	{
		Time.timeScale = 1.0f;
		Application.LoadLevel (Application.loadedLevelName);
		//cargo test
		//c.cargoDamaged ("Cargo1", 5);
	}

	public void quit_btn()
	{
		// check if in main menu or in-game
		//loadLevel("SelectionScene_2");
		Application.Quit ();
	}

	public void dialogNext()
	{
		triggerNextDialog ();
	}

	public void dialogComplete()
	{
		HideDialogBox ();
	}

	private void initGameState()
	{
		switch (menuState) {
		case 1: //start screen
			break;
		case 2: //menu screen
			break;
		case 3: //game screen
			break;
		}
	}
}

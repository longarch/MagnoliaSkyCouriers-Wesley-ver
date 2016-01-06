using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CrewSelTutorialHandler : MonoBehaviour {

	public enum Tutorial1Phase
	{
		PreStart,
		Dialog,
		Action,
		Ending
	}

	[SerializeField]
	Text txtInstruction1;

	[SerializeField]
	Button tutCompleteBtn,txtNextBtn;

	[SerializeField]
	Image DialogPnl;

	string emptySpeech = "";

	[SerializeField]
	List<string> Dialogs,Dialogs_2,Dialogs_3;
	int current = 0;

	public Tutorial1Phase TPhase = new Tutorial1Phase ();

	private static CrewSelTutorialHandler _instance = null;

	private bool _isSelectedCat, _isClickedProject, _isCrewFull;

	public static CrewSelTutorialHandler Instance
	{
		get { return _instance; }
	}

	public bool isSelectedCat()
	{
		return _isSelectedCat;
	}

	public void setCatSelected()
	{
		_isSelectedCat = true;
	}

	public bool isSelectedProject()
	{
		return _isClickedProject;
	}
	
	public void setProjectSelected()
	{
		_isClickedProject = true;
	}

	public bool isCrewFull()
	{
		return _isCrewFull;
	}
	
	public void setCrewFull()
	{
		_isCrewFull = true;
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

		TPhase = Tutorial1Phase.PreStart;
		HideCompleteBtn ();
		_isSelectedCat = false;
		_isClickedProject = false;
		_isCrewFull = false;
		if (Dialogs != null) {
			StartCoroutine (TypeText (Dialogs [current]));
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetButtonDown ("Jump") && current < Dialogs.Count - 1) {
			
			
			current += 1;
			StartCoroutine(TypeText (Dialogs[current]));


		}
		txtInstruction1.text = emptySpeech;

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
		if (current == Dialogs.Count - 1)
		{
			
			tutCompleteBtn.gameObject.SetActive(true);
		}

	}

	public void HideCompleteBtn()
	{
		tutCompleteBtn.gameObject.SetActive (false);
	}

	public void HideDialogBox()
	{
		TPhase = Tutorial1Phase.Action;
		DialogPnl.gameObject.SetActive (false);
	}

	public void setUpDialog(int i)
	{
		TPhase = Tutorial1Phase.Dialog;
		DialogPnl.gameObject.SetActive (true);
		tutCompleteBtn.gameObject.SetActive (false);
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


		//txtNextBtn.onClick.AddListener (() => triggerNextDialog_2());
	}

	public void setUpDialog(List<string> newDialog)
	{
		TPhase = Tutorial1Phase.Dialog;
		DialogPnl.gameObject.SetActive (true);
		tutCompleteBtn.gameObject.SetActive (false);
		current = 0;
		Dialogs.Clear ();

		for (int j = 0; j < newDialog.Count; j ++) {
			Dialogs.Add (newDialog[j]);
		}


		
		
		StopAllCoroutines();
		StartCoroutine (TypeText (Dialogs [current]));
		
		
		//txtNextBtn.onClick.AddListener (() => triggerNextDialog_2());
	}






}

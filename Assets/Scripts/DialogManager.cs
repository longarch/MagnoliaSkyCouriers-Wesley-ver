using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
public class DialogManager : MonoBehaviour {
	
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
	public TextAsset TutorialXML;
	private XmlDocument xmlDoc;
	
	[SerializeField]
	List<string> Dialogs,Dialogs_2,Dialogs_3;
	
	[SerializeField]
	List<Sprite> _NPCFaces;
	
	[SerializeField]
	Image _NPCMainImage;
	
	
	
	private List<List<string>> _listofDialogs;
	private List<List<int>> _listofSpriteRef;
	
	int current = 0;
	
	public Tutorial1Phase TPhase = new Tutorial1Phase ();
	
	private static DialogManager _instance = null;
	
	private bool _isSelectedCat, _isClickedProject, _isCrewFull, _isinTutorial;
	
	private bool additionalEvent = false;
	
	private int additionalEventInstance = 0;
	
	private int dialogInstance = 0;
	
	//For rich Text testing
	bool bold = false; //toggles the style for bold;
	bool red = false; // toggle red
	bool italics = false;
	
	bool ignore = false;//for ignoring special characters that toggle styles
	
	public static DialogManager Instance
	{
		get { return _instance; }
	}
	
	public bool isSelectedCat()
	{
		return _isSelectedCat;
	}
	
	public bool isinTutorial()
	{
		return _isinTutorial;
	}
	
	public void setInTutorial(bool t)
	{
		_isinTutorial = t;
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


		TPhase = Tutorial1Phase.PreStart;
		HideCompleteBtn ();
		_isSelectedCat = false;
		_isClickedProject = false;
		_isCrewFull = false;
		_isinTutorial = false;
		_listofDialogs = new List<List<string>> ();
		_listofSpriteRef = new List<List<int>> ();
		Dialogs = new List<string> ();
		loadXMLFromAsset ();
		readXml ();
	}
	
	// Use this for initialization
	void Start () {
		

		//setUpDialog (_listofDialogs[0]);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetButtonDown ("Jump") && current < Dialogs.Count - 1) {
			
			
			//current += 1;
			triggerNextDialog();
			
			
		}
		txtInstruction1.text = emptySpeech;
		
	}
	
	IEnumerator TypeText (string speech) {
		emptySpeech = "";
		foreach (char letter in speech) {
			
			if (letter == '@')
			{
				red = !red;
				
				continue;
			}
			
			emptySpeech += letter;
			
			
			yield return 0;
			yield return new WaitForEndOfFrame();
		}      
	}
	
	IEnumerator TypeRichText (string speech) {
		
		emptySpeech = "";
		bool bold = false; //toggles the style for bold;
		bool red = false; // toggle red
		bool yellow = false;
		bool italics = false;
		
		bool ignore = false; //for ignoring special characters that toggle styles
		
		foreach (char nextletter in speech) {
			
			switch (nextletter) {
			case '[':
				ignore = true;
				yellow = !yellow;
				break;
			case '@':
				ignore = true; //make sure this character isn't printed by ignoring it
				red = !red; //toggle red styling
				break;
			case '¬':
				ignore = true; //make sure this character isn't printed by ignoring it
				bold = !bold; //toggle bold styling
				break;
			case '/':
				ignore = true; //make sure this character isn't printed by ignoring it
				italics = !italics; //toggle italic styling
				break;
			}
			
			
			string letter = nextletter.ToString();
			
			if (!ignore) {
				
				if (bold){
					
					letter = "<b>"+letter+"</b>";
					
				}
				if (italics){
					
					letter = "<i>"+letter+"</i>";
					
				}
				if (red){
					
					letter = "<color=#ff0000>"+letter+"</color>";
					
				}
				if (yellow){
					
					letter = "<color=#ffff00>"+letter+"</color>";
					
				}
				
				emptySpeech += letter;
				
			}
			//make sure the next character isn't ignored
			ignore = false;
			yield return 0;
			yield return new WaitForEndOfFrame();
		}
	}
	
	//Incomplete function
	public void forceDialogComplete(string speech)
	{
		emptySpeech = speech;
	}
	
	public void triggerNextDialog()
	{
		txtInstruction1.text = "";
		if (current < Dialogs.Count - 1) {
			current += 1;
			StopAllCoroutines();
			StartCoroutine (TypeRichText (Dialogs [current]));
		}
		
		_NPCMainImage.sprite = _NPCFaces[_listofSpriteRef [dialogInstance] [current]];
		
		//Last line of dialog!
		if (current == Dialogs.Count - 1)
		{
			
			tutCompleteBtn.gameObject.SetActive(true);
		}
		
	}
	
	public void HideCompleteBtn()
	{
		tutCompleteBtn.gameObject.SetActive (false);
		//_NPCMainImage.gameObject.SetActive (false);
	}
	
	public void HideDialogBox()
	{
		TPhase = Tutorial1Phase.Action;
		DialogPnl.gameObject.SetActive (false);
		_NPCMainImage.gameObject.SetActive (false);
		setInTutorial (false);
		if (additionalEvent) {
			setUpDialog(additionalEventInstance);
			additionalEvent = false;
		}
		
		
		
		
	}
	
	public void setUpDialog(int i)
	{
		TPhase = Tutorial1Phase.Dialog;
		DialogPnl.gameObject.SetActive (true);
		_NPCMainImage.gameObject.SetActive (true);
		tutCompleteBtn.gameObject.SetActive (false);
		current = 0;
		Dialogs.Clear ();
		for (int j = 0; j < _listofDialogs[i].Count; j ++) {
			Dialogs.Add (_listofDialogs[i][j]);
		}
		dialogInstance = i;
		_NPCMainImage.sprite = _NPCFaces[_listofSpriteRef [dialogInstance] [0]];
		
		/*
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
		}*/
		
		StopAllCoroutines();
		//StartCoroutine (TypeText (Dialogs [current]));
		StartCoroutine (TypeRichText (Dialogs [current]));
		
		//txtNextBtn.onClick.AddListener (() => triggerNextDialog_2());
	}
	
	
	
	//In case there is a need to add additional dialog
	public void dialogStack(List<string> extraDialog)
	{
		for (int j = 0; j < extraDialog.Count; j ++) {
			Dialogs.Add (extraDialog[j]);
		}
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
		StartCoroutine (TypeRichText (Dialogs [current]));
		
		
		//txtNextBtn.onClick.AddListener (() => triggerNextDialog_2());
	}
	
	public void setUpDialog(string singleLine)
	{
		TPhase = Tutorial1Phase.Dialog;
		DialogPnl.gameObject.SetActive (true);
		//tutCompleteBtn.gameObject.SetActive (false);
		current = 0;
		Dialogs.Clear ();
		Dialogs.Add (singleLine);
		
		
		
		
		StopAllCoroutines();
		StartCoroutine (TypeRichText (Dialogs [current]));
		
		
		//txtNextBtn.onClick.AddListener (() => triggerNextDialog_2());
	}
	
	// Following method load xml file from resouces folder under Assets
	private void loadXMLFromAsset()
	{
		//TextAsset tipsFile = (TextAsset)Resources.Load("Projects", typeof(TextAsset));
		xmlDoc = new XmlDocument();
		xmlDoc.LoadXml (TutorialXML.text);
		//xmlDoc.Load( Application.persistentDataPath + "/Resources/Projects.xml" );
		//TextAsset tipsFile = Resources.Load("CatData") as TextAsset;
		//xmlDoc.Load(tipsFile.text);
		//xmlDoc.Load(tipsFile.text);
	}
	
	private void readXml()
	{
		//bug.Log (xmlDoc.Name);
		//Debug.Log ("Im here again");
		
		int currentID = 0;
		foreach(XmlElement node in xmlDoc.SelectNodes("DialogCollection/Dialogs/Dialog"))
		{
			
			List<string> _tempList = new List<string>();
			List<int> _tempRef = new List<int>();
			for (int i = 0; i < node.SelectSingleNode("Texts").ChildNodes.Count; i ++)
			{
				_tempList.Add(node.SelectSingleNode("Texts").ChildNodes[i].InnerText);
				_tempRef.Add(int.Parse(node.SelectSingleNode("Texts").ChildNodes[i].Attributes["face"].InnerText));
				//Debug.Log("Adding " + _tempList[i]);
			}
			
			
			
			_listofDialogs.Add(_tempList);
			_listofSpriteRef.Add(_tempRef);
			
			
			
			//_dialogDictionary.Add(currentID,_tempList);
			//Debug.Log ("Now is" + _dialogDictionary[0][0]);
			//_tempList.Clear();
			//currentID++;
			
			//Debug.Log("Adding: "  + node.Name);
			//Debug.Log("Dialog dictionary counter: " + _dialogDictionary.Count);
			/*
			ProjID.Add(int.Parse (node.Attributes["id"].InnerText));
			ProjName.Add(node.SelectSingleNode("Name").InnerText);
			ProjDescription.Add (node.SelectSingleNode("Description").InnerText);
			ProjReq.Add (node.SelectSingleNode("Requirements").InnerText);
			ProjCritCats.Add (int.Parse(node.SelectSingleNode("CriticalCat").InnerText));
			*/
			
		}
	}
	
	
	
	
	
}

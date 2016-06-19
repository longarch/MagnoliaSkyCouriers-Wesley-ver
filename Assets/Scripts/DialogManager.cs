using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class DialogManager : MonoBehaviour {



	private static DialogManager _instance = null;

	public class Dialog
	{



		public List<string> texts;

		public Dialog()
		{

		}

		public Dialog(TextAsset assets)
		{
			texts = new List<string>();

			if (assets != null)
			{
				loadXMLFromAsset(assets);
			}

		}

		// Following method load xml file from resouces folder under Assets
		private void loadXMLFromAsset(TextAsset asset)
		{
			//Loads XML
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml (asset.text);

			//Reads XML
			foreach (XmlElement node in xmlDoc.SelectNodes("DialogCollection/Dialogs")) {
				for (int i = 0; i < node.SelectSingleNode("Dialog").ChildNodes.Count; i ++)
				{
					//p.ProjReq.Add(node);
					texts.Add(node.SelectSingleNode("Dialog").ChildNodes[i].InnerText);
					
				}

			}
			//xmlDoc.Load( Application.persistentDataPath + "/Resources/Projects.xml" );
			//TextAsset tipsFile = Resources.Load("CatData") as TextAsset;
			//xmlDoc.Load(tipsFile.text);
			//xmlDoc.Load(tipsFile.text);
		}

		public List<string> getListofTexts()
		{
			return texts;
		}
	}

	[SerializeField]
	TextAsset asset;

	List<Dialog> _Dialogs;
	


	public static DialogManager Instance
	{
		get { return _instance; }
	}


	// Use this for initialization
	void Awake () {
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		_instance = this;

		_Dialogs =  new List<Dialog>();

		Dialog D = new Dialog (asset);

		_Dialogs.Add (D);
	}



	void Start() {

	}

	public Dialog returnDialogs(int i)
	{
		return _Dialogs[i];
	}
}

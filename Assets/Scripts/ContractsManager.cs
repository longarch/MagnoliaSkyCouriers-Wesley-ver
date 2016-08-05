using UnityEngine;
using System.Collections;
using System.Xml;

public class ContractsManager : MonoBehaviour {

	XmlDocument xmlDoc;
	TextAsset contractXML;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	// Following method load xml file from resouces folder under Assets
	private void loadXMLFromAsset()
	{
		//TextAsset tipsFile = (TextAsset)Resources.Load("Projects", typeof(TextAsset));
		xmlDoc = new XmlDocument();
		xmlDoc.LoadXml (contractXML.text);

		foreach (XmlElement node in xmlDoc.SelectNodes("ContractsCollection/Contracts")) {

			Contract c = new Contract();

			c.setContractID(int.Parse(node.SelectSingleNode("Contract").InnerText));
			c.setCargoHealth(int.Parse(node.SelectSingleNode("Contract").InnerText));
			c.setContractDescription(node.SelectSingleNode("Contract").InnerText);

			c.setContractDifficulty(int.Parse(node.SelectSingleNode("Contract").InnerText));
			
			c.setContractType(int.Parse(node.SelectSingleNode("Contract").InnerText));

		}

		//xmlDoc.Load( Application.persistentDataPath + "/Resources/Projects.xml" );
		//TextAsset tipsFile = Resources.Load("CatData") as TextAsset;
		//xmlDoc.Load(tipsFile.text);
		//xmlDoc.Load(tipsFile.text);
	}
}

public class Contract {


	private int _contractID;
	private string _contractDescription;
	private int _cargoHealth;
	private int _contractDifficulty;

	public enum ContractType
	{
		General,
		OneStop,
		MultiStop
	}

	ContractType _contractType;


	public Contract()
	{
		_contractType = ContractType.General;
	}

	public void setContractType(int i)
	{
		_contractType = (ContractType)i;
	}

	public ContractType getContractType()
	{
		return _contractType;
	}

	public int getContractID()
	{
		return _contractID;
	}

	public void setContractID(int ID)
	{
		_contractID = ID;
	}

	public string getContractDescription()
	{
		return _contractDescription;
	}

	public void setContractDescription(string desc)
	{
		_contractDescription = desc;
	}

	public int getCargoHealth()
	{
		return _cargoHealth;
	}

	public void setCargoHealth(int health)
	{
		_cargoHealth = health;
	}

	public int getContractDifficulty()
	{
		return _contractDifficulty;
	}

	public void setContractDifficulty(int difficulty)
	{
		_contractDifficulty = difficulty;
	}





}

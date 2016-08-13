using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class ContractsManager : MonoBehaviour {

	XmlDocument xmlDoc;
	[SerializeField]
	TextAsset contractXML;

	List<Contract> _contractPool;

	[SerializeField]
	int currentLocaleID = 0;

	[SerializeField]
	Text _ContractsTitle;

	[SerializeField]
	Text _ContractsObjective;

	[SerializeField]
	Text _ContractsSubObjective;

	int _currentContractID = 0;

	// Use this for initialization
	void Start () {

		_contractPool = new List<Contract> ();
		loadXMLFromAsset (currentLocaleID);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Contract getCurrentContract()
	{
		return _contractPool [_currentContractID];
	}

	public void displayContract()
	{

		_ContractsTitle.text = _contractPool [_currentContractID].getContractName ();
		_ContractsObjective.text = _contractPool [_currentContractID].getContractDescription ();
	}

	// Following method load xml file from resouces folder under Assets
	private void loadXMLFromAsset(int ID)
	{
		//TextAsset tipsFile = (TextAsset)Resources.Load("Projects", typeof(TextAsset));
		xmlDoc = new XmlDocument();
		xmlDoc.LoadXml (contractXML.text);

		foreach (XmlElement node in xmlDoc.SelectNodes("ContractsColle/Contracts/Contract")) {


			if (int.Parse(node.SelectSingleNode("ID").InnerText) == ID)
			{
				Debug.Log ("Found something!");
				Contract c = new Contract();

				c.setContractName(node.Attributes[0].Value);
				c.setCargoHealth(int.Parse(node.SelectSingleNode("CargoHealth").InnerText));

				
				c.setContractDifficulty(int.Parse(node.SelectSingleNode("Difficulty").InnerText));
				c.setContractDescription(node.SelectSingleNode("Description").InnerText);
				
				c.setContractType(int.Parse(node.SelectSingleNode("Type").InnerText));
				c.setDestinationID(int.Parse(node.SelectSingleNode("DestinationID").InnerText));
			
				_contractPool.Add(c);


			}




		

		}

		Debug.Log ("No of contracts found: " + _contractPool.Count);
	
	}
}

public class Contract {


	private int _contractID;
	private string _contractName;
	private string _contractDescription;
	private int _cargoHealth;
	private int _contractDifficulty;
	private int _destinationID;
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

	public void setContractName(string s)
	{
		_contractName = s;
	}

	public string getContractName()
	{
		return _contractName;
	}

	public void setDestinationID(int i)
	{
		_destinationID = i;

	}

	public int getDestinationID()
	{
		return _destinationID;
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

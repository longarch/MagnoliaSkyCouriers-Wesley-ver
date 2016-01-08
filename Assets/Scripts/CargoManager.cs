using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using UnityEngine.UI;

public class CargoManager : MonoBehaviour {

	[SerializeField]
	private TextAsset cargoXML;

	private XmlDocument xmlDoc;
	private List<Cargo> cargoStores;
    private int damage;

	void Start () {
		loadCargo ();
        damage = 2;
	}

    void Update()
    {
        if (GameManager.Instance.getStatus() == GameManager.STATE.EVENT)
        {
            cargoDamaged(cargoStores[0].getName(), damage);
            Debug.Log("Cargo in Damaged");
            foreach(Cargo c in cargoStores)
            {
                if (c.getLost())
                {
                    GameManager.Instance.setStatus(GameManager.STATE.CARGOLOST);
                    break;
                }
            }
        }
    }

	public void loadCargo()
	{
		cargoStores = new List<Cargo> ();
		loadXMLFromAsset ();
		readXml ();
	}		

	// Following method load xml file from resouces folder under Assets
	private void loadXMLFromAsset()
	{
		xmlDoc = new XmlDocument();
		xmlDoc.LoadXml (cargoXML.text);
	}

	private void readXml()
	{
		foreach(XmlElement node in xmlDoc.SelectNodes("CargoInfo/Cargo"))
		{
			Cargo c = new Cargo (node.SelectSingleNode ("Name").InnerText, int.Parse (node.SelectSingleNode ("Health").InnerText), int.Parse (node.SelectSingleNode ("Weight").InnerText), node.SelectSingleNode ("Description").InnerText, false);

			//c.name = node.SelectSingleNode ("Name").InnerText;
			//c.health = int.Parse (node.SelectSingleNode ("Health").InnerText);
			//c.description = node.SelectSingleNode ("Description").InnerText;

			cargoStores.Add (c);

			Debug.Log("Adding: "  + node.Name);
		}
	}

	public void cargoDamaged (string name, int damage) {
		for (int i = 0; i < cargoStores.Count; i++) {
			if (cargoStores [i].getName ().Contains (name)) {
				cargoStores [i].setHealth (cargoStores [i].getHealth () - damage);
				if (cargoStores [i].getHealth () <= 0) {
					cargoStores [i].setLost (true);
				}
			}
		}
	}
}

public class Cargo
{
	[XmlAttribute("Name")]
	private string name, description;
	private int health, weight;
	private bool lost;

	public Cargo(string n, int h, int w, string d, bool l)
	{
		setName (n);
		setHealth (h);
		setWeight (w);
		setDescription (d);
		setLost (l);
	}

	public string getName()
	{
		return name;
	}

	public string getDescription()
	{
		return description;
	}

	public int getHealth()
	{
		return health;
	}

	public int getWeight()
	{
		return weight;
	}

	public bool getLost()
	{
		return lost;
	}

	public void setName (string s)
	{
		name = s;
	}

	public void setDescription (string d)
	{
		description = d;
	}

	public void setHealth (int h)
	{
		health = h;
	}

	public void setWeight (int w)
	{
		weight = w;
	}

	public void setLost (bool l)
	{
		lost = l;
	}
}

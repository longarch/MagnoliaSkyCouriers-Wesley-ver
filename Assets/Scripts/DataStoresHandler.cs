using UnityEngine;
using System.Collections;

public class DataStoresHandler : MonoBehaviour {

	private static DataStoresHandler _instance;

	[SerializeField]
	public float enemyCount;
	public float difficultyMultiplier { get; set; }
	public float shipHP { get; set; }
	public float cargoHP { get; set; }

	public static DataStoresHandler Instance
	{
		get { return _instance; }
	}

	void Awake() {
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}

		enemyCount = 0;
		difficultyMultiplier = 1;
		shipHP = 0;
		cargoHP = 0;


		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void setEnemyKill(int i)
	{
		enemyCount = i;
	}

	public void setShiphealth(int i)
	{
		shipHP = i;
	}

	public void setCargoHealth(int i)
	{
		cargoHP = i;
	}

	public void setDifficulty(int i)
	{
		difficultyMultiplier = i;
	}

}

using UnityEngine;
using System.Collections;

//Use this to store all stats and load them from here
public class LevelLoadHandler : MonoBehaviour {

	private static LevelLoadHandler _instance;

	int leader = 0;
	int Difficulty = 0;

	public static LevelLoadHandler Instance
	{
		get { return _instance; }
	}

	void Awake() {

		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}


	public void setDifficulty (int i)
	{
		Difficulty = i;
	}

	public int returnDiff()
	{
		return Difficulty;
	}

	public void setLeader(int i)
	{
		leader = i;
	}

	public int getLeader()
	{
		return leader;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

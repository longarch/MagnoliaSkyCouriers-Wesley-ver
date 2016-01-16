using UnityEngine;
using System.Collections;

//Use this to store all stats and load them from here
public class LevelLoadHandler : MonoBehaviour {

	int leader = 0;
	float Distance = 0;
	void Awake() {

		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () {
	
	}

	public void setDistance (float i)
	{
		Distance = i;
	}

	public float returnDist()
	{
		return Distance;
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

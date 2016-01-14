using UnityEngine;
using System.Collections;

public class Crew : MonoBehaviour {

    public bool isAssigned;
    public enum race { Elf, Fairy, Human, Wolfman };
	[SerializeField]
	private bool isLeader = false;
    [SerializeField]
    private race CrewRace;

	[SerializeField]
	private bool isRepair = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void assignLeader()
	{
		isLeader = true;
	}

    public bool getAssigned()
    {
        return isAssigned;
    }

    public void setAssigned(bool assign)
    {
        isAssigned = assign;
    }

    public race getRace()
    {
        return CrewRace;
    }
}

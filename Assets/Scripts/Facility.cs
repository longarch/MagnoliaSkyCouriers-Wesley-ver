using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Facility : MonoBehaviour {

    private List<GameObject> assignedCrews;
    
    private enum type { Magic, Combat, Core, Movement };
    
    private int ResourcesNeeded;
    private int rValue;

    [SerializeField]
    private type facilityType;
    bool isActivated;


    // Use this for initialization
    void Start () {
        isActivated = false;
    }
	
	// Update is called once per frame
	void Update () {
        checkResource();
        // Always be checking for the resources
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Crew"))
        {
            Debug.Log("Race "+ other.gameObject.GetComponent<Crew>().getRace() + " working on " + name);
            //if (!other.gameObject.GetComponent<Crew>().getAssigned())
                checkRaceVsType((int)other.gameObject.GetComponent<Crew>().getRace(), true);
        }
        //Debug.Log(assignedCrews);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Crew"))
        {
            Debug.Log("Race " + other.gameObject.GetComponent<Crew>().getRace() + " left " + name);
            checkRaceVsType((int)other.gameObject.GetComponent<Crew>().getRace(), false);
        }
        //Debug.Log(assignedCrews);
    }

    void checkRaceVsType(int race, bool enter)
    {
        switch(race)
        {
            case 0: //Race = Elf
                if (facilityType == type.Magic)
                    rValue = 3;
                else
                    rValue = 1;
                break;
            case 1: //Race = Fairy
                if (facilityType == type.Movement)
                    rValue = 3;
                else
                    rValue = 1;
                break;
            case 2: //Race = Human
                if (facilityType == type.Core)
                    rValue = 3;
                else
                    rValue = 1;
                break;
            case 3: //Race = Wolfman
                if (facilityType == type.Combat)
                    rValue = 3;
                else
                    rValue = 1;
                break;
        }
        if (enter)
            ResourcesNeeded += rValue;
        else ResourcesNeeded -= rValue;
    }

    void checkResource()
    {
        if (ResourcesNeeded >= 3)
        {
            isActivated = true;
            Debug.Log(name + " is working");
        }
    }

    /*void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.gameObject.layer == LayerMask.NameToLayer("Crew"))
        {

        }
    }


    /*ontriggercollide()
    {
        GetComponent component of crew by checking script
        to see hu is it(keep track of type n adds resources accordingly)
    }
    */
}

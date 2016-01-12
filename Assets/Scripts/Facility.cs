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
    Ship shipInteractions;

    [SerializeField]
    EnemyManager eventEnemies;
    [SerializeField]
    EnemyPool ePool;

    [SerializeField]
    private type facilityType;
	[SerializeField]
	private ScrollingBackground SkyBG; //To be used to indirectly manipulate scroll speed

    private BaseEnemy target;

    bool isActivated;
    bool scanned;
    bool needToReset = false;
    private float facilityOutput = 0;
    private float originalOutput;


    // Use this for initialization
    void Start () {
        isActivated = false;
        switch ((int)facilityType)
        {
            case 0: // Magic Type
                scanned = false;
                facilityOutput = 1;
                break;
            case 1: // Combat
                scanned = false;
                facilityOutput = 1;
                break;
            case 2: // Core
                facilityOutput = 2;
                break;
            case 3: // Movement
                scanned = true;
                facilityOutput = 3;
                originalOutput = shipInteractions.ShipSpeed;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!isActivated)
        {
            if (needToReset)
            {
                resetOutputs();
            }
            return;
        }
        //checkResource();
        if (!scanned)
        {
            if (facilityType == type.Combat || facilityType == type.Magic)
                scanForTarget();
        }
        startFacility();
        // Always be checking for the resources
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Crew"))
        {
            Debug.Log("Race "+ other.gameObject.GetComponent<Crew>().getRace() + " working on " + name);
            //if (!other.gameObject.GetComponent<Crew>().getAssigned())
            //other.gameObject.GetComponent<AbstractMover>().IsMoving = false;
            checkRaceVsType((int)other.gameObject.GetComponent<Crew>().getRace(), true);
        }
        //Debug.Log(assignedCrews);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Crew"))
        {
            Debug.Log("Race " + other.gameObject.GetComponent<Crew>().getRace() + " left " + name);
            checkRaceVsType((int)other.gameObject.GetComponent<Crew>().getRace(), false);
        }
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
        checkResource();
    }

    void checkResource()
    {
        if (ResourcesNeeded >= 3)
        {
            isActivated = true;
            Debug.Log(name + " is working");
        }
        else {
            isActivated = false;
            needToReset = true;
        }
    }

    public void startFacility()
    {
        switch ((int)facilityType)
        {
            case 0: // Magic Type
                break;
            case 1: // Combat
                if (target == null)
                {
                    return;
                }
                target.TakeDamage((int)facilityOutput);
                if (target.getHealth() <= 0)
                {
                    target.gameObject.SetActive(false);
                    scanned = false;
                }
                break;
            case 2: // Core
                break;
            case 3: // Movement
                float accel = 1;
                while (accel < facilityOutput)
                {
                    shipInteractions.ShipSpeed = originalOutput * accel;
                    SkyBG.setBGSpeed(1);
                    accel += 0.2f;
                }
                break;
        }
    }

    private void resetOutputs()
    {
        switch ((int)facilityType)
        {
            case 0: // Magic Type
                break;
            case 1: // Combat
                break;
            case 2: // Core
                break;
            case 3: // Movement
                float accel = facilityOutput;
                while (accel > 1)
                {
                    shipInteractions.ShipSpeed = originalOutput * accel;
                    SkyBG.setBGSpeed(0.5f);
                    accel -= 0.2f;
                }
                break;
        }
        needToReset = false;
    }

    private void scanForTarget()
    {
        foreach (BaseEnemy pEnemy in ePool.GetAllActiveEnemies())
        {
            foreach (BaseEnemy sEnemy in eventEnemies.enemy)
            {
                if (sEnemy.GetType().Equals(pEnemy.GetType()))
                {
                    target = pEnemy;
                    scanned = true;
                    break;
                }
            }
        }
    }

    private void TimerLoop(float delay)
    {
        if (delay <= 0)
        { }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using DG.Tweening;

public class Facility : MonoBehaviour {

    private List<GameObject> assignedCrews;
    
    private enum type { Magic, Combat, Core, Movement, Evasion };


    private int fHealth;
    private int ResourcesNeeded;
    private int ResourcesCount;
    private int rValue;
    [SerializeField]
    Ship shipInteractions;
    [SerializeField]
    GameObject bulletHole;


    [SerializeField]
    EnemyManager eventEnemies;
    [SerializeField]
    EnemyPool ePool;
    [SerializeField]
    public GameObject Bullet;

    [SerializeField]
    private type facilityType;
	[SerializeField]
	private ScrollingBackground SkyBG; //To be used to indirectly manipulate scroll speed

    private BaseEnemy target;

    bool isActivated, isWorking;
    bool scanned;
    float delay, atkDelay;
    bool needToReset = false;
    private float facilityOutput = 0;
    private float originalOutput;
    Color oriColor;


    // Use this for initialization
    void Start () {
        isActivated = false;
        isWorking = true;
        oriColor = gameObject.GetComponent<SpriteRenderer>().color;
        gameObject.GetComponent<SpriteRenderer>().DOColor(Color.gray, 0.5f);
        ResourcesCount = 0;
        switch ((int)facilityType)
        {
            case 0: // Magic Type
                scanned = false;
                atkDelay = 5;
                delay = atkDelay;
                facilityOutput = 50; //currently not used
                break;
            case 1: // Combat
                scanned = false;
                atkDelay = 5;
                delay = atkDelay;
				facilityOutput = 50; //currently not used
                break;
            case 2: // Core
                fHealth = shipInteractions.currentHealth;
                scanned = true;
                atkDelay = 3;
                delay = atkDelay;
                facilityOutput = 2;
                break;
            case 3: // Movement
                scanned = true;
                facilityOutput = 2;
                originalOutput = shipInteractions.ShipSpeed;
                break;
            case 4: // Evasion
                scanned = true;
                facilityOutput = 3;
                atkDelay = 3;
                delay = atkDelay;
                originalOutput = shipInteractions.ShipSpeed;
                break;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (!isWorking)
        {
            return;
        }
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
        if (!isWorking)
        {
            return;
        }
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
        if (!isWorking)
        {
            return;
        }
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
                if (facilityType == type.Movement || facilityType == type.Evasion)
                    rValue = 2;
                else
                    rValue = 1;
                break;
            case 1: //Race = Fairy
                if (facilityType == type.Magic)
                    rValue = 2;
                else
                    rValue = 1;
                break;
            case 2: //Race = Human
                rValue = 1;
                break;
            case 3: //Race = Wolfman
                if (facilityType == type.Combat)
                    rValue = 2;
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
        if (ResourcesNeeded >= 2)
        {
            isActivated = true;
            gameObject.GetComponent<SpriteRenderer>().DOColor(oriColor, 0.5f);
            Debug.Log(name + " is working");
        } else {
            isActivated = false;
            gameObject.GetComponent<SpriteRenderer>().DOColor(Color.gray, 0.5f);
            needToReset = true;
        }
    }

    public void startFacility()
    {
        switch ((int)facilityType)
        {
            case 0: // Magic Type
                if (scanned)
                {
                    delay -= 0.02f;
                    if (delay <= 0)
                    {
                        fireBullet();
                        delay = atkDelay;
                    }
                    if (target.getHealth() <= 0)
                    {
                        target.Targeted = false;
                        target.gameObject.SetActive(false);
                        delay = atkDelay;
                        scanned = false;
                    }
                }
                break;
            case 1: // Combat
                if (scanned)
                {
                    delay -= 0.02f;
                    if (delay <= 0)
                    {
                        fireBullet();
                        Debug.Log(target.getHealth());
                        delay = atkDelay;
                    }
                    if (target.getHealth() <= 0)
                    {
                        target.Targeted = false;
                        target.gameObject.SetActive(false);
                        delay = atkDelay;
                        scanned = false;
                    }
                }
                break;
            case 2: // Core
                if (shipInteractions.currentHealth < 100)
                {
                    delay -= 0.2f;
                    if (delay <= 0)
                    {
                        shipInteractions.repairShip((int)facilityOutput);
                        delay = atkDelay;
                    }
                }
                break;
            case 3: // Movement
                float accel = 1;
                while (accel < facilityOutput)
                {
                    shipInteractions.ShipSpeed = originalOutput * accel;
                    SkyBG.setBGSpeed(Mathf.Clamp(shipInteractions.ShipSpeed,0.5f,2));
                    accel += 0.2f;
                }
                break;
            case 4: // Evasion
                delay -= 0.2f;
                if (delay <= 0)
                {
                    if (randomType() < 1)
                    { shipInteractions.evadeChance = true; }
                    else shipInteractions.evadeChance = false;
                    delay = atkDelay;
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
            case 4: // Evasion
                shipInteractions.evadeChance = false;
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
                    target.Targeted = true;
                    scanned = true;
                    break;
                }
            }
        }
    }

    void fireBullet()
    {
        Debug.Log(target.transform.position);
        if (target != null)
        {
           // Debug.Log(target);
            GameObject bullet = Instantiate(Bullet, bulletHole.transform.position, Quaternion.identity) as GameObject;
            //bullet.transform.position += Vector3.left;
            Vector2 direction = (target.transform.position - bullet.transform.position).normalized;

		
            bullet.GetComponent<enemyBullet>().damageValue = (int)facilityOutput;
			bullet.GetComponent<enemyBullet>().setBulletSpeed(0.5f);
			bullet.GetComponent<enemyBullet>().setDirection(direction);
            //Debug.Log(direction);
        }
        else {
            return;
        }
    }


    private int randomType()
    {
        int i = UnityEngine.Random.Range(0, 3);

        return i;
    }

    void damageFacility(int dmg)
    {
        fHealth -= dmg;
        if (fHealth <= 0)
        {
            gameObject.GetComponent<SpriteRenderer>().DOColor(Color.black, 0.5f);
            isWorking = false;
        }
    }


}

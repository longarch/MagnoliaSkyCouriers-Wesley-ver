using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class Facility : MonoBehaviour {

    private List<GameObject> assignedCrews;
    
    private enum type { Magic, Combat, Core, Movement, Evasion };

	[SerializeField]
    private int fHealth = 10;
	private int MaximumHealth;
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


	private Killable _healthHandler;

    bool isActivated, isWorking, isCriticalWorking;
    bool scanned;
    float delay, atkDelay;
    bool needToReset = false;
    private float facilityOutput = 0;
    private float originalOutput;
    Color oriColor;
	GameObject _loadHandlerObj;

	//Repair feature
	[SerializeField]
	float repairTime = 3;
	float repairCap;
	bool isRepair;

    // Use this for initialization
    void Start () {

		isRepair = false;
		repairCap = repairTime;
		_healthHandler = GetComponent < Killable> ();
		_loadHandlerObj = GameObject.Find ("levelHandler");
		MaximumHealth = fHealth;
		assignedCrews = new List<GameObject> ();
		//fHealth = 10;
        isActivated = false;
		isCriticalWorking = false;
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
                facilityOutput = 25; //currently not used
                break;
            case 1: // Combat
                scanned = false;
                atkDelay = 5;
                delay = atkDelay;
				facilityOutput = 25; //currently not used
                break;
            case 2: // Core
                fHealth = shipInteractions.currentHealth;
                scanned = true;
                atkDelay = 3;
                delay = atkDelay;
                facilityOutput = 1;
                break;
            case 3: // Movement
                scanned = true;
                facilityOutput = 2;
                originalOutput = shipInteractions.ShipSpeed;
                break;
            case 4: // Evasion
                scanned = true;
                facilityOutput = 25;
                atkDelay = 3;
                delay = atkDelay;
                originalOutput = shipInteractions.ShipSpeed;
                break;
        }
		if (_loadHandlerObj != null) {
			applyLeaderEffects (_loadHandlerObj.GetComponent<LevelLoadHandler>());
		}

		//Apply to true health settings
		_healthHandler.Health = MaximumHealth;
		_healthHandler.MaximumHealth = MaximumHealth;
    }
	
	// Update is called once per frame
	void Update () {
		if (isRepair) {
			Debug.Log ("fixing myself");
			repairTime -= Time.deltaTime;
			//fHealth += (int)(2 * Time.deltaTime);
			if (repairTime <= 0.0f)
			{
				gameObject.GetComponent<SpriteRenderer>().DOColor(Color.gray, 0.5f);
				//Debug.Log ("Im done fixing!");
				isRepair = false;
				isWorking = true;

				_healthHandler.Reset();
				_healthHandler.Health = MaximumHealth;
				_healthHandler.MaximumHealth = MaximumHealth;
				repairTime = repairCap;
				
			}
		}


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
		if (fHealth <= 0) {
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
        if (enter) {
			ResourcesNeeded += rValue;
		} else {
			ResourcesNeeded -= rValue;
		}
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

	void applyLeaderEffects(LevelLoadHandler _loadHandler)
	{

		if (_loadHandler != null) {
			switch (_loadHandler.getLeader ()) {
			case 0: // Human
				isCriticalWorking = true;
				break;
			case 1: // Elf
				if (facilityType == type.Evasion) {
					facilityOutput = 50;
				}
				if (facilityType == type.Movement) {
					facilityOutput = 2.25f;
				}
				break;
			case 2: // Wolfman
				if (facilityType == type.Combat) {
					atkDelay = 4;
					delay = atkDelay;
					facilityOutput = 40;
				}

				break;
			case 3: // Fairy
				shipInteractions.setCurrentHealth(150);
				fHealth = 40;
				MaximumHealth = fHealth;
				/*
				if (facilityType == type.Magic) {
					atkDelay = 4;
					delay = atkDelay;
					facilityOutput = 40;

					//Higher health
					//shipInteractions.setCurrentHealth(150);

				}
				*/
				break;
			}
		}
	}

	void modifyOutputs()
	{
		for (int i = 0; i < assignedCrews.Count; i++)
		{

		}
	}

    public void startFacility()
    {
        switch ((int)facilityType)
        {
            case 0: // Magic Type
                if (scanned)
                {
                    //eventEnemies.enemy.RemoveAt(0);
                    delay -= 0.02f;
                    if (delay <= 0)
                    {
                        fireBullet();
                        delay = atkDelay;
                    }
                    if (target.getHealth() <= 0)
                    {
						eventEnemies.enemy.RemoveAt(0);
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
                        eventEnemies.enemy.RemoveAt(0);
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
                Debug.Log("Evade working");
                delay -= 0.2f;
                if (delay <= 0)
                {
                    if (randomType(0, 100) < facilityOutput)
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
            //foreach (BaseEnemy sEnemy in eventEnemies.enemy)
            if (eventEnemies.enemy.Count > 0)
            {
                BaseEnemy sEnemy = eventEnemies.enemy[0];
                if (sEnemy.GetType().Equals(pEnemy.GetType()))
                {
                    target = sEnemy;
                    target.Targeted = true;
                    scanned = true;
                    break;
                }
            }
            else break;
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

	public float HealthFraction
	{
		get { return ((float)fHealth + 0.0f)/(float)MaximumHealth; }
	}


    private int randomType(int min, int max)
    {
        int i = Random.Range(min, max);
        return i;
    }



    public void damageFacility(int dmg)
    {
		_healthHandler.Health -= dmg;
		_healthHandler.Health = Mathf.Clamp(_healthHandler.Health, 0, _healthHandler.MaximumHealth);
        fHealth -= dmg;
		if (_healthHandler.Health <= 0)
        {
            gameObject.GetComponent<SpriteRenderer>().DOColor(Color.black, 0.5f);
            isWorking = false;
			isRepair = true;
        }
    }



	public void AddOnKillCallback(Killable.OnKilled callback)
	{
		
		
		
		_healthHandler.AddOnKillCallback(callback);
	}

}

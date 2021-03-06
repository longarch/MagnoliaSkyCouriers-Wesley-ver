﻿using UnityEngine;
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
	Camera Fieldcam;
	[SerializeField] private LayerMask _selectionLayerMask;
	static bool isOver;
	[SerializeField]
	public GameObject actionIndicator1, actionIndicator2, actionIndicator3;
	[SerializeField] GameObject ActionBar;
	[SerializeField]
	GameObject over;
	static float actionDelay = 2f;
	static bool actionOut;


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
	[SerializeField]
	GameObject Notifier;

    private BaseEnemy target;


	private Killable _healthHandler;

    bool isActivated, isWorking, isCriticalWorking;
    bool scanned;
    float delay, atkDelay,orgDelay;
    bool needToReset = false;
    private float facilityOutput = 0;
    private float originalOutput;
    Color oriColor;
	GameObject _loadHandlerObj;

	//Repair feature
	[SerializeField]
	float repairTime = 3;
	float repairCap;
	bool isRepair, reCheck;

	int efficiency = 0;

	//Speed facility
	float accel = 0;



    // Use this for initialization
    void Start () {

		isOver = false;
		actionOut = false;
		isRepair = false;
        reCheck = false;
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
		isOver = false;
        switch ((int)facilityType)
        {
            case 0: // Magic Type
                scanned = false;
                atkDelay = 5;
                delay = atkDelay;
                facilityOutput = 35; //currently not used
                break;
            case 1: // Combat
                scanned = false;
                atkDelay = 4;
                delay = atkDelay;
				facilityOutput = 25; //currently not used
                break;
            case 2: // Core
                fHealth = shipInteractions.currentHealth;
                scanned = true;
                atkDelay = 1;
                delay = atkDelay;
                facilityOutput = 1;
                break;
            case 3: // Movement
                scanned = true;
                facilityOutput = 1;
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
		orgDelay = atkDelay;
		//Apply to true health settings
		_healthHandler.Health = MaximumHealth;
		_healthHandler.MaximumHealth = MaximumHealth;
    }
	
	// Update is called once per frame
	void Update () {

		if (isRepair) {
			//Debug.Log ("fixing myself");
			repairTime -= Time.deltaTime;
			//fHealth += (int)(2 * Time.deltaTime);
			if (repairTime <= 0.0f)
			{
				gameObject.GetComponent<SpriteRenderer>().DOColor(Color.gray, 0.5f);
				//Debug.Log ("Im done fixing!");
				isRepair = false;
				isWorking = true;
                reCheck = false;
                checkResource();
				ApplyEfficiency();

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

		if (isActivated) {
			if (!scanned)
			{
				if (facilityType == type.Combat || facilityType == type.Magic)
					scanForTarget();
			}
			startFacility();
		}
        // Always be checking for the resources
    }

	public void CallActions()
	{
		Debug.Log ("Called the action");
		checkResource();
		if (!isActivated)
		{
			if (needToReset)
			{
				//Notifier.GetComponentInChildren<NotificationManager> ().NotifyText (this.name + " deactivating.");
				resetOutputs();
			}
			return;
		}
	}


    void OnTriggerEnter2D(Collider2D other)
    {
        /*if (!isWorking)
        {
            if (!isRepair)
                return;
            else
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Crew"))
                {
                    Debug.Log("Race " + other.gameObject.GetComponent<Crew>().getRace() + " left " + name);
                    checkRaceVsType((int)other.gameObject.GetComponent<Crew>().getRace(), true);
                }
            }
        }
		if (fHealth <= 0) {
			return;
		}*/

        if (other.gameObject.layer == LayerMask.NameToLayer("Crew"))
        {
            //Debug.Log("Race "+ other.gameObject.GetComponent<Crew>().getRace() + " working on " + name);
            //if (!other.gameObject.GetComponent<Crew>().getAssigned())
            //other.gameObject.GetComponent<AbstractMover>().IsMoving = false;
			efficiency ++;
			ApplyEfficiency();
            checkRaceVsType((int)other.gameObject.GetComponent<Crew>().getRace(), true);
			Notifier.GetComponentInChildren<NotificationManager> ().NotifyText (other.gameObject.GetComponent<Crew>().getRace() + " assigned to " + this.name);
        }
        //Debug.Log(assignedCrews);
    }
		
    void OnTriggerExit2D(Collider2D other)
    {
        /*if (!isWorking)
        {
            if (!isRepair)
                return;
            else
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Crew"))
                {
                    Debug.Log("Race " + other.gameObject.GetComponent<Crew>().getRace() + " left " + name);
                    checkRaceVsType((int)other.gameObject.GetComponent<Crew>().getRace(), false);
                }
            }
        }*/
        if (other.gameObject.layer == LayerMask.NameToLayer("Crew"))
        {
          //  Debug.Log("Race " + other.gameObject.GetComponent<Crew>().getRace() + " left " + name);
			efficiency --;
			ApplyEfficiency();
            checkRaceVsType((int)other.gameObject.GetComponent<Crew>().getRace(), false);
			Notifier.GetComponentInChildren<NotificationManager> ().NotifyText (other.gameObject.GetComponent<Crew>().getRace() + " left " + this.name);
			CallActions ();
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
                rValue = 2; //Human is to be at 1, 2 for testing
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
    }

    void checkResource()
    {
        if (!isWorking)
        {
            return;
        }
        if (ResourcesNeeded >= 2)
        {
            isActivated = true;
            gameObject.GetComponent<SpriteRenderer>().DOColor(oriColor, 0.5f);
			Notifier.GetComponentInChildren<NotificationManager> ().NotifyText ("Resources Condition met. " + this.name + " Activating.");
           // Debug.Log(name + " is working");
        } else {
			if (isActivated) {
				isActivated = false;
				gameObject.GetComponent<SpriteRenderer> ().DOColor (Color.gray, 0.5f);
				needToReset = true;
				Notifier.GetComponentInChildren<NotificationManager> ().NotifyText ("Resources Condition lost. " + this.name + " deactivating.");
			}
        }
        //Debug.Log(gameObject.name + " has tis many : " + ResourcesNeeded);
    }

	void ApplyEfficiency()
	{
		if (efficiency >= 2) {
			atkDelay = atkDelay / 2.0f;
			Debug.Log ("Attack Delay is" + atkDelay);
		} else {
			atkDelay = orgDelay;
		}
	}

	void applyLeaderEffects(LevelLoadHandler _loadHandler)
	{

		if (_loadHandler != null) {
			switch (_loadHandler.getLeader ()) {
			case 0: // Human
				repairTime = repairTime / 2;
				repairCap = repairTime;
				isCriticalWorking = true;
				break;
			case 1: // Elf
				if (facilityType == type.Evasion) {
					facilityOutput = 40;
				}
				if (facilityType == type.Movement) {
					facilityOutput = 1.25f;
				}
				break;
			case 2: // Wolfman
				if (facilityType == type.Combat) {
					atkDelay = 3;
					delay = atkDelay;
					facilityOutput = 40;
				}

				if (facilityType == type.Magic)
				{
					atkDelay = 4;
					delay = atkDelay;
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
					delay -= Time.deltaTime;
                    if (delay <= 0)
                    {
                        fireBullet();
                        delay = atkDelay;
                    }
                    if (target.getHealth() <= 0)
                    {
						//eventEnemies.enemy.RemoveAt(0);
                        target.Targeted = false;
					Notifier.GetComponentInChildren<NotificationManager> ().NotifyText (target.name + " shot down.");
                        target.gameObject.SetActive(false);
                        delay = atkDelay;
                        scanned = false;
                    }
                }
                break;
            case 1: // Combat
                if (scanned)
                {
					delay -= Time.deltaTime;
                    if (delay <= 0)
                    {
                        fireBullet();
                        Debug.Log(target.getHealth());
                        delay = atkDelay;
                    }
                    if (target.getHealth() <= 0)
                    {
                        //eventEnemies.enemy.RemoveAt(0);
                        target.Targeted = false;
					Notifier.GetComponentInChildren<NotificationManager> ().NotifyText (target.name + " shot down.");
                        target.gameObject.SetActive(false);
                        delay = atkDelay;
                        scanned = false;
                    }
                }
                break;
            case 2: // Core
                if (shipInteractions.currentHealth < 100)
                {
                    delay -= Time.deltaTime;
                    if (delay <= 0)
                    {
                        shipInteractions.repairShip((int)facilityOutput);
                        delay = atkDelay;
                    }
                }
                break;
            case 3: // Movement, ship speed is locked at 3

                if (accel > facilityOutput)
                {
                    return;
                }
                accel += Time.deltaTime;// * 0.1f;
                shipInteractions.ShipSpeed = originalOutput + accel;
                //float BGspd = (((accel - 0) * (2 - 0.5f)) / (facilityOutput - 0));
                //SkyBG.setBGSpeed(1.0f);
                //float accel = 1;
                /*
                    while (accel < facilityOutput)
                    {
                        shipInteractions.ShipSpeed = originalOutput + accel;
                        SkyBG.setBGSpeed(Mathf.Clamp(shipInteractions.ShipSpeed,0.5f,2));
                        accel += Time.deltaTime;
                    }
                    */
                break;
            case 4: // Evasion
                //Debug.Log("Evade working");
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
                needToReset = false;
                break;
            case 1: // Combat
                needToReset = false;
                break;
            case 2: // Core
                needToReset = false;
                break;
            case 3: // Movement
                if (accel < 0)
                {
                    needToReset = false;
                    return;
                }
                accel -= Time.deltaTime * 0.1f;
                shipInteractions.ShipSpeed = originalOutput + accel;
                float BGspd = (((accel - 0) * (2 - 0.5f)) / (facilityOutput - 0));
                SkyBG.setBGSpeed(0.25f);
                //SkyBG.setBGSpeed(0.5f);
                //float accel = facilityOutput;
                /*
                    while (accel > 1)
                    {
                        shipInteractions.ShipSpeed = originalOutput * accel;
                        SkyBG.setBGSpeed(0.5f);
                        accel -= 0.2f;
                    }
                    */
                break;
            case 4: // Evasion
                needToReset = false;
                shipInteractions.evadeChance = false;
                break;
        }

    }

    private void scanForTarget()
    {
        foreach (BaseEnemy pEnemy in ePool.GetAllActiveEnemies())
        {
            //foreach (BaseEnemy sEnemy in eventEnemies.enemy)
            if (shipInteractions.getTargetsInRange.Count > 0)
            {
                BaseEnemy sEnemy = shipInteractions.getTargetsInRange[0].GetComponent<BaseEnemy>();
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
			AudioController.Instance.PlayPunchSound();
			switch ((int)facilityType) {
			case 0: //Magic
				Notifier.GetComponentInChildren<NotificationManager> ().NotifyText ("Firing magic gun.");
				break;
			case 1: // Combat
				Notifier.GetComponentInChildren<NotificationManager> ().NotifyText ("Firing shell gun.");
				break;
			}
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
		Notifier.GetComponentInChildren<NotificationManager> ().NotifyText (this.name + " took " + dmg + " damage");
		if (_healthHandler.Health <= 0)
        {
            gameObject.GetComponent<SpriteRenderer>().DOColor(Color.black, 0.5f);
			Notifier.GetComponentInChildren<NotificationManager> ().NotifyText (this.name + " has 0 health. Deactivating");
			efficiency = 0;
            isWorking = false;
			isRepair = true;
            reCheck = true;
        }
    }


    public void AddOnKillCallback(Killable.OnKilled callback)
	{
		_healthHandler.AddOnKillCallback(callback);
	}

	public void OnMouseEnter2(){
		//Not used currently
		if (!isOver) {
			Debug.Log (this.name + "In");
			DOTween.Complete (actionIndicator1.transform);
			DOTween.Complete (actionIndicator2.transform);
			DOTween.Complete (actionIndicator3.transform);

			actionIndicator1.transform.localScale = new Vector3 (1, 1, 1);
			actionIndicator2.transform.localScale = new Vector3 (1, 1, 1);
			actionIndicator3.transform.localScale = new Vector3 (1, 1, 1);
			actionIndicator1.SetActive (true);
			actionIndicator1.transform.DOScale (new Vector3 (3f, 3f, 3f), 1.0f);
			actionIndicator2.SetActive (true);
			actionIndicator2.transform.DOScale (new Vector3 (3f, 3f, 3f), 1.0f);
			actionIndicator3.SetActive (true);
			actionIndicator3.transform.DOScale (new Vector3 (3f, 3f, 3f), 1.0f);//*/

			actionOut = true;
			isOver = true;
		}
	}

	public void OnMouseEnter(){
		if (!isOver) {
			Debug.Log (this.name + "In");
			ActionBar.SetActive(true);
			//*
			DOTween.Complete (actionIndicator1.transform);
			DOTween.Complete (actionIndicator2.transform);
			DOTween.Complete (actionIndicator3.transform);

			actionIndicator1.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
			actionIndicator2.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
			actionIndicator3.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
			actionIndicator1.SetActive (true);
			actionIndicator1.transform.DOScale (new Vector3 (0.3f, 0.3f, 0.3f), 0.5f);
			actionIndicator2.SetActive (true);
			actionIndicator2.transform.DOScale (new Vector3 (0.3f, 0.3f, 0.3f), 0.5f);
			actionIndicator3.SetActive (true);
			actionIndicator3.transform.DOScale (new Vector3 (0.3f, 0.3f, 0.3f), 0.5f);//*/
			actionOut = true;
			isOver = true;
		}
	}

	public void OnMouseExit(){
		//if (!isOver)
			//return;
		actionDelay -= Time.deltaTime;
		ActionBar.SetActive (false);
		actionOut = false;
		actionDelay = 2f;
		isOver = false;
		/*if (actionDelay <= 0.0f) {
			Debug.Log ("Testing in out");
			Debug.Log (this.name + "out");
			actionIndicator1.SetActive (false);
			actionIndicator1.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
			actionIndicator2.SetActive (false);
			actionIndicator2.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
			actionIndicator3.SetActive (false);
			actionIndicator3.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
			actionOut = false;
			actionDelay = 2f;
			isOver = false;
		} 		//*/

	}

	void testMouseEnter()
	{
		//if (isOver) {
		//	return;
		//}
		RaycastHit2D hit = Physics2D.Raycast(Fieldcam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity,
			_selectionLayerMask.value);
		if (hit.collider != null) {

			Debug.Log("hit something!");
			if (!isOver && hit.collider.gameObject.name == this.gameObject.transform.parent.gameObject.name)
			{
				Debug.Log ("yes");
				this.gameObject.GetComponentInChildren<Facility>().OnMouseEnter();
			}



		} else {
			//Set a timer for the exit function
			if (this.gameObject.GetComponentInChildren<Facility>().getIsOver())
			{
				isOver = false;
			}
		}
	}

	public bool getIsOver()
	{
		return isOver;
	}

	public bool getActionOut()
	{
		return actionOut;
	}

	public void repairFacility()
	{
		_healthHandler.Reset();
		_healthHandler.Health = MaximumHealth;
		_healthHandler.MaximumHealth = MaximumHealth;
		repairTime = repairCap;
		Notifier.GetComponentInChildren<NotificationManager> ().NotifyText (this.name + " repairing.");
	}

}

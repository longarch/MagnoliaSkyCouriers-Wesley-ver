using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;


public class Ship : MonoBehaviour
{
	public float maxDist = 0;
	private float heightChangeTimer = 5.0f,  maxHealth = 1.0f, heightAscent = 0;// speed = 0.05f;
	private Vector3 position;
	public int currentHealth = 100, cargoHealth = 100;
	private int healthCap;
	[SerializeField] 
	private float speed = 0.05f;
    [SerializeField] 
	GameObject Goal;

	[SerializeField]
	Image healthImage;

	[SerializeField]
	Image cargoHealthImage;

	[SerializeField]
	Text healthTxt, cargoCountTxt, shipSpeedTxt;

	[SerializeField]
	Camera innerCam;
    //####Changes
    [SerializeField]
    GameObject sLOS;
    public bool evade;

	[SerializeField]
	List<Facility> facilityList;

    // Use this for initialization
    void Start()
    {
		position = getPosition();
		//currentHealth = 100;
		healthImage.fillAmount = maxHealth;

		cargoHealthImage.fillAmount = maxHealth;
		CargoManager.Instance.loadCargo ();
		cargoHealth = CargoManager.Instance.getCargoHealth ("Cargo1");
		healthTxt.text = "Health: " + currentHealth;
		healthCap = currentHealth;
        //####Changes
        sLOS = Instantiate(sLOS, transform.position, Quaternion.identity) as GameObject;
        sLOS.GetComponent<LineOfSight>().setAssigned(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
		// shipDamages (); //placed here to test
       // Debug.Log(Health);
		GameManager.Instance.checkShipStatus(this,currentHealth);
        switch(GameManager.Instance.getStatus())
        {
            case GameManager.STATE.START: //Starting
				moveShipHeight();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameManager.Instance.setStatus(GameManager.STATE.ONGOING);
                    GameManager.Instance.setFollow(true);
                }
                break;
			case GameManager.STATE.ONGOING: // On Route Stage
				moveShip();
			/*
                if (UnityEngine.Random.Range(0, 1) > 4) //Random change to eventmode...will not be in here
                    GameManager.Instance.setStatus(GameManager.STATE.EVENT);
                else 
                    moveShip();
            */
                break;
			case GameManager.STATE.EVENT: // Event Stage
                shipDamages();
				if (Input.GetButtonDown ("FIRE1")) {
				//takeDamage(5);
				}
                break;
			case GameManager.STATE.CARGOLOST:
				startLoseSequence();

				break;
            case GameManager.STATE.GOAL: //Reached Goal
                GameManager.Instance.setFollow(false);
                break;
        }
    }

    private void shipDamages()
    {	//commented out the if....statement to test
        //if (GameManager.Instance.getStatus() == GameManager.STATE.EVENT)
        //{
			if (Input.GetKeyDown(KeyCode.LeftArrow)) {
				cargoTakeDamage (5);
			}
			if (Input.GetKeyDown(KeyCode.RightArrow)) {
				shipTakeDamage (5);
			}
        //}
    }

	public void startLoseSequence()
	{
		innerCam.DOShakePosition(10.0f, 1.0f, 30);


	}

	public void shipTakeDamage(int i ) {
		if (currentHealth > 0) {
			currentHealth -= i;
			//healthSlider.value = currentHealth;
			//Debug.Log(currentHealth/100);
			healthImage.DOFillAmount (((float)currentHealth) / 100, 0.5f);

			//Randomizing which facility gets damaged
			int randomFaci = Random.Range (0,facilityList.Count);
			int randomCrit = Random.Range (0,2);
			if (randomCrit == 0)
			{
				facilityList[randomFaci].damageFacility(i);
			}
			//Debug.Log (healthImage.fillAmount);
			healthTxt.text = "Health: " + currentHealth;
			DOTween.Complete(innerCam.transform);
			innerCam.transform.DOShakePosition(0.5f, 5.0f, 30);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Projectile")) {
            if (!evade)
            {
                shipTakeDamage(other.GetComponent<enemyBullet>().damageValue);
				if ((Random.Range (0, 9)) < 4) {
					cargoTakeDamage ((other.GetComponent<enemyBullet> ().damageValue));
				}
				TweenHelper.FlashSprite(GetComponent<SpriteRenderer>(),0.2f);
                Destroy(other.gameObject);
            }
			else
			{

			}
            //else
            //    Debug.Log("Evaded");
        }
	}

	public void cargoTakeDamage(int i) {
		if (cargoHealth > 0) {
			CargoManager.Instance.cargoDamaged ("Cargo1", i);
			cargoHealth -= i;
			cargoHealthImage.DOFillAmount (((float)cargoHealth) / 100, 0.5f);
			cargoCountTxt.text = "Cargo Health: " + cargoHealth;
		}
	}

	public void moveShipHeight()
	{    
		heightChangeTimer -= Time.deltaTime;
		
		if (heightChangeTimer <= 0.0f) {
			heightAscent = heightVariantChange();
			heightChangeTimer = 15.0f; //Hard coded for now
		}
		//DOTween.Complete (gameObject.transform);
		//transform.position += new Vector3(1,heightAscent,0) * speed;
		//position += new Vector3(1, 0,0) * speed;
		//transform.DOMoveX (position.x, 5.0f, false);
		transform.DOMoveY (heightAscent, 5.0f, false);
		position = gameObject.transform.position;
		transform.position = position;
		//Debug.Log("Distance left : " + distance);

	}

    public void moveShip()
    {    
		heightChangeTimer -= Time.deltaTime;

		if (heightChangeTimer <= 0.0f) {
			heightAscent = heightVariantChange();
			heightChangeTimer = 15.0f; //Hard coded for now
		}

		//transform.position += new Vector3(1,heightAscent,0) * speed;
		//position += new Vector3(1, 0,0) * speed;

		position += Vector3.right * speed * Time.deltaTime;
		shipSpeedTxt.text = "Speed: " + (int)(speed * 15) + "km/h";
		transform.position = position;
        //####Changes
        sLOS.transform.position = transform.position;
        
        //transform.DOMoveX (position.x, 5.0f, false);
        //transform.DOMoveY (heightAscent, 10.0f, false);

        //Debug.Log("Distance left : " + distance);

    }

	public void setCurrentHealth(int f)
	{
		currentHealth = f;
	}

	//Returns random between descending and ascending
	public float heightVariantChange()
	{
		return Random.Range (-1.5f,1.5f);

	}		

	public Vector3 getPosition()
	{
		return transform.position;
	}

    public float ShipSpeed
    {
        get { return speed; }
        set { speed = value; }
    }

    public bool evadeChance
    {
        get { return evade; }
        set { evade = value; }
    }

    public void repairShip(int value)
    {
        currentHealth += value;
        if (currentHealth > healthCap) {
			currentHealth = healthCap;
		}

		healthImage.DOFillAmount (((float)currentHealth) / 100, 0.5f);
		healthTxt.text = "Health: " + currentHealth;
    }

    public List<GameObject> getTargetsInRange
    {
        get { return sLOS.GetComponent<LineOfSight>().targets; }
    }
}

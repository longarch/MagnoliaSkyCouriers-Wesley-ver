using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

using DG.Tweening;


public class Ship : MonoBehaviour
{
	private int Health, currentHealth = 100, cargoHealth = 100;
    public float position;

	[SerializeField]
    private float speed = 0.05f;
    [SerializeField]
    GameObject Goal;
	[SerializeField]
	float heightChangeTimer = 5.0f;
	private float heightAscent = 0;

	float maxHealth = 1.0f;
   

	[SerializeField]
	Image healthImage;

	[SerializeField]
	Image cargoHealthImage;

	[SerializeField]
	Text healthTxt, cargoCountTxt;

	[SerializeField]
	Camera innerCam;
    

    // Use this for initialization
    void Start()
    {
        position = 0;
        Health = 100;
		healthImage.fillAmount = maxHealth;
		cargoHealthImage.fillAmount = maxHealth;
		CargoManager.Instance.loadCargo ();
    }

    // Update is called once per frame
    void Update()
    {
		shipDamages (); //placed here to test
       // Debug.Log(Health);
        GameManager.Instance.checkShipStatus(this,Health);
        switch(GameManager.Instance.getStatus())
        {
            case GameManager.STATE.START: //Starting
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

	public void shipTakeDamage(int i ) {
		if (currentHealth > 0) {
			currentHealth -= i;
			//healthSlider.value = currentHealth;
			//Debug.Log(currentHealth/100);
			healthImage.DOFillAmount (((float)currentHealth) / 100, 0.5f);
			//Debug.Log (healthImage.fillAmount);
			healthTxt.text = "Health: " + currentHealth;
			innerCam.DOShakePosition(0.5f, 5.0f, 30);
		}
	}

	public void cargoTakeDamage(int i) {
		CargoManager.Instance.cargoDamaged ("Cargo1", i);
		cargoHealth -= i;
		cargoHealthImage.DOFillAmount(((float)cargoHealth)/100, 0.5f);
		cargoCountTxt.text = "Cargo Health: " + cargoHealth;
	}

    private void testEvent()
    {
        if (UnityEngine.Random.Range(0, 10) > 4)
        {
            GameManager.Instance.setStatus(GameManager.STATE.ONGOING);
        }
        else Health -= 1;
    }

    public void moveShip()
    {
		heightChangeTimer -= Time.deltaTime;

		if (heightChangeTimer <= 0.0f) {
			heightAscent = heightVariantChange();
			heightChangeTimer = 5.0f; //Hard coded for now
		}

		transform.position += new Vector3(1,heightAscent,0) * speed;

        float distance = Goal.transform.position.x - transform.position.x;
        
        //Debug.Log("Distance left : " + distance);
        
    }

	//Returns random between descending and ascending
	public float heightVariantChange()
	{


		return Random.Range (-0.2f,0.2f);

	}
}

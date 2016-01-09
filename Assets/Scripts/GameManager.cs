using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public enum STATE { START , TUTORIAL, ONGOING, EVENT, CARGOLOST, GOAL };
    private STATE gameMode;
    private bool followShip;

    //private float destination;
    [SerializeField]
    GameObject destination;

	[SerializeField]
	Image healthImage;

	[SerializeField]
	Image cargoHealthImage;

	[SerializeField]
	Text healthTxt, cargoCountTxt;

	private float maxHealth = 1.0f;
	private int currentHealth = 100, cargoHealth = 100;
	public CargoManager c;

	private float countDownTimer = 5.0f;


    public static GameManager Instance //can call from any other class w/o reference
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        _instance = this;



    }

 
    // Use this for initialization
    void Start()
    {
		//Debug.Log (healthImage.fillAmount);
        gameMode = STATE.START;


        followShip = false;
    }

    // Update is called once per frame
    void Update()
    {

		countDown ();

		if (Input.GetButtonDown ("Jump")) {
			//takeDamage(5);
		}
		if (Input.GetButtonDown ("Fire1")) {
			c.cargoDamaged ("Cargo1", 5);
			cargoHealth -= 5;
			cargoHealthImage.DOFillAmount(((float)cargoHealth)/100, 0.5f);
			cargoCountTxt.text = "Cargo Health: " + cargoHealth;
		}

    }

    public STATE getStatus()
    {
        return gameMode;
    }

    public Collider2D getDestination()
    {
        return destination.GetComponent<Collider2D>();
    }

    public void checkShipStatus(Ship shipPos, float shipHP)
    {
        if (gameMode.Equals(STATE.ONGOING))
        {
            //if (shipPos.GetComponent<Collider2D>().IsTouching(destination.GetComponent<Collider2D>())) 
            //if (shipPos >= destination)
              //gameMode = STATE.END;
        }
        if (shipHP <= 0)
            gameMode = STATE.CARGOLOST;
    }

    public void checkCargoStatus(float cargoHP)
    {
        if (cargoHP <= 0)
            gameMode = STATE.CARGOLOST;
    }

    public void setStatus(STATE curState)
    {
        gameMode = curState;

    }

	public void countDown()
	{
		if (gameMode == STATE.START) {
			countDownTimer -= Time.deltaTime;
			
			if (countDownTimer <= 0.0f) {
				gameMode = STATE.ONGOING;
				setFollow(true);
			}
		}
	}

    public void loadLevel(string s)
    {
        //Application.LoadLevel(s);
        //Debug.Log("Hello");
    }

    public bool shouldFollow()
    {
        return followShip;
    }

    public void setFollow(bool follow)
    {
        followShip = follow;
    }		
}
﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public enum STATE { START , TUTORIAL, ONGOING, EVENT, CARGOLOST, GOAL };
    [SerializeField]
	private STATE gameMode;
    private bool followShip;

    //private float destination;
    [SerializeField]
	GameObject destination;

	[SerializeField]
	Ship player;

	[SerializeField]
	Text distanceTxt;

	private float countDownTimer = 5.0f;
	public float maxDist;
	public int position;


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
        //gameMode = STATE.ONGOING;
				  
        followShip = false;
    }

    // Update is called once per frame
    void Update()
    {
		countDown ();
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
			if (maxDist <= 0.0f) {
				maxDist = getDestination().transform.position.x - shipPos.transform.position.x;
			}
            //if (shipPos.GetComponent<Collider2D>().IsTouching(destination.GetComponent<Collider2D>())) 
            //if (shipPos >= destination)
              //gameMode = STATE.END;
			//position = getDestination().transform.position.x - shipPos.transform.position.x; 
			position = (int)((shipPos.transform.position.x / maxDist) * 100) + 1;
			//Debug.Log (position);
			goalDistance ();

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
				gameObject.GetComponent<EnemyManager>().IsEnabled = true; //Turns on the enemy manager
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

	public void goalDistance ()
	{
		//distanceTxt.text = "Distance: " + ((int)position).ToString() + " km";
		//distanceTxt.text = "Distance: " + ((int)position + 1).ToString() + " %";
		distanceTxt.text = "Distance: " + position.ToString() + " %";
	}
}
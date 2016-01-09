using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Ship : MonoBehaviour
{
    private int Health;
    public float position;
	[SerializeField]
    private float speed = 0.05f;
    [SerializeField]
    GameObject Goal;
	[SerializeField]
	float heightChangeTimer = 5.0f;
	private float heightAscent = 0;
    // Use this for initialization
    void Start()
    {
        position = 0;
        Health = 100;
    }

    // Update is called once per frame
    void Update()
    {
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
                break;
            case GameManager.STATE.GOAL: //Reached Goal
                GameManager.Instance.setFollow(false);
                break;
        }
    }

    private void shipDamages()
    {
        if (GameManager.Instance.getStatus() == GameManager.STATE.EVENT)
        {

        }
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

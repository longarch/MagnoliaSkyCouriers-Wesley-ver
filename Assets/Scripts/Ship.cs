using UnityEngine;
using System.Collections;
using System;

public class Ship : MonoBehaviour
{
    private int Health;
    public float position;
    private float speed = 0.05f;
    [SerializeField]
    GameObject Goal;

    
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
        switch((int)GameManager.Instance.getStatus())
        {
            case 0: //Starting
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameManager.Instance.setStatus(GameManager.STATE.ONGOING);
                    GameManager.Instance.setFollow(true);
                }
                break;
            case 1: // On Route Stage
                if (UnityEngine.Random.Range(0, 1) > 4) //Random change to eventmode...will not be in here
                    GameManager.Instance.setStatus(GameManager.STATE.EVENT);
                else 
                    moveShip();
                break;
            case 2: // Event Stage
                shipDamages();
                break;
            case 4: //Reached Goal
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
        //transform.position += Vector3.right * speed;

        //float distance = Goal.transform.position.x - transform.position.x;
        
        //Debug.Log("Distance left : " + distance);
        
    }
}

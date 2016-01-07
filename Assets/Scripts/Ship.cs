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
    [SerializeField]
    Camera mainCam;
    
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
            case 0:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    GameManager.Instance.setStatus(GameManager.STATE.ONGOING);
                }
                break;
            case 1:
                //if (UnityEngine.Random.Range(0, 10) > 4)
                //GameManager.Instance.setStatus(GameManager.STATE.EVENT);
                //else 
                moveShip();
                break;
            case 2:
                testEvent();
                break;
            case 3:
                GameManager.Instance.loadLevel("Level_1");
                break;
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
        transform.position += Vector3.right * speed;
        //this.collider

        float distance = Goal.transform.position.x - transform.position.x;

        if (distance > 16)
            mainCam.transform.position += Vector3.right * speed;
        Debug.Log("Distance left : " + distance);

        //if (GameManager.Instance.getDestination().IsTouching(this.GetComponent<Collider2D>()))
        //if (distance <= 0)
          //  GameManager.Instance.setStatus(GameManager.STATE.END);
    }
}

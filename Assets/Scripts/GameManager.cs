using UnityEngine;
//using UnityEngine.UI;
using System.Collections;
using System;
//using DG.Tweening;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public enum STATE { START = 0, TUTORIAL, ONGOING, EVENT, CARGOLOST, GOAL };
    private STATE gameMode;
    private bool followShip;

    //private float destination;
    [SerializeField]
    GameObject destination;

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
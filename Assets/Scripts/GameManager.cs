using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance = null;
    public enum STATE { START = 0, ONGOING, EVENT, END };
    private STATE gameMode;

    private float destination;

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
        gameMode = STATE.START;
        destination = 100f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public STATE getStatus()
    {
        return gameMode;
    }

    public void checkShipStatus(float shipPos, float shipHP)
    {
        if (gameMode.Equals(STATE.ONGOING))
        {
            if (shipPos >= destination)
                gameMode = STATE.END;
        }
        if (shipHP <= 0)
            gameMode = STATE.END;
    }

    public void checkCargoStatus(float cargoHP)
    {
        if (cargoHP <= 0)
            gameMode = STATE.END;
    }

    public void setStatus(STATE curState)
    {
        gameMode = curState;
    }

    public void loadLevel(string s)
    {
        Application.LoadLevel(s);
        Debug.Log("Hello");
    }
}
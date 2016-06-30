using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using DG.Tweening;

public class TutorialGameManager : MonoBehaviour
{
	private static TutorialGameManager _instance = null;
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
	
	[SerializeField]
	DataStoresHandler _storeHandler;
	
	LevelLoadHandler _levelHandler;

	[SerializeField]
	TutorialObjectScript[] _tutObjects;

	[SerializeField]
	int counter = 0;
	
	[SerializeField]
	int Difficulty = 0;

	[SerializeField]
	int currentTutorialIndex = 0;

	public static TutorialGameManager Instance //can call from any other class w/o reference
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



		

		_tutObjects = FindObjectsOfType (typeof(TutorialObjectScript)) as TutorialObjectScript[];
		
		for (int i = 0; i < _tutObjects.Length; i ++)
		{
			_tutObjects[i].setGameManager(this);
			_tutObjects[i].setTutorialIndex(i);
		}

	
		
	}
	
	// Update is called once per frame
	void Update()
	{
		//countDown ();
	}

	public void GetNotified()
	{
		DialogManager.Instance.setUpDialog (currentTutorialIndex);
		currentTutorialIndex ++;
	}

	public int getTutorialIndex()
	{
		return currentTutorialIndex;
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
			if (shipHP <= 0)
			{
				Destroy (_storeHandler.gameObject);
				gameMode = STATE.CARGOLOST;
			}
		}
		
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
	
	public void SetEndlessJourney()
	{
		gameMode = STATE.ONGOING;
		setFollow(true);
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
	
	public int getDifficulty()
	{
		return Difficulty;
	}
	
	public void incrementDeath()
	{
		counter ++;
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
		
		if (position >= 100) {
			_storeHandler.setShiphealth(player.currentHealth);
			_storeHandler.setCargoHealth(player.cargoHealth);
			_storeHandler.setEnemyKill(counter);
			if (Difficulty == 0)
			{
				_storeHandler.setDifficulty(1);
			}
			else
			{
				_storeHandler.setDifficulty(2);
			}
			TutorialGameManager.Instance.setStatus(TutorialGameManager.STATE.GOAL);
		}
	}
}
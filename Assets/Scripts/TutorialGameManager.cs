using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using DG.Tweening;

public class TutorialGameManager : MonoBehaviour
{
	private static TutorialGameManager _instance = null;

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


	

	

}
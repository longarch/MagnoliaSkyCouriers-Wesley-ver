using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour {

	public enum SpawnType
	{
		Enemy,
		Raider,
		Dragon
	}

	public List<BaseEnemy> enemy;

	[SerializeField]
	GameObject raider;
	[SerializeField]
	GameObject dragon;
	[SerializeField]
	GameObject playerObject;
    [SerializeField]
    Ship player;

	[Serializable]
	public class EnemySpawnSettings
	{
		//Obsolete
		[SerializeField]
		private EnemyPool.EnemyType _enemyType;
		[SerializeField]
		private EnemyPool.EnemyType[] _spawnEnemyTypes;
		[SerializeField]
		private float _probability = 1.0f;
		[SerializeField]
		private bool _alterEnemySpeed = false;
		[SerializeField]
		private float _enemySpeed;
		[SerializeField]
		private bool _burstSpawn = false;
		[SerializeField]
		private int _burstAmount = 3;
		[SerializeField]
		private float _burstInterval = 0.5f;
		
		
		
		
		public EnemyPool.EnemyType EnemyType
		{
			get { return _enemyType; }
		}
		
		public float Probability
		{
			get { return _probability; }
		}
		
		public float EnemySpeed
		{
			get { return _enemySpeed; }
		}
		
		public bool AlterEnemySpeed
		{
			get { return _alterEnemySpeed; }
		}
		
		public bool IsBurstSpawn
		{
			get { return _burstSpawn; }
		}
		
		public int BurstAmount
		{
			get { return _burstAmount; }
		}
		
		public float BurstInterval
		{
			get { return _burstInterval; }
		}
		
		public EnemyPool.EnemyType GetRandomSpawnEnemyType()
		{
			if (_spawnEnemyTypes.Length <= 0)
			{
				return _enemyType;
			}
			return _spawnEnemyTypes[Random.Range(0, _spawnEnemyTypes.Length)];
		}
	}

	[SerializeField]
	private bool _enabled = true;
	[SerializeField]
	private bool _limitToMax = false; // Can only spawn a given number of objects before stopping
	[SerializeField]
	private int _spawns = 0, _maxSpawns = 10, _currentInScene = 0, _maxInScene = 4;
	[SerializeField]
	private bool _spawnAtLocation;
	[SerializeField]
	private SpawnType _spawnType = SpawnType.Raider;
	[SerializeField]
	private EnemySpawnSettings[] _spawnSettings;
	[SerializeField]
	private GameObject _spawnObject;

	public BaseEnemy enemies;
	float eventTime = 5.0f, spawnTime;



	public bool IsEnabled
	{
		get { return _enabled; }
		set
		{
			if (_enabled == value)
			{
				return;
			}
			_enabled = value;
			if (_enabled)
			{
				InvokeRepeating("Spawn", _spawnInterval, _spawnInterval);
			}
			else
			{
				CancelInvoke("Spawn");
			}
		}
	}

	[SerializeField]
	private float _spawnInterval = 2.0f;
	private float _timeElapsed = 0;

	//int i, currentNo = 0, maxNo = 3;
	// Use this for initialization
	void Start () {
		spawnTime = eventTime;
		enemy = new List<BaseEnemy> ();
		//player = new Ship ();
		/*
		if (_enabled)
		{
			InvokeRepeating("Spawn", _spawnInterval, _spawnInterval);
		}
		*/
	}

	// Update is called once per frame
	void Update () {

		if (GameManager.Instance.getStatus () == GameManager.STATE.START) {
			return;
		}

		_timeElapsed += Time.deltaTime;
		_timeElapsed = Mathf.Repeat(_timeElapsed, _spawnInterval);

		/*
		eventTime -= Time.deltaTime;
		if (eventTime <= 0) {
			Spawn ();
			eventTime = spawnTime;
		}*/
	}

	public void Spawn()
	{
		if (GameManager.Instance.getStatus() != GameManager.STATE.ONGOING) {
			return;
		}

		/*
		if (GameController.Instance.IsGameOver)
		{
			
			return;
		}*/
		
		if (_limitToMax && _spawns >= _maxSpawns)
		{
			CancelInvoke("Spawn");
			_enabled = false;
			return;
		}
		
		if (_currentInScene >= _maxInScene)
		{
			return;
		}
		
		// Determine spawn position
		Vector3 spawnPosition = new Vector3();
		
		if (_spawnAtLocation)
		{
			spawnPosition = transform.position;
		}
		else
		{
			//spawnPosition.x = Random.Range(-_spawnBounds.x, _spawnBounds.x);
			//spawnPosition.z = Random.Range(-_spawnBounds.y, _spawnBounds.y);
			//spawnPosition.y = 1.1f;
		}
		
		GameObject spawnedObject = null;
		if (_spawnType == SpawnType.Enemy)
		{
			float random = Random.value;
			float cumulativeProb = 0;
			foreach (EnemySpawnSettings setting in _spawnSettings)
			{
				cumulativeProb += setting.Probability;
				if (random <= cumulativeProb)
				{
					// Spawn!
					int spawnAmount = setting.IsBurstSpawn ? setting.BurstAmount : 1;
					Sequence spawnSequence = DOTween.Sequence();
					spawnSequence.AppendCallback(() =>
					                             {
						BaseEnemy _baseEnemy = EnemyPool.Instance.SpawnEnemy(new Vector3(), setting.GetRandomSpawnEnemyType());
						//BaseEnemy(_baseEnemy);
						spawnedObject = _baseEnemy.gameObject;
						
						if (setting.AlterEnemySpeed)
						{
							//_baseEnemy.spee = setting.EnemySpeed;
						}
						
						//virezEnemy.AddOnKillCallback(DecreaseOnSceneCount);
						spawnedObject.transform.position = spawnPosition;
						_spawns++;
						_currentInScene++;
						enemy.Add (_baseEnemy);

					}).AppendInterval(setting.BurstInterval).SetLoops(spawnAmount);
					
					break;
				}
			}
			
		}
		else
		{
			Instantiate(_spawnObject, new Vector3(), Quaternion.identity);
		}
		
	}
	//Obselete
	/*
	void Spawn()
	{
        if (player.currentHealth <= 0 || currentNo >= maxNo) {
			return;
		} else {
            //Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
            Debug.Log("Start Spawn 1 " + playerObject.transform.position.x);
            Debug.Log("Start Spawn 2 " + player.getPosition().x);
            i = randomType ();
			if (randomType () < 1) {
                //Debug.Log(playerObject.transform.position.x);
				enemies = EnemyPool.Instance.SpawnEnemy(new Vector3(playerObject.transform.position.x - 2,
                    Random.Range(player.getPosition().y - 10, player.getPosition().y + 10), 0), EnemyPool.EnemyType.Dragon);

                //Instantiate(dragon, new Vector3 (playerObject.transform.position.x - 10,
					//Random.Range (playerObject.transform.position.y - 10, playerObject.transform.position.y + 10), 0), Quaternion.identity) as GameObject;
				enemy.Add (enemies);
				currentNo++;
				spawnTime = 15f; //change the time between spawns
			} else {
				enemies = EnemyPool.Instance.SpawnEnemy(new Vector3(playerObject.transform.position.x - 2,
                    Random.Range(player.getPosition().y - 10, player.getPosition().y + 10), 0), EnemyPool.EnemyType.Raider);

                //Instantiate (raider, new Vector3 (playerObject.transform.position.x - 10,
                //Random.Range (playerObject.transform.position.y - 10, playerObject.transform.position.y + 10), 0), Quaternion.identity) as GameObject;
                enemy.Add (enemies);
				currentNo++;
				spawnTime = 10f; //change the time between spawns
			}
		}
		Debug.Log (enemy.Count);
	}

	*/
	/*
	//Scenarios more dragons than raiders or more raiders than dragons
	public void Spawn(int count, int state)
	{
		maxNo = count;
		switch (state) {
		case 1: //more dragons than raiders
			i = randomType (state);
			if (randomType () < 1) {
				enemies = EnemyPool.Instance.SpawnEnemy(new Vector3(playerObject.transform.position.x - 2,
					Random.Range(player.getPosition().y - 10, player.getPosition().y + 10), 0), EnemyPool.EnemyType.Raider);
				enemy.Add (enemies);
				currentNo++;
				spawnTime = 15f; //change the time between spawns
			} else {
				enemies = EnemyPool.Instance.SpawnEnemy(new Vector3(playerObject.transform.position.x - 2,
					Random.Range(player.getPosition().y - 10, player.getPosition().y + 10), 0), EnemyPool.EnemyType.Dragon);
				enemy.Add (enemies);
				currentNo++;
				spawnTime = 10f; //change the time between spawns
			}
			break;
		case 2: //more raiders than dragons
			i = randomType (state);
			if (randomType () < 1) {
				enemies = EnemyPool.Instance.SpawnEnemy(new Vector3(playerObject.transform.position.x - 2,
					Random.Range(player.getPosition().y - 10, player.getPosition().y + 10), 0), EnemyPool.EnemyType.Dragon);
				enemy.Add (enemies);
				currentNo++;
				spawnTime = 15f; //change the time between spawns
			} else {
				enemies = EnemyPool.Instance.SpawnEnemy(new Vector3(playerObject.transform.position.x - 2,
					Random.Range(player.getPosition().y - 10, player.getPosition().y + 10), 0), EnemyPool.EnemyType.Raider);
				enemy.Add (enemies);
				currentNo++;
				spawnTime = 10f; //change the time between spawns
			}
			break;
		}
	}
	*/
	private int randomType()
	{
		int i = Random.Range (0, 10);

		return i;
	}

	private int randomType(int i)
	{
		int j = 0;
		switch (i){
		case 1:
			j = Random.Range (0, 3);
			return j;
		case 2:
			j = Random.Range (0, 5);
			return j;
		}
		return j;
	}
}
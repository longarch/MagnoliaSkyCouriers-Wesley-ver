using System;
using UnityEngine;
using System.Collections.Generic;
#pragma warning disable 649

/*
 * Class with method called pooling to avoid resource destruction of gameobjects
 * */
public class EnemyPool : MonoBehaviour
{
	
	public enum EnemyType
	{
		None,
		Raider,
		Dragon
	}
	
	[SerializeField]
	private GameObject _normalEnemyPrefab, _heavyEnemyPrefab;
	
	[SerializeField] private int _normalEnemyPoolSize = 10, 
	_heavyEnemyPoolSize = 8;



	private Dictionary<EnemyType, GameObject> _enemyDictionary;
	private Dictionary<EnemyType, List<BaseEnemy>> _enemyPoolDictionary;
	private Dictionary<EnemyType, int> _enemyPoolSizeDictionary; 



	private static EnemyPool _poolInstance;
	
	public static EnemyPool Instance
	{
		get
		{
			return _poolInstance;
		}
	}
	
	void Awake()
	{
		// Perform initialization of the Singleton
		if (_poolInstance != null && _poolInstance != this)
		{
			Destroy(gameObject);
		}
		_poolInstance = this;
		
		_enemyDictionary = new Dictionary<EnemyType, GameObject>();
		_enemyDictionary.Add(EnemyType.Raider, _normalEnemyPrefab);
		_enemyDictionary.Add(EnemyType.Dragon, _heavyEnemyPrefab);

		
		
		_enemyPoolDictionary = new Dictionary<EnemyType, List<BaseEnemy>>();
		_enemyPoolDictionary.Add(EnemyType.Raider, new List<BaseEnemy>());
		_enemyPoolDictionary.Add(EnemyType.Dragon, new List<BaseEnemy>());
		
		
		_enemyPoolSizeDictionary = new Dictionary<EnemyType, int>();
		_enemyPoolSizeDictionary.Add(EnemyType.Raider, _normalEnemyPoolSize);
		_enemyPoolSizeDictionary.Add(EnemyType.Dragon, _heavyEnemyPoolSize);
		
		
	}
	
	void Start()
	{
		InitPool();
	}
	
	public BaseEnemy SpawnEnemy(Vector3 spawnPosition, EnemyType enemyType)
	{
		foreach (BaseEnemy enemy in _enemyPoolDictionary[enemyType])
		{
			if (!enemy.gameObject.activeInHierarchy)
			{
                //enemy.transform.position = spawnPosition;
                enemy.Setup(spawnPosition);
				enemy.reset();
				if (enemyType == EnemyType.Dragon)
				{
					enemy.transform.localScale = _heavyEnemyPrefab.transform.localScale;
				}
				else
				{
					enemy.transform.localScale = _normalEnemyPrefab.transform.localScale;
				}
               
				enemy.gameObject.SetActive(true);
				return enemy;
			}
		}
		

		GameObject newEnemyObject = Instantiate(_enemyDictionary[enemyType], spawnPosition, Quaternion.identity) as GameObject;
		if (newEnemyObject != null)
		{
            BaseEnemy newEnemy = newEnemyObject.GetComponent<BaseEnemy>();
			_enemyPoolDictionary[enemyType].Add(newEnemy);
			
			return newEnemy;
		}
		
		return null;
	}
	
	public List<BaseEnemy> GetAllActiveEnemies()
	{
		List<BaseEnemy> enemies = new List<BaseEnemy>();
		foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
		{
			if (enemyType == EnemyType.None)
			{
				continue;
			}
			foreach (BaseEnemy enemy in _enemyPoolDictionary[enemyType])
			{
				if (enemy.gameObject.activeInHierarchy)
				{
					enemies.Add(enemy);
				}
			}
		}
		
		return enemies;
	}
	

	//public void AddEnemy(BaseEnemy newEnemy)
	//{
		//_enemyPoolDictionary[EnemyType.Normal].Add(newEnemy);
	//}
	
	public void AddEnemyByType(EnemyType enemyType, BaseEnemy newEnemy)
	{
		_enemyPoolDictionary[enemyType].Add(newEnemy);
	}
	
	public bool IsInPool(EnemyType enemyType, BaseEnemy enemyToCheck)
	{
		return _enemyPoolDictionary[enemyType].Contains(enemyToCheck);
	}
	
	/**
     * Pre-instantiate bullets at the start to avoid expensive calls later
     */
	void InitPool()
	{
		foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
		{
			if (enemyType == EnemyType.None)
			{
				continue;
			}
			for (int i = 0; i < _enemyPoolSizeDictionary[enemyType]; i++)
			{
				GameObject enemyObject = Instantiate(_enemyDictionary[enemyType]) as GameObject;
				if (enemyObject != null)
				{
                    BaseEnemy enemy = enemyObject.GetComponent<BaseEnemy>();
					enemy.gameObject.SetActive(false);
					_enemyPoolDictionary[enemyType].Add(enemy);
				}
			}
		}
	}
}

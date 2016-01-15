using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyHealth : Killable
{
	[SerializeField]
	private int _health, _maximumHealth;
	
	private bool _onKillCalled = false;
	
	public override int Health
	{
		get { return _health; }
		set
		{
			_health = value;
			if (_health <= 0 && !_onKillCalled)
			{
				foreach (OnKilled killCallback in _onKilledList)
				{
					killCallback(this);
				}
				_onKillCalled = true;
			}
		}
	}
	
	public override int MaximumHealth
	{
		get { return _maximumHealth; }
		
		set { _maximumHealth = value; }
	}
	
	// Use this for initialization
	void Awake()
	{
		_onKilledList = new List<OnKilled>();
		if (_showUi)
		{
			_healthUI = GetComponent<HealthUI>();        
		}
	}
	
	public override void Reset()
	{
		_onKillCalled = false;
		Health = MaximumHealth;
		_healthUI.reenableHealthBars ();
	}
	
}

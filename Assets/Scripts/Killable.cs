using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Killable : MonoBehaviour
{
	
	[SerializeField]
	protected bool _showUi;
	
	public delegate void OnKilled(Killable killable);
	
	// Note - callbacks in onKilledList have to be called by the class extending Killable
	protected List<OnKilled> _onKilledList;
	
	protected HealthUI _healthUI;
	
	public abstract int Health { get; set; }
	
	public abstract int MaximumHealth { get; set; }
	
	public float HealthFraction
	{
		get { return (Health + 0.0f)/MaximumHealth; }
	}
	
	public void AddOnKillCallback(OnKilled newCallback)
	{
		if (_onKilledList.Contains(newCallback))
		{
			return;
		}
		_onKilledList.Add(newCallback);
	}
	
	public void UpdateUI()
	{
		if (_showUi)
		{
			_healthUI.UpdateUI();
		}
	}
	
	public void HideUI()
	{
		if (_showUi)
		{
			_healthUI.HideUI();        
		}
	}
	
	public abstract void Reset();
	
}

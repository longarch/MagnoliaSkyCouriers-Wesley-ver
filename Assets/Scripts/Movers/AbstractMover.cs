using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMover : MonoBehaviour
{
	[SerializeField]
	protected float _speed = 5;
	
	protected bool _isMoving = false;
	
	public bool IsMoving
	{
		get { return _isMoving; }
		set { _isMoving = value; }
	}
	
	public float Speed
	{
		get
		{
			return _speed;
		}
		set { _speed = value; }
	}
	
	public delegate void MoveCompleteDelegate();
	
	protected List<MoveCompleteDelegate> _moveCompleteDelegates;
	
	public void AddMoveComplete(MoveCompleteDelegate moveCompleteDelegate)
	{
		_moveCompleteDelegates.Add(moveCompleteDelegate);
	}
	
	public void RemoveMoveComplete(MoveCompleteDelegate moveCompleteDelegate)
	{
		_moveCompleteDelegates.Remove(moveCompleteDelegate);
	}
	
	public void OnMoveComplete()
	{
		foreach (MoveCompleteDelegate moveComplete in _moveCompleteDelegates.ToArray())
		{
			if (moveComplete == null) continue;
			moveComplete();
		}
	}
	
	public abstract void MoveTowards(Vector3 destination);
	
	public abstract void MoveTowards(Vector3 destination, bool ignoreInterval);
	
	public abstract void StopMoving();
	
	public abstract void CalculatePath();
}

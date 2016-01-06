using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractMover : MonoBehaviour
{
	[SerializeField]
	protected float _speed = 8;
	
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
	public delegate void MoveUpdateDelegate(Vector3 currentPosition);
	
	protected List<MoveCompleteDelegate> _moveCompleteDelegates;
	protected List<MoveUpdateDelegate> _moveUpdateDelegates;
	
	
	public void AddMoveComplete(MoveCompleteDelegate moveCompleteDelegate)
	{
		_moveCompleteDelegates.Add(moveCompleteDelegate);
	}
	
	public void OnMoveComplete()
	{
		foreach (MoveCompleteDelegate moveComplete in _moveCompleteDelegates)
		{
			moveComplete();
		}
	}
	
	public void AddMoveUpdate(MoveUpdateDelegate moveUpdateDelegate)
	{
		_moveUpdateDelegates.Add(moveUpdateDelegate);
	}
	
	public void OnMoveUpdate()
	{
		foreach (MoveUpdateDelegate moveUpdate in _moveUpdateDelegates)
		{
			moveUpdate(transform.position);
		}
	}
	
	
	/**
     * Start the movement towards the given destination. Only has to be called once to initiate movement
     */
	public abstract void MoveTowards(Vector3 destination);
	
	/**
     * Move towards the first destination for the given duration before moving towards the final destination
     */
	public abstract void MoveTowardsForTime(Vector3 firstDestination, float duration, Vector3 finalDestination);
	
	public abstract void StopMoving();
	
	public abstract void CalculatePath(); // Only needed for pathfinding based movers
	
	//public void ResumeMovement()
	//{
	//    _isMoving = true;
	//}
}

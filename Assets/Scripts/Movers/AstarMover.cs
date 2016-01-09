using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using Pathfinding;

/**
 * Movement using the A* Pathfinding system
 */
[RequireComponent(typeof(Seeker))]
public class AstarMover : AbstractMover
{
	public enum MoveMethod
	{
		CharacterController,
		Rigidbody,
		Direct
	}
	[SerializeField]
	private float _closenessThreshold = 1.5f;
	
	[SerializeField]
	private MoveMethod _moveMethod;
	
	private Vector3 _goalPosition;
	private Vector3 _postPrimaryMoveGoalPosition;
	
	private Path _calculatedPath;
	
	private int _currentWaypoint = 0;
	
	private CharacterController _characterController;
	private Seeker _seeker;
	
	
	private float _moveInterval = 0.3f, _elapsedInterval = 0.0f; // Only recalculate every moveInterval seconds 
	
	void Awake()
	{
		_seeker = GetComponent<Seeker>();
		_moveCompleteDelegates = new List<MoveCompleteDelegate>();
		_characterController = GetComponent<CharacterController>();
	}
	
	void Update()
	{
		_elapsedInterval += Time.deltaTime;
		if (!_isMoving)
		{
			return;
		}
		if (_calculatedPath == null)
		{
			return;
		}
		
		if (_currentWaypoint >= _calculatedPath.vectorPath.Count)
		{
			// Reached destination
			OnMoveComplete();
			_calculatedPath = null;
			return;
		}
		Vector3 waypoint = _calculatedPath.vectorPath[_currentWaypoint];
		Vector3 dir = (waypoint - transform.position).normalized;
		
		if (_moveMethod == MoveMethod.CharacterController)
		{
			dir *= _speed;
			_characterController.SimpleMove(dir);
		}
		else if (_moveMethod == MoveMethod.Direct)
		{
			dir *= _speed * Time.deltaTime;
			transform.position += dir;
		}
		else if (_moveMethod == MoveMethod.Rigidbody)
		{
			dir *= _speed * Time.deltaTime;
			GetComponent<Rigidbody2D>().velocity = dir; //.AddForce(dir, ForceMode.VelocityChange);
		}
		
		if (Vector3.Distance(transform.position, waypoint) <= _closenessThreshold)
		{
			_currentWaypoint++;
			return;
		}
	}
	
	public void SetNewDestination(Vector3 newPosition)
	{
		_goalPosition = newPosition;
		CalculatePath();
	}
	
	public override void CalculatePath()
	{
		_seeker.StartPath(transform.position, _goalPosition, OnPathCalculated);
	}
	
	public bool IsCalculatingPath()
	{
		return !_seeker.IsDone();
	}
	
	public override void StopMoving()
	{
		_isMoving = false;
		//_calculatedPath = null;
	}
	
	public void ResetPath()
	{
		_calculatedPath = null;
	}
	
	private void OnPathCalculated(Path path)
	{
		_calculatedPath = path;
		_currentWaypoint = 0;
		_isMoving = true;
	}
	
	
	public override void MoveTowards(Vector3 destination)
	{
		if (destination == _goalPosition)
		{
			return;
		}
		else
		{
			if (_elapsedInterval >= _moveInterval)
			{
				_elapsedInterval = 0.0f;
				ResetPath();
				SetNewDestination(destination);
			}
		}
	}
	
	public override void MoveTowards(Vector3 destination, bool ignoreInterval)
	{
		if (ignoreInterval)
		{
			_elapsedInterval = 0.0f;
			ResetPath();
			SetNewDestination(destination);
		}
		else
		{
			MoveTowards(destination);
		}
	}
	
}

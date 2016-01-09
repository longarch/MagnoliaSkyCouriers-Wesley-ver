using UnityEngine;
using System.Collections.Generic;

public class DirectMover : AbstractMover
{
	[SerializeField]
	private float _closenessThreshold = 1.0f;
	
	private Vector3 _destination;
	
	void Awake()
	{
		_moveCompleteDelegates = new List<MoveCompleteDelegate>();
	}
	
	void Update()
	{
		if (_isMoving)
		{
			Vector3 displacement = _speed * (_destination - transform.position).normalized * Time.deltaTime;
			transform.position += displacement;
			if (Vector3.Distance(transform.position, _destination) <= _closenessThreshold)
			{
				_isMoving = false;
				OnMoveComplete();
			}
		}
	}
	
	public override void MoveTowards(Vector3 destination)
	{
		_isMoving = true;
		_destination = destination;
	}
	
	public override void MoveTowards(Vector3 destination, bool ignoreInterval)
	{
		MoveTowards(destination);
	}
	
	public override void StopMoving()
	{
		_isMoving = false;
	}
	
	public override void CalculatePath()
	{
	}
}

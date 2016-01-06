using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * A mover which moves the transform directly, ignoring physics
 */
public class DirectMover : AbstractMover
{
	[SerializeField]
	private float _closenessThreshold = 1.0f;
    [SerializeField]
    private Animator animator;
    private Vector3 _destination;
	
	void Awake()
	{
		_moveUpdateDelegates = new List<MoveUpdateDelegate>();
		_moveCompleteDelegates = new List<MoveCompleteDelegate>();
	}
	
	void Update()
	{
		if (_isMoving)
		{
            //Debug.Log ("Move!");
            if (transform.position.x < _destination.x) //move right
            {
                if (Mathf.Abs(transform.position.x - _destination.x) > 2)
                    animator.SetInteger("Direction", 3);
                else {
                    if (transform.position.y > _destination.y) //move front
                        animator.SetInteger("Direction", 0);
                    else if (transform.position.y < _destination.y) //move back
                        animator.SetInteger("Direction", 2);
                }
            }
            else if (transform.position.x > _destination.x) //move left
            {
                if (Mathf.Abs(transform.position.x - _destination.x) > 2)
                    animator.SetInteger("Direction", 1);
                else {
                    if (transform.position.y > _destination.y) //move front
                        animator.SetInteger("Direction", 0);
                    else if (transform.position.y < _destination.y) //move back
                        animator.SetInteger("Direction", 2);
                }
            }

            Vector3 displacement = _speed * (_destination - transform.position).normalized * Time.deltaTime;
			transform.position += displacement;
            

            OnMoveUpdate();
			if (Vector3.Distance(transform.position, _destination) <= _closenessThreshold)
			{
                animator.SetInteger("Direction", 4); //Idle
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
	
	public override void MoveTowardsForTime(Vector3 firstDestination, float duration, Vector3 finalDestination)
	{
		throw new System.NotImplementedException();
	}
	
	public override void StopMoving()
	{
        //animator.SetInteger("Direction", 4);
        _isMoving = false;
	}
	
	public override void CalculatePath()
	{
	}
}

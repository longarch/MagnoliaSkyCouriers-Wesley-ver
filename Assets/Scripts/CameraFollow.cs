// Obselete/ Not In used
using UnityEngine;
using DG.Tweening;

/**
 * Script that forces the camera to follow the target object
 */
public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	private Transform _followTarget;
	[SerializeField]
	private bool _isDeactivated;
	
	[SerializeField]
	private bool _lockX, _lockY, _lockZ;
	[SerializeField]
	private bool _use2DBounds;
	[SerializeField]
	private Rect _boundsRect2D;
	[SerializeField]
	private float _smoothTime = 0.5f;
	
	private Vector3 _positionDifference;
	
	private Quaternion _previousCameraRotation;
	
	private Vector3 _velocity = Vector3.zero;
	
	public Transform FollowTarget
	{
		get
		{
			return _followTarget;
		}
		set
		{
			_followTarget = value;
			_positionDifference = transform.position - _followTarget.transform.position;
		}
	}
	
	public bool Use2DBounds
	{
		get { return _use2DBounds; }
		set { _use2DBounds = value; }
	}
	
	public Rect BoundsRect2D
	{
		get { return _boundsRect2D; }
		set { _boundsRect2D = value; }
	}
	
	public bool IsDeactivated
	{
		get
		{
			return _isDeactivated;
		}
		set
		{
			_isDeactivated = value;
			
		}
	}
	
	public float SmoothTime
	{
		get { return _smoothTime; }
		set { _smoothTime = value; }
	}
	
	void Start()
	{
		_positionDifference = transform.position - _followTarget.transform.position;
	}
	
	void Update()
	{
		if (_isDeactivated)
		{
			return;
		}
		Vector3 newPosition = _followTarget.transform.position + _positionDifference;
		
		if (_lockX) newPosition.x = transform.position.x;
		if (_lockY) newPosition.y = transform.position.y;
		if (_lockZ) newPosition.z = transform.position.z;
		
		if (_use2DBounds)
		{
			if (!_lockX)
			{
				if (newPosition.x <= _boundsRect2D.xMin)
				{
					newPosition.x = _boundsRect2D.xMin;
				}
				if (newPosition.x >= _boundsRect2D.width)
				{
					newPosition.x = _boundsRect2D.width;
				}
			}
			
			if (!_lockY)
			{
				if (newPosition.y <= _boundsRect2D.yMin)
				{
					newPosition.y = _boundsRect2D.yMin;
				}
				if (newPosition.y >= _boundsRect2D.yMax)
				{
					newPosition.y = _boundsRect2D.yMax;
				}
			}
		}
		transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref _velocity, _smoothTime);
	}
	
	public void SetLocks(bool lockX, bool lockY, bool lockZ)
	{
		_lockX = lockX;
		_lockY = lockY;
		_lockZ = lockZ;
	}
	
	public void RepositionCamera()
	{
		_positionDifference = transform.position - _followTarget.transform.position;
	}
	
	public void PauseCamera()
	{
		_previousCameraRotation = transform.rotation;
		_isDeactivated = true;
	}
	
	public void ResumeCamera()
	{
		Vector3 newPosition = _followTarget.transform.position + _positionDifference;
		Sequence sequence = DOTween.Sequence();
		sequence.Insert(0, transform.DOMove(newPosition, 1.0f));
		sequence.Insert(0, transform.DORotate(_previousCameraRotation.eulerAngles, 1.0f));
		sequence.OnComplete(ActivateCamera);
	}
	
	private void ActivateCamera()
	{
		_isDeactivated = false;
	}
}

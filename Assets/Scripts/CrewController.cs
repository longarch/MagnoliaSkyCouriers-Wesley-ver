﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CrewController : MonoBehaviour {

    private float _lastPressTime = -1;
    private Vector3 _firstPressLocation;
    bool _canMove = true;

    [SerializeField] private LayerMask _movableLayerMask;
    [SerializeField] private LayerMask _selectionLayerMask;
    [SerializeField] private float _touchDistanceThreshold = 50.0f;
    [SerializeField] private float _clickRegisterTime = 0.3f;

    [SerializeField]
    Camera cam;
    private AbstractMover _mover;

    [SerializeField]
    private List<GameObject> crew;
    // Use this for initialization
    void Start () {
        _mover = crew[0].GetComponent<AbstractMover>(); //Default moving is the 1st character
    }
	
	// Update is called once per frame
	void Update () {
        HandleMovement();
    }


    private void HandleMovement()
    {
        if (!_canMove)
        {
            return;
        }

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.phase == TouchPhase.Began)
                {
                    _lastPressTime = Time.time;
                    _firstPressLocation = touch.position;
                }
                else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                         && (Time.time - _lastPressTime <= _clickRegisterTime))
                {
                    _lastPressTime = -1;

                    if (Vector3.Distance(_firstPressLocation, touch.position) < _touchDistanceThreshold)
                    {
                        // Counted
                        bool touched = CheckScreenPoint(touch.position);
                        if (touched)
                        {
                            return;
                        }
                    }
                }
            }
        }
        else
        {
            // Only use mouse controls if no touch detected
            if (Input.GetMouseButtonDown(0))
            {
                _lastPressTime = Time.time;
                _firstPressLocation = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (Time.time - _lastPressTime <= _clickRegisterTime
                    && Vector3.Distance(_firstPressLocation, Input.mousePosition) < _touchDistanceThreshold)
                {
                    _lastPressTime = -1;
                    bool clicked = CheckScreenPoint(Input.mousePosition);
                    if (clicked)
                    {
                    }
                }
            }
        }
    }

    private bool CheckScreenPoint(Vector3 screenPoint)
    {
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity,
                                             _selectionLayerMask.value);
        if (hit.collider != null)
        {
            Debug.Log("Target Position in crew Layer: " + hit.collider.gameObject.name);
            _mover = hit.collider.GetComponent<AbstractMover>();
        }
        else {
            RaycastHit2D moveTo = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity,
                                             _movableLayerMask.value);
            //Debug.Log("Target Position in moving Layer: " + moveTo.collider.gameObject.name);
            Vector3 moveWorldPos = new Vector3(moveTo.point.x, moveTo.point.y, 0);
            _mover.StopMoving();
            _mover.MoveTowards(moveWorldPos);
        }
        return true;
    }

}

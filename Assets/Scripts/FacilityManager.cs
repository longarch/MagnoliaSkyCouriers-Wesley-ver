using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class FacilityManager : MonoBehaviour {

	[SerializeField]
	List<GameObject> facilitiesList;

	[SerializeField]
	Camera Fieldcam;
	[SerializeField] private LayerMask _selectionLayerMask;
	[SerializeField] private LayerMask _selectionLayerMask1;

	[SerializeField]
	bool[] isOver;
	static float actionDelay = 0.5f;
	static bool actionOut;
	Color oriColor;

	int facil;

	// Use this for initialization
	void Start () {
		isOver = new bool[facilitiesList.Count];
	}
	
	// Update is called once per frame
	void Update () {
		ToggleActionIcon ();
		FacilityActionButtons ();
		//CheckActionAccessibility (); //Place holder currently for calling of actions, use for testing

		/*if (!isOver) {
			if (over != null)
			{
			if (actionOut) {
				over.GetComponentInChildren<Facility> ().OnMouseExit ();
			}
			}
		}*/
	}

	void FacilityActionButtons()
	{
		//Ray ray = camReference.ScreenPointToRay (Input.mousePosition);
		if (Input.GetMouseButtonUp (0)) {
			RaycastHit2D hit = Physics2D.Raycast(Fieldcam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity,
				_selectionLayerMask.value);
			if (hit.collider != null) {
				Debug.Log ("hit something!--Action " + hit.collider.gameObject.name);
				Debug.Log ("hit something!--Action " + facilitiesList[facil].name + hit.collider.gameObject.name);
				switch (hit.collider.gameObject.name) {
				//Engine Actions
				case "EngineAction1": //For Repair
					//facilitiesList [0].GetComponentInChildren<Facility> ().repairFacility ();
					break;
				case "EngineAction2": //For Operation
					facilitiesList [0].GetComponentInChildren<Facility> ().CallActions ();
					break;
				case "EngineAction3":
					break;
					//Ship Control Actions
				case "ControlAction1": //For Repair
					//facilitiesList [1].GetComponentInChildren<Facility> ().repairFacility ();
					break;
				case "ControlAction2": //For Operation
					facilitiesList [1].GetComponentInChildren<Facility> ().CallActions ();
					break;
				case "ControlAction3":
					break;
					//Magic Turret Actions
				case "MagicAction1": //For Repair
					//facilitiesList [2].GetComponentInChildren<Facility> ().repairFacility ();
					break;
				case "MagicAction2": //For Operation
					oriColor = hit.collider.gameObject.GetComponent<SpriteRenderer>().color;
					facilitiesList [2].GetComponentInChildren<Facility> ().CallActions ();
					//hit.collider.gameObject.GetComponent<SpriteRenderer>().DOColor(Color.gray, 0.5f);
					break;
				case "MagicAction3":
					break;
					//Shell Gun Actions
				case "ShellAction1": //For Repair
					//facilitiesList [3].GetComponentInChildren<Facility> ().repairFacility ();
					break;
				case "ShellAction2": //For Operation
					facilitiesList [3].GetComponentInChildren<Facility> ().CallActions ();
					break;
				case "ShellAction3":
					break;
					//Ship Core Actions
				case "CoreAction1": //For Repair
					//facilitiesList [4].GetComponentInChildren<Facility> ().repairFacility ();
					break;
				case "CoreAction2": //For Operation
					facilitiesList [4].GetComponentInChildren<Facility> ().CallActions ();
					break;
				case "CoreAction3":
					break;
				default:
					break;
				}
			}
		}
	}

	void ToggleActionIcon()
	{
		//if (isOver) {
		//	return;
		//}
		RaycastHit2D hit = Physics2D.Raycast(Fieldcam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity,
			_selectionLayerMask1.value);
		if (hit.collider != null) {
			Debug.Log("hit something! in FM " + hit.collider.gameObject.name);
			switch (hit.collider.gameObject.name) {
			case "EngineHoverArea":
			case "EngineActionIconBar":
				facil = 0;
				isOver[facil] = true;
				break;
			case "ControlHoverArea":
			case "ControlActionIconBar":
				facil = 1;
				isOver[facil] = true;
				break;
			case "MagicHoverArea":
			case "MagicActionIconBar":
				facil = 2;
				isOver[facil] = true;
				break;
			case "ShellHoverArea":
			case "ShellActionIconBar":
				facil = 3;
				isOver[facil] = true;
				break;
			case "CoreHoverArea":
			case "CoreActionIconBar":
				facil = 4;
				isOver[facil] = true;
				break;
			default:
				facil = -1;
			break;
			}
			if (facil != -1) {
				if (isOver[facil]) {
					facilitiesList [facil].GetComponentInChildren<Facility> ().OnMouseEnter ();
				}
			}
		} else {
			for (int i = 0; i < isOver.Length; i++) {
				isOver [i] = false;
				if (facil != -1) {
					facilitiesList [facil].GetComponentInChildren<Facility> ().OnMouseExit ();
				}
			}
		}
	}

	//For testing
	void CheckActionAccessibility()
	{
		if (!facilitiesList [facil].GetComponentInChildren<Facility> ().getIsOver ()) {
			return;
		} else {
			RaycastHit2D hit = Physics2D.Raycast(Fieldcam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity,
				_selectionLayerMask1.value);
			if (hit.collider != null) {
				Debug.Log ("hit something!--Action " + hit.collider.gameObject.name);
				Debug.Log ("hit something!--Action " + facilitiesList[facil].name + hit.collider.gameObject.name);
				switch (hit.collider.gameObject.name) {
				//Engine Actions
				case "EngineAction1": //For Repair
					//facilitiesList [0].GetComponentInChildren<Facility> ().CallActions ();
					break;
				case "EngineAction2": //For Operation
					facilitiesList [0].GetComponentInChildren<Facility> ().CallActions ();
					break;
				case "EngineAction3":
					break;
					//Ship Control Actions
				case "ControlAction1": //For Repair
					break;
				case "ControlAction2": //For Operation
					facilitiesList [1].GetComponentInChildren<Facility> ().CallActions ();
					break;
				case "ControlAction3":
					break;
					//Magic Turret Actions
				case "MagicAction1": //For Repair
					break;
				case "MagicAction2": //For Operation
					facilitiesList [2].GetComponentInChildren<Facility> ().CallActions ();
					break;
				case "MagicAction3":
					break;
					//Shell Gun Actions
				case "ShellAction1": //For Repair
					break;
				case "ShellAction2": //For Operation
					facilitiesList [3].GetComponentInChildren<Facility> ().CallActions ();
					break;
				case "ShellAction3":
					break;
					//Ship Core Actions
				case "CoreAction1": //For Repair
					break;
				case "CoreAction2": //For Operation
					facilitiesList [4].GetComponentInChildren<Facility> ().CallActions ();
					break;
				case "CoreAction3":
					break;
				default:
					break;
				}
			}
		}
	}

	public void CallActions()
	{
		Debug.Log ("Test Actions");
		facilitiesList [facil].GetComponentInChildren<Facility> ().CallActions ();
	}

}

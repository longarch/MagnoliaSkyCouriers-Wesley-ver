using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FacilityManager : MonoBehaviour {

	[SerializeField]
	List<GameObject> facilitiesList;

	[SerializeField]
	Camera Fieldcam;
	[SerializeField] private LayerMask _selectionLayerMask;

	[SerializeField]
	bool[] isOver;
	static float actionDelay = 4f;
	static bool actionOut;

	int facil;

	// Use this for initialization
	void Start () {
		isOver = new bool[facilitiesList.Count];

	}
	
	// Update is called once per frame
	void Update () {
		ToggleActionIcon ();

		/*if (!isOver) {
			if (over != null)
			{
			if (actionOut) {
				
				over.GetComponentInChildren<Facility> ().OnMouseExit ();
			}
			}
		}*/
	}


	void ToggleActionIcon()
	{
		//if (isOver) {
		//	return;
		//}
		RaycastHit2D hit = Physics2D.Raycast(Fieldcam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity,
			_selectionLayerMask.value);
		if (hit.collider != null) {

			Debug.Log("hit something!" + hit.collider.gameObject.name);
			switch (hit.collider.gameObject.name) {
			case "Engine":
				facil = 0;
				isOver[facil] = true;
				break;
			case "ShipControls":
				facil = 1;
				isOver[facil] = true;
				break;
			case "Magicturret":
				facil = 2;
				isOver[facil] = true;
				break;
			case "Shellgun":
				facil = 3;
				isOver[facil] = true;
				break;
			case "ShipCore":
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
			/*if ( hit.collider.gameObject.name == this.gameObject.transform.parent.gameObject.name)
			{
				Debug.Log ("yes");
				this.gameObject.GetComponentInChildren<Facility>().OnMouseEnter();
			}*/
		} else {
			for (int i = 0; i < isOver.Length; i++) {
				isOver [i] = false;
				if (facil != -1) {
					facilitiesList [facil].GetComponentInChildren<Facility> ().OnMouseExit ();
				}
			}
			//Set a timer for the exit function
			//if (this.gameObject.GetComponentInChildren<Facility>().getIsOver())
			//{
				//isOver = false;
			//}
		}
	}

}

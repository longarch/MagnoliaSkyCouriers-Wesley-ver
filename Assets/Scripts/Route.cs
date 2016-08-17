using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Route : MonoBehaviour {

	public int routeID;

	public int routeDays;

	public string routeDescription;


	// Use this for initialization
	void Start () {




	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseEnter()
	{
		
		Debug.Log ("Found something");
		//GamePrepController.Instance.DisplayRouteInfo("Test","Description","Status","Estimate");
	}

	void OnMouseOver()
	{

	}

	public void FitColliderToChildren(Vector3 startVectorPos , Vector3 goalVectorPos)
	{
		BoxCollider bc = gameObject.GetComponent<BoxCollider>();



		Vector3 newTrans = Vector3.Lerp(startVectorPos,goalVectorPos,0.5f);

		//bc.center = newTrans;

		Bounds bounds = new Bounds(newTrans,gameObject.GetComponent<BoxCollider>().size);
		bounds.Encapsulate (gameObject.GetComponent<LineRenderer> ().bounds);

		bc.size = bounds.size;

		/*
		Bounds bounds = new Bounds (Vector3.zero, Vector3.zero);
		bool hasBounds = false;
		Renderer[] renderers =  gameObject.GetComponentsInChildren<Renderer>();
		foreach (Renderer render in renderers) {
			if (hasBounds) {
				bounds.Encapsulate(render.bounds);
			} else {
				bounds = render.bounds;
				hasBounds = true;
			}
		}
		if (hasBounds) {
			bc.center = bounds.center - gameObject.transform.position;
			bc.size = bounds.size;
		} else {
			bc.size = bc.center = Vector3.zero;
			bc.size = Vector3.zero;
		}
		*/
	}




}

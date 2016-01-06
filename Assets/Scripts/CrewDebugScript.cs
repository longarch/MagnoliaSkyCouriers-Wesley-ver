using UnityEngine;
using System.Collections;

public class CrewDebugScript : MonoBehaviour {

	[SerializeField]
	Camera cam;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	

	}

	void checkForCasting()
	{
		RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
		
		if(hit.collider != null)
		{
			Debug.Log ("Target Position: " + hit.collider.gameObject.name);
		}

	}

}

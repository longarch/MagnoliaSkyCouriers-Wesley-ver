using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour {

	[SerializeField]
	float backgroundSpeed = 2;
	float bgWidth =0;

	// Use this for initialization
	void Start () {
		bgWidth = gameObject.GetComponent<Renderer>().bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 vel  = this.GetComponent<Rigidbody2D>().position;
		//vel+= new Vector3(-backgroundSpeed,0,0);
		//this.GetComponent<Rigidbody2D>().position = vel;


		if (Camera.main.WorldToScreenPoint(gameObject.transform.position).x < 0) {
			//Vector2 pos = gameObject.transform.position + Camera.main.rect.position;
			//pos.x +=  bgWidth;
			//this.transform.position = pos;
		}

		/*
		if(Camera.main.transform.position.x > gameObject.transform.position.x){
			Vector2 pos = gameObject.transform.position;
			pos.x +=  bgWidth;
			this.transform.position = pos;
		}*/
		//if background offcamera, transform to the right of the current background
	}
}

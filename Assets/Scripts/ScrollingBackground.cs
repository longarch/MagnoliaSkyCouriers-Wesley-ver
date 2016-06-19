using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour {

	[SerializeField]
	float backgroundSpeed = 0.5f;
	//float bgWidth =0;
	Renderer render;
	// Use this for initialization
	void Start () {
		render = gameObject.GetComponent<Renderer> ();
	//	bgWidth = gameObject.GetComponent<Renderer>().bounds.size.x;
	}

	public void setBGSpeed(float i)
	{
		backgroundSpeed = i;

	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 vel  = this.GetComponent<Rigidbody2D>().position;
		//vel+= new Vector3(-backgroundSpeed,0,0);
		//this.GetComponent<Rigidbody2D>().position = vel;
		Vector2 offset = new Vector2 (Time.time * backgroundSpeed, 2);
		render.material.mainTextureOffset = offset;

		//Debug.Log (backgroundSpeed.ToString ());

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

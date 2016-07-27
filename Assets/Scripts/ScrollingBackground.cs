using UnityEngine;
using System.Collections;

public class ScrollingBackground : MonoBehaviour {

	[SerializeField]
	float backgroundSpeed = 0.5f;
	//float bgWidth =0;
	Renderer render;

	Ship ship;
	// Use this for initialization

	Vector3 BGPos;

	void Start () {
		render = gameObject.GetComponent<Renderer> ();
	//	bgWidth = gameObject.GetComponent<Renderer>().bounds.size.x;

		ship = GameObject.FindObjectOfType<Ship> ();

		BGPos = this.gameObject.transform.position;

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

		if (ship != null) {
			BGPos = new Vector3(ship.gameObject.transform.position.x,
			                    this.gameObject.transform.position.y,
			                    this.gameObject.transform.position.z);

			this.gameObject.transform.position = BGPos;
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

using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hellooooooo");
        Debug.Log(other);
        if (other.name == "Ship")
            GameManager.Instance.setStatus(GameManager.STATE.GOAL);
        if (other.name == "Camera")
            //GameManager.Instance.setFollow(false);
            other.GetComponentInParent<CameraFollow>().IsDeactivated = true;
    }
    
}

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
        //if (other.GetComponent<GameObject>().name == "Ship")
            GameManager.Instance.setStatus(GameManager.STATE.END);
    }
}

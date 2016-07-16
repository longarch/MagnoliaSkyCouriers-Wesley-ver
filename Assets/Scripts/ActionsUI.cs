using UnityEngine;
using System.Collections;

public class ActionsUI : MonoBehaviour {

	[SerializeField]
	private Sprite _backgroundSprite, _foregroundSprite;

	[SerializeField]
	private Camera cam;

	[SerializeField] private Vector2 _offset;
	[SerializeField]
	public GameObject actionIndicator1, actionIndicator2, actionIndicator3;
	GameObject over;
	static float actionDelay = 4f;
	static bool actionOut;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseExit(){
		//if (!isOver)
		//return;
		Debug.Log("Testing in out");
		while (actionDelay > 0) {
			actionDelay -= Time.deltaTime;
		}
		if (actionDelay <= 0) {
			Debug.Log (this.name + "out");
			actionIndicator1.SetActive (false);
			actionIndicator1.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
			actionIndicator2.SetActive (false);
			actionIndicator2.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
			actionIndicator3.SetActive (false);
			actionIndicator3.transform.localScale = new Vector3 (0.3f, 0.3f, 0.3f);
			//isOver = false;
			actionOut = false;
			actionDelay = 4f;
			over = null;
		}
	}
}

using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	[SerializeField]
	private Button start_btn;
	[SerializeField]
	private Image pausePnl;

	private bool paused;
	public CargoManager c;

	// Use this for initialization
	void Start () {
		if (pausePnl != null) {
			pausePnl.gameObject.SetActive (false);
		}
		paused = false;
		CargoManager c = new CargoManager ();
	}

	// Update is called once per frame
	void Update () {

	}

	public void loadLevel(string s)
	{
		Application.LoadLevel (s);
	}	

	public void startButton ()
	{		
		//start_btn.gameObject.transform.DOMoveX (start_btn.gameObject.transform.position.x - 1.0f, 2.0f, false);
		//loadLevel("Level_1");
	}

	public void pause_btn() //in-game pause button
	{
		if (paused) {

			Sequence sequence = DOTween.Sequence();
			//sequence.Complete();
			sequence.Append(pausePnl.rectTransform.DOLocalMoveY(950, 1.0f, false));

			Time.timeScale = 1.0f;
			paused = false;
			pausePnl.gameObject.SetActive (false);
		} else {			
			pausePnl.gameObject.SetActive (true);
			Sequence sequence = DOTween.Sequence();
			//sequence.Complete ();
			sequence.SetUpdate (true);
			//DOTween.Complete (pausePnl.rectTransform);
			sequence.Append(pausePnl.rectTransform.DOLocalMoveY(0, 1.0f, false));

			Time.timeScale = 0.0f;
			paused = true;
			//Debug.Log (pausePnl.gameObject.transform.position.y);
		}				
	}

	public void restart_btn()
	{
		c.cargoDamaged ("Cargo1", 5);
	}

	public void quit_btn()
	{
		// check if in main menu or in-game
		loadLevel("SelectionScene_2");
	}


}

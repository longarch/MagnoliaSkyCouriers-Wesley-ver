using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CutSceneManager : MonoBehaviour {


	int _sceneIndex = 0;
	int expectedScenes = 8;
	bool _isCutScene;

	private static CutSceneManager _instance = null;

	public static CutSceneManager Instance
	{
		get { return _instance; }
	}


	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		_instance = this;

		_sceneIndex = 0;

	}

	// Use this for initialization

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setIsCutScene(bool b)
	{
		_isCutScene = true;
	}

	public bool getIsCutScene()
	{
		return _isCutScene;
	}

	public int getSceneIndex()
	{
		return _sceneIndex;
	}

	public void setSceneIndex(int i )
	{
		_sceneIndex = i;
	}

	public void incrementSceneIndex()
	{
		_sceneIndex ++;
	}

	public void animateScene()
	{
		switch (_sceneIndex) {
		case 0:
			DialogManager.Instance.setUpCutSceneDialog(0);
			break;
		case 1:
			DialogManager.Instance.setUpCutSceneDialog(1);

			break;
		case 2:
			UIManager.Instance.StartTutorialOpeningSequence();
				/*
			if ()
			{
				incrementSceneIndex();
				animateScene();
			}
			*/
			break;
		case 3:
			DialogManager.Instance.setUpCutSceneDialog(2);
			TutorialGameManager.Instance.enableTutorialObject(0,true);

			break;
		case 4:

			break;
		case 7:

			break;
		case 8:

			break;
		}

		Debug.Log ("Current scene at: " + _sceneIndex);
	}


}

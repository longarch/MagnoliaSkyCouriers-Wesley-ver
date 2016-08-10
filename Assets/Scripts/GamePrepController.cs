using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;
public class GamePrepController : MonoBehaviour {

	[SerializeField]
	Image _faderPanel;

	[SerializeField]
	RectTransform _menuPanel;

	[SerializeField]
	Image _contractsPanel;

	[SerializeField]
	RectTransform _contractsContainer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void transitToOutpost()
	{
		_faderPanel.gameObject.SetActive (true);
		Sequence sequence = DOTween.Sequence();
		//DOTween.Complete (_spriteFeedback.transform);
		sequence.Insert(0.1f,_faderPanel.DOFade (1.0f, 0.5f));
		sequence.Append(_menuPanel.DOAnchorPosY(200,0.5f,false));
		//sequence.Insert(0.1f,txtOpeningText.rectTransform.DOAnchorPosY(0,1.0f,false));
		sequence.Append(_faderPanel.DOFade (0.0f, 0.5f).SetDelay(1.0f));
		//sequence.Insert(1.1f,txtOpeningText.rectTransform.DOAnchorPosY(800,1.0f,false));
		sequence.OnComplete(() =>
		                    {
			_faderPanel.gameObject.SetActive (false);
			_menuPanel.DOAnchorPos(new Vector2(0,0),0.5f,false);
			_contractsPanel.gameObject.SetActive(true);
			_contractsContainer.DOAnchorPos(new Vector2(-460,0),0.5f,false);
			//camera.orthographic = !currentMode;
			//_spriteFeedback.SetActive (false);
			//gameObject.SetActive(false);
		});


	}

	public void transitToMap()
	{

		_contractsPanel.gameObject.SetActive(false);
		_faderPanel.gameObject.SetActive (true);
		Sequence sequence = DOTween.Sequence();
		//DOTween.Complete (_spriteFeedback.transform);
		sequence.Insert(0.1f,_faderPanel.DOFade (1.0f, 0.5f));
		sequence.Insert(0.1f,_contractsContainer.DOAnchorPos(new Vector2(-1500,0),0.5f,false));
		sequence.Append(_menuPanel.DOAnchorPosY(200,0.5f,false));
		//sequence.Insert(0.1f,txtOpeningText.rectTransform.DOAnchorPosY(0,1.0f,false));
		sequence.Append(_faderPanel.DOFade (0.0f, 0.5f).SetDelay(1.0f));
		//sequence.Insert(1.1f,txtOpeningText.rectTransform.DOAnchorPosY(800,1.0f,false));
		sequence.OnComplete(() =>
		                    {

			_faderPanel.gameObject.SetActive (false);
			_menuPanel.DOAnchorPos(new Vector2(0,0),0.5f,false);



		});
	}
}

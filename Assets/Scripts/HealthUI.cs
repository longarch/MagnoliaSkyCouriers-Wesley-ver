using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 649

public class HealthUI : MonoBehaviour
{
	
	[SerializeField]
	private Sprite _backgroundSprite, _foregroundSprite;

	[SerializeField]
	private Camera cam;

	
	[SerializeField] private GameObject _healthbarPrefab, _numberPrefab;
	
	[SerializeField] private Vector2 _healthbarSize, _offset;
	
	[SerializeField] private Color _foregroundColor, _backgroundColor;
	
	[SerializeField]
	private bool _useNumber, _autoUpdate, _destroyOnDeath = true, _usePresetBossBar = false;
	
	[SerializeField] private GameObject _healthBarBasePrefab;

	[SerializeField]
	private GameObject _iconRepairBasePrefab;
	
	private Image _healthBarBaseFill;
	
	private GameObject _healthBarBase;
	
	private Image _backgroundImage, _foregroundImage, _repairImage;
	private Text _numberText;
	
	private Killable _followObject;

	private bool isDead = false;
	
	private Canvas _canvasParent;
	private CanvasScaler _canvasScaler;
	private float _xMultiplier, _baseWidth;

	[SerializeField]
	private RectTransform forcedTrans;
	
	public bool DestroyOnDeath
	{
		get { return _destroyOnDeath; }
		set { _destroyOnDeath = value; }
	}
	
	void Awake()
	{
		if (!enabled)
		{
			return;
		}
		_followObject = GetComponent<Killable>();
		//_canvasParent = FindObjectOfType<Canvas>();
		_canvasParent = GameObject.Find ("pauseCanvas").GetComponent<Canvas> ();
		
		_canvasScaler = _canvasParent.GetComponent<CanvasScaler>();
		
		if (_useNumber)
		{
			GameObject numberObject = Instantiate(_numberPrefab, new Vector3(), Quaternion.identity) as GameObject;
			numberObject.transform.SetParent(_canvasParent.transform, false);
			numberObject.transform.SetAsFirstSibling();
			_numberText = numberObject.GetComponent<Text>();
		}
		else if (_usePresetBossBar)
		{
			_healthBarBase = Instantiate(_healthBarBasePrefab, new Vector3(), Quaternion.identity) as GameObject;
			_healthBarBaseFill = _healthBarBase.transform.GetChild(0).GetComponent<Image>();
		}
		else
		{
			GameObject backgroundObject = Instantiate(_healthbarPrefab, new Vector3(), Quaternion.identity) as GameObject;
			backgroundObject.transform.SetParent(_canvasParent.transform, false);
			backgroundObject.transform.SetAsFirstSibling();
			_backgroundImage = backgroundObject.GetComponent<Image>();
			
			GameObject foregroundObject = Instantiate(_healthbarPrefab, new Vector3(), Quaternion.identity) as GameObject;
			foregroundObject.transform.SetParent(_canvasParent.transform, false);
			foregroundObject.transform.SetSiblingIndex(1);
			_foregroundImage = foregroundObject.GetComponent<Image>();

			GameObject btnObject = Instantiate(_iconRepairBasePrefab, new Vector3(), Quaternion.identity) as GameObject;
			btnObject.transform.SetParent(_canvasParent.transform, false);
			btnObject.transform.SetSiblingIndex(2);
			_repairImage = btnObject.GetComponent<Image>();
			_repairImage.gameObject.SetActive(false);
			
			_backgroundImage.rectTransform.sizeDelta = _healthbarSize;
			_foregroundImage.rectTransform.sizeDelta = _healthbarSize;
			
			_backgroundImage.color = _backgroundColor;
			_foregroundImage.color = _foregroundColor;

			_foregroundImage.type = Image.Type.Filled;
			_foregroundImage.fillMethod = Image.FillMethod.Horizontal;
			_foregroundImage.fillOrigin = (int)Image.OriginHorizontal.Left;
			
			_baseWidth = _foregroundImage.rectTransform.sizeDelta.x;
		}
		
	}
	
	// Use this for initialization
	void Start()
	{

	}
	
	void Update()
	{
		if (_autoUpdate)
		{
			UpdateUI();
		}
	}
	
	public void HideUI()
	{
		_backgroundImage.color = new Color(1, 1, 1, 0);
		_foregroundImage.color = new Color(1, 1, 1, 0);
	}
	
	// Update is called once per frame
	public void UpdateUI()
	{
		//Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, _followObject.transform.position);
		
		//_xMultiplier = _canvasScaler.referenceResolution.x / Screen.width;
		//screenPoint *= _xMultiplier;

		if (!checkIsDead() || GameManager.Instance.getStatus() != GameManager.STATE.CARGOLOST || 
		    GameManager.Instance.getStatus() != GameManager.STATE.GOAL) {

			Vector3 worldPos = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
			Vector3 screenPos = cam.WorldToScreenPoint (worldPos);
			_backgroundImage.transform.position = new Vector3 (screenPos.x, screenPos.y + _offset.y, screenPos.z);
			_foregroundImage.transform.position = new Vector3 (screenPos.x, screenPos.y + _offset.y, screenPos.z);

			//float newWidth = _followObject.HealthFraction * _baseWidth;
			//Vector2 healthbarDimensions = new Vector2 (newWidth, _foregroundImage.rectTransform.sizeDelta.y);
			_foregroundImage.fillAmount = _followObject.HealthFraction;
		}

		if (_destroyOnDeath && (_followObject.gameObject == null || _followObject.Health <= 0
		                        || GameManager.Instance.getStatus() == GameManager.STATE.CARGOLOST
		                        || GameManager.Instance.getStatus() == GameManager.STATE.GOAL))
		{
			_backgroundImage.gameObject.SetActive(false);
			_foregroundImage.gameObject.SetActive(false);

			//Destroy(_backgroundImage.gameObject);
			//Destroy(_foregroundImage.gameObject);
			//Destroy(this);
		}

	}

	public void reenableHealthBars()
	{

		_foregroundImage.gameObject.SetActive(true);
		_backgroundImage.gameObject.SetActive(true);
		isDead = false;
	}

	public bool checkIsDead()
	{
		return isDead;
	}


	
}

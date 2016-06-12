using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BaseEnemy : MonoBehaviour {

    [SerializeField]
    protected GameObject player;

    //####Changes
    [SerializeField]
    protected GameObject eLOS;
    bool isItInRange;

    public enum EnemyType
    {
        None,
        Raider,
        Dragon
    }

    [SerializeField]
    protected float heightChangeTimer = 1.0f, speed = 0.01f, heightAscent = 0;

    protected Vector3 position;
	[SerializeField]
    protected int currentHealth;
    [SerializeField]
    protected EnemyType type = EnemyType.None;

	[SerializeField]
	GameObject _spriteFeedback;
    bool targeted;

	float chaseTimer = 0;

	//Chasing variables
	[SerializeField]
	bool isChasing;
	float _closenessThreshold = 5.0f;
	float _tooCloseThreshold = 2.0f;
	float _chaseMultiplier = 1.2f;
	float _chaseTime = 3.0f; //Check every 3 seconds;

	private Rigidbody2D _rigidBody;


	protected Killable _healthHandler;

	Ship _ship;

	Vector3 destination;
    void Awake()
    {
		_healthHandler = GetComponent<Killable>();
        player = GameObject.Find("Ship");
        heightChangeTimer = 5.0f;
		speed = player.GetComponent<Ship> ().ShipSpeed;
		_ship = player.GetComponent<Ship> ();
        //speed = 0.05f;
        heightAscent = randomOffset(-2,-1);
		_closenessThreshold = randomOffset (2, 4);
        //currentHealth = 25;
		_spriteFeedback.SetActive (false);
        targeted = false;
		isChasing = true;
		_rigidBody = gameObject.GetComponent<Rigidbody2D> ();
        //####Changes
        eLOS = Instantiate(eLOS, transform.position, Quaternion.identity) as GameObject;
        eLOS.GetComponent<LineOfSight>().setAssigned(gameObject);
        eLOS.SetActive(false);
		_healthHandler.Health = currentHealth;
		_healthHandler.MaximumHealth = currentHealth;
    }

    // Use this for initialization
    void Start () {
		_chaseTime = Random.Range (2, 4);
		position = gameObject.transform.position;
		destination = new Vector3(player.transform.position.x - _closenessThreshold,player.transform.position.y + randomOffset(-3,-2),0);
        //####Changes
        eLOS.SetActive(true);
        //transform.DOMoveY(player.transform.position.y + heightAscent, 1.0f, false);

    }
	
	// Update is called once per frame
	void Update () {
        moveEnemy();

		_chaseTime -= Time.deltaTime;
		if (_chaseTime <= 0.0f) {
			_chaseTime = 3.0f;
			_chaseMultiplier = Random.Range(0.9f,1.2f);
		}

    }

    //Returns random between descending and ascending
    public float heightVariantChange()
    {
        return Random.Range(-1.4f, 1.4f);
    }

	public float randomOffset(float min,float max)
	{
		return Random.Range(min, max);
	}


    public void TakeDamage(int i)
    {

		_healthHandler.Health -= i;
		_healthHandler.Health = Mathf.Clamp(_healthHandler.Health, 0, _healthHandler.MaximumHealth);
		if (_healthHandler.Health <= 0) {
			GameManager.Instance.incrementDeath();
			gameObject.SetActive(false);
			//currentHealth -= i;
			//healthSlider.value = currentHealth;
			//Debug.Log(currentHealth/100);
			//healthImage.DOFillAmount (((float)currentHealth) / 100, 0.5f);
			//Debug.Log (healthImage.fillAmount);
			//healthTxt.text = "Health: " + currentHealth;
			///innerCam.DOShakePosition(0.5f, 5.0f, 30);
		} 
    }

	public void setHealth(float i)
	{ 
		_healthHandler.Health = (int)(_healthHandler.Health * i);
		_healthHandler.MaximumHealth = _healthHandler.Health;
	}

    public int getHealth()
    {
        return _healthHandler.Health;
    }

	public void promptDeathCallback()
	{
		_spriteFeedback.SetActive (true);
		Sequence sequence = DOTween.Sequence();
		DOTween.Complete (_spriteFeedback.transform);
		sequence.Append(_spriteFeedback.transform.DOPunchScale (new Vector3 (0.05f, 0.05f, 0.05f), 0.5f, 5, 1));
		sequence.OnComplete(() =>
		                    {

			//camera.orthographic = !currentMode;
			currentHealth = 25;
			_spriteFeedback.SetActive (false);
			gameObject.SetActive(false);

		});
	}

    public EnemyType returnEType()
    {
        return type;
    }

    public void moveEnemy()
    {
        heightChangeTimer -= Time.deltaTime;

        if (heightChangeTimer <= 0.0f)
        {
            heightAscent = heightVariantChange();
            heightChangeTimer = 5.0f; //Hard coded for now
        }
		if (isChasing) {

			transform.DOMove (player.transform.position + new Vector3(0,heightAscent,0),_chaseTime, false);
			position = gameObject.transform.position;

			/*
			if (Vector3.Distance(transform.position, player.transform.position) <= _closenessThreshold)
			{

				isChasing = false;

			}
			*/
			if (Mathf.Abs(transform.position.x - player.transform.position.x) <= _closenessThreshold)
			{
				
				isChasing = false;
				
			}
			/*.OnComplete (() =>
			{
				gameObject.transform.position = destination;
				isChasing = false;
				//camera.orthographic = !currentMode;
				//_spriteFeedback.SetActive (false);
				//gameObject.SetActive(false);
			});
			*/
		} else {
			//position = transform.position;

			if (Mathf.Abs(transform.position.x - player.transform.position.x) <= _tooCloseThreshold)
			{
				
				speed = _ship.ShipSpeed;
				
			}
			else
			{
				speed = _ship.ShipSpeed * _chaseMultiplier ;
			}


			position += Vector3.right * speed * Time.deltaTime;
			transform.position = position;

		}

        //####Changes
        eLOS.transform.position = transform.position;
        //_rigidBody.DOMoveX(position.x, 5.0f, false);
        //_rigidBody.DOMoveY(player.transform.position.y + heightAscent, 4.0f, false);

        //transform.DOMoveX(player.transform.position.x, 10.0f, false);
        //transform.DOMove(new Vector3(1,heightAscent,0) * speed, 10.0f, false);
        //Debug.Log("Distance left : " + distance);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
            if (other.gameObject.layer == LayerMask.NameToLayer("ShipProjectile"))
            {
				TweenHelper.FlashSprite(GetComponent<SpriteRenderer>(),0.2f);
				//TweenHelper.FlashSprite(
                //Debug.Log("My position is: " + gameObject.transform.position.x.ToString() + "," + gameObject.transform.position.y.ToString());
                //Debug.Log("Ouch");
                TakeDamage(other.GetComponent<enemyBullet>().damageValue);
                Destroy(other.gameObject);
           	}
        
    }

	public void reset()
	{
		//Debug.Log ("Reseting!");
		heightChangeTimer = 5.0f;
		speed = player.GetComponent<Ship> ().ShipSpeed;
		heightAscent = randomOffset(2,-1);
		_closenessThreshold = randomOffset (2, 4);
		//currentHealth = 100;_healthHandler.Reset();
		_healthHandler.Reset();
		_healthHandler.Health = currentHealth;

		_spriteFeedback.SetActive (false);
		targeted = false;
		isChasing = true;
		_chaseTime = Random.Range (2, 4);
		position = gameObject.transform.position;
		destination = new Vector3(player.transform.position.x - _closenessThreshold,player.transform.position.y + randomOffset(-5,5),0);
	}


    public void Setup(Vector3 startingPos)
    {
        position = startingPos;
    }

    public bool Targeted
    {
        get { return targeted; }
        set { targeted = value; }
    }

	public void AddOnKillCallback(Killable.OnKilled callback)
	{
		

		
		_healthHandler.AddOnKillCallback(callback);
	}

    //####Changes
    public bool inRange
    {
        get { return isItInRange; }
        set { isItInRange = value; }
    }
}

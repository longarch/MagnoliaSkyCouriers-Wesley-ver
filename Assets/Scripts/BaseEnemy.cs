using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BaseEnemy : MonoBehaviour {

    [SerializeField]
    protected GameObject player;

    public enum EnemyType
    {
        None,
        Raider,
        Dragon
    }

    [SerializeField]
    protected float heightChangeTimer = 1.0f, speed = 0.01f, heightAscent = 0;

    protected Vector3 position;
    protected int currentHealth;
    [SerializeField]
    protected EnemyType type = EnemyType.None;

	[SerializeField]
	GameObject _spriteFeedback;
    bool targeted;

	//Chasing variables
	bool isChasing;
	float _closenessThreshold = 5.0f;
	float _chaseTime = 0;

	private Rigidbody2D _rigidBody;

	Vector3 destination;
    void Awake()
    {
        player = GameObject.Find("Ship");
        heightChangeTimer = 5.0f;
        //speed = 0.05f;
        heightAscent = 0;
        currentHealth = 100;
		_spriteFeedback.SetActive (false);
        targeted = false;
		isChasing = true;
		_rigidBody = gameObject.GetComponent<Rigidbody2D> ();
    }

    // Use this for initialization
    void Start () {
		_chaseTime = Random.Range (4, 5);
		position = gameObject.transform.position;
		destination = new Vector3(player.transform.position.x - _closenessThreshold,player.transform.position.y + heightAscent,0);

		//transform.DOMoveY(player.transform.position.y + heightAscent, 1.0f, false);

	}
	
	// Update is called once per frame
	void Update () {
        moveEnemy();
    }

    //Returns random between descending and ascending
    public float heightVariantChange()
    {
        return Random.Range(-1.4f, 1.4f);
    }

    public void TakeDamage(int i)
    {
        if (currentHealth > 0)
        {
            currentHealth -= i;
            //healthSlider.value = currentHealth;
            //Debug.Log(currentHealth/100);
            //healthImage.DOFillAmount (((float)currentHealth) / 100, 0.5f);
            //Debug.Log (healthImage.fillAmount);
            //healthTxt.text = "Health: " + currentHealth;
            ///innerCam.DOShakePosition(0.5f, 5.0f, 30);
        }
    }

    public int getHealth()
    {
        return currentHealth;
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
			currentHealth = 100;
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

			transform.DOMove (player.transform.position,_chaseTime, false);
			position = gameObject.transform.position;
			if (Vector3.Distance(transform.position, player.transform.position) <= _closenessThreshold)
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
			position += Vector3.right * speed * Time.deltaTime;
			transform.position = position;

		}

		//_rigidBody.DOMoveX(position.x, 5.0f, false);
		//_rigidBody.DOMoveY(player.transform.position.y + heightAscent, 4.0f, false);

        //transform.DOMoveX(player.transform.position.x, 10.0f, false);
        //transform.DOMove(new Vector3(1,heightAscent,0) * speed, 10.0f, false);
        //Debug.Log("Distance left : " + distance);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (targeted)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("ShipProjectile"))
            {
                Debug.Log("My position is: " + gameObject.transform.position.x.ToString() + "," + gameObject.transform.position.y.ToString());
                Debug.Log("Ouch");
                TakeDamage(other.GetComponent<enemyBullet>().damageValue);
                Destroy(other.gameObject);
            }
        }
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
}

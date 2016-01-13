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

    void Awake()
    {
        player = GameObject.Find("Ship");
        heightChangeTimer = 5.0f;
        speed = 0.05f;
        heightAscent = 0;
        currentHealth = 100;
		_spriteFeedback.SetActive (false);
        targeted = false;
    }

    // Use this for initialization
    void Start () {
	    
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
        //position = transform.position;
        position += Vector3.right * speed;
        transform.DOMoveX(position.x, 5.0f, false);
        transform.DOMoveY(player.transform.position.y + heightAscent, 4.0f, false);
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

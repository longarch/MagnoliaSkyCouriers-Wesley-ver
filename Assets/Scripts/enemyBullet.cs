using UnityEngine;
using System.Collections;

public class enemyBullet : MonoBehaviour {

	float speed; // the bullet speed
	Vector2 _direction; // the direction of the bullet
	bool isReady; //when bullet direction is set
    [SerializeField]
    int damage;

    void Awake()
	{
		speed = 0.2f;
		isReady = false;
	}

	// Use this for initialization
	void Start () {
	
	}

	public void setDirection(Vector2 direction)
	{
		_direction = direction.normalized;

		isReady = true;
	}

	// Update is called once per frame
	void Update () {
		if (isReady) {
			// get bullet's current position
			Vector2 position = transform.position;

			position += _direction * speed;

			transform.position = position;

			// bottome-left of screen
			Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));

			//top-right of screen
			Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));

			if ((transform.position.x < min.x) || (transform.position.x > max.x) ||
			    (transform.position.y < min.y) || (transform.position.y > max.y)) {

				Destroy (gameObject);
			}
		}
	}

    public int damageValue
    {
         get { return damage; }
        set { damage = value; }
    }
}

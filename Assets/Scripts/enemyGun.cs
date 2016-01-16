using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class enemyGun : MonoBehaviour {

	float eventTime = 0f;
	float fixedeventTime;
    float _animTimer;


    public GameObject enemyBullet;
	[SerializeField]
	public GameObject self;
    [SerializeField]
    Animator animator;
	[SerializeField]
	public GameObject attkIndicator;
	[SerializeField]
	GameObject playership;

	// Use this for initialization
	void Start () {
		//Invoke ("fireEnemyBullet", 1.0f);
		if (self.name.Contains ("Dragon")) {
			eventTime = 10.0f;
            _animTimer = 3.0f;

        } else {
			eventTime = 10.0f;
			_animTimer = 3.0f;
		}
		fixedeventTime = eventTime;
		playership  = GameObject.Find ("Ship");
		attkIndicator.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
        if (self.GetComponent<BaseEnemy>().inRange)
        {
            eventTime -= Time.deltaTime;
            if (self.name.Contains("Dragon"))
            {
                if (eventTime <= _animTimer)
                {
                    animator.SetBool("Attacking", true);
                    attkIndicator.SetActive(true);
                    attkIndicator.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), _animTimer);
                }
                else {
                    animator.SetBool("Attacking", false);
                    attkIndicator.SetActive(false);
                    attkIndicator.transform.localScale = new Vector3(0.125f, 0.125f, 0.125f);
                }
            }
            else {
                if (eventTime <= _animTimer)
                {
                    attkIndicator.SetActive(true);
                    attkIndicator.transform.DOScale(new Vector3(0.3f, 0.3f, 0.3f), _animTimer);
                }
                else {
                    attkIndicator.SetActive(false);
                    attkIndicator.transform.localScale = new Vector3(0.125f, 0.125f, 0.125f);
                }
            }
            if (eventTime <= 0)
            {
                fireEnemyBullet();
                fixedeventTime = Random.Range(7, 15);
                eventTime = fixedeventTime;
            }
        }
        else {
            if (self.name.Contains("Dragon"))
            {
                animator.SetBool("Attacking", false);
                attkIndicator.SetActive(false);
                attkIndicator.transform.localScale = new Vector3(0.125f, 0.125f, 0.125f);
            }
            else {
                attkIndicator.SetActive(false);
                attkIndicator.transform.localScale = new Vector3(0.125f, 0.125f, 0.125f);
            }
            fixedeventTime = Random.Range(7, 15);
            eventTime = fixedeventTime;
        }
    }

    void fireEnemyBullet()
	{
		if (playership != null) {
			GameObject bullet = (GameObject)Instantiate (enemyBullet);

			bullet.transform.position = transform.position;

			Vector2 direction = playership.transform.position - bullet.transform.position + new Vector3(2f,0,0);

			bullet.GetComponent<enemyBullet> ().setDirection (direction);
		}
		else {
			return;
		}
	}
}
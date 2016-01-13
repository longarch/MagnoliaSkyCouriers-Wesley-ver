using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	GameObject playership;

	// Use this for initialization
	void Start () {
		//Invoke ("fireEnemyBullet", 1.0f);
		if (self.name.Contains ("Dragon")) {
			eventTime = 10.0f;
            _animTimer = 3.0f;

        } else {
			eventTime = 10.0f;
		}
		fixedeventTime = eventTime;
		playership  = GameObject.Find ("Ship");
	}

	// Update is called once per frame
	void Update () {
		eventTime -= Time.deltaTime;
        if (self.name.Contains("Dragon"))
        {
            if (eventTime <= _animTimer)
                animator.SetBool("Attacking", true);
            else
                animator.SetBool("Attacking", false);
        }
            if (eventTime <= 0) {
			fireEnemyBullet();
			fixedeventTime = Random.Range (7,15);
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
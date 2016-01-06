using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private Animator animator;

    // Use this for initialization
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        var vertical = Input.GetAxis("Vertical");
        var horizontal = Input.GetAxis("Horizontal");

        if (vertical > 0) //Back
        {
            animator.SetInteger("Direction", 2);
            if (transform.position.y < 4)
                transform.position += Vector3.up * 0.05f;
        }
        else if (vertical < 0) //Front
        {
            animator.SetInteger("Direction", 0);
            if (transform.position.y > -4)
                transform.position += Vector3.down * 0.05f;
        }
        else if (horizontal > 0) //Right
        {
            animator.SetInteger("Direction", 3);
            if (transform.position.x < 8)
                transform.position += Vector3.right * 0.05f;
        }
        else if (horizontal < 0) //Left
        {
            animator.SetInteger("Direction", 1);
            if (transform.position.x > -7)
                transform.position += Vector3.left * 0.05f;
        }
    }
}
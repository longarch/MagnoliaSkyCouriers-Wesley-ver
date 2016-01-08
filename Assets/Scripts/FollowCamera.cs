using UnityEngine;
using System.Collections;

public class FollowCamera : MonoBehaviour
{

    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    //[SerializeField]
    public GameObject target;
    private Vector3 offset;
    Vector3 targetPos;
    // Use this for initialization
    void Start()
    {
        offset = new Vector3(0.9f, -0.2f);
        targetPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.shouldFollow())
        {
            if (target)
            {
                Vector3 posNoZ = transform.position;
                posNoZ.z = target.transform.position.z;

                Vector3 targetDirection = (target.transform.position - posNoZ);

                interpVelocity = targetDirection.magnitude * 5f;

                targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

                transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.2f);
            }
        }
    }

}
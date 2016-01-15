using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineOfSight : MonoBehaviour {

    public enum TargetLayer
    {
        None,
        ELOS,
        PLOS
    }

    [SerializeField]
    TargetLayer targetLayer = TargetLayer.None;
    // Use this for initialization
    GameObject assignedObj;
    GameObject targetObj;

    public List<GameObject> targets;
    
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        /*if (assignedObj == null)
        {
            return;
        }
        if (targetLayer == TargetLayer.PLOS)
        {
            if (assignedObj.GetComponent<BaseEnemy>().getHealth() <= 0)
            {
                gameObject.transform.position += Vector3.left * 3;
                gameObject.SetActive(false);
            }
        }*/
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(targetLayer.ToString()))
        {
            targetObj = other.gameObject.GetComponent<LineOfSight>().getAssigned();
            Debug.Log(targetObj + "is In View");
            targets.Add(targetObj);
            if (targetLayer.ToString().Equals("PLOS")) //If target is ship, current shld be enemy
            { //targetObj.GetComponent<Ship>().shipTakeDamage(4);
                assignedObj.GetComponent<BaseEnemy>().inRange = true;
            }
            /*else if (targetLayer.ToString().Equals("PLOS")) //If target is enemy, current shld be ship
            {
                targets[0].GetComponent<BaseEnemy>().Targeted = true;
            }*/
        }
        Debug.Log(assignedObj.name + "'s targets are" + targets);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer(targetLayer.ToString()))
        {
            targetObj = other.gameObject.GetComponent<LineOfSight>().getAssigned();
            Debug.Log(targetObj + "Left View");
            targets.Remove(targetObj);
            if (targetLayer.ToString().Equals("PLOS")) //If target is ship, current shld be enemy
            { //targetObj.GetComponent<Ship>().shipTakeDamage(4);
                assignedObj.GetComponent<BaseEnemy>().inRange = false;
            }
            /*else if (targetLayer.ToString().Equals("PLOS")) //If target is enemy, current shld be ship
            {
                targetObj.GetComponent<BaseEnemy>().Targeted = false; //set the leaving enemy to be not targeted
                if (targets.Count > 0)
                    //set the oldest enemy in the array to be targeted
                    targets[0].GetComponent<BaseEnemy>().Targeted = true;
            }*/
        }
        Debug.Log(assignedObj.name + "'s targetz are" + targets);
    }

    public void setAssigned(GameObject item)
    {
        assignedObj = item;
    }
    public GameObject getAssigned()
    {
        return assignedObj;
    }
}

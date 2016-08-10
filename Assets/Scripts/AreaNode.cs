using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
public class AreaNode : MonoBehaviour {

	[SerializeField]
	string _nodeName;

	[SerializeField]
	List<AreaNode> _destinationNodes;

	[SerializeField]
	GameObject ContractSprite;

	[SerializeField]
	bool _hasContract;


	// Use this for initialization
	void Start () {
	
		setIsContract (false);

	}
	
	// Update is called once per frame
	void Update () {

		if (_hasContract) {

		}

	
	}

	void setIsContract(bool b)
	{
		if (ContractSprite != null) {
			ContractSprite.SetActive (b);
		}
	}


	void HoverContractSprite()
	{
		if (ContractSprite != null) {
			ContractSprite.transform.DOLocalJump (new Vector3 (0, 1, 0), 1.0f, 1, 0.5f, false);
		}
	}

	void OnMouseDown() {

		Debug.Log ("Node name is" + _nodeName);

		if (MapManager.Instance != null) {
			MapManager.Instance.moveShipToLocation(this);
		}

	}

	public List<AreaNode> getDestinationNode()
	{
		return _destinationNodes;
	}





	

}

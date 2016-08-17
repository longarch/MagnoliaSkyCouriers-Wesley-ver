using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
public class MapManager : MonoBehaviour {


	private static MapManager _instance = null;

	AreaNode[] _areaNodes;

	AreaNode _currentNode;

	[SerializeField]
	GameObject _ship;

	public static MapManager Instance //can call from any other class w/o reference
	{
		get { return _instance; }
	}

	void Awake()
	{
		if (_instance != null && _instance != this)
		{
			Destroy(gameObject);
		}
		_instance = this;
		
		
	}


	// Use this for initialization
	void Start () {

		_areaNodes = FindObjectsOfType(typeof(AreaNode)) as AreaNode[];
	
		if (_areaNodes != null) {
			_currentNode = _areaNodes[0];


			_ship.transform.position = _currentNode.transform.position;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}



	public void moveShipToLocation(AreaNode node)
	{





		Sequence sequence = DOTween.Sequence();




		sequence.Append(_ship.transform.DOMove (new Vector3 (node.transform.position.x, node.transform.position.y, 0), 0.5f, false));
		sequence.OnComplete(() =>
		                    {
			_ship.GetComponent<Ship> ().setPosition (node.transform.position);     
		});
	                                                                                                     
		                 
	}

	public void VisualizeContract(int nodeID)
	{
		_areaNodes [nodeID].setIsContract (true);
	}






}

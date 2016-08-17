using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.EventSystems;

public class NodeLineDrawer : MonoBehaviour {
	
	
	[SerializeField]
	private GameObject _progressLineRendererPrefab;



	[SerializeField]
	private AreaNode startNode,goalNode;

	[SerializeField]
	private List<AreaNode> goalNodes;

	private List<LineRenderer> _instantLine;

	private LineRenderer _tempLine;

	// Use this for initialization
	void Start () {

		_instantLine = new List<LineRenderer> ();
		//goalNodes = new List<AreaNode> ();

		for (int i = 0; i < goalNodes.Count; i++) {
			_tempLine = Instantiate (_progressLineRendererPrefab).gameObject.GetComponent<LineRenderer> ();

			_tempLine.transform.position = Vector3.Lerp(startNode.transform.position, goalNodes[i].transform.position,0.5f);

			//Route _routeInfo = _tempLine.gameObject.AddComponent<Route>( ) as Route;

			_tempLine.GetComponent<Route>().FitColliderToChildren(startNode.transform.position,goalNodes[i].transform.position);
			//_tempLine.gameObject.GetComponent<EventTrigger>().triggers.a
			_instantLine.Add(_tempLine);
		}

		for (int i = 0; i < _instantLine.Count; i ++) {
			DrawActualLine (i,goalNodes[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {

		/*
		for (int i = 0; i < _instantLine.Count; i ++) {
			DrawActualLine (i,goalNodes[i]);
		}*/
	}
	
	

	private void DrawActualLine(int lineID, AreaNode destNode)
	{
		if (_instantLine != null) {

				float distance = 0.0f;
			_instantLine[lineID].SetVertexCount (2);
			_instantLine[lineID].SetPosition (0, startNode.transform.position);
			_instantLine[lineID].SetPosition (1, destNode.transform.position);
			distance = Vector3.Distance (startNode.transform.position, destNode.transform.position);

		
				Vector2 newMaterialScale = new Vector2 (distance * 0.25f, 0.5f);
			_instantLine[lineID].material.mainTextureScale = newMaterialScale;
			Vector2 lineOffset = _instantLine[lineID].material.mainTextureOffset;
				lineOffset.x -= 0.75f * Time.deltaTime;
				lineOffset.y -= 0.75f * Time.deltaTime;
			_instantLine[lineID].material.mainTextureOffset = lineOffset;

		}
	}

	private void animateLine()
	{

	}

}

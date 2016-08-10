using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class NodeLineDrawer : MonoBehaviour {
	
	
	[SerializeField]
	private LineRenderer _progressLineRenderer;
	
	[SerializeField]
	private AreaNode startNode,goalNode;
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		DrawActualLine ();
		
	}
	
	

	private void DrawActualLine()
	{
		if (_progressLineRenderer != null) {

				float distance = 0.0f;
				_progressLineRenderer.SetVertexCount (2);
				_progressLineRenderer.SetPosition (0, startNode.transform.position);
				_progressLineRenderer.SetPosition (1, goalNode.transform.position);
				distance = Vector3.Distance (startNode.transform.position, goalNode.transform.position);

		
				Vector2 newMaterialScale = new Vector2 (distance * 0.25f, 0.5f);
				_progressLineRenderer.material.mainTextureScale = newMaterialScale;
				Vector2 lineOffset = _progressLineRenderer.material.mainTextureOffset;
				lineOffset.x -= 0.75f * Time.deltaTime;
				lineOffset.y -= 0.75f * Time.deltaTime;
				_progressLineRenderer.material.mainTextureOffset = lineOffset;

		}
	}

}

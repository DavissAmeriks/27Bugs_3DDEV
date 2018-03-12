using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public Point GridPosition { get; private set; }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Setup(Point gridPos, Vector3 worldPos)
    {
        this.GridPosition = GridPosition;
        transform.position = worldPos;
    }
}

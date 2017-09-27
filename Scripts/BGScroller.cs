using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour {
    public float scrollSpeed;       // Speed of background scrolling
    public float tileSizeZ;         // Size of background

    private Vector3 startPosition;  // Position to start scrolling from
   	
	void Update () {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, tileSizeZ);    // Repeat the position of the background, repeat along the size of the tile
        transform.position = startPosition + Vector3.forward * newPosition;
	}
}

/*
 * 20/09/2017
 * Joe O'Regan
 * K00203642
 * 
 * DestroyByBoundary.cs
 * 
 * When a game object comes in contact with the boundary, destroy it
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByBoundary : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        Destroy(other.gameObject);
    }
}

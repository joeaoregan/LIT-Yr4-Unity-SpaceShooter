/*
 * 20/09/2017
 * Joe O'Regan
 * K00203642
 * 
 * DestroyByBoundary.cs
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

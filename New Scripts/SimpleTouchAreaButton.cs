/*
 * 27/09/2017
 * Joe O'Regan
 * K00203642
 * 
 * SimpleTouchAreaButton.cs
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SimpleTouchAreaButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler { // Don't need drag handler for firebutton

    private bool touched;                                                                   // Holding finger down
    private int pointerID;                                                                  // Make sure only one finger press works
    private bool canFire;                                                                   // Player fires weapon

    void Awake() {
        touched = false;                                                                    // Not touched yet
    }
    public void OnPointerDown(PointerEventData data) {
        // Set out start point
        if (!touched) {                                                                     // If Fire Zone has not been touched
            touched = true;                                                                 // Fire Zone has been touched
            pointerID = data.pointerId;                                                     // Get the pointer ID
            canFire = true;                                                                 // If the pointer goes down we can fire
        }
    }
    
    public void OnPointerUp(PointerEventData data) {
        // Reset everything
        if (data.pointerId == pointerID) {
            canFire = false;                                                                // When Fire Zone not touched we can't fire
            touched = false;                                                                // Reset Fire Zone being touched
        }
    }

    public bool CanFire() { return canFire; }                                               // Player can fire weapon
}

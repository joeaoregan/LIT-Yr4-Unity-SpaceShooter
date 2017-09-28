/*
 * 27/09/2017
 * Joe O'Regan
 * K00203642
 * 
 * SimpleTouchPad.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleTouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler { // handle finger up, down, and drag on device screen

    public float smoothing;                                                                         // Smooth the movement input

    private Vector2 origin;
    private Vector2 direction;
    private Vector2 smoothDirection;                                                                // Smooth direction
    private bool touched;                                                                           // Only allow one touch on touchpad
    private int pointerID;

    void Awake() {
        direction = Vector2.zero;                                                                   // No direction at the start
        touched = false;                                                                            // Not touched yet
    }

    public void OnPointerDown (PointerEventData data) {
        // Set out start point
        if (!touched) {                                                                             // Only do this if device is not touched
            touched = true;                                                                         // Touchpad has been touched
            pointerID = data.pointerId;                                                             // Get the pointer ID
            origin = data.position;                                                                 // Set the origin to the point the pointer comes down to
        }
    }

    public void OnDrag(PointerEventData data) {
        // Movement Zone Square, wherever you put you finger down first in the zone is the center point
        // Compare the difference betweeen start point and current pointer position
        if (data.pointerId == pointerID) {                                                          // Only do this if it is the correct pointer
            Vector2 currentPosition = data.position;                                                // As we drag around, we take a look at what our position is
            Vector2 directionRaw = currentPosition - origin;                                        // Difference between our current position and our origin
            //Vector2 direction = directionRaw.normalized;                                          // Normalise the vector, magnitude of 1, but direction is unchanged
            direction = directionRaw.normalized;                                                    // Declared global
            //Debug.Log(direction);                                                                 // Check direction works, appears in lower left of Unity
        }
    }

    public void OnPointerUp(PointerEventData data) {
        // Reset everything
        if (data.pointerId == pointerID) {
            direction = Vector2.zero;                                                               // Reset direction
            touched = false;                                                                        // Reset touchpad being touched
        }
    }

    public Vector2 GetDirection() {
        smoothDirection = Vector2.MoveTowards(smoothDirection, direction, smoothing);               // Like a lerp
        //return direction;                                                                         // Allows us to grab direction from the player
        return smoothDirection;
    }
}

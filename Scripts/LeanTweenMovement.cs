using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenMovement : MonoBehaviour
{
    public float moveTime = 2f;

    // These are now settable from outside
    public Vector3 endPosition;
    public Vector3 endRotation;

    // Removed the movement and rotation logic from Start

    // Public method to initiate the movement and rotation
    public void StartMovement(Vector3 newPosition, Vector3 newRotation)
    {
        Debug.Log("----");
        endPosition = newPosition;
        endRotation = newRotation;

        // Corrected Debug.Log statement
        Debug.Log("Move to " + endPosition, this);

        LeanTween.move(this.gameObject, endPosition, moveTime);
        LeanTween.rotate(this.gameObject, endRotation, moveTime);
    }
}


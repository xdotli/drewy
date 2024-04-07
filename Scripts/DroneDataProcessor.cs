using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DroneData
{
    public string id;
    public string curr;
}

[Serializable]
public class DronesData
{
    public List<DroneData> drones;
}

public class DroneDataProcessor : MonoBehaviour
{
    public LeanTweenMovement leanTweenMovement;

    public void ProcessData(DroneData droneData)
    {
        Vector3 newPosition = ParsePosition(droneData.curr);
        Vector3 newRotation = ParseRotation(droneData.curr);

        leanTweenMovement.StartMovement(newPosition, newRotation);
    }

    private Vector3 ParsePosition(string data)
    {
        // Your parsing logic to convert data string to Vector3
        return new Vector3(); // Return the parsed position
    }

    private Vector3 ParseRotation(string data)
    {
        // Your parsing logic to convert data string to Vector3
        return new Vector3(); // Return the parsed rotation
    }
}

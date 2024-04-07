using UnityEngine;
// using UnityEngine.Networking;

public class ServerManager : MonoBehaviour {
    // private EchoServer echoServer;

    void Update() {
        // Check for incoming messages
        // int recHostId; 
        // int connectionId; 
        // int channelId; 
        // byte[] recBuffer = new byte[1024];
        // int dataSize;
        // byte error;
        // NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, recBuffer.Length, out dataSize, out error);
        // switch (recData) {
        //     case NetworkEventType.DataEvent: // Data received
        //         // Deserialize the data
        //         Debug.Log(recBuffer.ToString());
        //         // TransformParameters parameters = Deserialize(recBuffer);
        //         // Apply the transform parameters to the target GameObject
        //         // ApplyTransformParameters(parameters);
        //         break;
        // }
    }

    // void ApplyTransformParameters(TransformParameters parameters) {
    //     // Instead of applying the parameters, print them to the console
    //     Debug.Log("Received Position: " + parameters.position.ToString());
    //     Debug.Log("Received Rotation: " + parameters.rotation.ToString());
    //     // Add similar lines if you're receiving scale or other parameters
    // }


    // void OnApplicationQuit() {
    //     echoServer.StopServer();
    // }
}

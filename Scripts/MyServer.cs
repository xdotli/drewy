using UnityEngine;
using System;
using System.Collections;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using System.Text;
 
 
public class MyServer : MonoBehaviour
{
    Socket SeverSocket = null;
    Thread Socket_Thread = null;
    bool Socket_Thread_Flag = false;
    public List<LeanTweenMovement> drones;
 
    //for received message
//    private float mouse_delta_x;
//    private float mouse_delta_y;
//    private bool isTapped;
//    private bool isDoubleTapped;
//
//    public float getMouseDeltaX(){return mouse_delta_x;    }
//    public float getMouseDeltaY(){return mouse_delta_y;    }
//    public bool getTapped(){return isTapped;}
//    public bool getDoubleTapped(){return isDoubleTapped;}
//
//    public void setMouseDeltaX(float dx){mouse_delta_x = dx;}
//    public void setMouseDeltaY(float dy){mouse_delta_y = dy;}
//    public void setTapped(bool t){isTapped = t;}
//    public void setDoubleTapped(bool t){isDoubleTapped = t;}
//
//    private int tick =0;
    //private string[] receivedMSG;
    //public string[] getMsg(){return receivedMSG;    }
 
 
    string[] stringSeparators = new string[] {"*TOUCHEND*","*MOUSEDELTA*","*Tapped*","*DoubleTapped*"};
 
    void Awake()
    {
        Socket_Thread = new Thread(Dowrk);
        Socket_Thread_Flag = true;
        Socket_Thread.Start();
    }
 
    private void Dowrk()
    {
        //receivedMSG = new string[10];
        SeverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9999);
        SeverSocket.Bind(ipep);
        SeverSocket.Listen(10);
     
        Debug.Log("Socket Standby....");
        Socket client = SeverSocket.Accept();//client에서 수신을 요청하면 접속합니다.
        Debug.Log("Socket Connected.");
     
        IPEndPoint clientep = (IPEndPoint)client.RemoteEndPoint;
        NetworkStream recvStm = new NetworkStream(client);
        //tick = 0;
             
        while (Socket_Thread_Flag)
        {
            byte[] receiveBuffer = new byte[1024 * 80];
            try
            {
                 
                //print (recvStm.Read(receiveBuffer, 0, receiveBuffer.Length));
                if(recvStm.Read(receiveBuffer, 0, receiveBuffer.Length) == 0 ){
                    // when disconnected , wait for new connection.
                    client.Close();
                    SeverSocket.Close();
                 
                    SeverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    ipep = new IPEndPoint(IPAddress.Any, 10000);
                    SeverSocket.Bind(ipep);
                    SeverSocket.Listen(10);
                    Debug.Log("Socket Standby....");
                    client = SeverSocket.Accept();//client에서 수신을 요청하면 접속합니다.
                    Debug.Log("Socket Connected.");
                    Debug.Log("Socket Connected.");
                    Debug.Log("Socket Connected.");
                    clientep = (IPEndPoint)client.RemoteEndPoint;
                    recvStm = new NetworkStream(client);
                 
                }else{
                             
 
                    // string Test = Encoding.Default.GetString(receiveBuffer); 
                    // print (Test);
                    try
                    {
                        string Test = Encoding.Default.GetString(receiveBuffer).Trim('\0'); 
                        print("Received JSON: " + Test);

                        // Deserialize the JSON array
                        DroneData[] droneDataArray = JsonUtility.FromJson<DroneDataArray>("{\"droneDataArray\":" + Test + "}").droneDataArray;
                        print(droneDataArray.Length);
                        // Process each DroneData object
                        for (int i = 0; i < droneDataArray.Length; i++)
                        {
                            Debug.Log("Drone ID: " + droneDataArray[i].id + ", Curr: " + droneDataArray[i].curr);

                            // Extracting the position data from the curr string
                            string trimmedData = droneDataArray[i].curr.Trim(new char[] { '(', ')' });
                            string[] posValues = trimmedData.Split(',');
                            if (posValues.Length >= 3)
                            {
                                Vector3 newPosition = new Vector3(float.Parse(posValues[0]), float.Parse(posValues[1]), float.Parse(posValues[2]));
                                Vector3 newRotation = new Vector3(0, 90, 0); // Example rotation, replace this as needed
                                // Inside your networking thread
                                MainThreadExecutor.Enqueue(() => drones[i].StartMovement(newPosition, newRotation));
                            }
                            else
                            {
                                Debug.LogError("Invalid position data for drone ID: " + droneDataArray[i].id);
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        print("Exception occurred: " + e.Message);
                    }

 
                }
             
             
            }
         
            catch (Exception e)
            {
                Socket_Thread_Flag = false;
                client.Close();
                SeverSocket.Close();
                continue;
            }
         
        }
     
    }
 
    void OnApplicationQuit()
    {
        try
        {
            Socket_Thread_Flag = false;
            Socket_Thread.Abort();
            SeverSocket.Close();
            Debug.Log("Bye~~");
        }
     
        catch
        {
            Debug.Log("Error when finished...");
        }
    }
 
 
}

[Serializable]
public class DroneDataArray
{
    public DroneData[] droneDataArray;
}

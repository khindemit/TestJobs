using UnityEngine;
using System.Collections;

public class NetworkListenerClient : MonoBehaviour
{
    private NetworkViewID _viewID;

    void Start()
    {
        _TryToConnectToServer();
    }

    private static void _TryToConnectToServer()
    {
        Debug.Log("Try to connect to server...");
        Network.Connect("127.0.0.1", 12000);
    }

    void OnConnectedToServer()
    {
        Debug.Log("Connected to server");
    }
    void OnFailedToConnect(NetworkConnectionError error)
    {
        Debug.LogWarning("Could not connect to server: " + error);
        _TryToConnectToServer();
    }


    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        if (info == NetworkDisconnection.LostConnection)
        {
            Debug.Log("Lost connection to the server");
        }
        else
        {
            Debug.Log("Successfully disconnected from the server");
        }
    }
}
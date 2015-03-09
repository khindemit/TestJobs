using UnityEngine;
using System.Collections;

public class NetworkListenerServer : MonoBehaviour
{
    void Awake()
    {
        Network.InitializeServer(5, 12000, false);
    }

    void OnPlayerConnected(NetworkPlayer player)
    {
        Debug.Log("********* Player connected from " + player.ipAddress + ":" + player.port);
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("********* Clean up after player " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    // This apparently never gets called???
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Debug.Log("********* OnSerializeNetworkView");
        if (stream.isReading)
        {
            int value = 0;
            stream.Serialize(ref value);
            Debug.Log(value);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DisconnectClientOnLoad : NetworkBehaviour
{
  
    private void Awake()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += Disconnect;
    }

    private void Disconnect(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (IsServer)
        {
            foreach (ulong client in clientsCompleted)
            {
                if(OwnerClientId != client)
                {
                    NetworkManager.DisconnectClient(client);
                }
            }
            NetworkManager.Shutdown();
        }

    }


   
}

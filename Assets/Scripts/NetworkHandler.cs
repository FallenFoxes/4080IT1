using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;
using Unity.Netcode;

public class NetworkHandler : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.Singleton.OnClientStarted += OnClientStarted;
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
    }

    private bool hasPrinted = false;
    private void PrintMe() {
        if (hasPrinted) {
            return;
        }
        Debug.Log("I AM");
        hasPrinted = true;
        if (IsServer) {
            Debug.Log ("  the Server!");
        }
        if (IsHost) {
            Debug.Log ("  the Host!");
        }
        if (IsClient) {
            Debug.Log ("  the Client!");
        }
        if (!IsServer && !IsClient) {
            Debug.Log ("  Nothing Yet!");
            hasPrinted = false;
        }
    }

    private void OnClientStarted()
    {
        PrintMe();
    }

    private void OnServerStarted()
    {
        PrintMe();
    }

}

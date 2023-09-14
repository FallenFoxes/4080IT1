using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    public Button startButton;
    public TMPro.TMP_Text statusLabel;
    void Start()
    {
        startButton.gameObject.SetActive(false);
        statusLabel.text = "Start Something, like the server or the host or the client.";

        NetworkManager.OnClientStarted += OnClientStarted;
        NetworkManager.OnServerStarted += OnServerStarted;
    }

    private void OnServerStarted(){
        startButton.gameObject.SetActive(true);
        statusLabel.text = "Press Start";
    }

    private void OnClientStarted(){
        if (!IsHost){
            statusLabel.text = "Waiting for game to start";
        }
    }
}

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
        NetworkManager.OnClientStarted += OnClientStarted;
        NetworkManager.OnServerStarted += OnServerStarted;
        startButton.onClick.AddListener(OnStartButtonCLicked);

        startButton.gameObject.SetActive(false);
        statusLabel.text = "Start Client/Host/Server";
    }

     private void OnClientStarted(){
        if (!IsHost){
            statusLabel.text = "Waiting for game to start";
        }
    }

    private void OnServerStarted(){
        StartGame();
       // startButton.gameObject.SetActive(true);
        //statusLabel.text = "Press Start";
    }


    private void OnStartButtonCLicked()
    {
        StartGame();
    }

    public void StartGame()
    {
        NetworkManager.SceneManager.LoadScene(
            "TestChat",
            UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}

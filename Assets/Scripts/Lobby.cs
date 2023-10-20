using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Runtime.CompilerServices;

public class Lobby : NetworkBehaviour
{
    public NetworkedPlayers networkedPlayers;
    public LobbyUi lobbyUi;
    void Start()
    {
        if (IsServer) {
            ServerPopulateCards();
            networkedPlayers.allNetPlayers.OnListChanged += ServerNetPlayersChanged;
            lobbyUi.ShowStart(true);
        } else {
            ClientPopulateCards();
            networkedPlayers.allNetPlayers.OnListChanged += ClientNetPlayerChanged;
            lobbyUi.ShowStart(false);
            lobbyUi.OnReadyToggled += ClientOnReadyToggled;
            NetworkManager.OnClientDisconnectCallback += ClientOnClientDisconnected;
    }
}

    private void ServerPopulateCards()
    {
        lobbyUi.playerCards.Clear();
        foreach(NetworkPlayerInfo info in networkedPlayers.allNetPlayers)
        {
            PlayerCard pc = lobbyUi.playerCards.AddCard("Some Player");
            pc.ready = info.ready;
            pc.color = info.color;
            pc.clientId = info.clientId;
            if(info.clientId == NetworkManager.LocalClientId)
            {
                pc.ShowKick(false);
            } else {
                pc.ShowKick(true);
            }
            pc.OnKickClicked += ServerOnKickClicked;
            pc.UpdateDisplay();
        }
    }

        private void ClientPopulateCards()
    {
        lobbyUi.playerCards.Clear();
        foreach(NetworkPlayerInfo info in networkedPlayers.allNetPlayers)
        {
            PlayerCard pc = lobbyUi.playerCards.AddCard("Some Player");
            pc.ready = info.ready;
            pc.color = info.color;
            pc.ShowKick(false);
            pc.clientId = info.clientId;
            pc.UpdateDisplay();
        }
    }

    private void ClientOnReadyToggled(bool newValue)
    {
        UpdateReadyServerRpc(newValue);
    }

    private void ServerNetPlayersChanged(NetworkListEvent<NetworkPlayerInfo> changeEvent) {
    ServerPopulateCards();
    }

    private void ServerOnKickClicked(ulong clientId)
    {
        NetworkManager.DisconnectClient(clientId);
    }
      private void ClientNetPlayerChanged(NetworkListEvent<NetworkPlayerInfo> changeEvent) {
        ClientPopulateCards();
      }

      private void ClientOnClientDisconnected(ulong clientId) {
        lobbyUi.gameObject.SetActive(false);
      }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateReadyServerRpc(bool newValue, ServerRpcParams rpcParams = default)
    {
        networkedPlayers.UpdateReady(rpcParams.Receive.SenderClientId, newValue);
    }
}



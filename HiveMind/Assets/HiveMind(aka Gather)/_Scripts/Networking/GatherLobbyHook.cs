using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class GatherLobbyHook : LobbyHook 
{
	int playerCount;
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
		PlayerMomController player = gamePlayer.GetComponent<PlayerMomController>();

        player.name = lobby.name;
        player.TeamColor = lobby.playerColor;
        player.teamID = playerCount;
        playerCount++;
       	//UnityEventManager.TriggerEvent("StartGame");
    }
}

using UnityEngine;
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine.Networking;

public class TestLobbyHook : LobbyHook 
{
	int playerCount;
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
		Interact player = gamePlayer.GetComponent<Interact>();

        player.name = lobby.name;
        player.teamColor = lobby.playerColor;
		//GameController.instance.RegisterPlayer(gamePlayer);
    }
}

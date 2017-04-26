using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine;

public class CustomNetManager : NetworkManager
{
	int playerCount;

	public override void OnServerAddPlayer (NetworkConnection conn, short playerControllerId)
	{
		//base.OnServerAddPlayer (conn, playerControllerId);
//
//		GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
//		PlayerMomController mom = player.GetComponent<PlayerMomController>();
//		mom.teamID = (int)Mathf.Repeat(playerCount, 2);
//        playerCount++;
//		mom.TeamColor =  mom.teamID == 0 ? Color.blue : Color.red;
//		//GameController.instance.levelGen.SpawnMoM();
//		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
	}

}

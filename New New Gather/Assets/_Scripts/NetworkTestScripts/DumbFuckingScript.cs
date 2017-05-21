using UnityEngine;
using UnityEngine.Networking;

public class DumbFuckingScript : NetworkBehaviour 
{
	[SyncVar(hook = "OnChangeColor")] public Color myColor;
	[SyncVar]bool hasChanged;

	public override void OnStartClient()
	{
		if(hasChanged)
		GetComponent<MeshRenderer>().material.color = myColor;
	}
	[ClientRpc]
	public void RpcChangeColor(Color newColor)
	{
		print("I'm a dumbfuckingcube");
		myColor = newColor;
	}

	void OnChangeColor(Color newColor)
	{
		myColor = newColor;
		GetComponent<MeshRenderer>().material.color = newColor;
		hasChanged = true;
	}
}

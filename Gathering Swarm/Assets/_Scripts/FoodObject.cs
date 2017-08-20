using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FoodObject : NetworkBehaviour//, IAttachable 
{
	public static int TotalCreated;
	public int Id{get{return id;}}
	public bool CanBeTargetted{get{return gameObject.activeSelf && !bAttached;}}
	public Vector3 Location{get{return transform.position;}}
	[SerializeField]private int id, idOffset = 1000;
	[SerializeField][SyncVar]private bool bAttached;
	LineRenderer lineRenderer;
	SyncListVector3 lines = new SyncListVector3();
	bool initialized;
	//[SyncVar]bool hasLine;

	void OnEnable()
	{
		id = TotalCreated+idOffset;
		TotalCreated++;
		lineRenderer = GetComponent<LineRenderer>();
		//lines.Callback += OnLinesChanged;
		//lines = new Vector3[lineRenderer.positionCount];
	}
//	void OnLinesChanged(SyncListVector3.Operation op, int ind)
//	{
////		switch(op)
////		{
////			case SyncList<Vector3>.Operation.OP_ADD:
////			{
////			break;
////			}	
////		}
//		RpcSetLines(lines.ToArray());
//	}
	void Update()
	{	
		//CmdSetMeStraight();
		if(isServer)
		{
			//RpcSetLines(lines.ToArray());
		}

	}

//	[Command]
//	void CmdSetMeStraight()
//	{
//		RpcSetLines(lines.ToArray());
//	}
//	[ClientRpc]
//	public void RpcSetLines(Vector3[] points)
//	{
//		lineRenderer.SetPositions(points);
//	}
	public void SetLine(int index,Vector3 point)
	{
		//lineRenderer.SetPosition(index,point);
		lines.Add(point);
	}
	public void Destroy()
	{
		transform.position = Vector3.zero;
		Detach();
		UnityEventManager.TriggerEvent("TargetUnavailable",Id);
		gameObject.SetActive(false);
	}
	[ClientRpc]
	public void RpcDestroy()
	{
		//transform.position = Vector3.zero;
		//UnityEventManager.TriggerEvent("TargetUnavailable",Id);
		gameObject.SetActive(false);
	}
	public void Attach(GameObject newParent, Vector3 point)
	{
		transform.SetParent(newParent.transform);
		transform.localPosition = point;
		bAttached = true;
		RpcAttach(newParent, point);
		UnityEventManager.TriggerEvent("TargetUnavailable",Id);
	}
	[ClientRpc]
	public void RpcAttach(GameObject newParent, Vector3 point)
	{
		transform.SetParent(newParent.transform);
		//transform.localPosition = point;
		//bAttached = true;
		//UnityEventManager.TriggerEvent("TargetUnavailable",Id);
	}

	public void Reset(Vector3 position)
	{
		gameObject.SetActive(true);
		transform.position = position;
	}
	[ClientRpc]
	public void RpcReset(Vector3 position)
	{
		gameObject.SetActive(true);
		transform.position = position;
	}

	public void Detach()
	{
		transform.SetParent(null);
		bAttached = false;
		RpcDetach();
	}
	[ClientRpc]
	public void RpcDetach()
	{
		transform.SetParent(null);
		//bAttached = false;
	}


}

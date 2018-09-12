using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FoodObject : NetworkBehaviour
{
	public static int TotalCreated;
	public int Id{get{return id;}}
	public bool CanBeTargetted{get{return gameObject.activeSelf && !bAttached;}}
	public Vector3 Location{get{return transform.position;}}
	[SerializeField]private int id, idOffset = 1000;
	[SerializeField][SyncVar]private bool bAttached;
	LineRenderer lineRenderer;
	SyncListVector3 lines = new SyncListVector3();
	//NetworkTransform tran;
	void Start()
	{
		//tran = GetComponent<NetworkTransform>();
	}
	void OnEnable()
	{
		id = TotalCreated+idOffset;
		TotalCreated++;
		lineRenderer = GetComponent<LineRenderer>();
		//lines.Callback += OnLinesChanged;
		//lines = new Vector3[lineRenderer.positionCount];
	}

	public void SetLine(int index,Vector3 point)
	{
		lineRenderer.SetPosition(index,point);
		//lines.Add(point);
	}
	public void Destroy()
	{
		transform.position = Vector3.zero;
		Detach();
		UnityEventManager.TriggerEvent("TargetUnavailable",Id);
		gameObject.SetActive(false);
		RpcDestroy();
	}
	[ClientRpc]
	public void RpcDestroy()
	{
		//transform.position = Vector3.zero;
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
		transform.localPosition = point;
		//bAttached = true;
		//UnityEventManager.TriggerEvent("TargetUnavailable",Id);
	}

	public void Reset(Vector3 position)
	{
		gameObject.SetActive(true);
		transform.position = position;
		RpcReset(position);
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

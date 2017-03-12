using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class DroneController : Unit_Base 
{
	public bool CanBeTargetted{get{return gameObject.activeSelf && !bAttached;}}
	[SerializeField] GameObject clone;
	[SerializeField] protected float orbit = 25, sightRange;
	[SerializeField] protected Vector3 nose; //set in editor
	protected bool bInDanger, bAttached;
	protected float sqrDist;

	protected override void OnEnable()
	{
		base.OnEnable();
		sqrDist = orbit*orbit;
		UnityEventManager.StartListeningInt("TargetUnavailable", TargetLost);
	}
	protected virtual void OnDisable()
	{
		UnityEventManager.StopListeningInt("TargetUnavailable", TargetLost);
		StopCoroutine(Idle());
	}
	[ClientRpc]
	public virtual void RpcSetMoM(GameObject mom, Color tc)
	{
		base.SetMoM(mom);
		GetComponentInChildren<MeshRenderer>().materials[1].color = tc;
		StartCoroutine(Idle());
	}

	public void GoLimp()
	{
		
	}
	public void Attach(Transform newParent, Vector3 point)
	{
//		bAttached = true;
//		agent.Stop();
//		bMoving = false;
//		transform.SetParent(newParent);
//		transform.localPosition = point;
//		GetComponentInChildren<Rigidbody>().constraints = RigidbodyConstraints.None;
//		UnityEventManager.TriggerEvent("TargetUnavailable",unitID);
		var g = Instantiate(clone, point, Quaternion.identity)as GameObject;
		g.transform.SetParent(newParent);
		//g.transform.localPosition = point;
		Death();
	}
	public void Detach()
	{
		transform.SetParent(null);
		bAttached = false;
	}

	protected virtual void UpdateFlagLocation(int team)
	{

	}

	protected virtual void TargetLost(int id)
	{
		
	}


	protected override IEnumerator Idle()
	{
		while(true)
		{
			if(!bMoving&& !bAttached)
			{
				ArrivedAtTargetLocation();
			}
			yield return new WaitForSeconds(1);
		}
	}

}

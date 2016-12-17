using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneController : Unit_Base, IAttachable 
{
	public bool CanBeTargetted{get{return gameObject.activeSelf && !bAttached;}}
	[SerializeField] GameObject clone;
	[SerializeField] protected float orbit = 25, sightRange;
	[SerializeField] protected Vector3 nose; //set in editor
	protected bool bInDanger;
	protected float sqrDist;
	bool bAttached;

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

	public virtual void setMoM(MoMController mom, Color tc)
	{
		base.setMoM(mom);
		GetComponentInChildren<MeshRenderer>().materials[1].color = tc;
		StartCoroutine(Idle());
	}

	protected virtual void UpdateFlagLocation(int team)
	{

	}

	protected virtual void TargetLost(int id)
	{
		
	}

	protected virtual void MoveRandomly()
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
		Death();
	}
	public void Detach()
	{
		transform.SetParent(null);
		bAttached = false;
	}
	protected IEnumerator Idle()
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
	protected virtual void ArrivedAtTargetLocation()
	{
		MoveRandomly();
	}

}

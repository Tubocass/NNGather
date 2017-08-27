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
		if(GetComponentInChildren<MeshRenderer>().materials.Length>1)
		TeamColorMat = GetComponentInChildren<MeshRenderer>().materials[1];
		sqrDist = orbit*orbit;
		UnityEventManager.StartListeningInt("TargetUnavailable", TargetLost);
		StartCoroutine(Idle());
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		UnityEventManager.StopListeningInt("TargetUnavailable", TargetLost);
		StopCoroutine(Idle());
	}
	//[ClientRpc]
	public virtual void SetMoM(GameObject mom, Color tc)
	{
		base.SetMoM(mom);
		TeamColor = tc;
		//GetComponentInChildren<MeshRenderer>().materials[1].color = tc;
		StartCoroutine(Idle());
	}

//	public void GoLimp()
//	{
//		
//	}
	public void Attach(Transform newParent, Vector3 point)
	{
		var g = Instantiate(clone, point, Quaternion.identity)as GameObject;
		g.transform.SetParent(newParent);
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


	protected virtual IEnumerator Idle()
	{
		while(true)
		{
			if(!bMoving&& !bAttached)
			{
				ArrivedAtTargetLocation();
			}
			yield return new WaitForSeconds(.25f);
		}
	}

	protected virtual void ArrivedAtTargetLocation()
	{
	}

}

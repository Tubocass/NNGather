using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class DroneController : Unit_Base 
{
	public bool CanBeTargetted{get{return gameObject.activeSelf;}}
	[SerializeField] GameObject clone;
	[SerializeField] protected float orbit = 25, sightRange;
	[SerializeField] protected Vector3 nose; //set in editor
	protected float sqrDist;
	protected NetworkTransformChild childTransform;

	protected override void OnEnable()
	{
		base.OnEnable();
		if(GetComponentInChildren<MeshRenderer>().materials.Length>1)
		TeamColorMat = GetComponentInChildren<MeshRenderer>().materials[1];
		sqrDist = orbit*orbit;
		UnityEventManager.StartListeningInt("TargetUnavailable", TargetLost);
//		childTransform = GetComponent<NetworkTransformChild>();
//		if(childTransform!=null)
//		childTransform.enabled = false;
		//StartCoroutine(Idle());
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		UnityEventManager.StopListeningInt("TargetUnavailable", TargetLost);
		//StopCoroutine(Idle());
	}
	void Update()
	{
		if(!isServer)
		return;

		if(agent.remainingDistance<1)
		{
			ArrivedAtTargetLocation();
		}
	}
	//[ClientRpc]
//	public virtual void SetMoM(GameObject mom, Color tc)
//	{
//		base.SetMoM(mom);
//		TeamColor = tc;
//	}

	public void Attach(Transform newParent, Vector3 point)
	{
		var g = Instantiate(clone, point, Quaternion.identity)as GameObject;
		g.transform.SetParent(newParent);
		Death();
	}


	protected virtual void UpdateFlagLocation(int team)
	{

	}

	protected virtual void TargetLost(int id)
	{
		
	}

//	protected virtual IEnumerator Idle()
//	{
//		while(true)
//		{
//			if(!bMoving&& !bAttached)
//			{
//				ArrivedAtTargetLocation();
//			}
//			yield return new WaitForSeconds(.25f);
//		}
//	}

	protected virtual void ArrivedAtTargetLocation()
	{
	}

}

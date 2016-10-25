using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DroneController : Unit_Base 
{
	[SerializeField] protected float orbit = 25;
	[SerializeField] protected Vector3 nose; //set in editor
	protected bool bInDanger;
	protected float sqrDist = 20f*20f;

	protected override void OnEnable()
	{
		base.OnEnable();
		UnityEventManager.StartListening("TargetUnavailable", TargetLost);
	}
	protected virtual void OnDisable()
	{
		UnityEventManager.StopListening("TargetUnavailable", TargetLost);
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

	protected IEnumerator Idle()
	{
		while(true)
		{
			if(!bMoving)
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

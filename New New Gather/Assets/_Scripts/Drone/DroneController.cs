using UnityEngine;
using System.Collections;

public class DroneController : Unit_Base 
{
	//public int TeamID;
	//public Vector3 Location{get{return transform.position;}}
	[SerializeField] protected float orbit = 25;
	[SerializeField] protected Vector3 nose; //set in editor
	//protected NavMove navMove;
	protected bool bInDanger;
	protected MoMController myMoM;

	protected virtual void OnEnable()
	{
		base.OnEnable();
		UnityEventManager.StartListening("TargetUnavailable", TargetLost);
	}
	protected virtual void OnDisable()
	{
		UnityEventManager.StopListening("TargetUnavailable", TargetLost);
		StopCoroutine(Idle());
	}

	public void setMoM(MoMController mom, Color tc)
	{
		myMoM = mom;
		TeamID = myMoM.TeamID;
		GetComponentInChildren<MeshRenderer>().materials[1].color = tc;
		StartCoroutine(Idle());
	}

	protected virtual void UpdateFlagLocation(int team)
	{

	}

	protected void ReturnToHome()
	{
		MoveTo(myMoM.Location);
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

	public virtual void OnTriggerEnter(Collider other)
	{

	}

	public virtual void OnTriggerStay(Collider other)
	{
		
	}

	public virtual void OnCollisionEnter(Collision bang)
	{
		
	}
}

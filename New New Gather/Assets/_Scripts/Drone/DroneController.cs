using UnityEngine;
using System.Collections;

public class DroneController : MonoBehaviour 
{
	public int TeamID;
	[SerializeField] protected float orbit = 25;
	[SerializeField] protected Vector3 nose; //set in editor
	protected NavMove navMove;
	protected bool bInDanger;
	protected MoMController myMoM;

	protected virtual void OnEnable()
	{
		navMove = GetComponent<NavMove>();
		UnityEventManager.StartListeningInt("TargetUnavailable", TargetLost);
	}
	protected virtual void OnDisable()
	{
		UnityEventManager.StopListeningInt("TargetUnavailable", TargetLost);
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
		navMove.MoveTo(myMoM.Location);
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
			if(!navMove.BMoving)
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

	public void OnTriggerEnter(Collider other)
	{

	}

	public void OnTriggerStay(Collider other)
	{
		
	}

	public void OnCollisionEnter(Collision bang)
	{
		
	}
}

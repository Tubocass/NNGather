using UnityEngine;
using System.Collections;

public class FighterController : DroneController 
{
	protected override void OnEnable()
	{
		base.OnEnable();
		UnityEventManager.StartListeningInt("PlaceFightFlag", UpdateFlagLocation);
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		UnityEventManager.StopListeningInt("PlaceFightFlag", UpdateFlagLocation);
	}
	protected override void UpdateFlagLocation(int team)
	{
		if(TeamID == team && CanTargetEnemy() && Vector3.Distance(transform.position, myMoM.FightAnchor)>orbit)
		{
			navMove.MoveTo(myMoM.FoodAnchor);
		}
	}
	protected override void TargetLost(int id)
	{
		
	}

	protected override void ArrivedAtTargetLocation()
	{


	}
	protected override void MoveRandomly()
	{
		Vector3 rVector = navMove.RandomVector(myMoM.FightAnchor, orbit);
		navMove.MoveTo(rVector);
	}
	bool CanTargetEnemy()
	{
		return true;
	}
	public override void OnTriggerEnter(Collider other)
	{
		
	}
	public override void OnTriggerStay(Collider other)
	{
		
	}
	public override void OnCollisionEnter(Collision bang)
	{
	}
}

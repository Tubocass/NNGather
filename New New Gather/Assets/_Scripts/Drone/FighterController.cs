using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FighterController : DroneController 
{
	Unit_Base targetEnemy;
	static List<Unit_Base> enemies;
	float enemyDist;
	protected override void OnEnable()
	{
		enemies = new List<Unit_Base>();
		base.OnEnable();
		UnityEventManager.StartListening("PlaceFightFlag", UpdateFlagLocation);
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		UnityEventManager.StopListening("PlaceFightFlag", UpdateFlagLocation);
	}
	protected override void UpdateFlagLocation(int team)
	{
		if(TeamID == team && CanTargetEnemy() && Vector3.Distance(transform.position, myMoM.FightAnchor)>orbit)
		{
			MoveTo(myMoM.FoodAnchor);
		}
	}
	protected override void TargetLost(int id)
	{
		
	}

	Unit_Base TargetNearest()
	{
		float nearestEnemyDist, newDist;
		Unit_Base enemy = null;
		Unit_Base[] enemiesCopy = enemies.ToArray();
		if(enemies.Count>0)
		{
			nearestEnemyDist = (enemiesCopy[0].Location-Location).sqrMagnitude; //Vector3.Distance(Location,enemies[0].Location);
			foreach(Unit_Base unit in enemiesCopy)
			{
				if(unit.isActive)
				{
					newDist = (unit.Location-Location).sqrMagnitude;//Vector3.Distance(Location,unit.Location);
					if(newDist <= nearestEnemyDist)
					{
						nearestEnemyDist = newDist;
						enemy = unit;
					}
				}//else enemies.RemoveAll(e=> !e.isActive);
			}
		}
		return enemy;
	}
	protected override void ArrivedAtTargetLocation()
	{
		//base.ArrivedAtTargetLocation();

		if(targetEnemy != null)
		{
			if(Vector3.Distance(Location,targetEnemy.Location)<1.5f)
			{
				Attack(targetEnemy);
			}else{
		 	MoveTo(targetEnemy.Location);
		 	}
		}else {
			targetEnemy = TargetNearest();
			if(targetEnemy!=null)
			{
				MoveTo(targetEnemy.Location);
			}else MoveRandomly();
		}
		

	}

	protected override void MoveRandomly()
	{
		Vector3 rVector = RandomVector(myMoM.FightAnchor, orbit);
		MoveTo(rVector);
	}

	void Attack(Unit_Base target)
	{
		target.Health = -5f;
	}

	bool CanTargetEnemy()
	{
		return true;
	}
	public override void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Drone")
		{
			DroneController ot = other.gameObject.GetComponent<DroneController>();
			if(ot!=null && ot.TeamID!=TeamID)
			{
				if(!enemies.Contains(ot))
				{
					enemies.Add(ot);
				}
//				targetEnemy = ot.gameObject;
//				navMove.MoveTo(ot.Location);
			}
		}
	}
	public override void OnTriggerStay(Collider other)
	{
//		if(other.tag == "Fighter")
//		{
//			DroneController ot = other.gameObject.GetComponent<DroneController>();
//			if(ot!=null && ot.TeamID!=TeamID)
//			{
//				targetEnemy = ot.gameObject;
//				navMove.MoveTo(ot.Location);
//			}
//		}	
	}
	public override void OnCollisionEnter(Collision bang)
	{
		if(bang.collider.tag == "Drone")
		{
			DroneController ot = bang.gameObject.GetComponent<DroneController>();
			if(ot!=null && ot.TeamID!=TeamID)
			{
				Attack(ot);
			}
		}
	}
}

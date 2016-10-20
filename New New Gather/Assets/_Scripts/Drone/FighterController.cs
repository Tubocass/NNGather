﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FighterController : DroneController 
{
	Unit_Base targetEnemy;
	List<Unit_Base> enemies;
	List<Unit_Base> enemiesCopy;
	bool canAttack=true;
	ParticleSystem spark;
	LayerMask mask;

	protected override void OnEnable()
	{
		spark = GetComponentInChildren<ParticleSystem>();
		enemies = new List<Unit_Base>();
		mask = 1<<LayerMask.NameToLayer("Units");
		canAttack=true;
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
		if(myMoM.unitID == team  && Vector3.Distance(Location, myMoM.FightAnchor)>orbit)
		{
			targetEnemy = null;
			MoveTo(myMoM.FightAnchor);
		}
	}
	protected override void TargetLost(int id)
	{
		if(targetEnemy!=null && id == targetEnemy.unitID)
		{
			enemies.Remove(targetEnemy);
			targetEnemy = null;
			ArrivedAtTargetLocation();
		}
		// (enemies[enemies.FindIndex(e=> e.unitID == id)]);
	}

	public override void setMoM(MoMController mom, Color tc)
	{
		base.setMoM(mom, tc);
		StartCoroutine(LookForEnemies());
	}

	protected override void Death()
	{
		base.Death();
		myMoM.fighters-=1;
	}

	protected override void ArrivedAtTargetLocation()
	{
		//base.ArrivedAtTargetLocation();

		if(targetEnemy != null && targetEnemy.isActive)
		{
			if(canAttack && Vector3.Distance(Location,targetEnemy.Location)<1f)
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

	protected override IEnumerator MovingTo()
	{
		while(bMoving)
		{
			if(agent.remainingDistance<1)
			{
				bMoving = false;
				//Debug.Log("I arrived");
				//controller.ArrivedAtTargetLocation(); //Apparently this is causing a huge buffer oveload
			}else
			{
				yield return new WaitForSeconds(0.5f);
				if(IsTargetingEnemy()) MoveTo(targetEnemy.Location);
			}
		}
	}

	Unit_Base TargetNearest()
	{
		float nearestEnemyDist, newDist;
		Unit_Base enemy = null;
		enemies.RemoveAll(e=> !e.isActive);
		enemiesCopy = enemies.FindAll(e=> e.isActive && e.teamID!=teamID && (e.Location-Location).sqrMagnitude<sqrDist);

		if(enemiesCopy.Count>0)
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
				}else enemies.Remove(unit);
			}
		}
		return enemy;
	}

	void Attack(Unit_Base target)
	{
		spark.Play();
		target.Health = -5f;
		Health = -2;
		canAttack = false;
		if(this.isActive)
		StartCoroutine(AttackCooldown());
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(1f);
		canAttack = true;
	}

	bool IsTargetingEnemy()
	{
		if(targetEnemy!=null && targetEnemy.isActive)
		return true;
		else return false;
	}

	bool CanTargetEnemy()
	{
		if(!IsTargetingEnemy())
		return true;
		else return false;
	}
	IEnumerator LookForEnemies()
	{
		while(true)
		{
			RaycastHit[] hits = Physics.SphereCastAll(Location,20,tran.forward,1,mask, QueryTriggerInteraction.Ignore);
			if(hits.Length>0)
			{
				foreach(RaycastHit f in hits)
				{
					if(f.collider.tag == "Drone")
					{
						Unit_Base ot = f.collider.GetComponent<Unit_Base>();
						if(ot!=null && ot.teamID!=teamID && !enemies.Contains(ot))
						{
							enemies.Add(ot);
						}
					}
				}
			}
			yield return new WaitForSeconds(2f);
		}
	}

	public override void OnTriggerEnter(Collider other)
	{
//		if(CanTargetEnemy() && other.tag == "Drone")
//		{
//			Unit_Base ot = other.gameObject.GetComponent<Unit_Base>();
//			if(ot!=null && ot.teamID!=teamID)
//			{
//				if(!enemies.Contains(ot))
//				{
//					enemies.Add(ot);
//				}
//			}
//		}
	}
//	public override void OnTriggerStay(Collider other)
//	{
////		if(other.tag == "Fighter")
////		{
////			DroneController ot = other.gameObject.GetComponent<DroneController>();
////			if(ot!=null && ot.TeamID!=TeamID)
////			{
////				targetEnemy = ot.gameObject;
////				navMove.MoveTo(ot.Location);
////			}
////		}	
//	}
	public override void OnCollisionEnter(Collision bang)
	{
		if(bang.collider.tag == "Drone")
		{
			DroneController ot = bang.gameObject.GetComponent<DroneController>();
			if(ot!=null && ot.teamID!=teamID && canAttack)
			{
				Attack(ot);
			}
		}
	}
}

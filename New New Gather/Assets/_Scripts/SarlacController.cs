﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SarlacController : DroneController 
{
	public Vector3 anchor;
	[SerializeField] int maxEaten;
	//[SerializeField] protected float orbit = 25;
	//float sqrDist = 20f*20f;
	Unit_Base targetEnemy;
	List<Unit_Base> enemies;
	List<Unit_Base> enemiesCopy;
	bool canAttack=true, bReturning;
	ParticleSystem spark;
	LayerMask mask;
	int eaten;

	protected override void OnEnable()
	{
		base.OnEnable();
		eaten = 0;
		bMoving = false;
		bReturning = false;
		spark = GetComponentInChildren<ParticleSystem>();
		enemies = new List<Unit_Base>();
		mask = 1<<LayerMask.NameToLayer("Units");
		canAttack=true;
		StartCoroutine(Idle());
		//StartCoroutine(LookForEnemies());
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
	public override void TakeDamage(float damage)
	{
		eaten++;
	}
//	protected override IEnumerator Idle()
//	{
//		while(true)
//		{
//			if(!bMoving)
//			{
//				ArrivedAtTargetLocation();
//			}
//			yield return new WaitForSeconds(1);
//		}
//	}
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
				if(bReturning) MoveTo(anchor);
				else if(IsTargetingEnemy()) MoveTo(targetEnemy.Location);
			}
		}
	}
	protected override void MoveRandomly()
	{
		Vector3 rVector = RandomVector(anchor, orbit);
		MoveTo(rVector);
	}
	protected void ReturnToHome()
	{
		bReturning = true;
		Vector3 nearest = NearestTarget(PitController.Pits,Location);
		MoveTo(nearest);
	}
	protected override void ArrivedAtTargetLocation()
	{
		//base.ArrivedAtTargetLocation();
		if(eaten<maxEaten&&!GenerateLevel.IsDayLight())
		{
			if(IsTargetingEnemy())
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
		}else ReturnToHome();
	}
	bool IsTargetingEnemy()
	{
		if(!GenerateLevel.IsDayLight() && targetEnemy!=null && targetEnemy.isActive && eaten<maxEaten)
		return true;
		else return false;
	}
	Unit_Base TargetNearest()
	{
		float nearestEnemyDist, newDist;
		Unit_Base enemy = null;
		enemies.RemoveAll(e=> !e.isActive);
		//enemiesCopy = enemies.FindAll(e=> e.isActive && e.teamID!=teamID && (e.Location-Location).sqrMagnitude<sqrDist);

		RaycastHit[] hits = Physics.SphereCastAll(Location,20,tran.forward,1,mask, QueryTriggerInteraction.Ignore);
		if(hits.Length>0)
		{
			foreach(RaycastHit f in hits)
			{
				if(f.collider.tag == "Drone")
				{
					Unit_Base ot = f.collider.GetComponent<Unit_Base>();
					if(ot!=null && !enemies.Contains(ot))
					{
						enemies.Add(ot);
					}
				}
			}
		}

		enemiesCopy = enemies.FindAll(e=> e.isActive && (e.Location-Location).sqrMagnitude<sqrDist);
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
	Vector3 NearestTarget(List<PitController> Objects, Vector3 targetLoc)
	{
		float nearestDist, newDist;
		PitController obj = null;

		//foods = Foods.FindAll(e=> e.CanBeTargetted && (e.Location-Location).sqrMagnitude<sqrDist);

		if(Objects.Count>0)
		{
			nearestDist = (Objects[0].transform.position-targetLoc).sqrMagnitude; //Vector3.Distance(Location,enemies[0].Location);
			foreach(PitController o in Objects)
			{
				if(o!=null)
				{
					newDist = (o.transform.position-targetLoc).sqrMagnitude;//Vector3.Distance(Location,unit.Location);
					if(newDist <= nearestDist)
					{
						nearestDist = newDist;
						obj = o;
					}
				}
			}
		}
		return obj.transform.position;
	}

	void Attack(Unit_Base target)
	{
		spark.Play();
		target.Health = -5f;
		eaten++;
		//Health = -2;
		canAttack = false;
		if(this.isActive)
		StartCoroutine(AttackCooldown());
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(1f);
		canAttack = true;
	}
	public override void OnCollisionEnter(Collision bang)
	{
		if(bang.collider.tag == "Drone")
		{
			DroneController ot = bang.gameObject.GetComponent<DroneController>();
			if(ot!=null && canAttack)
			{
				Attack(ot);
			}
		}
		if(bang.collider.tag == "Pit")
		{
			if(eaten>=maxEaten||GenerateLevel.IsDayLight())
			{
				PitController pc = bang.gameObject.GetComponent<PitController>();
				this.bReturning = false;
				pc.StartTimer();
				this.Death();
			}
		}
	}
}

﻿using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class FighterController : DroneController 
{
	[SerializeField] float attackStrength , selfAttack, refractoryPeriod;
	Unit_Base targetEnemy;
	List<Unit_Base> enemies;
	List<Unit_Base> enemiesCopy;
	ParticleSystem spark;
	LayerMask mask;
	bool canAttack=true, bReturning;

	protected override void OnEnable()
	{
		spark = GetComponentInChildren<ParticleSystem>();
		enemies = new List<Unit_Base>();
		targetEnemy = null;
		mask = 1<<LayerMask.NameToLayer("Units");
		canAttack=true;
		base.OnEnable();
		UnityEventManager.StartListeningInt("PlaceFightFlag", UpdateFlagLocation);
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		UnityEventManager.StopListeningInt("PlaceFightFlag", UpdateFlagLocation);
	}
	protected override void UpdateFlagLocation(int mom)
	{
		if(isServer)
		{
			if(myMoM.unitID == mom  && Vector3.Distance(Location, myMoM.FightAnchor)>orbit)
			{
				targetEnemy = null;
				MoveTo(myMoM.FightAnchor);
			}
		}
	}

	protected override void TargetLost(int id)
	{
		if(targetEnemy!=null && id == targetEnemy.unitID)
		{
			enemies.Clear();
			targetEnemy = null;
			ArrivedAtTargetLocation();
		}
	}

	protected override void Death()
	{
		base.Death();
		myMoM.fighters-=1;
	}

	protected override void ArrivedAtTargetLocation()
	{
		if(isServer && myMoM!= null)
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
				}else MoveRandomly(myMoM.FightAnchor,orbit);
			}
		}
	}

	Unit_Base TargetNearest()
	{
		float nearestEnemyDist, newDist;
		Unit_Base enemy = null;
		enemies.Clear();
		Collider[] cols = Physics.OverlapSphere(tran.position,sightRange,mask);

		if(cols.Length>0)
		{
			for(int e = 0; e<cols.Length; e++)
			{
				if(cols[e].CompareTag("Sarlac"))
				{
					enemy = cols[e].GetComponent<SarlacController>();
					if(enemy!=null )
					{
						return enemy;
					}
				}
				if(cols[e].CompareTag("MoM"))
				{
					Unit_Base ot = cols[e].GetComponent<MoMController>();
					if(ot!=null && !ot.teamID.Equals(teamID) && !enemies.Contains(ot))
					{
						enemies.Add(ot);
					}
				}
				if(cols[e].CompareTag("Drone"))
				{
					Unit_Base ot = cols[e].GetComponent<Unit_Base>();
					if(ot!=null && !ot.teamID.Equals(teamID) && !enemies.Contains(ot))
					{
						enemies.Add(ot);
					}
				}
			}
		}

		//enemiesCopy = enemies.FindAll(e=> e.isActive && e.teamID!=teamID && (e.Location-Location).sqrMagnitude<sqrDist);
		if(enemies.Count>0)
		{
			nearestEnemyDist = (enemies[0].Location-Location).sqrMagnitude; //Vector3.Distance(Location,enemies[0].Location);
			for(int f = 0; f<enemies.Count;f++)
			{
				if(enemies[f].isActive)
				{
					newDist = (enemies[f].Location-Location).sqrMagnitude;//Vector3.Distance(Location,unit.Location);
					if(newDist <= nearestEnemyDist)
					{
						nearestEnemyDist = newDist;
						enemy = enemies[f];
					}
				}
			}
		}
		return enemy;
	}

	void Attack(Unit_Base target)
	{
		spark.Play();
		target.TakeDamage(attackStrength);
		TakeDamage(selfAttack);
		canAttack = false;
		if(this.isActive)
		StartCoroutine(AttackCooldown());
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(refractoryPeriod);
		canAttack = true;
	}

	bool IsTargetingEnemy()
	{
		if(targetEnemy!=null && targetEnemy.isActive)
		return true;
		else return false;
	}

	public override void OnCollisionEnter(Collision bang)
	{
		if(!isServer)
		return;
		if(bang.collider.CompareTag("Drone")||bang.collider.CompareTag("Sarlac")||bang.collider.CompareTag("MoM"))
		{
			Unit_Base ot = bang.gameObject.GetComponent<Unit_Base>();
			if(ot!=null && !ot.teamID.Equals(teamID) && canAttack)
			{
				Attack(ot);
			}
		}
	}
}

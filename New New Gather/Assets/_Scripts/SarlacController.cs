using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SarlacController : DroneController 
{
	[SerializeField] float attackStrength;
	public Vector3 anchor;
	[SerializeField] int maxEaten;
	//[SerializeField] protected float orbit = 25;
	//float sqrDist = 20f*20f;
	Unit_Base targetEnemy;
	GameObject[] enemiesCarried;
	[SerializeField]int numCarried, maxCarry;
	List<Unit_Base> enemies;
	List<Unit_Base> enemiesCopy;
	ParticleSystem spark;
	LayerMask mask;
	bool canAttack=false, bReturning;
	int eaten;

	protected override void OnEnable()
	{
		base.OnEnable();
		TeamColorMat = GetComponentInChildren<MeshRenderer>().material;
		eaten = 0;
		bMoving = false;
		bReturning = false;
		spark = GetComponentInChildren<ParticleSystem>();
		enemies = new List<Unit_Base>();
		enemiesCarried = new GameObject[maxCarry];
		mask = 1<<LayerMask.NameToLayer("Units");
		canAttack=false;
		StartCoroutine(Idle());
		StartCoroutine(AttackCooldown());
		//StartCoroutine(LookForEnemies());
	}
	protected override void OnDisable()
	{
		StopAllCoroutines();
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
		targetEnemy = TargetNearest();
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
				if(currntPoint<points-1)
				{
					currntPoint +=1;
					currentVector = Path[currntPoint];
					agent.SetDestination(currentVector);
				}else bMoving = false;
				//Debug.Log("I arrived");
				//controller.ArrivedAtTargetLocation(); //Apparently this is causing a huge buffer oveload
			}else
			{
				//if(bReturning) ReturnToHome();
				//else 
				if(IsTargetingEnemy()) MoveTo(targetEnemy.Location);
			}
			yield return new WaitForSeconds(0.5f);
		}
	}
	protected override void MoveRandomly()
	{
//		Vector3 rVector = RandomVector(anchor, orbit);
//		MoveTo(rVector);

		UnityEngine.AI.NavMeshPath rVector = RandomPath(anchor, orbit);
		if(rVector.status!=UnityEngine.AI.NavMeshPathStatus.PathPartial)
		{
			agent.SetPath(rVector);
			agent.Resume();
		}
	}
	protected override void DaySwitch(bool b)
	{
		base.DaySwitch(b);
		if(bDay)
		ReturnToHome();
	}
	protected void ReturnToHome()
	{
		bReturning = true;
		Vector3 nearest = GenerateLevel.NearestTarget(GenerateLevel.Pits,Location);
		MoveTo(nearest);
	}
	protected override void ArrivedAtTargetLocation()
	{
		//base.ArrivedAtTargetLocation();
		if(!isServer)
		return;

		if(isActive&& eaten<maxEaten && !IsCarryingFood())
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
		if(!bDay && targetEnemy!=null && targetEnemy.isActive && eaten<maxEaten)
		return true;
		else return false;
	}
	bool IsCarryingFood()
	{	
		numCarried = 0;
		foreach(Transform fo in GetComponentsInChildren<Transform>())
		{
			if(fo !=null&& fo.tag!="Sarlac")
			{
				if(numCarried<maxCarry)
				{
					enemiesCarried[numCarried] = fo.gameObject;
					numCarried++;
				}else Destroy(fo.gameObject);
			}
		}
		return numCarried>0?true:false;
	}

	bool IsTargetingFood()
	{
		if(targetEnemy!=null && targetEnemy.gameObject.activeSelf)
		return true;
		else return false;
	}

	bool CanTargetFood()
	{
		if(!IsTargetingFood() && !IsCarryingFood() && canAttack)
		return true;
		else return false;
	}
	bool CanAttack()
	{
		Vector3 nearest = GenerateLevel.NearestTarget(GenerateLevel.Pits,Location);
		if(Vector3.Distance(nearest,Location)>2 && canAttack)
		{
			return true;
		}else return false;
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

	void Attack(Unit_Base target)
	{
		spark.Play();
		target.Health = -attackStrength;
		//eaten++;
		//Health = -2;
		canAttack = false;
		if(this.isActive)
		StartCoroutine(AttackCooldown());
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(0.5f);
		canAttack = true;
	}
	public override void OnCollisionEnter(Collision bang)
	{
		if(bang.collider.tag == "Drone")
		{
			DroneController ot = bang.gameObject.GetComponent<DroneController>();
			if(ot!=null && ot.CanBeTargetted)
			{
				if(IsTargetingFood() || CanTargetFood())
				{
					targetEnemy = null;
					//carriedEnemy = ot;
					if(numCarried<maxCarry)
					ot.Attach(this.tran,tran.TransformPoint(nose));
					else Attack(ot);
					eaten++;
					numCarried++;
					if(numCarried>3)
					ReturnToHome();
				}
			}
		}

		if(bang.collider.tag == "Pit")
		{
			if(IsCarryingFood())
			{
				for(int i = 0; i<numCarried;i++)
				{
					if(enemiesCarried[i] != null)
					{
						Destroy(enemiesCarried[i]);
					}
				}
				numCarried = 0;
				this.bReturning = false;
			}
			if(eaten>=maxEaten||bDay)
			{
				//PitController pc = bang.gameObject.GetComponent<PitController>();
				this.bReturning = false;
				GameController.StartTimer();
				this.Death();
			}
		}
	}
}

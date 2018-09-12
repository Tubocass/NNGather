using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SarlacController : DroneController 
{
	[SerializeField] float attackStrength;
	public Vector3 anchor;
	[SerializeField] int maxEaten;
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
		StartCoroutine(AttackCooldown());
		bDay = GameController.instance.IsDayLight();
		UnityEventManager.StartListeningBool("DayTime", DaySwitch);
	}
	protected override void OnDisable()
	{
		base.OnDisable();
		UnityEventManager.StopListeningBool("DayTime", DaySwitch);
	}

	protected override void TargetLost(int id)
	{
		if(targetEnemy!=null && id == targetEnemy.unitID)
		{
			enemies.Remove(targetEnemy);
			targetEnemy = null;
			ArrivedAtTargetLocation();
		}
	}
	public override void TakeDamage(float damage)
	{
		eaten++;
		targetEnemy = TargetNearest();
	}

	protected void DaySwitch(bool b)
	{
		//base.DaySwitch(b);
		bDay = b;
		if(isActive && bDay)
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
				}else MoveRandomly(anchor, orbit);
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
		return numCarried>0 ? true:false;
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
		Collider[] cols = Physics.OverlapSphere(transform.position,sightRange,mask,QueryTriggerInteraction.Ignore);
		float nearestDist, newDist;
		Collider temp = new Collider();
		Unit_Base target = null;

		if(cols.Length>0)
		{
			nearestDist = (cols[0].transform.position-transform.position).sqrMagnitude; //Vector3.Distance(Location,enemies[0].Location);
			foreach(Collider o in cols)
			{
				if(o.CompareTag("Drone"))
				{
					newDist = (o.transform.position-transform.position).sqrMagnitude;//Vector3.Distance(Location,unit.Location);
					if(newDist <= nearestDist)
					{
						nearestDist = newDist;
						temp = o;
					}
				}
			}
			if(temp!=null)
			target = temp.GetComponent<Unit_Base>();
		}
		return target;
	}

	void Attack(Unit_Base target)
	{
		spark.Play();
		target.Health = -attackStrength;
		canAttack = false;
		if(this.isActive)
		StartCoroutine(AttackCooldown());
	}

	IEnumerator AttackCooldown()
	{
		yield return new WaitForSeconds(0.5f);
		canAttack = true;
	}
	protected override void Death()
	{
		base.Death();
		UnityEventManager.StopListeningBool("DayTime", DaySwitch);
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
				GameController.instance.StartTimer();
				this.Death();
			}
		}
	}
}

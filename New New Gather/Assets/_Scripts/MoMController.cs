using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MoMController : MonoBehaviour 
{
	public static int MoMCount;
	public int FoodAmount, TeamID;
	public Vector3 FoodAnchor
	{
		get{
			if(activeFarmFlag)
				return farmFlagTran.position;
			else return transform.position;
			}
	}
	public Vector3 Location{get{return transform.position;}}
	protected Transform farmFlagTran, fightFlagTran;
	protected bool activeFarmFlag, activeFightFlag;
	[SerializeField] protected GameObject farmer, soldier, farmFlag, fightFlag;
	Queue<Vector3> foodQ = new Queue<Vector3>(10);
	NavMeshAgent agent;

	protected virtual void OnEnable()
	{
		agent = GetComponent<NavMeshAgent>();
		SetID();
	}
	protected virtual void OnDisable()
	{
		StopCoroutine(UpdateLocation());
	}

	protected virtual void Start()
	{	
		farmFlag = Instantiate(farmFlag) as GameObject; //GameObject.Find("FarmFlag");
		fightFlag = Instantiate(fightFlag) as GameObject;//GameObject.Find("FightFlag");
		farmFlagTran = farmFlag.GetComponent<Transform>();
		fightFlagTran = fightFlag.GetComponent<Transform>();
		StartCoroutine(UpdateLocation());
	}
	protected virtual void SetID()
	{
		TeamID = MoMCount+1;
	}

	protected virtual void PlaceFarmFlag(Vector3 location)
	{
		farmFlag.SetActive(true);
		farmFlagTran.position = location;
		farmFlag.GetComponent<ParticleSystem>().Play();
		UnityEventManager.TriggerEventInt("PlaceFarmFlag", TeamID);
		activeFarmFlag = true;
	}
	protected virtual void RecallFarmFlag()
	{
		farmFlagTran.position = transform.position;
		farmFlag.GetComponent<ParticleSystem>().Stop();
		farmFlag.SetActive(false);
		UnityEventManager.TriggerEventInt("PlaceFarmFlag", TeamID);
		activeFarmFlag = false;
	}
	protected virtual void PlaceFightFlag(Vector3 location)
	{
		fightFlag.SetActive(true);
		fightFlagTran.position = location;
		fightFlag.GetComponent<ParticleSystem>().Play();
		UnityEventManager.TriggerEventInt("PlaceFightFlag", TeamID);
		activeFightFlag = true;
	}
	protected virtual void RecallFightFlag()
	{
		fightFlagTran.position = transform.position;
		fightFlag.GetComponent<ParticleSystem>().Stop();
		fightFlag.SetActive(false);
		UnityEventManager.TriggerEventInt("PlaceFightFlag", TeamID);
		activeFightFlag = false;
	}
	public virtual void CreateFarmer()
	{
		if(FoodAmount>0)
		{
			FoodAmount -= 1;
			GameObject spawn = Instantiate(farmer,transform.position + new Vector3(1,0,1),Quaternion.identity) as GameObject;
			spawn.GetComponent<FarmerController>().setMoM(this);
		}
	}
	public virtual void AddFoodLocation(Vector3 loc)
	{
		if(foodQ.Count>9)
		{
			foodQ.Dequeue();
			foodQ.Enqueue(loc);
			Debug.Log("newness");
		}else {foodQ.Enqueue(loc);Debug.Log("newness");}

		FoodAmount++;
	}

	IEnumerator UpdateLocation()
	{
		while(true)
		{
			if(foodQ.Count>0)
			MoveToCenter();
			yield return new WaitForSeconds(3);
		}
	}
	void MoveToCenter()
	{
		Debug.Log("Updating");
		Vector3 newLoc;
		float xx = 0, zz = 0;
		int size = 1;
		foreach(Vector3 v in foodQ)
		{
			if(v != null)
			{
				size++;
				xx += v.x;
				zz += v.z;
			}
		}
		newLoc = new Vector3(xx /size, 1f ,zz /size);
		if(Vector3.Distance( transform.position, newLoc)>1)
		{
			Debug.Log("I'm gonna move");
			agent.SetDestination(newLoc);
		}
	}
}

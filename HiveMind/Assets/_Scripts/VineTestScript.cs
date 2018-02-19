using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineTestScript : MonoBehaviour 
{
	public Vector3 Location{get{return transform.position;}}
	[SerializeField] int amount = 4;
	[SerializeField] float forwardMin, forwardMax, sideToSideMin, sideToSideMax, timer;
	LineRenderer lineRender;
	public bool isFullyGrown;
 	//GameObject[] foodPile;
	//Vector3[] spawnPoints;

	void Start () 
	{
		lineRender = GetComponent<LineRenderer>();
//		foodPile = new GameObject[amount];
//		spawnPoints = new Vector3[amount];
		//SetVine();
		StartCreepin();
	}
	void Update () 
	{
		
	}

//	GameObject InitialSpawn(Vector3 position)
//	{
//		GameObject food = Instantiate(foodObj, position + new Vector3(0,.5f,0), Quaternion.identity) as GameObject;
//		food.GetComponent<FoodObject>().SetLine(0, food.transform.position);
//		food.GetComponent<FoodObject>().SetLine(1, this.transform.position);
//		//food.SetActive(false);
//		return food;
//	}
	void SetVine()
	{
	//endpoint is within a 90 degree arc facing outward from pit. - not implemented
		float randX, randZ;
		Vector3 next, current = Vector3.zero;
		Vector3[] positions = new Vector3[lineRender.positionCount];
//		positions[pos] = current;
//		pos++;
		for(int pos = 0;pos<lineRender.positionCount;pos++)
		{ //forwardMin, forwardMax, sideToSideMin, sideToSideMax
			randX = Random.Range(sideToSideMin,sideToSideMax);
			randX = Mathf.Abs(randX)>=1? randX: randX<0? -1:1;
			randZ = Random.Range(forwardMin,forwardMax);
//			randZ = Mathf.Abs(randX)>=1? randX: randX<0? -1:1;
			next = new Vector3(randX, 0, randZ)+current;
			positions[pos] = pos==0? Vector3.zero: next;
			current = next;
		}
		lineRender.SetPositions(positions);
	}
	public void StartCreepin()
	{
		StartCoroutine(VineCreep());
	}
	IEnumerator VineCreep()
	{
		int pos = 0;
		Vector3 next, current = Vector3.zero;
		float randX, randZ;
		while(pos<amount)
		{
			lineRender.positionCount++;//this must be ugly under the hood, but it's the easiest way to extend out naturally.
			randX = Random.Range(sideToSideMin,sideToSideMax);
			//randX = Mathf.Abs(randX)>=1? randX: randX<0? -1:1;
			randZ = Random.Range(forwardMin,forwardMax);
			//randZ = Mathf.Abs(randX)>=1? randX: randX<0? -1:1;
			if(pos == 1)
			{	
				randZ = 1f;
				randX = Mathf.Clamp(randX,-0.25f, 0.25f);
			}
			next = new Vector3(randX, 0, randZ)+current;
			current = pos==0? Vector3.zero: next;
			lineRender.SetPosition(pos,current);

			pos++;
			yield return new WaitForSeconds(timer);
		}
		isFullyGrown = true;
		GameObject food = ObjectPool.DrawFromPool("Foods");
		food.transform.parent = this.transform;
		food.transform.localPosition = lineRender.GetPosition(pos-1);

		food.SetActive(true);
	}
	void ClearVines()
	{
		lineRender.enabled = false;
		this.gameObject.SetActive(false);
	}
}

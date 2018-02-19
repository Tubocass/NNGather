using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiTest : MonoBehaviour 
{
/*
	Resource:
	Health
		*goes down over time
	Strength
		used to hunt

	Actions:
	Eat/Hunt
	Sleep
*/
	public int health, strength;
	public float hungerTime, sleepTime, huntTime;
	public bool isHunting, isSleeping, isHungry;

	delegate void TimerFunc();

	void Start()
	{
	  	StartCoroutine(Timer(hungerTime, Hunger));
	}
	void Update()
	{

		if(health<=4&& !isHunting)
		{
			StartCoroutine(Timer(huntTime, Hunt));
			isHunting=true;
			Debug.Log("Started Hunting");
		}
		if(strength<=2&& !isSleeping)
		{
			StartCoroutine(Timer(sleepTime, Sleep));
			isSleeping = true;
			Debug.Log("Started Sleeping");
		}
	}
	void Hunger()
	{
		health-=1;
		StartCoroutine(Timer(hungerTime, Hunger));
		Debug.Log("Am Hungry");
	}
	void Hunt()
	{
		strength-=1;
		health+=4;
		isHunting = false;
	}
	void Sleep()
	{
		strength += 5;
		isSleeping = false;
	}
	IEnumerator Timer(float time, TimerFunc func)
	{
		while(time>=0f)
		{
			time-= Time.deltaTime;
			//Debug.Log(time);
			yield return null;
		}

		func();
	}
}

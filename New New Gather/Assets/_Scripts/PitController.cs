using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PitController : MonoBehaviour 
{
	public static List<PitController> Pits = new List<PitController>();
	public Vector3 Location{get{return transform.position;}}
	static float timer;
	static SarlacController SarlacInstance;
	[SerializeField] GameObject SarlacFab;
	[SerializeField] float Timer = 30;

	void Start () 
	{
		timer = Timer;
		if(SarlacInstance == null)
		{
			GameObject spawn = Instantiate(SarlacFab, Vector3.zero,Quaternion.identity) as GameObject;
			SarlacInstance = spawn.GetComponent<SarlacController>();
			SarlacInstance.isActive = false;
		}
		StartCoroutine(Release());
	}

	public void StartTimer()
	{
		StartCoroutine(Release());
	}
	public static IEnumerator Release()
	{
		yield return new WaitForSeconds(timer);
		PitController spawnPoint = Pits[Random.Range(0,Pits.Count-1)];//Find(p=> p.Location.x>GenerateLevel.Sun_A.position.x && p.Location.x<GenerateLevel.Sun_B.position.x);
		if(spawnPoint!=null&& !GenerateLevel.IsDayLight())
		{
			SarlacInstance.anchor = spawnPoint.Location;
			SarlacInstance.transform.position = spawnPoint.Location;
			SarlacInstance.isActive = true;
		}else yield return Release();

	}

}

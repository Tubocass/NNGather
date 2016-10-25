using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PitController : MonoBehaviour 
{

	[SerializeField] GameObject SarlacFab;
	[SerializeField] float Timer = 30;
	static float timer;
	public static List<PitController> Pits = new List<PitController>();
	public Vector3 Location{get{return transform.position;}}
	static SarlacController SarlacInstance;
	// Use this for initialization
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
		PitController spawnPoint = Pits.Find(p=> p.Location.x>GameController.Sun_A.position.x && p.Location.x<GameController.Sun_B.position.x);
		SarlacInstance.anchor = spawnPoint.Location;
		SarlacInstance.transform.position = spawnPoint.Location;
		SarlacInstance.isActive = true;

	}

}

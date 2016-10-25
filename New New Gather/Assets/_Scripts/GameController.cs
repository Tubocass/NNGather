using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	[SerializeField] GameObject SunBar;
	public static Transform Sun_A, Sun_B;
	void Start()
	{
		
			GameObject Sun = Instantiate(SunBar, Vector3.zero,Quaternion.identity) as GameObject;
			Sun_A = Sun.transform;
	
			Sun = Instantiate(SunBar, new Vector3(50,0,0),Quaternion.identity) as GameObject;
			Sun_B = Sun.transform;
		
	}

}

using UnityEngine;
using System.Collections;

public class LightController : MonoBehaviour 
{
	[SerializeField] Light myLight;
	[SerializeField] float nightIntensity, dayIntensity;
	bool bDay;

	void Start () 
	{
		myLight = GetComponentInChildren<Light>();
	}
	void OnEnable()
	{
		UnityEventManager.StartListeningBool("DayTime", DaySwitch);
	}
	void OnDisable()
	{
		UnityEventManager.StopListeningBool("DayTime", DaySwitch);
	}
	void DaySwitch(bool b)
	{
		bDay = b;
		if(bDay)
		{
			myLight.intensity = dayIntensity;
		}
		else myLight.intensity = nightIntensity;
	}
	
	// Update is called once per frame
	void Update () 
	{
//		if(GenerateLevel.IsDayLight())
//		{
//			myLight.intensity = dayIntensity;
//		}
//		else myLight.intensity = nightIntensity;
	}
}

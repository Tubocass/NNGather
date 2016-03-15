using UnityEngine;
using System.Collections;

public class FoodObject : MonoBehaviour 
{
	public static int TotalCreated;
	public int Id{get{return id;}}
	public bool CanBeTargetted{get{return !bAttached;}}
	public Vector3 Location{get{return transform.position;}}
	[SerializeField]private int id;
	[SerializeField]private bool bAttached;

	void OnEnable()
	{
		id = TotalCreated;
		TotalCreated++;
	}
	public void Destroy()
	{
		transform.position = Vector3.zero;
		Detach();
		UnityEventManager.TriggerEventInt("TargetUnavailable",Id);
		gameObject.SetActive(false);
	}
	public void Attach(Transform newParent, Vector3 point)
	{
		transform.SetParent(newParent);
		transform.localPosition = point;
		//GetComponent<Rigidbody>().isKinematic = true;
		bAttached = true;
		UnityEventManager.TriggerEventInt("TargetUnavailable",Id);
	}
	public void Detach()
	{
		transform.SetParent(null);
		//GetComponent<Rigidbody>().isKinematic = false;
		bAttached = false;
	}


}

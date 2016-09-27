using UnityEngine;
using System.Collections;

public class FoodObject : MonoBehaviour 
{
	public static int TotalCreated;
	public int Id{get{return id;}}
	public bool CanBeTargetted{get{return gameObject.activeSelf && !bAttached;}}
	public Vector3 Location{get{return transform.position;}}
	[SerializeField]private int id, idOffset = 1000;
	[SerializeField]private bool bAttached;

	void OnEnable()
	{
		id = TotalCreated+idOffset;
		TotalCreated++;
	}
	public void Destroy()
	{
		transform.position = Vector3.zero;
		Detach();
		UnityEventManager.TriggerEvent("TargetUnavailable",Id);
		gameObject.SetActive(false);
	}
	public void Attach(Transform newParent, Vector3 point)
	{
		transform.SetParent(newParent);
		transform.localPosition = point;
		//GetComponent<Rigidbody>().isKinematic = true;
		bAttached = true;
		UnityEventManager.TriggerEvent("TargetUnavailable",Id);
	}
	public void Detach()
	{
		transform.SetParent(null);
		//GetComponent<Rigidbody>().isKinematic = false;
		bAttached = false;
	}


}

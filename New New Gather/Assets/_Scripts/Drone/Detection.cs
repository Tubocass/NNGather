using UnityEngine;
using System.Collections;

public class Detection : MonoBehaviour 
{
	FarmerController controller;
	// Use this for initialization
	void Start () 
	{
		controller = transform.parent.GetComponent<FarmerController>();
	}
	
	void OnTriggerEnter(Collider other)
	{
		controller.OnTriggerEnter(other);
	}
	void OnCollisionEnter(Collision bang)
	{
		controller.OnCollisionEnter(bang);
	}
}

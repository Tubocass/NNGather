using UnityEngine;
using System.Collections;

public class Detection : MonoBehaviour 
{
	DroneController controller;
	// Use this for initialization
	void Start () 
	{
		controller = transform.parent.GetComponent<DroneController>();
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

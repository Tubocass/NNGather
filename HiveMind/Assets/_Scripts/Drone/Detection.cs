using UnityEngine;
using System.Collections;

public class Detection : MonoBehaviour 
{
	Unit_Base controller;
	// Use this for initialization
	void Start () 
	{
		controller = transform.parent.GetComponent<Unit_Base>();
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

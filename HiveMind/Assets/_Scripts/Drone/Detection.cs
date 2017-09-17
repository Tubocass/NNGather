using UnityEngine;
using System.Collections;

public class Detection : MonoBehaviour 
{
	SpriteDrone controller;
	// Use this for initialization
	void Start () 
	{
		controller = transform.parent.GetComponent<SpriteDrone>();
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

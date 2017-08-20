using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class walker : MonoBehaviour 
{

	[SerializeField] float speed = 2;
	
	// Update is called once per frame
	void Update () 
	{

		float v = Input.GetAxis("Vertical");
		float h = Input.GetAxis("Horizontal");
		if(h>.01||h<0 || v>.01||v<0)
		{
			transform.Translate(new Vector3(h,0,v)* Time.deltaTime* speed);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMovement : MonoBehaviour 
{

	[SerializeField] float speed = 10, maxFOV = 25, minFOV= 20, scrollSpeed = 3f;
	Vector3 movement;
	CameraFollow camFollow;
	// Use this for initialization
	void Start () 
	{
		camFollow = GetComponent<CameraFollow>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float lastInputX = Input.GetAxis ("Horizontal");
		float lastInputY = Input.GetAxis ("Vertical");
		float lastInputScroll = Input.GetAxis("Mouse ScrollWheel");
		if(lastInputX != 0f || lastInputY != 0f)
		{
			movement = new Vector3 	(speed * lastInputX,0 ,  speed * lastInputY);
			camFollow.MoveTo(movement);
		}

		if(lastInputScroll>0f || lastInputScroll<0f)
		{
			Camera.main.orthographicSize -= lastInputScroll* scrollSpeed;
			Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minFOV, maxFOV);
		}
	}
}

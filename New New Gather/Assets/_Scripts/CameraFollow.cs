using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
	public Transform target;            // The position that that camera will be following.
	public float smoothing = 5f;        // The speed with which the camera will be following.
	public bool bFollowing = false;
	Vector3 targetCamPos;
	Transform tran;
	
	Vector3 offset;                     // The initial offset from the target.
	
	void Start ()
	{
		tran = GetComponent<Transform>();
		// Calculate the initial offset.
		//offset = transform.position - target.position;
	}
	public void SetTarget(Transform newTarget)
	{
		target = newTarget;
		tran.position = new Vector3(newTarget.position.x, tran.position.y, newTarget.position.z);
		offset = transform.position - target.position;
	}
	public void MoveTo(Vector3 position)
	{
		bFollowing = false;
		Vector3 targetDir  = tran.position + position*Time.deltaTime; 
		tran.position = Vector3.MoveTowards(tran.position,targetDir,0.25f);
	}
	public void SetFollow()
	{
		bFollowing = true;
	}
	
	void FixedUpdate ()
	{
		if(bFollowing)
		{
			// Create a postion the camera is aiming for based on the offset from the target.
			targetCamPos = target.position + offset;
			
			// Smoothly interpolate between the camera's current position and it's target position.
			transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
		}

	}
}
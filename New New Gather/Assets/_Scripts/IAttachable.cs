using System.Collections;
using UnityEngine;

public interface IAttachable
{
	void Attach(Transform newParent, Vector3 attachPoint);
	void Detach();


}

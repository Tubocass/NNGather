using UnityEngine;
using System.Collections;

public interface IBehaviour
{
	IEnumerator EnterState
	(
	);
	//void EnterState ();
	void Update ();
	void ExitState ();
	void ToWander ();
}

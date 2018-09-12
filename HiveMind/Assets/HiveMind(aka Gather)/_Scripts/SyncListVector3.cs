using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class SyncListVector3: SyncList<Vector3>
{
	public float x,y,z;
//	void OnListChanged(SyncListVector3.Operation op, int index)
//	{
//		Debug.Log("List Changed"+op.ToString());
//	}
	public Vector3[] ToArray()
	{
		Vector3[] array = new Vector3[this.Count];
		for(int i = 0;i<this.Count;i++)
		{
			array[i] = this[i];
		}
		return array;
	}
	protected override void SerializeItem(NetworkWriter writer, Vector3 item)
     {
         writer.Write(item);
     }

     protected override Vector3 DeserializeItem(NetworkReader reader)
     {
         return reader.ReadVector3();
     }
}

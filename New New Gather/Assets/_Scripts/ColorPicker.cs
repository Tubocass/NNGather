using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorPicker: MonoBehaviour
{
	[SerializeField] List<Material> farmMatList;
	[SerializeField] List<Material> fightMatList;

	public Material ColorFarmer(int team)
	{
		return farmMatList[team];
	}

	public Material ColorFighter(int team)
	{
		return fightMatList[team];
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grower : Consumable
{
	public float growMultiplier = 1.03f;
	public override void Affect(PlayerConroller pc)
	{
		StartCoroutine(StartGrow(pc));
	}

	private IEnumerator StartGrow(PlayerConroller pc)
	{
		while(pc.GetBodyScale() < 2)
		{
			pc.MultiplyScale(growMultiplier);
			yield return null;
		}

		yield return new WaitForSeconds(3.0f);

		while(pc.GetBodyScale() > 1)
		{
			pc.MultiplyScale(1/ growMultiplier);
			yield return null;
		}
		canAffect = true;
	}
}

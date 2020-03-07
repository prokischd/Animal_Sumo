using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grower : Consumable
{
	public float growMultiplier = 1.03f;
	public float maxScale = 2.2f;
	public float cooldown = 6.0f;
	public override void Affect(PlayerConroller pc)
	{
		StartCoroutine(StartGrow(pc));
	}

	private IEnumerator StartGrow(PlayerConroller pc)
	{
		while(pc.GetBodyScale() < maxScale)
		{
			pc.MultiplyScale(growMultiplier);
			pc.MultiplyLines(growMultiplier);
			yield return null;
		}

		yield return new WaitForSeconds(cooldown);

		while(pc.GetBodyScale() > 1)
		{
			pc.MultiplyScale(1 / growMultiplier);
			pc.MultiplyLines(1 / growMultiplier);
			yield return null;
		}
		canAffect = true;
	}
}

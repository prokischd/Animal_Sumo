using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grower : Consumable
{
	public float growMultiplier = 1.03f;
	public override void Affect(PlayerConroller pc)
	{
		canAffect = false;
		StartCoroutine(StartGrow(pc));
	}

	private IEnumerator StartGrow(PlayerConroller pc)
	{
		GameObject player = pc.gameObject;

		while(player.transform.localScale.x < 2)
		{
			player.transform.localScale *= growMultiplier;
			yield return null;
		}

		yield return new WaitForSeconds(3.0f);

		while(player.transform.localScale.x > 1)
		{
			player.transform.localScale /= 1.1f;
			yield return null;
		}
		canAffect = true;
	}
}

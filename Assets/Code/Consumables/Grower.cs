using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grower : Consumable
{
	private float growMultiplier = 1.02f;
	private float maxScale = 2.5f;
	private float cooldown = 6.0f;
	public AudioClip aclip;
	private AudioSource source;

	private void Start()
	{
		source = GetComponent<AudioSource>();
	}
	public override void Affect(PlayerConroller pc)
	{
		StartCoroutine(StartGrow(pc));
	}

	private IEnumerator StartGrow(PlayerConroller pc)
	{
		source.Play();
		pc.CanGrow = false;
		AudioSource.PlayClipAtPoint(aclip, Camera.main.transform.position);
		while(pc.GetBodyScale() < maxScale)
		{
			pc.MultiplyScale(growMultiplier);
			pc.MultiplyLines(growMultiplier);
			pc.MultiplyRayLine(growMultiplier);
			yield return null;
		}

		yield return new WaitForSeconds(cooldown);

		while(pc.GetBodyScale() > 1)
		{
			pc.MultiplyScale(1 / growMultiplier);
			pc.MultiplyLines(1 / growMultiplier);
			pc.MultiplyRayLine(1 / growMultiplier);
			yield return null;
		}
		pc.CanGrow = true;
	}
}

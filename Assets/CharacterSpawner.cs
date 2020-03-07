using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner :MonoBehaviour
{
	public Action<Transform> OnTargetRevived;
	internal void ResetTargets(IEnumerable<Transform> deadTargets)
	{
		StartCoroutine(ResetTargetsAfterTime(deadTargets, 3.0f));		
	}

	private IEnumerator ResetTargetsAfterTime(IEnumerable<Transform> deadTargets, float v)
	{
		yield return new WaitForSeconds(v);
		foreach(var target in deadTargets)
		{
			target.position = this.transform.position;
			target.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
			OnTargetRevived?.Invoke(target);
		}
	}
}

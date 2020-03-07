using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawner :MonoBehaviour
{
	public Action<Transform> OnTargetRevived;
	public Action<Transform> OnDeath;
	internal void ResetTarget(Transform target)
	{
		StartCoroutine(ResetTargetsAfterTime(target, 3.0f));		
	}

	private IEnumerator ResetTargetsAfterTime(Transform target, float v)
	{
		yield return new WaitForSeconds(v);
		target.position = this.transform.position;
		target.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		OnTargetRevived?.Invoke(target);
		target.parent.GetComponent<PlayerConroller>().alive = true;
	}
}

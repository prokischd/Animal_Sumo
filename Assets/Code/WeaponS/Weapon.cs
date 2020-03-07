using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public Rigidbody2D parentRb { get; set; }
	public float weaponForce = 300; 
	public float defaultTimer = 0.1f;
	public float distance = 4;
	protected float timer;
	protected System.Random rng;

	void Start()
	{
		timer = defaultTimer;
		rng = new System.Random();
	}

	public virtual bool CanExecute()
	{
		return timer <= 0.0f && parentRb != null;
	}

	public virtual void Execute(float v, Transform target)
	{
	}

	internal void RotateTowards(Transform enemyTransform)
	{
		Quaternion.RotateTowards(this.transform.rotation, enemyTransform.rotation, 10.0f);
	}
}

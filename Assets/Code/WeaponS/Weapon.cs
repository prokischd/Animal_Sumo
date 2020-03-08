using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public ObjectSpawner objSpawner;
	public Rigidbody2D parentRb { get; set; }
	public float weaponForce = 300; 
	public float defaultTimer = 0.1f;
	public float distance = 4;
	protected float timer;
	protected System.Random rng;
	public AudioClip equip;
	void Start()
	{
		timer = defaultTimer;
		rng = new System.Random();
	}

	public virtual bool CanExecute(PlayerConroller target)
	{
		return timer <= 0.0f && parentRb != null;
	}

	public virtual void Execute(float v, PlayerConroller target)
	{
	}

	internal void RotateTowards(Transform enemyTransform)
	{
		Quaternion.RotateTowards(this.transform.rotation, enemyTransform.rotation, 10.0f);
	}

	internal void Possess(WeaponSlot slot)
	{
		transform.parent = slot.go.transform;
		transform.localPosition = Vector3.zero;
		transform.rotation = slot.go.transform.rotation;
		parentRb = slot.go.GetComponent<Rigidbody2D>();
		slot.weapon = this;
		slot.taken = true;
		objSpawner.StartSpawning();
	}
}

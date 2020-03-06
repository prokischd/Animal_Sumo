using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public GameObject projectile;
	public Rigidbody2D parentRb;
	public float weaponForce = 300; 
	public float defaultTimer = 0.3f;
	private float timer;

	private void Start()
	{
		timer = defaultTimer;
	}

	private void Update()
	{
		timer -= Time.deltaTime;
	}

	internal bool CanExecute()
	{
		return timer <= 0.0f;
	}

	internal void Execute()
	{
		timer = defaultTimer;
		var go = Instantiate(projectile, this.transform.position, Quaternion.identity);
		var rb = go.GetComponent<Rigidbody2D>();
		rb.AddForce(weaponForce * transform.up, ForceMode2D.Impulse);
		parentRb.AddForce(-weaponForce * transform.up, ForceMode2D.Impulse);
	}
}

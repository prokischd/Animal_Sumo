using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public GameObject projectile;
	public Rigidbody2D parentRb;
	public float weaponForce = 300; 
	public float defaultTimer = 0.1f;
	public float distance = 4;
	private float timer;
	private System.Random rng;

	private void Start()
	{
		timer = defaultTimer;
		rng = new System.Random();
	}

	private void Update()
	{
		timer -= Time.deltaTime;
	}

	internal bool CanExecute()
	{
		return timer <= 0.0f && parentRb != null;
	}

	internal void Execute(float v)
	{
		timer = defaultTimer;
		var go = Instantiate(projectile, this.transform.position, Quaternion.identity);
		var rb = go.GetComponent<Rigidbody2D>();
		//var dir = LaciMath.GetRandomSign() * LaciMath.GetRandomValue() * distance;

		//int sign = v > 0 ? 1 : -1;
		//var dir =  sign * distance;
		//rb.AddForce(weaponForce * new Vector2(dir, 0), ForceMode2D.Impulse);
		rb.AddForce(weaponForce * -transform.right, ForceMode2D.Impulse);
		parentRb.AddForce(-weaponForce * transform.up, ForceMode2D.Impulse);
		Destroy(go, 5);
	}
}

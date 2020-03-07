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

	public virtual void Execute(float v)
	{
		timer = defaultTimer;
		//var go = Instantiate(projectile, this.transform.position, Quaternion.identity);
		var go = Instantiate(projectile, this.transform.position, this.transform.rotation);
		var rb = go.GetComponent<Rigidbody2D>();
		//var dir = LaciMath.GetRandomSign() * LaciMath.GetRandomValue() * distance;

		//int sign = v > 0 ? 1 : -1;
		//var dir =  sign * distance;
		//rb.AddForce(weaponForce * new Vector2(dir, 0), ForceMode2D.Impulse);
		//rb.AddForce(weaponForce * -transform.right, ForceMode2D.Impulse);
		parentRb.AddForce(-weaponForce * transform.up, ForceMode2D.Impulse);
		Destroy(go, 5);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnife : Weapon
{
	private Rigidbody2D rb;
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		timer -= Time.deltaTime;
	}

	public override void Execute(float v)
	{
		timer = defaultTimer;
		rb.AddForce(-weaponForce * transform.up, ForceMode2D.Impulse);
		Destroy(this.gameObject, 10);
	}
}

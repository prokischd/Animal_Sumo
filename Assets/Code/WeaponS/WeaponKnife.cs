using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponKnife : Weapon
{
	void Start()
	{

	}

	private void Update()
	{
		timer -= Time.deltaTime;
	}

	public override void Execute(float v)
	{
		timer = defaultTimer;
		var go = Instantiate(projectile, this.transform.position, this.transform.rotation);
		var rb = go.GetComponent<Rigidbody2D>();
		parentRb.AddForce(-weaponForce * transform.up, ForceMode2D.Impulse);
		Destroy(go, 5);
	}
}

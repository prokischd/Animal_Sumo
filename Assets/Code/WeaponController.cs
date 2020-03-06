using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	public CircleCollider2D bodyCollider;
	public GameObject weaponSpot;
	public List<Weapon> weapons;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.GetComponent<Weapon>() is Weapon weapon)
		{
			weapon.transform.parent = weaponSpot.transform;
			weapon.transform.localPosition = Vector3.zero;
			weapon.parentRb = weaponSpot.GetComponent<Rigidbody2D>();
			weapons.Add(weapon);			
		}
	}


	private void Update()
	{
		foreach(var wep in weapons)
		{
			if(wep.CanExecute())
			{
				wep.Execute();
			}
		}
	}
}

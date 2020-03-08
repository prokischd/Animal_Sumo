using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	private List<GameObject> publicSpots = new List<GameObject>();
	private List<WeaponSlot> weaponSpots = new List<WeaponSlot>();
	private PlayerConroller pc;
	private CircleCollider2D bodyCollider;

	private void Start()
	{
		var child1 = gameObject.transform.Find("Arm_R").GetChild(3).gameObject;
		var child2 = gameObject.transform.Find("Arm_L").GetChild(3).gameObject;
		publicSpots.Add(child1);
		publicSpots.Add(child2);
		foreach(var spot in publicSpots)
		{
			var ws = new WeaponSlot();
			ws.go = spot;
			weaponSpots.Add(ws);
		}
		bodyCollider = GetComponent<CircleCollider2D>();
		pc = bodyCollider.transform.parent.GetComponent<PlayerConroller>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.GetComponent<Weapon>() is Weapon weapon)
		{
			var slot = GetFreeWeaponSlot();
			if(slot != null && weapon.parentRb == null)
			{
				AudioSource.PlayClipAtPoint(weapon.equip, Camera.main.transform.position);
				weapon.Possess(slot);
			}			
		}
		else if(collision.gameObject.GetComponent<Consumable>() is Consumable consumable)
		{
			if(consumable.CanAffect() && pc.CanGrow)
			{
				consumable.Affect(pc);
			}
			
		}
	}

	internal void UseWeapon(PlayerConroller other)
	{
		foreach(var spot in weaponSpots)
		{
			if(spot.HasWeapon())
			{
				spot.weapon.Execute(pc.GetCurrentHorizontalForce(), other);
				spot.taken = false;
				break;
			}
		}
	}

	private WeaponSlot GetFreeWeaponSlot()
	{
		return weaponSpots.Where(w => !w.taken).FirstOrDefault();
	}

	private void Update()
	{
		/*if(pc.inputBlocked != 0)
		{
			return;
		}
		foreach(var spot in weaponSpots)
		{
			if(spot.HasWeapon() && spot.weapon.CanExecute(pc.GetRandomEnemy()))
			{
				spot.weapon.Execute(pc.GetCurrentHorizontalForce(), pc.GetRandomEnemy());
				spot.taken = false;
			}
		}*/
	}
}

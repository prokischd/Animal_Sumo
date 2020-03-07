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
		var child1 = gameObject.transform.parent.Find("Arm_R").GetChild(3).gameObject;
		var child2 = gameObject.transform.parent.Find("Arm_L").GetChild(3).gameObject;
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
				weapon.transform.parent = slot.go.transform;
				weapon.transform.localPosition = Vector3.zero;
				weapon.transform.localRotation = slot.go.transform.localRotation;
				weapon.parentRb = slot.go.GetComponent<Rigidbody2D>();
				slot.weapon = weapon;
				slot.taken = true;
			}			
		}
	}

	private WeaponSlot GetFreeWeaponSlot()
	{
		return weaponSpots.Where(w => !w.taken).FirstOrDefault();
	}

	private void Update()
	{
		foreach(var spot in weaponSpots)
		{
			if(spot.HasWeapon() && spot.weapon.CanExecute())
			{
				spot.weapon.Execute(pc.GetCurrentHorizontalForce());
			}
		}
	}
}

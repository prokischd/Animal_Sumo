using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
	public CircleCollider2D bodyCollider;
	public List<GameObject> publicSpots;
	private List<WeaponSlot> weaponSpots = new List<WeaponSlot>();
	public PlayerConroller pc;

	private void Start()
	{
		foreach(var spot in publicSpots)
		{
			var ws = new WeaponSlot();
			ws.go = spot;
			weaponSpots.Add(ws);
		}
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

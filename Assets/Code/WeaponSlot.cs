using UnityEngine;

[System.Serializable]
public class WeaponSlot
{
	public GameObject go;
	public bool taken = false;
	public Weapon weapon;

	public bool HasWeapon()
	{
		return weapon != null;
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
	protected bool canAffect = true;

	public virtual void Affect(PlayerConroller pc)
	{
	}

	internal bool CanAffect()
	{
		return canAffect;
	}
}

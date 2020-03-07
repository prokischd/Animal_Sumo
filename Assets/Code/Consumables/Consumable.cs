using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
	protected bool canAffect = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public virtual void Affect(PlayerConroller pc)
	{
	}

	internal bool CanAffect()
	{
		return canAffect;
	}
}

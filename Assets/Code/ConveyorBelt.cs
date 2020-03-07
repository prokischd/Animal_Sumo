using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
	public float conveyorForce = 10.0f;

	private void OnTriggerStay2D(Collider2D collision)
	{
		if(collision.GetComponent<WeaponController>() is WeaponController c)
		{
			var rb = c.GetComponent<Rigidbody2D>();
			rb.AddForce(transform.right * conveyorForce, ForceMode2D.Impulse);
			rb.gravityScale = -0.3f;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.GetComponent<WeaponController>() is WeaponController c)
		{
			var rb = c.GetComponent<Rigidbody2D>();
			rb.velocity = Vector3.zero;
			rb.gravityScale = 1.0f;
		}
	}
}

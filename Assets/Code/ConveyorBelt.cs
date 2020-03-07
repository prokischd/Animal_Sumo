using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
	public float conveyorForce = 100.0f;
	private BoxCollider2D collider;

	void Start()
    {
		collider = GetComponent<BoxCollider2D>();
    }


	private void OnTriggerStay2D(Collider2D collision)
	{
		if(collision.GetComponent<WeaponController>() is WeaponController c)
		{
			var rb = c.GetComponent<Rigidbody2D>();
			rb.AddForce(transform.right * conveyorForce, ForceMode2D.Impulse);
			rb.gravityScale = 0.0f;
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

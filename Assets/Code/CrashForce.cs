using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashForce : MonoBehaviour
{
	private PlayerConroller playerController;
	private Collider2D[] pushCol;
	public float force = 50f;
	public LayerMask colMask;
	public GameObject explosion;
	public float explosionForce = 5000;
	public float explosionRadious = 10;


	void Start()
	{
		playerController = transform.parent.GetComponent<PlayerConroller>();
		colMask = colMask & ~(1 << playerController.gameObject.layer);
	}

	public void Crash(RaycastHit2D hit)
	{
		if(playerController.crashing)
		{
			playerController.crashing = false;
			pushCol = Physics2D.OverlapCircleAll(hit.point, explosionRadious, colMask);
			foreach(var collidedWith in pushCol)
			{
				var rb = collidedWith.GetComponent<Rigidbody2D>();
				if(rb != null)
				{
					rb.AddExplosionForce(playerController.GetExplosionForce(), hit.point, explosionRadious);					
				}
				Instantiate(explosion, hit.point, Quaternion.identity);
			}		
		}
		playerController.crashBonus = 1.0f;
	}
}

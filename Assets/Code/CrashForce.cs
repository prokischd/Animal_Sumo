using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashForce :MonoBehaviour
{
	private PlayerConroller playerController;
	private Collider2D[] pushCol;
	public float force = 50f;
	public LayerMask colMask;
	private LayerMask colMask2;
	public GameObject explosion;
	public float explosionForce = 5000;
	public float explosionRadious = 25;
	private bool canCrash = true;

	void Start()
	{
		playerController = transform.parent.GetComponent<PlayerConroller>();
		colMask = colMask & ~(1 << playerController.gameObject.layer);
		colMask2 = colMask;
		colMask2 = colMask & ~(1 << LayerMask.GetMask("Ground"));
	}

	public void Crash(Vector2 hit)
	{
		if(playerController.crashing && canCrash)
		{

			playerController.crashing = false;
			pushCol = Physics2D.OverlapCircleAll(hit, explosionRadious, colMask);
			foreach(var collidedWith in pushCol)
			{
				var cf = collidedWith.GetComponent<CrashForce>();
				if(cf != null && cf != this)
				{
					cf.canCrash = false;
					StartCoroutine(cf.ResetCrashTimer(1.0f));
					var rb = cf.gameObject.GetComponent<Rigidbody2D>();
					rb.AddExplosionForce(playerController.GetExplosionForce(), mode: ForceMode2D.Impulse, explosionPosition: hit);
				}
			}
			var go = Instantiate(explosion, hit, Quaternion.identity);
			go.GetComponent<ProjectileScript>().IgnoreLayer = playerController.gameObject.layer;
			canCrash = false;
			StartCoroutine(ResetCrashTimer(1.0f));
		}
		playerController.crashBonus = 1.0f;
	}

	private IEnumerator ResetCrashTimer(float v)
	{
		yield return new WaitForSeconds(v);
		canCrash = true;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(LaciMath.IsInLayerMask(collision.gameObject.layer, colMask2))
		{
			Crash(collision.contacts[0].point);
		}
	}

}

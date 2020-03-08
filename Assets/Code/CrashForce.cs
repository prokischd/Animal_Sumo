using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashForce : MonoBehaviour
{
	private PlayerConroller playerController;
	private WeaponController wc;
	private Collider2D[] pushCol;
	public float force = 50f;
	public LayerMask colMask;
	private LayerMask colMask2;
	public GameObject explosion;
	private float explosionRadious = 3f;
	private bool canCrash = true;

	void Start()
	{
		wc = GetComponent<WeaponController>();
		playerController = transform.parent.GetComponent<PlayerConroller>();
		colMask = colMask & ~(1 << playerController.gameObject.layer);
		colMask2 = colMask;
		colMask2 = colMask & ~(1 << LayerMask.GetMask("Ground"));
	}

	public void Crash(Vector2 hit)
	{
		if(playerController.crashing && canCrash)
		{
			GetComponent<AudioSource>().Play();
			playerController.crashing = false;
			pushCol = Physics2D.OverlapCircleAll(hit, explosionRadious * playerController.rbBody.transform.localScale.x, colMask);
			foreach(var collidedWith in pushCol)
			{
				var cf = collidedWith.GetComponent<CrashForce>();
				if(cf != null && cf != this)
				{
					cf.canCrash = false;
					StartCoroutine(cf.ResetCrashTimer(1.0f));
					var rb = cf.gameObject.GetComponent<Rigidbody2D>();
					wc.UseWeapon(cf.GetComponentInParent<PlayerConroller>());
					rb.AddExplosionForce(playerController.GetExplosionForce(), mode: ForceMode2D.Impulse, explosionPosition: hit);
					playerController.rbBody.velocity = Vector3.zero;
				}
			}
			var go = Instantiate(explosion, hit, Quaternion.identity);
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

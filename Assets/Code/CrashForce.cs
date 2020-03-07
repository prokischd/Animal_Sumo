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
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(playerController.crashing)
		{
			playerController.crashing = false;
			if((colMask.value & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
			{
				pushCol = Physics2D.OverlapCircleAll(col.contacts[0].point, explosionRadious);

				for(int i = 0; i < pushCol.Length; i++)
				{
					var rb = pushCol[i].GetComponent<Rigidbody2D>();
					if(rb != null)
					{
						Debug.Log(pushCol[i]);
						rb.AddExplosionForce(playerController.GetExplosionForce(), col.contacts[0].point, explosionRadious);
					}
				}
				if(explosion != null)
				{
					var go = Instantiate(explosion, col.contacts[0].point, Quaternion.identity);
				}
			}
		}
		playerController.crashBonus = 1.0f;
	}
}

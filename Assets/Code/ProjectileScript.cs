using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    private Rigidbody2D myBody;
    public float force = 50f;
    public LayerMask colMask;
    public GameObject explosion;
    private float gravity;
    private Collider2D[] pushCol;
    public float explosionForce = 100;
    public float explosionRadious=10;

	public int IgnoreLayer { get; internal set; } = -11;

	public float deathTimer = 5.0f;

    void Start()
    {
        myBody = GetComponent < Rigidbody2D > ();
		Destroy(this.gameObject, deathTimer);
	}

    void Update()
    {
        gravity+= 300*Time.deltaTime;
        myBody.AddForce(-transform.right * force, ForceMode2D.Force);
        myBody.AddForce(Vector2.down * gravity, ForceMode2D.Force);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if ((colMask.value & 1 << col.gameObject.layer) == 1 << col.gameObject.layer)
        {
            Destroy(gameObject);
            pushCol = Physics2D.OverlapCircleAll(col.contacts[0].point, explosionRadious, ~(1 << IgnoreLayer));
            
            for (int i = 0; i < pushCol.Length; i++)
            {
				var rb = pushCol[i].GetComponent<Rigidbody2D>();
				if (rb != null)
                {
					rb.AddExplosionForce(explosionForce, col.contacts[0].point, explosionRadious);
                }
            }
            if (explosion != null)
            {
                var go = Instantiate(explosion, col.contacts[0].point, Quaternion.identity);
				Destroy(go, deathTimer);
				Destroy(this.gameObject, deathTimer);
			}
        }
    }

}

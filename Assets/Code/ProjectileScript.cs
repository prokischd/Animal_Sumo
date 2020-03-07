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

    void Start()
    {
        myBody = GetComponent < Rigidbody2D > ();
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
            Debug.Log("projectileCol");
            Destroy(gameObject);
            pushCol = Physics2D.OverlapCircleAll(col.contacts[0].point, explosionRadious);
            
            for (int i = 0; i < pushCol.Length; i++)
            {
                if (pushCol[i].GetComponent<Rigidbody2D>() != null)
                {
                    Debug.Log(pushCol[i]);
                    pushCol[i].GetComponent<Rigidbody2D>().AddExplosionForce(explosionForce, col.contacts[0].point, explosionRadious);
                }
            }
            if (explosion != null)
            {
                Instantiate(explosion, col.contacts[0].point, Quaternion.identity);
            }
        }
    }

}

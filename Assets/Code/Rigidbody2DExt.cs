using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rigidbody2DExt
{

    public static void AddExplosionForce(this Rigidbody2D rb, float explosionForce, Vector2 explosionPosition, float upwardsModifier = 0.0F, ForceMode2D mode = ForceMode2D.Force)
    {
        var explosionDir = rb.position - explosionPosition;
		explosionDir = explosionDir.normalized;
        rb.AddForce(explosionForce * explosionDir, mode);
    }
}

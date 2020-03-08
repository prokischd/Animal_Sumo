using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlow : MonoBehaviour
{

    public float scrollSpeed = 0.5f;
    private LineRenderer myline;
    private float offset;

    void Start()
    {
        myline = GetComponent<LineRenderer>();
    }

    void Update()
    {
        offset += Time.deltaTime * scrollSpeed;
        myline.material.SetTextureOffset("_MainTex", new Vector2(offset,0));
        if (Mathf.Abs(offset) >= 1)
        {
            offset = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineConnectScript : MonoBehaviour
{

    private LineRenderer myLine;
    private GameObject[] childObjs;

    void Start()
    {
        SetInitialReferences();
    }

    void Update()
    {
        SetLine();
    }

    void SetInitialReferences() {
        myLine = GetComponent<LineRenderer>();
        childObjs = new GameObject[transform.childCount];
        myLine.positionCount = childObjs.Length;
        for (int i = 0; i < childObjs.Length; i++)
        {
            childObjs[i] = transform.GetChild(i).gameObject;
            myLine.SetPosition(i, childObjs[i].transform.position);
        }


    }


    void SetLine() {
        for (int i = 0; i < childObjs.Length; i++)
        {
            myLine.SetPosition(i, childObjs[i].transform.position);
        }
    }
}

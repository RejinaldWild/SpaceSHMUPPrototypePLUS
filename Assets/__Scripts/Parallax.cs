using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject poi;
    public GameObject[] panels;
    public float scrollSpeed = -30f;
    public float motionMult = 0.25f;

    private float panelHeight;
    private float depth;

    // Start is called before the first frame update
    void Start()
    {
        panelHeight = panels[0].transform.localScale.y;
        depth = panels[0].transform.localScale.z;

        panels[0].transform.position = new Vector3(0,0,depth);
        panels[1].transform.position = new Vector3(0, panelHeight, depth);
    }

    // Update is called once per frame
    void Update()
    {
        float tX, tY = 0;
        tY = Time.time * scrollSpeed % panelHeight + (panelHeight * 0.5f);

        if(poi != null )
        {
            tX = -poi.transform.position.x * motionMult;

            panels[0].transform.position = new Vector3(tX,tY,depth);
            if (tY >= 0)
            {
                panels[1].transform.position = new Vector3(tX, tY-panelHeight,depth);
            }
            else
            {
                panels[1].transform.position = new Vector3(tX, tY + panelHeight, depth);
            }
        }
    }
}

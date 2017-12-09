using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flush : MonoBehaviour {
    public Color flushColor;
    public float flushTime = 1.0f;

    private Material fadeMaterial = null;
    private bool isFlushing = false;
    private float timeCount = 0.0f;
    // Use this for initialization
    void Start () {
        fadeMaterial = new Material(Shader.Find("Oculus/Unlit Transparent Color"));
        fadeMaterial.color = flushColor;
    }
	
	// Update is called once per frame
	void Update () {
        //print(flushColor);
        //print(fadeMaterial.color);
        if (isFlushing)
        { 
            if (timeCount > flushTime)
            {
                timeCount = 0.0f;
                isFlushing = false;
                return;
            }

            timeCount += Time.deltaTime;

            if (timeCount < (flushTime / 2.0f))
            {
                fadeMaterial.color = flushColor;
                flushColor.a = flushColor.a += (2 * 1.0f / flushTime) * Time.deltaTime;
                
                if(flushColor.a > 1.0f)
                {
                    flushColor.a = 1.0f;
                }
            }
            else
            {
                fadeMaterial.color = flushColor;
                flushColor.a = flushColor.a -= (2 * 1.0f / flushTime) * Time.deltaTime;
                
                if (flushColor.a < 0.0f)
                {
                    flushColor.a = 0.0f;
                }
            }
        }
    }

    void OnPostRender()
    {
        if (isFlushing)
        {
            fadeMaterial.SetPass(0);
            GL.PushMatrix();
            GL.LoadOrtho();
            GL.Color(fadeMaterial.color);
            GL.Begin(GL.QUADS);
            GL.Vertex3(0f, 0f, -12f);
            GL.Vertex3(0f, 1f, -12f);
            GL.Vertex3(1f, 1f, -12f);
            GL.Vertex3(1f, 0f, -12f);
            GL.End();
            GL.PopMatrix();
        }
    }

    public void flushScreen()
    {
        isFlushing = true;
    }
}

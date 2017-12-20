using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossheirScript : MonoBehaviour {
    private AILibrary parentLibrary;
    private int posNegMod;
    private Vector3 origPos;
    private float xVariance;
    private float yVariance;
    private float interval;
    private float lastTimeChanged;
    public bool isLightning;
    private SpriteRenderer renderer;
    public bool moving;
	void Start () {
        origPos = transform.position;
        xVariance = 1.2f;
        yVariance = 1.2f;
        interval = 4.0f;
        lastTimeChanged = Time.fixedTime;
        renderer = this.GetComponent<SpriteRenderer>();
        moving = true;
	}
	
	void Update () {
        if (parentLibrary == null)
            return;
        if(isLightning)
        {
            if (Time.fixedTime > lastTimeChanged + interval)
            {
                lastTimeChanged = Time.fixedTime;
                transform.position = origPos + new Vector3(Random.value * xVariance - xVariance / 4.0f, Random.value * yVariance - yVariance / 3.0f, 0);
                StartCoroutine(fadeCrossheir());
            }
        }
        else
        {
            //print(moving);
            if (moving)
            {
                transform.position = origPos + new Vector3(parentLibrary.GetCrossheirPosition() * posNegMod, 0, 0);
            }
        }
	}

    public void RegisterLibrary(AILibrary parent, bool isPositiveX)
    {
        parentLibrary = parent;
        posNegMod = isPositiveX ? 1 : -1;
        isLightning = false;
    }

    public void RegisterLightningLibrary(AILibrary parent, bool isPositiveX)
    {
        parentLibrary = parent;
        posNegMod = isPositiveX ? 1 : -1;
        isLightning = true;
    }

    private IEnumerator fadeCrossheir()
    {
        float startTime = Time.fixedTime;
        while (Time.fixedTime < startTime + interval)
        {
            Color col = renderer.color;
            col.a = 1f - .75f * Mathf.Clamp01((Time.fixedTime - startTime) / interval);
            renderer.color = col;
            yield return new WaitForEndOfFrame();
        }
    }

    public void setMoving(bool b)
    {
        this.moving = b;
    }
}

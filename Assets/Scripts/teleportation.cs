using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportation : MonoBehaviour {
	public GameObject OVRPlayerController;
	public GameObject eye;
    public GameObject menu;
    public GameObject teleportationSign;

    public GameObject playerLeftHand;
    public float coldDown;
    
    private bool XPressed;
    private GameObject sign;
    private float timeElapsed;

    private float targetX;
    private float targetY;

    private bool teleportationEnabled;
	// Use this for initialization
	void Start () {
        XPressed = false;
        timeElapsed = 0.0f;
        teleportationEnabled = false;
    }

    // Update is called once per frame
    void Update() {
        if(teleportationEnabled == false)
        {
            return;
        }

        timeElapsed += Time.deltaTime;
        if(timeElapsed < 2.0f)
        {
            return;
        }

        //print(timeElapsed);
        //Debug.Log(LayerMask.LayerToName(10));

        //print(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick));
        //print(OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick));

        float leftX = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).x;
        float leftY = OVRInput.Get(OVRInput.RawAxis2D.LThumbstick).y;
        float rightX = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).x;
        float rightY = OVRInput.Get(OVRInput.RawAxis2D.RThumbstick).y;

        //print(rightX);

        float diff = Mathf.Sqrt(Mathf.Pow(leftX - rightX, 2.0f) + Mathf.Pow(leftY - rightY, 2.0f));
        //print(Mathf.Sqrt(Mathf.Pow(leftX, 2.0f) + Mathf.Pow(leftY, 2.0f)));
        if( Mathf.Sqrt(Mathf.Pow(leftX, 2.0f) + Mathf.Pow(leftY, 2.0f)) < 0.9f || Mathf.Sqrt(Mathf.Pow(rightX, 2.0f) + Mathf.Pow(rightY, 2.0f)) < 0.9f ) {
            //print(new Vector2 ( Mathf.Sqrt(Mathf.Pow(leftX, 2.0f) + Mathf.Pow(leftY, 2.0f)), Mathf.Sqrt(Mathf.Pow(rightX, 2.0f) + Mathf.Pow(rightY, 2.0f))) );
            return;
        }
        if(diff < 0.5f)
        {
            float x = (leftX + rightX) / 2;
            float y = (leftY + rightY) / 2;
            if(y < -0.5)
            {
                y = -1.0f;
            }
            else if(y > 0.5)
            {
                y = 1.0f;
            }
            else
            {
                y = 0.0f;
            }

            if(x < -0.5)
            {
                x = -1.0f;
            }
            else if(x > 0.5)
            {
                x = 1.0f;
            }
            else
            {
                x = 0.0f;
            }

            float tmp = Mathf.Sqrt( Mathf.Pow(x, 2.0f) + Mathf.Pow(y, 2.0f) );
            targetX = this.transform.position.x + 5 * x / tmp;
            targetY = this.transform.position.z + 5 * y / tmp;

            if (targetX > 10.0f)
            {
                targetX = 10.0f;
            }
            if (targetX < -10.0f)
            {
                targetX = -10.0f;
            }

            if (targetY > 2.0f)
            {
                targetY = 2.0f;
            }
            if (targetY < -8.0f)
            {
                targetY = -10.0f;
            }
            timeElapsed = 0.0f;
            GameObject.Find("CenterEyeAnchor").GetComponent<flush>().flushScreen();
            Invoke("move", 0.35f);
        }
    }

    public void move()
    {
        this.transform.position = new Vector3(targetX, 1.0f, targetY);
        menu.transform.position = new Vector3(targetX, 1.0f, targetY);
    }
}
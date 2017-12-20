using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwfireball : MonoBehaviour {

    private GameObject righthand;
    GameObject thefireball;
    private Vector3 previousPosition;
    private Vector3 movingdirection;
    private float movingspeed = 1;
    private bool noDirectionYet = true;
    private bool beforeFireThrow;

    // Use this for initialization
    void Start () {
        righthand = GameObject.Find("RightHandAnchor");
    }

    // Update is called once per frame
    void Update () {
        FireballHandler();
    }


    public static bool GetDominantGripDown()
    {
        return OVRInput.Get(OVRInput.RawButton.RHandTrigger);
    }

    private void FireballHandler()
    {

        if (thefireball == null) //while no fire ball yet
        {
            if (GetDominantGripDown())
            {
                beforeFireThrow = true;
                if (beforeFireThrow && thefireball == null)
                {
                    thefireball = (GameObject)Instantiate(Resources.Load("FireMagic"), righthand.transform.position, righthand.transform.rotation);
                    Destroy(thefireball, 3);
                    noDirectionYet = true;
                }
            }
        }
        else // fire ball created
        {
            if (GetDominantGripDown() == false) //if released grip
            {
                //print("should be moving:" + movingdirection);
                thefireball.transform.position = thefireball.transform.position + movingdirection * movingspeed;
                if (noDirectionYet)
                {
                    movingdirection = righthand.transform.position - previousPosition;
                    noDirectionYet = false;
                }
            }
            else  //if holding grip
            {
                thefireball.transform.position = righthand.transform.position;
            }
        }

        previousPosition = righthand.transform.position;
    }
}

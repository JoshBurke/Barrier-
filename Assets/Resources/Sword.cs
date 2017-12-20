using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour { 
    private Vector3 previousPosition;

    private Color normalColor;
    private Color shottingColor;

    private ArrayList positions;
    private ArrayList rotations;

    private bool shooting;
    private float speed;
    private float timeGone;
    private int shotNum;

    private GameObject curveComponent;
    // Use this for initialization
    void Start () {
        //(this.transform.GetComponent<Collider>() as CapsuleCollider).radius = 0.005f;
        normalColor = this.GetComponent<VolumetricLines.VolumetricLineBehavior>().LineColor;
        shottingColor = new Color(1.0F, 0.0F, 0.0F, 1.0F);
        shooting = false;
        previousPosition = this.transform.position + this.transform.forward * 1.2f;
        timeGone = 0.0f;
        speed = 0.0f;
        shotNum = 0;
        curveComponent = (GameObject)Resources.Load("CurveComponent");
    }

    // Update is called once per frame
    void Update() {
        if (shotNum > 0)
        {
            if (shooting)
            {
                if (OVRInput.Get(OVRInput.RawButton.RHandTrigger) == false)
                {
                    shoot();
                }
                else
                {
                    Vector3 currP = this.transform.position + this.transform.forward * 1.2f;
                    positions.Add(currP);
                    Vector3 currR = this.transform.rotation.eulerAngles;
                    if (currR.x > 180.0f)
                    {
                            currR.x = currR.x - 360.0f;
                    }
                    if (currR.y > 180.0f)
                    {
                        currR.y = currR.y - 360.0f;
                    }
                    if (currR.z > 180.0f)
                    {
                        currR.z = currR.z - 360.0f;
                    }
                    //print(currR);
                    rotations.Add(currR);
                }
            }
            else
            {
                if (OVRInput.Get(OVRInput.RawButton.RHandTrigger) == true)
                {
                    shooting = true;
                    this.GetComponent<VolumetricLines.VolumetricLineBehavior>().LineColor = shottingColor;
                    positions = new ArrayList();
                    rotations = new ArrayList();
                }
            }
        }
    }

    private void shoot()
    {
        GameObject.Find("Asgore(Clone)").GetComponent<AILibrary>().setShotLive(true);
        GameObject.Find("Asgore(Clone)").GetComponent<AILibrary>().stopCrossheirMoving();
        this.shotNum -= 1;
        shooting = false;
        this.GetComponent<VolumetricLines.VolumetricLineBehavior>().LineColor = normalColor;

        Vector3 p = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 prev = (Vector3)positions[0];
        p += (Vector3)positions[0];
        if (positions.Count >= 1)
        {
            p += (Vector3)positions[positions.Count - 1];
        }
        p = p / 2.0f;

        float tmp = Mathf.Rad2Deg * Mathf.Atan((((Vector3)positions[0]).y - p.y) / (((Vector3)positions[0]).x - p.x));
        if(tmp > 180.0f)
        {
            tmp = tmp - 180.0f;
        }
        Vector3 r = new Vector3(0.0f, 0.0f, 0.0f);
        prev = (Vector3)rotations[0];
        r += (Vector3)rotations[0];
        if (rotations.Count >= 1)
        {
            r += (Vector3)rotations[rotations.Count - 1];
        }
        //print((Vector3)rotations[0]);
        //print((Vector3)rotations[rotations.Count - 1]);
        r = r / 2.0f;
        //print(r);

        r.z = tmp + 90.0f;
        r.x += 10.0f;

        positions = null;
        rotations = null;

        GameObject shot = (GameObject)Instantiate(Resources.Load("PlayerCurvedShot"), p, Quaternion.Euler(r));
        //GameObject empty = Instantiate(GameObject, p, Quaternion.identity);
        //GameObject shot = (GameObject)Instantiate(playerCurvedShot, p, Quaternion.Euler(r));
    }

    public void giveShot()
    {
        this.shotNum += 1;
    }

    public void changeColorTo(Color c)
    {
        this.GetComponent<VolumetricLines.VolumetricLineBehavior>().LineColor = c;
    }

    public void changeColorBackToNormal()
    {
        this.GetComponent<VolumetricLines.VolumetricLineBehavior>().LineColor = normalColor;
    }
}

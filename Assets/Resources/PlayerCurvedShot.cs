using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PlayerCurvedShot : MonoBehaviour {

    public float Speed = 3.0f;
    public float MaxDamage = 10.0f;

    public GameObject FireSound;
    public GameObject HitSound;

    public AudioClip HapticSound;

    private float startTime;
    private GameObject damageText;

    // Use this for initialization
    void Start () {
        OVRHapticsClip hapticClip = new OVRHapticsClip(HapticSound);
        int n = 0;
        if (Shield.currSword.transform.parent.gameObject.name == "RightShield")
        {
            n = 1;
        }
        OVRHaptics.Channels[n].Mix(hapticClip);
        Instantiate(FireSound, transform.position, transform.rotation);
        startTime = Time.fixedTime;
        damageText = GameObject.Find("DamageText");

        GameObject curveComponent = (GameObject)Resources.Load("CurveComponent");
        GameObject t1 = Instantiate(curveComponent);
        GameObject t2 = Instantiate(curveComponent);
        GameObject t3 = Instantiate(curveComponent);
        GameObject t4 = Instantiate(curveComponent);
        GameObject t5 = Instantiate(curveComponent);
        GameObject t6 = Instantiate(curveComponent);
        GameObject t7 = Instantiate(curveComponent);
        GameObject t8 = Instantiate(curveComponent);
        GameObject t9 = Instantiate(curveComponent);

        t1.transform.parent = this.transform;
        t2.transform.parent = this.transform;
        t3.transform.parent = this.transform;
        t4.transform.parent = this.transform;
        t5.transform.parent = this.transform;
        t6.transform.parent = this.transform;
        t7.transform.parent = this.transform;
        t8.transform.parent = this.transform;
        t9.transform.parent = this.transform;

        t1.transform.localPosition = new Vector3(0.0f, 0.272f, 0.054f);
        t2.transform.localPosition = new Vector3(0.0f, 0.239f, 0.153f);
        t3.transform.localPosition = new Vector3(0.0f, 0.18f, 0.231f);
        t4.transform.localPosition = new Vector3(0.0f, 0.094f, 0.295f);
        t5.transform.localPosition = new Vector3(0.0f, 0.0f, 0.325f);
        t6.transform.localPosition = new Vector3(0.0f, -0.1f, 0.325f);
        t7.transform.localPosition = new Vector3(0.0f, -0.19f, 0.293f);
        t8.transform.localPosition = new Vector3(0.0f, -0.27f, 0.24f);
        t9.transform.localPosition = new Vector3(0.0f, -0.328f, 0.157f);

        t1.transform.localRotation = Quaternion.Euler(new Vector3(18, 0, 0));
        t2.transform.localRotation = Quaternion.Euler(new Vector3(36, 0, 0));
        t3.transform.localRotation = Quaternion.Euler(new Vector3(54, 0, 0));
        t4.transform.localRotation = Quaternion.Euler(new Vector3(72, 0, 0));
        t5.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
        t6.transform.localRotation = Quaternion.Euler(new Vector3(108, 0, 0));
        t7.transform.localRotation = Quaternion.Euler(new Vector3(126, 0, 0));
        t8.transform.localRotation = Quaternion.Euler(new Vector3(144, 0, 0));
        t9.transform.localRotation = Quaternion.Euler(new Vector3(162, 0, 0));
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 p = this.transform.position;
        if (p.x > 15 || p.x < -15 || p.y > 10 || p.y < -5 || p.z > 10 || p.z < -12)
        {
            Object.Destroy(this.gameObject);
            if (damageText != null)
            {
                damageText.GetComponent<TextMesh>().text = "Miss";
                Object.Destroy(this.gameObject);
            }
        }
        else
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedProjectile : MonoBehaviour {
    public float Speed = 1.0f;
    public int Damage = 3;
    public GameObject BlockedSound;
    public GameObject HitSound;
    public Vector3 originalRotation;

    public float distanceBeforeBend;
    public bool bended;
    public GameObject self;

    public AudioClip HapticBlock;

    private OVRHapticsClip hapticClip;
    private Vector3 originalPosition;
    private bool beginToDisappear;
    private Vector3 originalScale;

    void Start()
    {
        Vector3 target = GameObject.Find("CenterEyeAnchor").transform.position;
        target.y -= 0.1f;
        //this.transform.rotation = Quaternion.LookRotation(target - this.transform.position);
        originalPosition = this.transform.position;
        beginToDisappear = false;
        originalScale = new Vector3(1.0f, 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 p = this.transform.position;
        if (p.x > 15 || p.x < -15 || p.y > 10 || p.y < -2 || p.z > 10 || p.z < -12)
        {
            Object.Destroy(this.gameObject);
        }

        float dis = Vector3.Distance(this.transform.position, originalPosition);


        if (bended == false && beginToDisappear)
        {
            Vector3 curr = this.transform.localScale;
            curr.z -= Speed * Time.deltaTime;
            if (curr.z <= 0)
            {
                Object.Destroy(this.gameObject);
            }
            this.transform.localScale = curr;
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
        else if (this.transform.localScale.z < originalScale.z)
        {
            Vector3 curr = this.transform.localScale;
            curr.z += Speed * Time.deltaTime;
            this.transform.localScale = curr;
            //transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }

        else if(dis > distanceBeforeBend && bended == false)
        {
            GameObject target = GameObject.Find("CenterEyeAnchor");
            GameObject tmp = Instantiate(self, this.transform.position + this.transform.forward, Quaternion.LookRotation(target.transform.position - (this.transform.position + this.transform.forward)));
            tmp.GetComponent<CurvedProjectile>().bended = true;
            Vector3 s = tmp.transform.localScale;
            s.z = 0.0f;
            tmp.transform.localScale = s;
            //Object.Destroy(this.gameObject);
            beginToDisappear = true;
        }
        else
        {
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject other = collider.gameObject;
        //print(other.name);
        //print(other.name);
        if (other.tag == "Shield" || other.tag == "Pointer")
        {
            if (BlockedSound != null)
                Instantiate(BlockedSound, transform.position, transform.rotation);
            hapticClip = new OVRHapticsClip(HapticBlock);
            int n = 1;
            if (other.transform.name == "LeftShield" || other.transform.parent.name == "LeftShield")
            {
                n = 0;
            }
            OVRHaptics.Channels[n].Mix(hapticClip);
            Object.Destroy(this.gameObject);
        }
        else if (other.tag == "Player")
        {
            GameObject.Find("OVRPlayerController").GetComponent<PlayerInfo>().Damage(Damage);
            if (HitSound != null)
                Instantiate(HitSound, transform.position, transform.rotation);
            Object.Destroy(this.gameObject);
        }
    }
}

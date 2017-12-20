using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagLazer : MonoBehaviour {
    public float Speed = 1.0f;
    public int Damage = 3;
    public GameObject BlockedSound;
    public GameObject HitSound;
    public Vector3 originalRotation;
    public GameObject previous;
    public GameObject next;

    public GameObject self;

    public AudioClip HapticBlock;

    private OVRHapticsClip hapticClip;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private bool fired;
    private float distanceBeforeBend;

    void Start()
    {
        Vector3 target = GameObject.Find("CenterEyeAnchor").transform.position;
        target.y -= 0.1f;
        //this.transform.rotation = Quaternion.LookRotation(target - this.transform.position);
        originalPosition = this.transform.position;
        originalScale = new Vector3(1.0f, 1.0f, 1.0f);
        fired = false;
        distanceBeforeBend = 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        if (fired == false)
        {
            Vector3 p = this.transform.position;
            if (p.x > 15 || p.x < -15 || p.y > 10 || p.y < -2 || p.z > 10 || p.z < -12)
            {
                Object.Destroy(this.gameObject);
            }

            float dis = Vector3.Distance(this.transform.position, originalPosition);

            if (this.transform.localScale.z < originalScale.z)
            {
                Vector3 curr = this.transform.localScale;
                curr.z += Speed * Time.deltaTime;
                this.transform.localScale = curr;
                //transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            }

            else if (dis > distanceBeforeBend)
            {
                GameObject target = GameObject.Find("CenterEyeAnchor");
                float dist = Vector3.Distance(target.transform.position, this.transform.position);
                //print(dist);
                Quaternion q = Quaternion.LookRotation(target.transform.position - (this.transform.position + this.transform.forward * distanceBeforeBend));
                if (dist < 3.0f)
                {
                    this.next = Instantiate(self, this.transform.position + this.transform.forward, q);
                }
                else
                {
                    Vector3 tmp = q.eulerAngles;
                    tmp.x += Random.Range(-45, 45);
                    tmp.y += Random.Range(-45, 45);
                    this.next = Instantiate(self, this.transform.position + this.transform.forward, Quaternion.Euler(tmp));
                }
                Vector3 s = this.next.transform.localScale;
                s.z = 0.0f;
                this.next.transform.localScale = s;
                this.next.GetComponent<ZigzagLazer>().previous = this.gameObject;
                //Object.Destroy(this.gameObject);
                //beginToDisappear = true;
                this.Speed = 0.0f;
                fired = true;
            }
            else
            {
                transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            }
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
            clean();
        }
        else if (other.tag == "Player")
        {
            GameObject.Find("OVRPlayerController").GetComponent<PlayerInfo>().Damage(Damage);
            if (HitSound != null)
                Instantiate(HitSound, transform.position, transform.rotation);
            clean();
        }
    }

    private void cleanPrev()
    {
        if (previous != null)
        {
            previous.GetComponent<ZigzagLazer>().cleanPrev();
        }
        Object.Destroy(this.gameObject);
    }

    private void cleanNext()
    {
        if (next != null)
        {
            next.GetComponent<ZigzagLazer>().cleanNext();
        }
        Object.Destroy(this.gameObject);
    }

    private void clean()
    {
        if (next != null)
        {
            next.GetComponent<ZigzagLazer>().cleanNext();
        }
        if (previous != null)
        {
            previous.GetComponent<ZigzagLazer>().cleanPrev();
        }
        Object.Destroy(this.gameObject);
    }
}

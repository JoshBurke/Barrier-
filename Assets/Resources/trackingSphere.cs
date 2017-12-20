using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trackingSphere : MonoBehaviour {
    public float maxSpeed = 3.0f;
    public int Damage = 3;
    public float acceleration = 0.3f;
    public float hp = 3.0f;

    public GameObject BlockedSound;
    public GameObject HitSound;

    public AudioClip HapticBlock;

    private OVRHapticsClip hapticClip;
    private float Speed;
    private GameObject target;

    private bool colliding;
    private bool decelerating;

    private Vector3 relativePosition;
    private GameObject collidingObject;
    void Start()
    {
        target = GameObject.Find("CenterEyeAnchor");
        Vector3 tmp = target.transform.position;
        tmp.y -= 0.1f;
        this.transform.rotation = Quaternion.LookRotation(tmp - this.transform.position);
        Speed = 0.0f;
        colliding = false;
        decelerating = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
        Speed += acceleration * Time.deltaTime;
        if(Speed > maxSpeed)
        {
            Speed = maxSpeed;
        }

        if (decelerating)
        {
            if(Speed > 0.0f)
            {
                this.transform.rotation = Quaternion.Inverse(this.transform.rotation);
                decelerating = false;
            }
        }
        else
        {
            Vector3 currDirection = (target.transform.position - this.transform.position).normalized;
            Vector3 currRotation = this.transform.rotation.eulerAngles;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(currDirection), Time.deltaTime * Speed);
        }
        Vector3 p = this.transform.position;
        if (p.x > 15 || p.x < -15 || p.y > 10 || p.y < -2 || p.z > 10 || p.z < -12)
        {
            Object.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject other = collider.gameObject;
        if (other.name == "ground")
        {
            Vector3 tmp = this.transform.rotation.eulerAngles;
            tmp.x = tmp.x * -1;
            this.transform.rotation = Quaternion.Euler(tmp);
        }

        else if (decelerating == false)
        {
            //print(other.name);
            //print(other.name);
            if (other.tag == "Shield" || other.tag == "Pointer")
            {
                colliding = true;
                if (BlockedSound != null)
                    Instantiate(BlockedSound, transform.position, transform.rotation);
                hapticClip = new OVRHapticsClip(HapticBlock);
                int n = 1;
                if (other.transform.name == "LeftShield" || other.transform.parent.name == "LeftShield")
                {
                    n = 0;
                }
                OVRHaptics.Channels[n].Mix(hapticClip);
                hp -= 1.0f;
                if (hp <= 0.0f)
                {
                    Object.Destroy(this.gameObject);
                }
                else
                {
                    /*
                    Vector3 newDirection = (this.transform.position - other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(this.transform.position)).normalized;
                    this.transform.rotation = Quaternion.Inverse(this.transform.rotation);
                    Speed = -1 * Speed;
                    */

                    Vector3 b = (this.transform.position - collider.gameObject.transform.position);
                    Vector3 a = collider.gameObject.transform.forward.normalized;
                    //this.transform.forward = (b - Vector3.Dot(b, a) * a).normalized;
                    this.transform.forward = (collider.transform.position + Vector3.Dot(b, a) * a - this.transform.position).normalized;
                    Speed = -2 * Speed;
                    
                    relativePosition = this.transform.position - collider.gameObject.transform.position;
                    collidingObject = collider.gameObject;
                    decelerating = true;
                }
            }
            else if (other.tag == "Player")
            {
                GameObject.Find("playerCollider").GetComponent<PlayerInfo>().Damage(Damage);
                if (HitSound != null)
                    Instantiate(HitSound, transform.position, transform.rotation);
                Object.Destroy(this.gameObject);
            }
        }
        /*
        else
        {
            this.transform.position = collidingObject.transform.position + relativePosition;
        }*/
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Shield" || other.tag == "Pointer")
        {
            colliding = false;

        }
    }
}

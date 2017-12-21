using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class CurvedComponent : MonoBehaviour
{
    public float MaxDamage = 10.0f;
    public GameObject HitSound;

    private float startTime;
    
    void Start()
    {
        /*
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
        */
    }


    void Update()
    {
        /*
        if (Time.fixedTime - startTime > MaxLifetime)
        {
            if (damageText != null)
            {
                damageText.GetComponent<TextMesh>().text = "Miss";
                Object.Destroy(this.gameObject);
            }
        }
        else
            transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            */
    }

    void OnCollisionEnter(Collision collision)
    {
        Collider collider = collision.collider;
        GameObject other = collider.gameObject;
        print(other.name);
        if (other.tag == "Crossheir")
        {
            Vector3 contactPoint = collision.contacts[0].point;
            Vector3 hitBoxSize = ((BoxCollider)collider).size;
            Vector3 hitBoxCenter = other.transform.position;
            Vector3 otherPos = other.transform.position;
            if (HitSound != null)
                Instantiate(HitSound, transform.position, transform.rotation);

            float offDist = Mathf.Pow(Mathf.Pow((contactPoint.x - otherPos.x) * 10, 2.0f) + Mathf.Pow((contactPoint.y - otherPos.y) * 10, 2.0f), (1.0f / 2.0f));
            float damage = 1.0f - (offDist / hitBoxSize.x);
            damage *= MaxDamage;
            damage = Mathf.Max(0.0f, damage);
            GameObject.FindGameObjectWithTag("Enemy").GetComponent<AILibrary>().GiveRawDamage(damage);
            Object.Destroy(this.transform.parent.gameObject);
            //GameObject.FindGameObjectWithTag("Enemy").GetComponent<AILibrary>
            GameObject[] tmp = GameObject.FindGameObjectsWithTag("Crossheir");
            for(int i = 0; i < tmp.Length; i++)
            {
                Destroy(tmp[i]);
            }
        }
    }
}

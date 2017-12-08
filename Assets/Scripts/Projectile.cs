using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public float Speed = 1.0f;
    public int Damage = 3;
    public GameObject BlockedSound;
    public GameObject HitSound;

    public AudioClip HapticBlock;

    private OVRHapticsClip hapticClip;


    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
	}

    void OnTriggerEnter(Collider collider)
    {
        GameObject other = collider.gameObject;
        if(other.tag == "Shield")
        {
            if(BlockedSound != null)
                Instantiate(BlockedSound, transform.position, transform.rotation);
            hapticClip = new OVRHapticsClip(HapticBlock);
            OVRHaptics.Channels[other.name == "LeftShield" ? 0 : 1].Mix(hapticClip);
            Object.Destroy(this.gameObject);
        }
        else if (other.tag == "Player")
        {
            other.GetComponent<PlayerInfo>().Damage(Damage);
            if(HitSound != null)
                Instantiate(HitSound, transform.position, transform.rotation);
            Object.Destroy(this.gameObject);
        }
    }
}

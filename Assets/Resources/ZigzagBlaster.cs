using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigzagBlaster : MonoBehaviour {
    public GameObject ProjectileToShoot;
    public GameObject FireSound;
    public GameObject Muzzle;

    private bool fired;
    private Transform muzzleTrans;

    private GameObject projectile;
    public int numOfAttack;
	// Use this for initialization
	void Start () {
        fired = false;
        muzzleTrans = Muzzle.transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (numOfAttack > 0)
        {
            if (projectile == null)
            {
                numOfAttack--;
                GameObject tmp = Instantiate(ProjectileToShoot, muzzleTrans.position, muzzleTrans.rotation);
                Vector3 f = this.transform.rotation.eulerAngles;
                //print(f);
                f.y = 180 + Random.Range(-45, 45);
                f.x = Random.Range(-45, 45);
                tmp.transform.rotation = Quaternion.Euler(f);
                projectile = tmp;
            }
        }
        else
        {
            if (projectile == null)
            {
                GameObject.Destroy(this.gameObject);
            }
        }
	}
}

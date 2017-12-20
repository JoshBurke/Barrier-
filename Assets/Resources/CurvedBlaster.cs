using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedBlaster : MonoBehaviour {
    public GameObject ProjectileToShoot;
    public float ShotDelay = 1.0f;
    public GameObject FireSound;
    public GameObject Muzzle;
    public int mode;

    private Transform muzzleTrans;

    public float TimeAlive = 3.0f;

    private float lastFireTime;
    public float spawnTime;

    private GameObject target;
    private float timeCount;
    void Start()
    {
        muzzleTrans = Muzzle.transform;
        /*initPos = transform.position;
        initRotEuler = transform.rotation.eulerAngles;
        initRot = transform.rotation;
        finalRot = Quaternion.Euler(initRotEuler.x + SweepPitch, initRotEuler.y + SweepYaw, initRotEuler.z);
        finalRotEuler = finalRot.eulerAngles;
        lastFireTime = spawnTime = Time.fixedTime;*/
        target = GameObject.Find("ProjectileSpawner");
        transform.rotation = Quaternion.LookRotation(target.transform.position - muzzleTrans.position);
        timeCount = 0.0f;
    }

    void FixedUpdate()
    {
        timeCount += Time.deltaTime;
        if(timeCount > spawnTime)
        {
            GameObject.Destroy(this.gameObject);
        }
        if (canFire())
        {
            if (mode == 1)
            {
                FireRandom();
            }
            else if(mode == 2)
            {
                FireCircle();
            }
            else
            {
                Fire();
            }
        }
    }

    private bool canFire()
    {
        return Time.fixedTime - lastFireTime >= ShotDelay;
    }

    private void Fire()
    {
        Instantiate(FireSound, muzzleTrans.position, muzzleTrans.rotation);
        GameObject tmp = Instantiate(ProjectileToShoot, muzzleTrans.position, muzzleTrans.rotation);
        Vector3 f = this.transform.rotation.eulerAngles;
        f.y -= 30;
        tmp.GetComponent<CurvedProjectile>().transform.rotation = Quaternion.Euler(f);
        tmp.GetComponent<CurvedProjectile>().bended = false;
        lastFireTime = Time.fixedTime;
    }

    private void FireRandom()
    {
        Instantiate(FireSound, muzzleTrans.position, muzzleTrans.rotation);
        GameObject tmp = Instantiate(ProjectileToShoot, muzzleTrans.position, muzzleTrans.rotation);
        Vector3 f = this.transform.rotation.eulerAngles;
        //print(f);
        f.y = 180 + Random.Range(-45, 45);
        f.x = Random.Range(-45, 45);
        tmp.GetComponent<CurvedProjectile>().transform.rotation = Quaternion.Euler(f);
        tmp.GetComponent<CurvedProjectile>().bended = false;
        lastFireTime = Time.fixedTime;
    }

    private int currX = 60;
    private int currY = 0;

    private int accX = 10;
    private int accY = 10;
    private void FireCircle()
    {
        Instantiate(FireSound, muzzleTrans.position, muzzleTrans.rotation);
        GameObject tmp = Instantiate(ProjectileToShoot, muzzleTrans.position, muzzleTrans.rotation);
        Vector3 f = this.transform.rotation.eulerAngles;
        
        if(currX == 60 || currX == -60)
        {
            accX = -1 * accX;
        }
        if (currY == 60 || currY == -60)
        {
            accY = -1 * accY;
        }

        currX += accX;
        currY += accY;

        f.x = 180 + currX;
        f.y = currY;

        tmp.GetComponent<CurvedProjectile>().transform.rotation = Quaternion.Euler(f);
        tmp.GetComponent<CurvedProjectile>().bended = false;
        lastFireTime = Time.fixedTime;
    }
}

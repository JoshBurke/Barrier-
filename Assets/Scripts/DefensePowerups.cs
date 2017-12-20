using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefensePowerups : MonoBehaviour {

    /** Variables essential for script generic function **/

    //public PlayerInfo playerInfo;
    public GameObject rightShield;

    public enum PowerUp { NONE, MAGNET, BOOM, SLOW, SHIELD}

    public PowerUp currPowerUp = PowerUp.NONE;

    public bool MagnetAvailable;
    public bool BoomAvailable;
    public bool SlowAvailable;
    public bool ShieldAvailable;

    /** End essential variables **/

    public RawImage screenTint;
    public AudioSource music;
    public Transform explosionTransform;
    public GameObject explosion;

    private bool areProjectilesReset;
    private ParticleSystem particles;
    private bool timeLerping = false;
    private bool pitchLerping = false;

    private void Start()
    {
        if (!rightShield)
            rightShield = GameObject.Find("RightShield");
        //if (!playerInfo)
        //    playerInfo = GameObject.Find("OVRPlayerController").GetComponent<PlayerInfo>();
        areProjectilesReset = true;
        particles = rightShield.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        
        switch (currPowerUp){
            case PowerUp.NONE:
                break;
            case PowerUp.MAGNET:
                if (OVRInput.Get(OVRInput.RawButton.A) && MagnetAvailable){
                    Magnetize();
                } else if (!areProjectilesReset)
                {
                    ResetProjectiles();
                }
                break;
            case PowerUp.BOOM:
                if (OVRInput.Get(OVRInput.RawButton.A) && BoomAvailable)
                {
                    Boom();
                }
                break;
            case PowerUp.SLOW:
                if (OVRInput.Get(OVRInput.RawButton.A) && SlowAvailable)
                {
                    Slow();
                } else
                {
                    screenTint.enabled = false;
                    if (Time.timeScale != 1.0f && !timeLerping)
                    {
                        StartCoroutine(LerpTime(1.0f));
                    }
                    if (music.pitch != 1.0f && !pitchLerping)
                    {
                        StartCoroutine(LerpPitch(1.0f));
                    }
                }
                break;
            case PowerUp.SHIELD:
                if (OVRInput.Get(OVRInput.RawButton.A) && ShieldAvailable)
                {
                    Shield();
                }
                break;
        }

        CheckNormalConditions();
    }

    void CheckNormalConditions()
    {
        if (Time.timeScale != 1.0f && !timeLerping && currPowerUp != PowerUp.SLOW)
        {
            StartCoroutine(LerpTime(1.0f));
            screenTint.enabled = false;
        }
        if (music.pitch != 1.0f && !pitchLerping && currPowerUp != PowerUp.SLOW)
        {
            StartCoroutine(LerpPitch(1.0f));
        }
        if (rightShield.GetComponent<Shield>().getIsMagnet() && currPowerUp != PowerUp.MAGNET)
        {
            ResetProjectiles();
        }
    }

    void Magnetize()
    {
        rightShield.GetComponent<Shield>().Magnetize();
        particles.Play();
        GameObject[] projs = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject p in projs)
        {
            p.transform.LookAt(rightShield.transform);
        }
        areProjectilesReset = false;
    }

    void Boom()
    {
        GameObject[] projs = GameObject.FindGameObjectsWithTag("Projectile");
        if (projs.Length == 0) return;
        GameObject boom = Instantiate(explosion, explosionTransform);
        foreach(GameObject p in projs)
        {
            Destroy(p);
        }
        BoomAvailable = false;
    }

    void Slow()
    {
        if (Time.timeScale != 0.5f && !timeLerping)
        {
            StartCoroutine(LerpTime(0.5f));
        }
        if (music.pitch != 0.5f && !pitchLerping)
        {
            StartCoroutine(LerpPitch(0.5f));
        }
        screenTint.enabled = true;
    }

    IEnumerator LerpTime(float timeScale)
    {
        timeLerping = true;
        float startTime = Time.time;
        float startScale = Time.timeScale;
        while (!(timeScale - 0.001f < Time.timeScale && Time.timeScale < timeScale + 0.001f))
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, timeScale, 10f*Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        Time.timeScale = timeScale;
        timeLerping = false;
    }

    IEnumerator LerpPitch(float pitch)
    {
        pitchLerping = true;
        float startTime = Time.time;
        float startPitch = Time.timeScale;
        while (!(pitch - 0.001f < music.pitch && music.pitch < pitch + 0.001f))
        {
            music.pitch = Mathf.Lerp(music.pitch, pitch, 10f*Time.deltaTime);
            yield return new WaitForSeconds(0.01f);
        }
        music.pitch = pitch;
        pitchLerping = false;

    }

    void Shield()
    {
        //Add shield
    }

    //This method resets projectiles to their original heading (toward the player)
    void ResetProjectiles()
    {
        rightShield.GetComponent<Shield>().DeMagnetize();
        particles.Stop();
        GameObject[] projs = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject p in projs)
        {
            p.transform.LookAt(transform.position + Vector3.up * 0.5f);
        }
        areProjectilesReset = true;
    }
}

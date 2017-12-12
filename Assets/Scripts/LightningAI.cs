using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Copied from UndyneAI.cs
public class LightningAI : MonoBehaviour
{
    public delegate void ProjectileFireEvent();

    public AILibrary lib;
    private Spawner spawner;
    private GameObject basicLaser;
    private GameObject laser;
    private GameObject fakeLaser;
    private GameObject fastLaser;
    private GameObject lightningBlaster;
    private GameObject basicCanon;
    private GameObject canon;
    private GameObject playerSword;
    private int prevPlayerHealth;
    private int turnCount;
    private bool attackActive;
    private bool enemyDead;
    private float lastTimeMoved;
    private float lastPos;

    private int totalKillCount;

    private PlayerInfo playerInfo;

    // Use this for initialization
    void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        laser = (GameObject)Resources.Load("LightningLaser");
        fakeLaser = (GameObject)Resources.Load("FakeLaser");
        fastLaser = (GameObject)Resources.Load("FastLaser");
        lightningBlaster = (GameObject)Resources.Load("LightningBlaster");
        basicCanon = (GameObject)Resources.Load("CanonBlaster");
        canon = (GameObject)Resources.Load("UndyneCanonBlaster");
        playerSword = GameObject.Find("PlayerSword");
        if(playerSword != null)
            playerSword.SetActive(false);

        playerInfo = GameObject.Find("playerCollider").GetComponent<PlayerInfo>();
        playerInfo.MaxHealth = 50;
        playerInfo.ResetHealth();
        lib.RegisterCrossheirPositionFunc(CrossheirPosition);
        lib.SetVitals(180.0f);
        lib.SetLightning();
        attackActive = false;
        enemyDead = false;
        turnCount = 0;
        lastTimeMoved = 0f;
        lastPos = 0f;
    }

    public void RegisterTotalKills(int kills)
    {
        totalKillCount = kills;
        Invoke("begin", 0.5f);
    }

    private void begin()
    {
        lib.PlayMusic((AudioClip)Resources.Load("Highscore"));
        lib.AddTextToQueue("So, you've come to test your speed, eh?");
        lib.AddTextToQueue("Sorry to tell you, but you're facing Electrus, the fastest man alive!");
        lib.AddTextToQueue("There's no way you can keep up with me!");
        lib.AddTextToQueue("Oh, that target? I don't like waiting for people to take turns.");
        lib.AddTextToQueue("If you want to hit me, you'll have to do it while defending against my attacks!");
        lib.AddTextToQueue("But I'll tell you what -- I'll go easy on you... for now.", attackSelect);
    }

    private void playerTurn()
    {
        //lib.solidifyEnemy(); they're already solid
        if(!attackActive && !enemyDead)
        {
            attackActive = true;
            lib.lightningAttack(enableNewAttack);
        }
    }

    private void enableNewAttack()
    {
        attackActive = false;
        lethalCheck();
    }

    private void attackSelect()
    {
        turnCount++;
        // this check prevents the AI from attacking even when they've already died
        if(lib.GetAIHealth() != 0.0f)
        {
            lib.setSpeechBubbleActive(true);
            switch (turnCount)
            {
                case 1:
                    startMove();
                    break;
                case 2:
                    secondMove();
                    break;
                case 3:
                    thirdMove();
                    break;
                case 4:
                    fourthMove();
                    break;
                case 5:
                    fifthMove();
                    break;
                default:
                    getRandomChat();
                    break;
            }
        }
    }

    private void startMove()
    {
        lib.AddTextToQueue("Here's something nice and slow.", startAttack());
    }

    private AILibrary.OnSpeechFinishedEvent startAttack()
    {
        StartCoroutine(disableSpeechBubble());
        return easyBulletHell;
    }

    private void secondMove()
    {
        lib.AddTextToQueue("So you already know how to block, huh?");
        lib.AddTextToQueue("Since you'll need the help, let's try something new.");
        lib.AddTextToQueue("THESE lasers will heal you -- but not if you block them!", secondAttack());
    }

    private AILibrary.OnSpeechFinishedEvent secondAttack()
    {
        StartCoroutine(disableSpeechBubble());
        return onlyFakes;
    }

    private void thirdMove()
    {
        lib.AddTextToQueue("Getting the hang of it?");
        lib.AddTextToQueue("Don't get ahead of yourself -- how about BOTH at once?", thirdAttack());
    }

    private AILibrary.OnSpeechFinishedEvent thirdAttack()
    {
        StartCoroutine(disableSpeechBubble());
        return easyBulletHellFakes;
    }

    private void fourthMove()
    {
        lib.AddTextToQueue("Still keeping up?");
        lib.AddTextToQueue("Then let's try something a little FASTER!", fourthAttack());
    }

    private AILibrary.OnSpeechFinishedEvent fourthAttack()
    {
        StartCoroutine(disableSpeechBubble());
        return randomBlasters;
    }

    private void fifthMove()
    {
        lib.AddTextToQueue("Hah.. huh.. I may have underestimated you.");
        lib.AddTextToQueue("You're faster than I thought...");
        lib.AddTextToQueue("Guess I'll stop holding back.");
        lib.AddTextToQueue("TIME TO GO");
        lib.AddTextToQueue("EVEN");
        lib.AddTextToQueue("FASTER!", getRandomHardAttack());
    }

    private void getRandomChat()
    {
        int choice = (int)(Random.value * 4);
        switch (choice)
        {
            case 0:
                lib.AddTextToQueue("You're gonna have to be faster than that!", getRandomHardAttack());
                break;
            case 1:
                lib.AddTextToQueue("There's no way you'll block this!", getRandomHardAttack());
                break;
            case 2:
                lib.AddTextToQueue("No one is faster than Electrus!", getRandomHardAttack());
                break;
            default:
                lib.AddTextToQueue("You wanna see FAST?");
                lib.AddTextToQueue("I'll show you fast!");
                lib.AddTextToQueue("I'M THE FASTEST MAN ALIVE!", insaneAttack());
                break;
        }
    }

    private AILibrary.OnSpeechFinishedEvent insaneAttack()
    {
        StartCoroutine(disableSpeechBubble());
        return insaneBulletHell;
    }

    private void insaneBulletHell()
    {
        float delay = 0.1363636f;
        int numProjectiles = 60;
        float attackLength = delay * numProjectiles;
        for (float i = 0; i < attackLength; i += delay)
        {
            StartCoroutine(spawnProjectiles("insaneBulletHell_spawn", i));
        }
        StartCoroutine(waitOnProjectiles(attackLength));
    }
    private AILibrary.OnSpeechFinishedEvent getRandomHardAttack()
    {
        StartCoroutine(disableSpeechBubble());
        int choice = (int)(Random.value * 5);
        switch (choice)
        {
            case 0:
                return hardBulletHell;
            case 1:
                return hardBulletHellFakes;
            case 2:
                return speedBullets;
            case 3:
                return zigZagHell;
            default:
                return miscFakes;
        }
    }

    private void speedBullets()
    {
        float delay = 0.272727f;
        int numProjectiles = 20;
        float attackLength = delay * numProjectiles;
        for(float i = 0; i < attackLength; i += delay)
        {
            StartCoroutine(spawnProjectiles("speed_spawn", i));
        }
        StartCoroutine(waitOnProjectiles(attackLength));
    }
    private void zigZagHell()
    {
        float delay = 0.06818181f;
        int numBeams = 9;
        int projectilesPerBeam = 8;
        float attackLength = delay * numBeams;
        float x1 = (int)(Random.value * 30) + 5;
        float y1 = (int)(Random.value * 30) - 30;
        float x2 = (int)(Random.value * 30) + 5;
        float y2 = (int)(Random.value * 30);
        List<float> dx = new List<float> { -4.0f, 2.4f, 2.4f, -4.0f, 5.0f, -4.0f, 2.4f, 2.4f, -4.0f };
        List<float> dy = new List<float> { 4.0f, -5.0f, 5.0f, -4.0f, 0.0f, 4.0f, -5.0f, 5.0f, -4.0f };
        for (int beam = 0; beam < numBeams; beam++)
        {
            for (int i = 0; i < projectilesPerBeam; i++)
            {
                StartCoroutine(spawnProjectileAtXY(beam * projectilesPerBeam * delay + i * delay, x1, y1));
                x1 += dx[beam];
                y1 += dy[beam];
                StartCoroutine(spawnProjectileAtXY(beam * projectilesPerBeam * delay + i * delay, x2, y2));
                x2 += dx[beam];
                y2 -= dy[beam];
            }
        }
        StartCoroutine(waitOnProjectiles(attackLength));
    }

    private void onlyFakes()
    {
        float delay = 0.545454f;
        int numProjectiles = 24;
        float attackLength = delay * numProjectiles;
        for (float i = 0; i < attackLength; i += delay)
        {
            StartCoroutine(spawnProjectiles("fakeLaserEasy_spawn", i));
        }
        StartCoroutine(waitOnProjectiles(attackLength));
    }

    private void randomBlasters()
    {
        int numBlasters = (int)(Random.value * 3) + 7;
        float attackLength = numBlasters * 0.8f;
        for (int i = 0; i < numBlasters; i++)
        {
            int xpos = (int)(Random.value * 25) + 15;
            int ypos = (int)(Random.value * 40) - 10;
            StartCoroutine(spawnBlaster(xpos, ypos, 0.8f * i));
        }
        StartCoroutine(waitOnProjectiles(attackLength));
    }
    private void easyBulletHell()
    {
        float delay = 0.545454f;
        int numProjectiles = 24;
        float attackLength = delay * numProjectiles;
        for (float i = 0; i < attackLength; i += delay)
        {
            StartCoroutine(spawnProjectiles("easyBulletHell_spawn", i));
        }
        StartCoroutine(waitOnProjectiles(attackLength));
    }

    private void easyBulletHellFakes()
    {
        float delay = 0.5454545f;
        int numProjectiles = 20;
        float attackLength = numProjectiles * delay;
        for (float i = 0; i < attackLength; i += delay)
        {
            float rval = Random.value;
            if (rval < .25f)
            {
                StartCoroutine(spawnProjectiles("fakeLaserEasy_spawn", i));
            }
            StartCoroutine(spawnProjectiles("easyBulletHell_spawn", i));
        }
        StartCoroutine(waitOnProjectiles(attackLength));
    }
    private void hardBulletHell()
    {
        float delay = 0.272727f;
        int numProjectiles = 40;
        float attackLength = delay * numProjectiles;
        for (float i = 0; i < attackLength; i += delay)
        {
            StartCoroutine(spawnProjectiles("hardBulletHell_spawn", i));
        }
        StartCoroutine(waitOnProjectiles(attackLength));
    }

    private void hardBulletHellFakes()
    {
        float delay = 0.272727f;
        int numProjectiles = 30;
        float attackLength = delay * numProjectiles;
        for (float i = 0; i < attackLength; i += delay)
        {
            float rval = Random.value;
            if (rval < .25f)
            {
                StartCoroutine(spawnProjectiles("fakeLaserHard_spawn", i));
            }
            StartCoroutine(spawnProjectiles("hardBulletHell_spawn", i));
        }
        StartCoroutine(waitOnProjectiles(attackLength));
    }

    private void zigZagBlaster()
    {
        float delay = 0.1f;
        float attackLength = 3.5f;
        float xvar = 1.0f;
        float yvar = 3.0f;
        float x1 = ((int)(Random.value * 10) + 20);
        float x2 = ((int)(Random.value * 10) + 20);
        float y1 = ((int)(Random.value * -30) - 10);
        float y2 = ((int)(Random.value * 30) + 10);
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(spawnProjectileAtXY(delay * i, x1, y1));
            x1 -= xvar;
            y1 += yvar;
            StartCoroutine(spawnProjectileAtXY(delay * i, x2, y2));
            x2 -= xvar;
            y2 -= yvar;
        }
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(spawnProjectileAtXY(delay * i + 0.5f, x1, y1));
            x1 -= xvar;
            y1 -= yvar;
            StartCoroutine(spawnProjectileAtXY(delay * i + 0.5f, x2, y2));
            x2 -= xvar;
            y2 += yvar;
        }
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(spawnProjectileAtXY(delay * i + 1.0f, x1, y1));
            x1 -= xvar;
            y1 += yvar;
            StartCoroutine(spawnProjectileAtXY(delay * i + 1.0f, x2, y2));
            x2 -= xvar;
            y2 -= yvar;
        }
        for (int i = 0; i < 5; i++)
        {
            StartCoroutine(spawnProjectileAtXY(delay * i + 1.5f, x1, y1));
            x1 -= xvar;
            y1 -= yvar;
            StartCoroutine(spawnProjectileAtXY(delay * i + 1.5f, x2, y2));
            x2 -= xvar;
            y2 += yvar;
        }
        float x_avg = (x1 + x2) / 2.0f;
        StartCoroutine(spawnBlaster(x_avg, 0, 2.5f));
        StartCoroutine(spawnBlaster(x_avg + 14, 0, 3.0f));
        StartCoroutine(spawnBlaster(x_avg + 28, 0, 3.5f));
        StartCoroutine(spawnBlaster(x_avg + 42, 0, 4.0f));
        StartCoroutine(waitOnProjectiles(attackLength));
    }

    private IEnumerator spawnBlaster(float x, float y, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        spawner.SpawnProjectileAtAngle(lightningBlaster, x, y, 0.8f);
    }

    private void miscFakes()
    {
        float delay = 0.545454f;
        int numProjectiles = 36;
        float attackLength = delay * numProjectiles;
        for (float i = 0; i < attackLength; i += delay)
        {
            float val = Random.value;
            int max = (int)(.5 + val);
            for (int j = 0; j < max; j++)
            {
                StartCoroutine(spawnProjectiles("fakeLaserHard_spawn", i));
            }
            StartCoroutine(spawnProjectiles("hardBulletHell_spawn", i));
        }
        StartCoroutine(waitOnProjectiles(attackLength));
    }

    private IEnumerator spawnProjectileAtXY(float waitTime, float x, float y)
    {
        yield return new WaitForSeconds(waitTime);
        spawner.SpawnProjectileAtAngle(laser, x, y, 1.0f);
    }
    private IEnumerator spawnProjectiles(string callbackName, float delay)
    {
        yield return new WaitForSeconds(delay);
        if(!enemyDead)
        {
            Invoke(callbackName, 0.0f);
        }
    }
    private IEnumerator waitOnProjectiles(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        lib.WaitForProjectiles(attackSelect);
    }

    private IEnumerator disableSpeechBubble()
    {
        yield return new WaitForSeconds(0.25f);
        lib.setSpeechBubbleActive(false);
    }

    private void speed_spawn()
    {
        int negy = Random.value > 0.5f ? 1 : -1;
        float y = ((int)(Random.value * 25.0f) + 8);
        float x = ((int)(Random.value * 35.0f) - 10);
        spawner.SpawnProjectileAtAngle(fastLaser, x, negy * y, 1.0f);
    }

    private void fakeLaserEasy_spawn()
    {
        float y = ((int)(Random.value * 40.0f) - 20);
        float x = ((int)(Random.value * 30.0f) - 10);
        spawner.SpawnProjectileAtAngle(fakeLaser, x, y, 1.0f);
    }
    private void fakeLaserHard_spawn()
    {
        float y = ((int)(Random.value * 60.0f) - 30);
        float x = ((int)(Random.value * 50.0f) - 15);
        spawner.SpawnFakeProjectileAtAngle(fakeLaser, x, y, 1.0f);
    }
    private void easyBulletHell_spawn()
    {
        float y = ((int)(Random.value * 40.0f) - 20);
        float x = ((int)(Random.value * 30.0f) - 10);
        spawner.SpawnProjectileAtAngle(laser, x, y, 1.0f);
    }
    
    private void hardBulletHell_spawn()
    {
        float y = ((int)(Random.value * 60.0f) - 30);
        float x = ((int)(Random.value * 50.0f) - 15);
        spawner.SpawnProjectileAtAngle(laser, x, y, 1.0f);
    }

    private void insaneBulletHell_spawn()
    {
        int negy = Random.value > 0.5f ? 1 : -1;
        float y = ((int)(Random.value * 25.0f) + 8);
        float x = ((int)(Random.value * 35.0f) - 10);
        spawner.SpawnProjectileAtAngle(laser, x, negy * y, 1.0f);
    }
    public float CrossheirPosition(float time)
    {
        /*if (Time.fixedTime >= lastTimeMoved + 2.0f)
        {
            lastPos = Random.value * 1.5f - 0.75f;
            lastTimeMoved = Time.fixedTime;
        }
        return lastPos;
        */
        return 2.5f - 1.0f * time;
    }
    private void die()
    {
        //playerInfo.MaxHealth = 200; unnecessary atm
        playerInfo.ResetHealth();
        lib.Die();
    }
    private void lethalCheck()
    {
        if(enemyDead)
        {
            return;
        }
        if (lib.GetAIHealth() == 0.0f)
        {
            enemyDead = true;
            lib.DisallowFire();
            lib.DestroyCrossheir();
            lib.DestroyProjectiles();
            lib.StopMusic();
            lib.AddTextToQueue("...");
            lib.AddTextToQueue("I ...");
            lib.AddTextToQueue("I can't believe it.");
            lib.AddTextToQueue("I thought I was ...");
            lib.AddTextToQueue("the fastest.", die);
        }
        // no support for "mercy" currently
    }
    private void theChoiceAttack()
    {
        lib.PlayerAttack(lethalCheck, true, mercy);
    }
    private void mercy()
    {
        lib.StopMusic();
        lib.AddTextToQueue("Hmph, giving up so soon?", spare);
    }
    private void spare()
    {
        lib.Spare();
    }
    /*public void setLastAttackActualTime(float attackTime)
    {
        lastAttackActualTime = attackTime;
    }*/
    // Update is called once per frame
    void Update()
    {
        if (!enemyDead)
        {
            lethalCheck();
        }
    }
}

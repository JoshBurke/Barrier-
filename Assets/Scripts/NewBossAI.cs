using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBossAI : MonoBehaviour
{
    public AILibrary lib;
    private Spawner spawner;
    private GameObject basicLaser;
    private GameObject gasterBlaster;
    private GameObject canon;
	private GameObject megaCanon;
    private GameObject megaLaser;
    private newPlayerInfo playerInfo;
    private int laserFailCount;
    private int prevPlayerHealth;
    private int turnCount;
    private float shuffleAttackDelay;
    public bool isfreeze;
    //private int MuteCount;

    private delegate void OnCutoutFinishedEvent();
    private bool overrideMusicHandleNextCutin;
    private OnCutoutFinishedEvent onCutFinishedCallback;
    SpriteRenderer sr;

    void Start()
    {
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        basicLaser = (GameObject)Resources.Load("BasicLaser");
        gasterBlaster = (GameObject)Resources.Load("GasterBlaster");
        canon = (GameObject)Resources.Load("CanonBlaster");
		megaCanon = (GameObject)Resources.Load("MegaCanonBlaster");

        sr = GetComponentInChildren<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("sans_color");

        playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<newPlayerInfo>();
        laserFailCount = 0;
        lib.RegisterCrossheirPositionFunc(CrossheirPosition);
        lib.SetVitals(180.0f);
        //lib.SetVitals(11.0f);
        turnCount = 0;
        Invoke("intro", 0.1f);
        //MuteCount = 1;
        isfreeze = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (OVRInput.Get(OVRInput.Button.Four) && MuteCount > 0)
        {
            //Debug.Log("HERE");
            Instantiate(Resources.Load("Die"));
            foreach (GameObject gameObj in GameObject.FindGameObjectsWithTag("Projectile"))
            {
                Destroy(gameObj);
            }
            MuteCount--;
        }
        //Debug.Log(MuteCount);
        */
    }


    private void cutout(float outTime, OnCutoutFinishedEvent finishedCallback, bool overrideMusicHandle = false)
    {
        overrideMusicHandleNextCutin = overrideMusicHandle;
        if (!overrideMusicHandle)
        {
            lib.PauseMusic();

        }
        //Instantiate(CutInSound, lib.GetPlayerPos(), Quaternion.identity);
        lib.DestroyProjectiles();
        lib.BlindPlayer(true);
        onCutFinishedCallback = finishedCallback;
        Invoke("cutoutEnd", outTime);
    }

    private void cutoutEnd()
    {
        //Instantiate(CutOutSound, lib.GetPlayerPos(), Quaternion.identity);
        lib.BlindPlayer(false);
        if (!overrideMusicHandleNextCutin)
        {
            lib.ResumeMusic();
        }
        overrideMusicHandleNextCutin = false;
        onCutFinishedCallback();
    }

    public float CrossheirPosition(float time)
    {
        //return 4.0f - (time * 1.5f);
		return 4.0f - (time * 3.5f);
    }

    public float FinishCrossheirs(float time)
    {
        return 0.0f;
    }

    private void die()
    {
        playerInfo.MaxHealth = 120;
        playerInfo.ResetHealth();
		SceneManager.LoadScene(0);
        lib.Die();
    }

    private void spare()
    {
        //lib.Spare();
		GameObject c = spawner.SpawnProjectileAtAngle(megaCanon, 40, -40, 1.0f);  // Down
		c.GetComponent<CanonSeries>().Init(-40, 0, 1.0f, 1.0f, 0.05f);
		c = spawner.SpawnProjectileAtAngle(megaCanon, 0, 40, 1.0f);  // Up
		c.GetComponent<CanonSeries>().Init(40, 0, 1.0f, 1.0f, 0.05f);
		lib.WaitForProjectiles(playerTurn);
    }

    private void lethalCheck()
    {
        if (lib.GetAIHealth() == 0.0f)
        {
            lib.StopMusic();
            lib.AddTextToQueue("...");
            lib.AddTextToQueue(".....");
            lib.AddTextToQueue(".......");
            lib.AddTextToQueue(".........", die);
        }
        else
            theChoiceAttack();
    }

    private void mercy()
    {
        lib.StopMusic();
        lib.AddTextToQueue(" ... ", spare);
    }
	/*
    private void theChoice()
    {
        lib.PlayMusic((AudioClip)Resources.Load("The Choice"));
        lib.RegisterCrossheirPositionFunc(FinishCrossheirs);
        theChoiceAttack();
    }
    */
    private void theChoiceAttack()
    {
        lib.PlayerAttack(lethalCheck, true, mercy);
    }
	/*
    private void finalDialogueDone()
    {
        lib.WaitForMusicFade(theChoice);
    }
    */

    private void finishHim()
    {
        lib.FadeoutMusic(0.0f, 6.0f);
		lib.RegisterCrossheirPositionFunc(FinishCrossheirs);
		lib.AddTextToQueue ("...");
		lib.AddTextToQueue ("H....");
		lib.AddTextToQueue ("HELP....ME", theChoiceAttack);
    }

    private void fastSplitSingle()
    {
        lib.fadeEnemy();
        GameObject c = spawner.SpawnProjectileAtAngle(canon, 40, -30, 1.0f);
        c.GetComponent<CanonSeries>().Init(-40, 60, 1, 2, 0.2f);
        c = spawner.SpawnProjectileAtAngle(canon, 0, -30, 1.0f);
        c.GetComponent<CanonSeries>().Init(0, 60, 1, 2, 0.2f);
        lib.WaitForProjectiles(fastSplitSingle_2);
    }
    private void fastSplitSingle_2()
    {
        spawner.SpawnProjectileAtAngle(gasterBlaster, 20, 0, 1.0f);
        lib.WaitForProjectiles(playerTurn);
    }

    private void fastDownXDouble()
    {
        lib.fadeEnemy();
        GameObject c = spawner.SpawnProjectileAtAngle(canon, 40, -30, 1.0f);
        c.GetComponent<CanonSeries>().Init(-40, 60, 1, 2, 0.39f);
        c = spawner.SpawnProjectileAtAngle(canon, 40, 30, 1.0f);
        c.GetComponent<CanonSeries>().Init(-40, -60, 1, 2, 0.39f);
        lib.WaitForProjectiles(fastDownXDouble_2);
    }
    private void fastDownXDouble_2()
    {
        lib.fadeEnemy();
        GameObject c = spawner.SpawnProjectileAtAngle(canon, 0, -30, 1.0f);
        c.GetComponent<CanonSeries>().Init(40, 60, 1, 2, 0.39f);
        c = spawner.SpawnProjectileAtAngle(canon, 0, 30, 1.0f);
        c.GetComponent<CanonSeries>().Init(40, -60, 1, 2, 0.39f);
        lib.WaitForProjectiles(playerTurn);
    }

    private void downUpDouble()
    {
        lib.fadeEnemy();
        GameObject c = spawner.SpawnProjectileAtAngle(canon, 0, -20, 1.0f);
        c.GetComponent<CanonSeries>().Init(40, 0, 1, 4, 0.8f);
        c = spawner.SpawnProjectileAtAngle(canon, 40, 20, 1.0f);
        c.GetComponent<CanonSeries>().Init(-40, 0, 1, 4, 0.8f);
        lib.WaitForProjectiles(downUpDouble_2);
    }
    private void downUpDouble_2()
    {
        GameObject c = spawner.SpawnProjectileAtAngle(canon, 40, -20, 1.0f);
        c.GetComponent<CanonSeries>().Init(-40, 0, 1, 4, 0.8f);
        c = spawner.SpawnProjectileAtAngle(canon, 0, 20, 1.0f);
        c.GetComponent<CanonSeries>().Init(40, 0, 1, 4, 0.8f);
        lib.WaitForProjectiles(playerTurn);
    }

    private void returnToPlayerTurn()
    {
        lib.WaitForProjectiles(playerTurn);
    }

    private void shuffleAttack()
    {
        lib.fadeEnemy();
        shuffleSpawnLL();
        Invoke("shuffleSpawnUR", shuffleAttackDelay * 1.0f);
        Invoke("shuffleSpawnUL", shuffleAttackDelay * 2.0f);
        Invoke("shuffleSpawnLR", shuffleAttackDelay * 3.0f);
        Invoke("shuffleSpawnLL", shuffleAttackDelay * 4.0f);
        Invoke("shuffleSpawnUR", shuffleAttackDelay * 5.0f);
        Invoke("shuffleSpawnUL", shuffleAttackDelay * 6.0f);
        Invoke("shuffleSpawnLR", shuffleAttackDelay * 7.0f);
        Invoke("shuffleSpawnLL", shuffleAttackDelay * 8.0f);
        Invoke("shuffleSpawnUR", shuffleAttackDelay * 9.0f);
        Invoke("shuffleSpawnUL", shuffleAttackDelay * 10.0f);
        Invoke("shuffleSpawnLR", shuffleAttackDelay * 11.0f);
        Invoke("shuffleSpawnLL", shuffleAttackDelay * 12.0f);
        Invoke("returnToPlayerTurn", shuffleAttackDelay * 2.0f);
    }
    private void shuffleSpawnLL()
    {
        spawner.SpawnProjectileAtAngle(basicLaser, -10, -20, 1.0f);
    }
    private void shuffleSpawnLR()
    {
        spawner.SpawnProjectileAtAngle(basicLaser, -10, 20, 1.0f);
    }
    private void shuffleSpawnUL()
    {
        spawner.SpawnProjectileAtAngle(basicLaser, 35, -20, 1.0f);
    }
    private void shuffleSpawnUR()
    {
        spawner.SpawnProjectileAtAngle(basicLaser, 35, 20, 1.0f);
    }

    private void slowXDouble()
    {
        lib.fadeEnemy();
        GameObject c = spawner.SpawnProjectileAtAngle(canon, 0, -20, 1.0f);
        c.GetComponent<CanonSeries>().Init(40, 40, 1, 4, 0.8f);
        c = spawner.SpawnProjectileAtAngle(canon, 0, 20, 1.0f);
        c.GetComponent<CanonSeries>().Init(40, -40, 1, 4, 0.8f);
        lib.WaitForProjectiles(playerTurn);
    }

    private void horizontalDouble()
    {
        lib.fadeEnemy();
        GameObject c = spawner.SpawnProjectileAtAngle(canon, 0, -30, 1.0f);
        c.GetComponent<CanonSeries>().Init(0, 60, 1, 5, 0.8f);
        c = spawner.SpawnProjectileAtAngle(canon, 0, 30, 1.0f);
        c.GetComponent<CanonSeries>().Init(0, -60, 1, 5, 0.8f);
        lib.WaitForProjectiles(playerTurn);
    }

    private void slowPlusSingle()
    {
        lib.fadeEnemy();
        GameObject c = spawner.SpawnProjectileAtAngle(canon, 0, 0, 1.0f);
        c.GetComponent<CanonSeries>().Init(50, 0, 1, 4, 0.8f);
        lib.WaitForProjectiles(slowPlusSingle_2);
    }
    private void slowPlusSingle_2()
    {
        GameObject c = spawner.SpawnProjectileAtAngle(canon, 0, -25, 1.0f);
        c.GetComponent<CanonSeries>().Init(0, 50, 1, 4, 0.8f);
        lib.WaitForProjectiles(playerTurn);
    }

    private void slowXSingle()
    {
        lib.fadeEnemy();
        GameObject c = spawner.SpawnProjectileAtAngle(canon, 0, -20, 1.0f);
        c.GetComponent<CanonSeries>().Init(40, 40, 1, 4, 0.8f);
        lib.WaitForProjectiles(slowXSingle_2);
    }
    private void slowXSingle_2()
    {
        GameObject c = spawner.SpawnProjectileAtAngle(canon, 0, 20, 1.0f);
        c.GetComponent<CanonSeries>().Init(40, -40, 1, 4, 0.8f);
        lib.WaitForProjectiles(playerTurn);
    }

    private void tracer2()
    {
        lib.fadeEnemy();
        GameObject c = spawner.SpawnProjectileAtAngle(megaCanon, 40, 0, 1.0f);  // Left
        c.GetComponent<CanonSeries>().Init(0, -40, 1.0f, 1.0f, 0.05f);
        c = spawner.SpawnProjectileAtAngle(megaCanon, 0, 0, 1.0f);  // Right
        c.GetComponent<CanonSeries>().Init(0, 40, 1.0f, 1.0f, 0.05f);
        lib.WaitForProjectiles(playerTurn);
    }

    private void firstBlood()
    {
        lib.fadeEnemy();
        GameObject c = spawner.SpawnProjectileAtAngle(megaCanon, 15, -50, 0.7f); // Right
        c.GetComponent<CanonSeries>().Init(0, 50, 0.8f, 1.5f, 0.1f);
        c = spawner.SpawnProjectileAtAngle(megaCanon, 50, -25, 0.7f); // Down
        c.GetComponent<CanonSeries>().Init(-50, 0, 0.8f, 1.5f, 0.1f);
        lib.WaitForProjectiles(playerTurn);
    }

    private AILibrary.OnSpeechFinishedEvent getRandomAttack()
    {
        int num = (int)(Random.value * 3.0f);
        if (num == 0)
            return shuffleAttack;
        else if (num == 1)
            return tracer2;
        return firstBlood;
    }

    private void randomSpeech()
    {
		
        int num = (int)(Random.value * 3.0f);
		if (num == 0)
			lib.AddTextToQueue (" There is something in my mind ... ", getRandomAttack ());
		else if (num == 1)
			lib.AddTextToQueue (" I cannot control myself... ", getRandomAttack ());
		else if (num == 2)
			lib.AddTextToQueue (" HAHAHAHAHA!!! ", getRandomAttack ());
		else {
			lib.AddTextToQueue (" No!! ");
			lib.AddTextToQueue (" Get out of my head!! ", getRandomAttack ());
		}
	}

    private void FreezeAttack()
    {
        GameObject c = spawner.SpawnProjectileAtAngle(canon, 0, 20, 0.3f);
        c.GetComponent<CanonSeries>().Init(40, -40, 1, 4, 0.8f);
        lib.WaitForProjectiles(playerTurn);
    }

    private AILibrary.OnSpeechFinishedEvent getFreezeAttack()
    {
        return FreezeAttack;
    }

    private void attackSelect()
    {
        if (lib.GetAIHealth() <= 10.0f)
        {
            finishHim();
            return;
        }
        else {
            if (isfreeze)
            {
                lib.AddTextToQueue(" (freeze) ", getFreezeAttack());
                isfreeze = false;
            }
            else {
                switch (turnCount)
                {   
            case 1:
                lib.AddTextToQueue(" ... ", getRandomAttack());//slowXSingle
                break;
            /*
            case 2:
                lib.AddTextToQueue("Because I'm giving this battle my all!", slowPlusSingle);
                break;
            case 3:
                lib.AddTextToQueue("For you see...");
                lib.AddTextToQueue("It's always been my dream to join the Elite Guard!", horizontalDouble);
                break;
            case 4:
                lib.AddTextToQueue("But Undyne won't let me in until I kill a human!");
                lib.AddTextToQueue("Talk about barriers to entry, am I right?");
                lib.AddTextToQueue("Nyeh heh heh!", slowXDouble);
                break;
            case 5:
                lib.AddTextToQueue("Seeing her in that sweet armor, she looks...");
                lib.AddTextToQueue("Well...");
                shuffleAttackDelay = 0.5f;
                lib.AddTextToQueue("Almost as cool as me!", shuffleAttack);
                break;
            case 6:
                lib.AddTextToQueue("So... uh...");
                lib.AddTextToQueue("What were we talking about?");
                lib.AddTextToQueue("Oh, yeah!");
                lib.AddTextToQueue("I need to kill you!", downUpDouble);
                break;
            case 7:
                lib.AddTextToQueue("C'mon, don't look at me like that!");
                lib.AddTextToQueue("Don't think of it as pointlessly dying!");
                lib.AddTextToQueue("Think of it as dying to make me look cool!");
                lib.AddTextToQueue("Cooler, I mean!", fastDownXDouble);
                break;
            case 8:
                shuffleAttackDelay = 0.3f;
                lib.AddTextToQueue("I can see it now... Papyrus, the great Guardsman!", shuffleAttack);
                break;
            case 9:
                lib.AddTextToQueue("Undyne's so proud of me...", fastSplitSingle);
                break;
            case 10:
                lib.AddTextToQueue("It's all I ever wanted, but...");
                lib.AddTextToQueue("Somehow it seems all... mean.", getRandomAttack());
                break;
            case 11:
                lib.AddTextToQueue("Do I really want to kill you, human?", getRandomAttack());
                break;
            case 12:
                lib.AddTextToQueue("I'm not...");
                lib.AddTextToQueue("I'm not sure anymore.", getRandomAttack());
                break;
            case 13:
                lib.AddTextToQueue("I'm sure you're giving me a real workout, though!");
                lib.AddTextToQueue("You might say you're...");
                lib.AddTextToQueue("Working me to the bone!");
                lib.AddTextToQueue("Nyeh heh heh!", getRandomAttack());
                break;
                */
            default:
                randomSpeech();
                break;
        }
            }
            
        }
        //MuteCount++;
    }

    private void bulletHellBlast1()
    {
        lib.fadeEnemy();
        float delay = 0.25f;
        bulletHellBlast1_spawn();
        for (int i = 1; i < 40; i++)
        {
            Invoke("bulletHellBlast1_spawn", delay * i);
        }
        lib.WaitForProjectiles(playerTurn);
    }

    private void bulletHellBlast1_spawn()
    {
        Vector2 center = bulletHellBlase1_getRandomCenter();
        spawner.SpawnProjectileAtAngle(megaLaser, center.x, center.y, 1.0f);
    }

    private Vector2 bulletHellBlase1_getRandomCenter()
    {
        return new Vector2((int)(Random.value * 45.0f) - 5, (int)(Random.value * 80.0f) - 40);
    }

    private void playerTurn()
    {
        lib.solidifyEnemy();
        turnCount++;
        lib.PlayerAttack(attackSelect);
    }

    private void beginTrueFight()
    {
        //lib.PlayMusic((AudioClip)Resources.Load("Bonetrousle"));
        playerTurn();
    }

    private void intro() {
        lib.PlayMusic((AudioClip)Resources.Load("sans."));
        lib.AddTextToQueue("Hi, buddy");
        lib.AddTextToQueue("Welcome to the my palace");
        lib.AddTextToQueue("Do you feel those bosses including me are hard to beat?");
        lib.AddTextToQueue("Let me tell you a new technique as a friend");
        lib.AddTextToQueue("Try press \"X\" or \"Y\" on your left hand");
        lib.AddTextToQueue("These are your magic power");
        lib.AddTextToQueue("You can use magic for defence and offence");
        lib.AddTextToQueue("Just keep in mind some of them have use limit");
        lib.AddTextToQueue("You can try it now");
        lib.AddTextToQueue("Also, here is another suggestion.");
        lib.AddTextToQueue("A super awesome trick to beat all bosses!");
        lib.AddTextToQueue("Which the developer won't tell you");
        lib.AddTextToQueue("Do you want to know?");
        lib.AddTextToQueue("Here it is...", intro2);
    }

    private void intro2() {
        lib.AddTextToQueue(" ! ! ! ");
        lib.StopMusic();
        lib.AddTextToQueue("UOOO!");
        lib.AddTextToQueue("GUAAAAAA");
        lib.AddTextToQueue("SHUT UP!!");
        lib.AddTextToQueue("SHI-----");
        lib.AddTextToQueue("UHHHHHHHHH",intro3);
    }

    private void intro3() {

        cutout(1.0f, postFirstCut, true);
    }

    private void postFirstCut()
    {
        sr.sprite = Resources.Load<Sprite>("newboss2");
        lib.AddTextToQueue(".");
        lib.AddTextToQueue("..", begin);
    }

    private void begin()
    {
        lib.PlayMusic((AudioClip)Resources.Load("ORIGINE DELLA VITA"));
        lib.AddTextToQueue("...");
        lib.AddTextToQueue("KILL...");
        lib.AddTextToQueue("I NEED TO KILL YOU...");
        lib.AddTextToQueue("NOW...");
        lib.AddTextToQueue("DIE !!", beginTrueFight);
    }
}

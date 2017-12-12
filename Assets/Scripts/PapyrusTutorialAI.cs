using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PapyrusTutorialAI : MonoBehaviour
{
    public AILibrary lib;
    private Spawner spawner;
    private GameObject basicLaser;
    private GameObject gasterBlaster;
    private GameObject canon;
    private PlayerInfo playerInfo;
    private int laserFailCount;
    private int prevPlayerHealth;
    private int turnCount;
    private float shuffleAttackDelay;
    private GameObject upButton;
    private GameObject downButton;
    private MenuButtonHighligher upButtonHighlighter;
    private MenuButtonHighligher downButtonHighlighter;
    public float HUD_NudgeAmount = 0.15f;
    public float Max_HUD_Height = 1.5f;
    public float Min_HUD_Height = 0.75f;
    private GameObject hud;
    private GameObject mainMenu;
    private GameObject projSpawner;
    private bool domPressedThisFrame;
    private bool domPressed;
    private GameObject continueButton;
    private MenuButtonHighligher continueButtonHighlighter;
    private bool alreadyContinued;

    void Start()
    {
        hud = GameObject.Find("ElasticHUD");
        mainMenu = GameObject.Find("MainMenu");
        projSpawner = GameObject.Find("ProjectileSpawner");
        domPressedThisFrame = false;
        domPressed = false;
        alreadyContinued = false;
        upButton = GameObject.Find("UpButton");
        upButtonHighlighter = upButton.GetComponent<MenuButtonHighligher>();
        downButton = GameObject.Find("DownButton");
        downButtonHighlighter = downButton.GetComponent<MenuButtonHighligher>();
        upButton.SetActive(true);
        downButton.SetActive(true);
        continueButton = GameObject.Find("ContinueButton");
        continueButtonHighlighter = continueButton.GetComponent<MenuButtonHighligher>();
        continueButton.SetActive(false);

        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<Spawner>();
        basicLaser = (GameObject)Resources.Load("BasicLaser");
        gasterBlaster = (GameObject)Resources.Load("GasterBlaster");
        canon = (GameObject)Resources.Load("CanonBlaster");
        playerInfo = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInfo>();
        laserFailCount = 0;
        lib.RegisterCrossheirPositionFunc(CrossheirPosition);
        lib.SetVitals(9001.0f); //he won't actually be killed
        //lib.SetVitals(11.0f);
        turnCount = 0;
        Invoke("begin", 0.1f);
    }
    private void nudgeMenu(float scale)
    {
        Vector3 nudgeVec = new Vector3(0.0f, HUD_NudgeAmount * scale, 0.0f);
        float newHudHeight = nudgeVec.y + hud.transform.position.y;
        if (newHudHeight > Max_HUD_Height || newHudHeight < Min_HUD_Height)
        {
            return;
        }
        hud.transform.position += nudgeVec;
        mainMenu.transform.position += nudgeVec;
        continueButton.transform.position += nudgeVec;
        projSpawner.transform.position += nudgeVec;
    }
    // Update is called once per frame
    void Update()
    {
        inputManager();
        buttonManager();
    }
    public bool GetDominantTriggerPressed()
    {
        return domPressedThisFrame;
    }

    public static bool GetDominantTriggerDown()
    {
        return OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
    }

    private void buttonManager()
    {
        if (domPressedThisFrame)
        {
            if (upButtonHighlighter.IsHovered())
            {
                nudgeMenu(1.0f);
            }
            else if (downButtonHighlighter.IsHovered())
            {
                nudgeMenu(-1.0f);
            }
            else if (continueButtonHighlighter.IsHovered())
            {
                upButton.SetActive(false);
                downButton.SetActive(false);
                continueButton.SetActive(false);
                if (!alreadyContinued)
                {
                    alreadyContinued = true;
                    continueFight();
                }
            }
        }
    }

    private void inputManager()
    {
        if (GetDominantTriggerDown() && !domPressed)
        {
            domPressedThisFrame = true;
            domPressed = true;
        }
        else
        {
            domPressedThisFrame = false;
        }
        if (!GetDominantTriggerDown())
        {
            domPressed = false;
        }
    }

    public float CrossheirPosition(float time)
    {
        return 4.0f - (time * 1.5f);
    }

    public float FinishCrossheirs(float time)
    {
        return 0.0f;
    }

    private void fakeDeath()
    {
        Invoke("die", 0.01f);
        return;
    }

    private void die()
    {
        playerInfo.ResetHealth();
        lib.fakeDeath();
    }

    private void spare()
    {
        lib.Spare();
    }

    private void blasterSuccessCheck()
    {
        lib.solidifyEnemy();
        if (lib.GetPlayerHealth() != prevPlayerHealth)
        {
            playerInfo.ResetHealth();
            switch (laserFailCount)
            {
                case 0:
                    lib.AddTextToQueue("Not quite!");
                    lib.AddTextToQueue("Remember, you need to use BOTH BARRIERS to block the beam!", blasterIntro);
                    break;
                case 1:
                    lib.AddTextToQueue("Okay, I'm not sure how else to put this.");
                    lib.AddTextToQueue("Put your shields together!");
                    lib.AddTextToQueue("Real close!");
                    lib.AddTextToQueue("Nice and snug!");
                    lib.AddTextToQueue("Capiche?", blasterIntro);
                    break;
                case 2:
                    lib.AddTextToQueue("Oh my God.");
                    lib.AddTextToQueue("I'm actually dying over here.");
                    lib.AddTextToQueue("Which is pretty impressive seeing as I'm a skeleton.");
                    lib.AddTextToQueue("Just... like...");
                    lib.AddTextToQueue("Put your shields together! They'll change color.");
                    lib.AddTextToQueue(".....");
                    lib.AddTextToQueue("Do you know what colors are?");
                    lib.AddTextToQueue("Seven billion humans and I get this one...", blasterIntro);
                    break;
                default:
                    lib.AddTextToQueue("...");
                    lib.AddTextToQueue(".....");
                    lib.AddTextToQueue(".......");
                    lib.AddTextToQueue("Just try again.", blasterIntro);
                    break;
            }
            laserFailCount++;
        }
        else
        {
            lib.AddTextToQueue("Fantastic!");
            lib.AddTextToQueue("Phenominal!");
            if (laserFailCount >= 3)
            {
                lib.AddTextToQueue("Kinda embarassing it took you this long, but hey.");
                lib.AddTextToQueue("We can't all not be terrible!");
            }
            lib.AddTextToQueue("With your skills, I bet you could take on anyone in the world!");
            lib.AddTextToQueue("Or maybe not... I did go easy on you, after all!");
            lib.AddTextToQueue("You can go ahead and start looking for people to fight now.");
            lib.AddTextToQueue("But be careful -- they might have some tricks up their sleeves!", fakeDeath);
        }
    }

    private void blasterIntro()
    {
        lib.fadeEnemy();
        spawner.SpawnProjectileAtAngle(gasterBlaster, 10, 0, 1.0f);
        lib.WaitForProjectiles(blasterSuccessCheck);
        prevPlayerHealth = lib.GetPlayerHealth();
    }

    private void blasterTutorial()
    {
        lib.AddTextToQueue("Now then, it's time for something harder!");
        lib.AddTextToQueue("I present to you-- a blaster!");
        lib.AddTextToQueue("Its might is unsurpassed! Its power is undeniable!");
        lib.AddTextToQueue("You must use BOTH BARRIERS to block its attack!");
        lib.AddTextToQueue("Ready? Here it comes!", blasterIntro);
        laserFailCount = 0;
    }

    private void canonSuccessCheck()
    {
        lib.solidifyEnemy();
        if (prevPlayerHealth == lib.GetPlayerHealth())
        {
            lib.AddTextToQueue("Congrats!");
            lib.AddTextToQueue("You didn't die!", blasterTutorial);
        }
        else
        {
            playerInfo.ResetHealth();
            switch (laserFailCount)
            {
                case 0:
                    lib.AddTextToQueue("Almost!");
                    lib.AddTextToQueue("Try again!", canonIntro);
                    break;
                case 1:
                    lib.AddTextToQueue("You're so close!");
                    lib.AddTextToQueue("I can feel it in my bones!");
                    lib.AddTextToQueue("Nyeh heh heh!", canonIntro);
                    break;
                case 2:
                    lib.AddTextToQueue("Alright!");
                    lib.AddTextToQueue("Maybe you actually weren't close!");
                    lib.AddTextToQueue("Honestly I was just saying that to make you feel better.");
                    lib.AddTextToQueue("You're actually pretty bad.");
                    lib.AddTextToQueue("Er... Try it again, I guess?", canonIntro);
                    break;
                default:
                    lib.AddTextToQueue("AAAAARGH");
                    lib.AddTextToQueue("You're killing me!");
                    lib.AddTextToQueue("Well not literally.");
                    lib.AddTextToQueue("Try again!", canonIntro);
                    break;
            }
            laserFailCount++;
        }
    }

    private void canonIntro()
    {
        lib.fadeEnemy();
        prevPlayerHealth = lib.GetPlayerHealth();
        GameObject c = spawner.SpawnProjectileAtAngle(canon, 0, -20, 1.0f);
        c.GetComponent<CanonSeries>().Init(0, 40, 1, 4, 0.8f);
        lib.WaitForProjectiles(canonSuccessCheck);
    }

    private void firstAttackSuccess()
    {
        lib.AddTextToQueue("Now let's see how you handle a canon!");
        lib.AddTextToQueue("It's a moving skull that shoots a bunch of lasers!");
        lib.AddTextToQueue("Not as attractive as my skull, but hey, what is?");
        laserFailCount = 0;
        lib.AddTextToQueue("Here it comes!", canonIntro);
    }

    private void afterFirstAttack()
    {
        if (lib.GetAIHealth() == lib.GetAIMaxHealth())
        {
            lib.AddTextToQueue("That was great!");
            lib.AddTextToQueue("For a loser!");
            lib.AddTextToQueue("Seriously, try that again...", firstAttack);
        }
        else
        {
            lib.AddTextToQueue("Ouch!");
            lib.AddTextToQueue("Hahaha, don't worry, it'll take waaaaaay more than that to take me down!", firstAttackSuccess);
        }
    }

    private void firstAttack()
    {
        lib.PlayerAttack(afterFirstAttack);
    }

    private void swingTutorial()
    {
        lib.AddTextToQueue("You should probably learn how to attack yourself, too!");
        lib.AddTextToQueue("It's super easy!");
        lib.AddTextToQueue("It's like I always say:");
        lib.AddTextToQueue("\"Violence--");
        lib.AddTextToQueue("It's... uh...");
        lib.AddTextToQueue("Pretty great!");
        lib.AddTextToQueue("Just like me!\"");
        lib.AddTextToQueue("Don't I have the best quotes?");
        lib.AddTextToQueue("They're pretty great.");
        lib.AddTextToQueue("So, uh, to attack, just point your barrier at your foe's weak points");
        lib.AddTextToQueue("And unleash the power within you!");
        lib.AddTextToQueue("Folks are most vulnerable to attacks when their weak points overlap!");
        lib.AddTextToQueue("So fire just as they're on top of each other!");
        lib.AddTextToQueue("That's the trick.");
        lib.AddTextToQueue("Give it a shot on me.");
        lib.AddTextToQueue("Get it? \"Shot\"?");
        lib.AddTextToQueue("Nyeh heh heh!", firstAttack);
    }

    private void done()
    {
        lib.solidifyEnemy();
        if (lib.GetPlayerHealth() == prevPlayerHealth)
        {
            lib.AddTextToQueue("Nice job! Lasers hurt, let me tell you!", swingTutorial);
        }
        else
        {
            playerInfo.ResetHealth();
            switch (laserFailCount)
            {
                case 0:
                    lib.AddTextToQueue("Wow! You're really bad!");
                    break;
                case 1:
                    lib.AddTextToQueue("Um.");
                    lib.AddTextToQueue("Perhaps we're not communicating here!");
                    lib.AddTextToQueue("You're supposed to BLOCK the LASERS with a BARRIER!");
                    break;
                case 2:
                    lib.AddTextToQueue("Wow!");
                    lib.AddTextToQueue("I don't know what you're doing!");
                    lib.AddTextToQueue("You're actually just the worst!");
                    break;
                default:
                    lib.AddTextToQueue("...");
                    break;
            }
            laserFailCount++;
            lib.AddTextToQueue("Let's try that again!", pewpew);
        }
    }

    private void pewpew()
    {
        lib.fadeEnemy();
        spawner.SpawnProjectileAtAngle(basicLaser, 10, 0, 1.0f);
        prevPlayerHealth = lib.GetPlayerHealth();
        lib.WaitForProjectiles(done);
    }

    private void begin()
    {
        lib.PlayMusic((AudioClip)Resources.Load("Nyeh Heh Heh!"));
        lib.AddTextToQueue("Welcome, human!");
        lib.AddTextToQueue("Although you are new to this world, fear not!");
        lib.AddTextToQueue("For I, the great Papyrus, am here to guide you on your path!");
        lib.AddTextToQueue("Not because I must-- but because I am great.");
        lib.AddTextToQueue("And handsome.");
        lib.AddTextToQueue("And also humble.");
        lib.AddTextToQueue("I'm pretty great.");
        lib.AddTextToQueue("Yup.");
        lib.AddTextToQueue("...");
        lib.AddTextToQueue("Anyway, life down here is fraught with peril!");
        lib.AddTextToQueue("Everyone you meet will surely try to kill you!");
        lib.AddTextToQueue("Not because we're mean, honestly it's just how we say \"hello\".");
        lib.AddTextToQueue("...");
        lib.AddTextToQueue(".....");
        lib.AddTextToQueue("Hey, don't judge my culture you ethnocentric ass.");
        lib.AddTextToQueue("Anyway, the great Papyrus shall now prepare you for the journey ahead!");
        lib.AddTextToQueue("See those glowey things on your hands?");
        lib.AddTextToQueue("Those are your BARRIERS!");
        lib.AddTextToQueue("Or possibly inoperable TUMORS!");
        lib.AddTextToQueue("But hopefully BARRIERS!");
        lib.AddTextToQueue("Your right BARRIER has a pointer that you can use to aim!");
        lib.AddTextToQueue("And you've already figured out that you click with the right trigger.");
        lib.AddTextToQueue("If you want, you can use those buttons on your left to move your interface.");
        lib.AddTextToQueue("You can do that from the main menu later, too.");
        lib.AddTextToQueue("Use the buttons to move the interface to a comfortable position.");
        lib.AddTextToQueue("Just go ahead and click \"continue\" when you're done with that.", showContinueButton);
    }

    private void showContinueButton()
    {
        continueButton.SetActive(true);
    }

    private void continueFight() {
        lib.AddTextToQueue("Anyway... Down here, we tend to throw LASERS at our guests.");
        lib.AddTextToQueue("They're super easy to draw, you see.");
        lib.AddTextToQueue("You can block them using your BARRIERS!");
        lib.AddTextToQueue("Hold a BARRIER in front of this LASER!", pewpew);
    }
}

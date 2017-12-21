using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour {

    public float HUD_NudgeAmount = 0.15f;
    public float Max_HUD_Height = 1.5f;
    public float Min_HUD_Height = 0.75f;
    public DefensePowerups defenseScript;

    private GameObject player;
    private PlayerInfo playerInfo;
    private OVRPlayerController playerController;
    private GameObject activePlayerPointer;
    private GameObject playerPointer;
    
    private GameObject lightningButton;
    private MenuButtonHighligher lightningButtonHighlighter;

    private GameObject papyrusButton;
    private MenuButtonHighligher papyrusButtonHighlighter;
    private GameObject undyneButton;
    private MenuButtonHighligher undyneButtonHighlighter;
    private GameObject sansButton;
    private MenuButtonHighligher sansButtonHighlighter;
    private GameObject quitButton;
    private MenuButtonHighligher quitButtonHighlighter;
    private GameObject asgoreButton;
    private MenuButtonHighligher asgoreButtonHighlighter;
    /*private GameObject cheatButton;
    private MenuButtonHighligher cheatButtonHighlighter;
    private GameObject megaCheatButton;
    private MenuButtonHighligher megaCheatButtonHighlighter;*/

    private GameObject NewBossButton;		
    private MenuButtonHighligher NewBossButtonHighlighter;

    private GameObject upButton;
    private MenuButtonHighligher upButtonHighlighter;
    private GameObject downButton;
    private MenuButtonHighligher downButtonHighlighter;

    private GameObject godButton;
    private MenuButtonHighligher godButtonHighlighter;
    private GameObject papyrusKillButton;
    private MenuButtonHighligher papyrusKillButtonHighlighter;
    private GameObject undyneKillButton;
    private MenuButtonHighligher undyneKillButtonHighlighter;

    private GameObject hud;
    private GameObject mainMenu;
    private GameObject projSpawner;

    private GameObject fightButton;
    private GameObject mercyButton;

    private GameObject undyne;
    private GameObject sans;
    private GameObject lightning;
    private GameObject asgore;

    private GameObject[] credits;

    private int enemyFought;
    private Color killedColor;
    private int killedCount;

    private bool domPressedThisFrame;
    private bool domPressed;

    private bool isEnabled;

    private GameObject gameOverHeart;
    private SpriteRenderer gameOverHeartSpriteRen;
    private ParticleSystem gameOverHeartParts;
    public Sprite origHeart;
    public Sprite brokenHeart;
    public GameObject heartCut;
    public GameObject heartBurst;
    private AudioClip gameOverMusic;

    private GameObject mainMenuButton;
    private MenuButtonHighligher mainMenuButtonHighlighter;

    // <Robert>
    //private GameObject tutorial;
    //private TextMesh tutorialText;
    private GameObject continueButton;
    private MenuButtonHighligher continueButtonHighlighter;
    //private bool tutorialEnabled;
    //private int tutorialScreen;
    //private List<string> tutorialTexts;
    //private float lastTimeContinued;
    private GameObject creditsButton;
    private MenuButtonHighligher creditsButtonHighlighter;
    private GameObject tutorialButton;
    private MenuButtonHighligher tutorialButtonHighlighter;
    private GameObject papyrusTutorial;
    // </Robert>

    void Start () {
        //player = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.Find("playerCollider");
        defenseScript = GameObject.Find("OVRPlayerController").GetComponent<DefensePowerups>();
        playerInfo = player.GetComponent<PlayerInfo>();
        playerController = GameObject.Find("OVRPlayerController").GetComponent<OVRPlayerController>();
        playerPointer = (GameObject)Resources.Load("PlayerPointer");
        //Transform attachShield = GameObject.Find("RightShield").transform;
        //activePlayerPointer = Instantiate(playerPointer, attachShield.position, attachShield.rotation, attachShield);

        papyrusButton = transform.Find("PapyrusButton").gameObject;
        papyrusButtonHighlighter = papyrusButton.GetComponent<MenuButtonHighligher>();
        undyneButton = transform.Find("UndyneButton").gameObject;
        undyneButtonHighlighter = undyneButton.GetComponent<MenuButtonHighligher>();
        sansButton = transform.Find("BadTimeButton").gameObject;
        sansButtonHighlighter = sansButton.GetComponent<MenuButtonHighligher>();
        quitButton = transform.Find("QuitButton").gameObject;
        quitButtonHighlighter = quitButton.GetComponent<MenuButtonHighligher>();
        asgoreButton = transform.Find("AsgoreButton").gameObject;
        asgoreButtonHighlighter = asgoreButton.GetComponent<MenuButtonHighligher>();
        /*cheatButton = transform.Find("CheatButton").gameObject;
        cheatButtonHighlighter = cheatButton.GetComponent<MenuButtonHighligher>();
        megaCheatButton = transform.Find("MegaCheatButton").gameObject;
        megaCheatButtonHighlighter = megaCheatButton.GetComponent<MenuButtonHighligher>();*/
        continueButton = GameObject.Find("ContinueButton");
        continueButtonHighlighter = continueButton.GetComponent<MenuButtonHighligher>();

        NewBossButton = transform.Find("NewBossButton").gameObject;
        NewBossButtonHighlighter = NewBossButton.GetComponent<MenuButtonHighligher>();

        upButton = transform.Find("UpButton").gameObject;
        upButtonHighlighter = upButton.GetComponent<MenuButtonHighligher>();
        downButton = transform.Find("DownButton").gameObject;
        downButtonHighlighter = downButton.GetComponent<MenuButtonHighligher>();

        godButton = GameObject.Find("GodButton");
        godButtonHighlighter = godButton.GetComponent<MenuButtonHighligher>();
        papyrusKillButton = transform.Find("PapyrusKillButton").gameObject;
        papyrusKillButtonHighlighter = papyrusKillButton.GetComponent<MenuButtonHighligher>();
        papyrusKillButton.SetActive(false);
        undyneKillButton = transform.Find("UndyneKillButton").gameObject;
        undyneKillButtonHighlighter = undyneKillButton.GetComponent<MenuButtonHighligher>();
        undyneKillButton.SetActive(false);
        
        lightningButton = transform.Find("LightningButton").gameObject;
        lightningButtonHighlighter = lightningButton.GetComponent<MenuButtonHighligher>();

        hud = GameObject.Find("ElasticHUD");
        mainMenu = GameObject.Find("MainMenu");
        projSpawner = GameObject.Find("ProjectileSpawner");

        fightButton = GameObject.Find("AttackButton");
        mercyButton = GameObject.Find("MercyButton");

        credits = new GameObject[3];
        credits[0] = GameObject.Find("Credits - People");
        credits[1] = GameObject.Find("Credits - UIUC");
        credits[2] = GameObject.Find("Credits - Toby");

        gameOverHeart = transform.Find("GameOverHeart").gameObject;
        gameOverHeartSpriteRen = gameOverHeart.GetComponent<SpriteRenderer>();
        gameOverHeartParts = transform.Find("GameOverParts").GetComponent<ParticleSystem>();

        mainMenuButton = GameObject.Find("MainMenuButton");
        mainMenuButtonHighlighter = mainMenuButton.GetComponent<MenuButtonHighligher>();
        mainMenuButton.SetActive(false);
        gameOverHeart.SetActive(false);

        killedColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        killedCount = 0;

        gameOverMusic = (AudioClip)Resources.Load("Determination");

        // <Robert>
        //tutorial = GameObject.Find("TutorialText");
        //tutorialText = tutorial.GetComponent<TextMesh>();
        //tutorialEnabled = true;
        //tutorialScreen = 0;
        //tutorialTexts = new List<string>();
        //lastTimeContinued = 0.0f;
        creditsButton = GameObject.Find("CreditsButton");
        creditsButtonHighlighter = creditsButton.GetComponent<MenuButtonHighligher>();
        tutorialButton = GameObject.Find("TutorialButton");
        tutorialButtonHighlighter = tutorialButton.GetComponent<MenuButtonHighligher>();
        //fillTutorialTexts();
        playMusic((AudioClip)Resources.Load("Menu (Full)"));
        Invoke("initOpenMenu", 0.01f);

        foreach(GameObject obj in credits)
        {
            obj.SetActive(false);
        }

        if(activePlayerPointer == null)
        {
            Transform attachShield = GameObject.Find("RightShield").transform;
            activePlayerPointer = Instantiate(playerPointer, attachShield.position, attachShield.rotation, attachShield);
        }
        // </Robert>
    }

    void Update () {
        if (!isEnabled) // && !tutorialEnabled)
            return;
        inputManager();
        buttonManager();
	}

    /*
    private void startTutorial()
    {
        closeMenu(true);
        tutorialScreen = 0;
        tutorialText.text = tutorialTexts[tutorialScreen];
        tutorial.SetActive(true);
        tutorialEnabled = true;
        continueButton.SetActive(true);
        fightButton.SetActive(false);
        mercyButton.SetActive(false);
    }
    */
    private void initOpenMenu()
    {
        OpenMenu(false, true);
    }

    public void OpenMenu(bool enemyKilled = false, bool pointerActive = false)
    {
        playerController.SetMusicPitch(1.0f);
        resetPlayerHealth();
        if (enemyKilled)
        {
            slayEnemy();
        }
        isEnabled = true;
        playMusic((AudioClip)Resources.Load("Menu (Full)"));
        /*if (!enemyKilled) { } // for returning to the menu from tutorial/credits
        else if (killedCount == 0)
            playMusic((AudioClip)Resources.Load("Menu (Full)"));
        else if (killedCount == 1)
            playMusic((AudioClip)Resources.Load("Start Menu"));
        else if (killedCount == 2)
            playMusic((AudioClip)Resources.Load("Small Shock"));
        else
            playMusic((AudioClip)Resources.Load("Dummy!"));
        */
       
        if (pointerActive == false)
        {
            Transform attachShield = GameObject.Find("RightShield").transform;
            activePlayerPointer = Instantiate(playerPointer, attachShield.position, attachShield.rotation, attachShield);
            pointerActive = true;
        }

        fightButton.SetActive(false);
        mercyButton.SetActive(false);
        papyrusButton.SetActive(true);
        undyneButton.SetActive(true);
        sansButton.SetActive(true);
        asgoreButton.SetActive(true);
        quitButton.SetActive(true);
        //cheatButton.SetActive(true);
        //megaCheatButton.SetActive(true);

        NewBossButton.SetActive(true);
        
        lightningButton.SetActive(true);
        creditsButton.SetActive(true);
        tutorialButton.SetActive(true);
        upButton.SetActive(true);
        downButton.SetActive(true);
        continueButton.SetActive(false);

        godButton.SetActive(true);
        /*if(killedCount == 0)
            papyrusKillButton.SetActive(true);
        else if(killedCount == 1)
            undyneKillButton.SetActive(true);
        */

        foreach(GameObject obj in credits)
        {
            obj.SetActive(false);
        }

        if (activePlayerPointer == null)
        {
            Transform attachShield = GameObject.Find("RightShield").transform;
            activePlayerPointer = Instantiate(playerPointer, attachShield.position, attachShield.rotation, attachShield);
        }

        GameObject tmp = GameObject.Find("PlayerSword(Clone)");
        if(tmp != null)
        {
            GameObject.Destroy(tmp);
        }

        GameObject.Find("LeftShield").GetComponent<Shield>().disableSword();
        GameObject.Find("RightShield").GetComponent<Shield>().disableSword();
        GameObject.Find("teleportationManager").GetComponent<teleportation>().disableTeleportation();
    }

    public void GameOver()
    {
        playerController.StopMusic();
        gameOverHeartSpriteRen.sprite = origHeart;
        gameOverHeart.SetActive(true);
        Invoke("breakHeart", 1.0f);
    }

    private void breakHeart()
    {
        gameOverHeartSpriteRen.sprite = brokenHeart;
        Instantiate(heartCut, player.transform.position, Quaternion.identity);
        Invoke("burstHeart", 1.5f);
    }

    private void burstHeart()
    {
        gameOverHeart.SetActive(false);
        Instantiate(heartBurst, player.transform.position, Quaternion.identity);
        gameOverHeartParts.Play();
        Invoke("showGameOverOptions", 1.0f);
    }

    private void showGameOverOptions()
    {
        playMusic(gameOverMusic);
        playerController.SetMusicPitch(1.0f);
        mainMenuButton.SetActive(true);
        Transform attachShield = GameObject.Find("RightShield").transform;
        if(playerPointer == null)
        {
            playerPointer = (GameObject)Resources.Load("PlayerPointer");
        }

        GameObject pointer = GameObject.Find("PlayerSword(Clone)");
        if (pointer != null)
        {
            Object.Destroy(pointer);
        }
        activePlayerPointer = Instantiate(playerPointer, attachShield.position, attachShield.rotation, attachShield);
        isEnabled = true;
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

    private void startUndyne()
    {
        undyne.GetComponent<AILibrary>().RegisterAttackButtons(fightButton, mercyButton);
        undyne.GetComponent<UndyneAI>().RegisterTotalKills(killedCount);
        defenseScript.Enable();
        defenseScript.ResetAvailability();
    }

    private void startSans()
    {
        sans.GetComponent<AILibrary>().RegisterAttackButtons(fightButton, mercyButton);
        sans.GetComponent<SansAI>().RegisterTotalKills(killedCount);
        defenseScript.Enable();
        defenseScript.ResetAvailability();
    }

    // <Robert>
    private void startLightning()
    {
        lightning.GetComponent<AILibrary>().RegisterAttackButtons(fightButton, mercyButton);
        lightning.GetComponent<LightningAI>().RegisterTotalKills(killedCount);
        defenseScript.Enable();
        defenseScript.ResetAvailability();
    }

    private void startPapyrusTutorial()
    {
        papyrusTutorial.GetComponent<AILibrary>().RegisterAttackButtons(fightButton, mercyButton);
        defenseScript.Disable();
    }
    // </Robert>

    private void startAsgore()
    {
        asgore.GetComponent<AILibrary>().RegisterAttackButtons(fightButton, mercyButton);
        asgore.GetComponent<AsgoreAI>().RegisterTotalKills(killedCount);
        defenseScript.Disable();
    }

    private void buttonManager()
    {
        /*if(tutorialEnabled)
        {
            tutorialButtonManager();
        }
        else
        {*/
        if (domPressedThisFrame)
        {
            if (papyrusButtonHighlighter.IsHovered())
            {
                closeMenu();
                enemyFought = 0;
                GameObject p = (GameObject)Instantiate(Resources.Load("Papyrus"));
                p.GetComponent<AILibrary>().RegisterAttackButtons(fightButton, mercyButton);
            }
            else if (NewBossButtonHighlighter.IsHovered())
            {
                closeMenu();
                SceneManager.LoadScene(1);
                GameObject n = (GameObject)Instantiate(Resources.Load("NewBoss"));
                n.GetComponent<AILibrary>().RegisterAttackButtons(fightButton, mercyButton);
                defenseScript.Enable();
                defenseScript.ResetAvailability();
            }
            else if (undyneButtonHighlighter.IsHovered())
            {
                closeMenu();
                enemyFought = 1;
                undyne = (GameObject)Instantiate(Resources.Load("Undyne"));
                Invoke("startUndyne", 0.01f);
            }
            else if (sansButtonHighlighter.IsHovered())
            {
                closeMenu(true);
                enemyFought = 2;
                sans = (GameObject)Instantiate(Resources.Load("Sans"));
                Invoke("startSans", 0.01f);
            } // <Robert>
            else if (lightningButtonHighlighter.IsHovered())
            {
                closeMenu();
                enemyFought = 3;
                lightning = (GameObject)Instantiate(Resources.Load("Lightning"));
                Invoke("startLightning", 0.01f);
            } // </Robert>
            // <X>
            else if(asgoreButtonHighlighter.IsHovered())
            {
                closeMenu(true);
                enemyFought = 4;
                asgore = (GameObject)Instantiate(Resources.Load("Asgore"));
                Invoke("startAsgore", 0.01f);
            }
            // </X>
            else if (quitButtonHighlighter.IsHovered())
            {
                Application.Quit();
            }
            else if (upButtonHighlighter.IsHovered())
            {
                nudgeMenu(1.0f);
            }
            else if (downButtonHighlighter.IsHovered())
            {
                nudgeMenu(-1.0f);
            }
            /*
            else if(cheatButtonHighlighter.IsHovered())
            {
                killedCount = 1;
                playerInfo.MaxHealth = 120;
                playerInfo.ResetHealth();
                playMusic((AudioClip)Resources.Load("Start Menu"));
            }
            else if(megaCheatButtonHighlighter.IsHovered())
            {
                killedCount = 2;
                playerInfo.MaxHealth = 200;
                playerInfo.ResetHealth();
                playMusic((AudioClip)Resources.Load("Small Shock"));
            }*/
            else if (godButtonHighlighter.IsHovered())
            {
                playerInfo.GodModeToggle();
            }
            /*
            else if (papyrusKillButtonHighlighter.IsHovered())
            {
                enemyFought = 0;
                slayEnemy();
                playerInfo.MaxHealth = 120;
                playerInfo.ResetHealth();
                playMusic((AudioClip)Resources.Load("Start Menu"));
                undyneKillButton.SetActive(true);
                papyrusKillButton.SetActive(false);
                papyrusKillButtonHighlighter.ForceDeselect();
            }
            else if (undyneKillButtonHighlighter.IsHovered())
            {
                enemyFought = 1;
                slayEnemy();
                playerInfo.MaxHealth = 200;
                playerInfo.ResetHealth();
                playMusic((AudioClip)Resources.Load("Small Shock"));
                undyneKillButton.SetActive(false);
                undyneKillButtonHighlighter.ForceDeselect();
            }*/
            else if (mainMenuButtonHighlighter.IsHovered())
            {
                mainMenuButton.SetActive(false);
                mainMenuButtonHighlighter.ForceDeselect();
                OpenMenu(false, true);
            }
            else if (creditsButtonHighlighter.IsHovered())
            {
                closeMenu(true);
                isEnabled = true; // bit of a hack to get around menu's current coding
                mainMenuButton.SetActive(true);
                foreach(GameObject obj in credits)
                {
                    obj.SetActive(true);
                }
            }
            else if (tutorialButtonHighlighter.IsHovered())
            {
                closeMenu();
                upButton.SetActive(true);
                downButton.SetActive(true);
                continueButton.SetActive(true);
                isEnabled = false;
                enemyFought = 0;
                papyrusTutorial = (GameObject)Instantiate(Resources.Load("PapyrusTutorial"));
                Invoke("startPapyrusTutorial", 0.01f);
            }
        }
        //}
    }

    /*
    private void tutorialButtonManager()
    {
        if(domPressedThisFrame)
        {
            if (Time.fixedTime > lastTimeContinued + 1.0f && continueButtonHighlighter.IsHovered())
            {
                lastTimeContinued = Time.fixedTime;
                tutorialScreen++;
                if(tutorialScreen == 3)
                {
                    Invoke("initOpenMenu", 0.01f);
                    tutorial.SetActive(false);
                    continueButton.SetActive(false);
                    tutorialEnabled = false;
                    return;
                }
                tutorialText.text = tutorialTexts[tutorialScreen];
                if(tutorialScreen == 1)
                {
                    upButton.SetActive(true);
                    downButton.SetActive(true);
                }
                else
                {
                    if (upButton.activeSelf)
                        upButton.SetActive(false);
                    if (downButton.activeSelf)
                        downButton.SetActive(false);
                }
            }
            else if (upButtonHighlighter.IsHovered())
            {
                nudgeMenu(1.0f);
            }
            else if (downButtonHighlighter.IsHovered())
            {
                nudgeMenu(-1.0f);
            }
        }
    }
    */

    private void nudgeMenu(float scale)
    {
        Vector3 nudgeVec = new Vector3(0.0f, HUD_NudgeAmount * scale, 0.0f);
        float newHudHeight = nudgeVec.y + hud.transform.position.y;
        if (newHudHeight > Max_HUD_Height || newHudHeight < Min_HUD_Height)
        {
            Debug.Log("out of bounds HUD height");
            return;
        }
        hud.transform.position += nudgeVec;
        mainMenu.transform.position += nudgeVec;
        //continueButton.transform.position += nudgeVec;
        projSpawner.transform.position += nudgeVec;
    }

    private void closeMenu(bool continueMusic = false)
    {
        if(!continueMusic)
            stopMusic();
        //Object.Destroy(activePlayerPointer);
        //activePlayerPointer = null;
        papyrusButtonHighlighter.ForceDeselect();
        papyrusButton.SetActive(false);
        undyneButtonHighlighter.ForceDeselect();
        undyneButton.SetActive(false);
        sansButtonHighlighter.ForceDeselect();
        sansButton.SetActive(false);
        quitButtonHighlighter.ForceDeselect();
        quitButton.SetActive(false);

        asgoreButtonHighlighter.ForceDeselect();
        asgoreButton.SetActive(false);

        NewBossButtonHighlighter.ForceDeselect();
        NewBossButton.SetActive(false);

        upButton.SetActive(false);
        upButtonHighlighter.ForceDeselect();
        downButton.SetActive(false);
        downButtonHighlighter.ForceDeselect();

        /*
        godButton.SetActive(false);
        godButtonHighlighter.ForceDeselect();
        papyrusKillButton.SetActive(false);
        papyrusKillButtonHighlighter.ForceDeselect();
        undyneKillButton.SetActive(false);
        undyneKillButtonHighlighter.ForceDeselect();
        */
        lightningButton.SetActive(false);
        lightningButtonHighlighter.ForceDeselect();
        creditsButton.SetActive(false);
        creditsButtonHighlighter.ForceDeselect();
        tutorialButton.SetActive(false);
        tutorialButtonHighlighter.ForceDeselect();
        continueButton.SetActive(false);
        continueButtonHighlighter.ForceDeselect();

        /*cheatButtonHighlighter.ForceDeselect();
        cheatButton.SetActive(false);
        megaCheatButtonHighlighter.ForceDeselect();
        megaCheatButton.SetActive(false);*/

        foreach (GameObject obj in credits)
        {
            obj.SetActive(false);
        }
        isEnabled = false;
    }

    private void slayEnemy()
    {
        /*killedCount++;
        switch (enemyFought)
        {
            case 0:
                papyrusButtonHighlighter.SetEnabled(false);
                papyrusButton.GetComponent<SpriteRenderer>().color = killedColor;
                break;
            case 1:
                undyneButtonHighlighter.SetEnabled(false);
                undyneButton.GetComponent<SpriteRenderer>().color = killedColor;
                break;
            case 2:
                sansButtonHighlighter.SetEnabled(false);
                sansButton.GetComponent<SpriteRenderer>().color = killedColor;
                break;
            default: // <Robert>
                killedCount--;
                break;
                // </Robert>
        }*/
    }

    private void playMusic(AudioClip clip)
    {
        playerController.PlayMusic(clip);
    }

    private void stopMusic()
    {
        playerController.StopMusic();
    }

    private void resetPlayerHealth()
    {
        playerInfo.ResetHealth();
    }

    public bool GetDominantTriggerPressed()
    {
        return domPressedThisFrame;
    }

    public static bool GetDominantTriggerDown()
    {
        return OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
    }

    /*
    private void fillTutorialTexts()
    {
        tutorialTexts.Add("Welcome to Barrier!\n\nA university of illinois\nat urbana-champaign\nvirtual reality game.\n\npoint the laser on your\nright hand at the \"continue\"\nbutton and press the right\ntrigger to continue");
        tutorialTexts.Add("click the buttons on the left\nto move the interface up\nor down. Position the \"continue\"\nbutton around sternum height.\nclick continue when you are done.");
        tutorialTexts.Add("in this game you will fight\nenemies that fire lasers\nat you. Use the barriers on your\nhands to block their lasers,\nand use your right hand to attack!");
    }
    */
}

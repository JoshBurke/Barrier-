using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewTheme : MonoBehaviour
{

    public float HUD_NudgeAmount = 0.15f;
    public float Max_HUD_Height = 1.5f;
    public float Min_HUD_Height = 0.75f;

    private GameObject player;
    private newPlayerInfo playerInfo;
    private OVRPlayerController playerController;
    private GameObject activePlayerPointer;
    private GameObject playerPointer;

    private GameObject hud;
    private GameObject mainMenu;
    private GameObject projSpawner;

    private GameObject fightButton;
    private GameObject mercyButton;

    private GameObject undyne;
    private GameObject sans;

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
    private GameObject RadialMenu;
    private MenuButtonHighligher RadialMenuHighlighter;

    void Start()
    {
        //current = SceneManager.GetActiveScene();
        player = GameObject.FindGameObjectWithTag("Player");
        playerInfo = player.GetComponent<newPlayerInfo>();
        playerController = GameObject.Find("OVRPlayerController").GetComponent<OVRPlayerController>();
        playerPointer = (GameObject)Resources.Load("PlayerPointer");


        hud = GameObject.Find("ElasticHUD");
        projSpawner = GameObject.Find("ProjectileSpawner");

        fightButton = GameObject.Find("AttackButton");
        mercyButton = GameObject.Find("MercyButton");

        mainMenuButton = GameObject.Find("MainMenuButton");
        mainMenuButtonHighlighter = mainMenuButton.GetComponent<MenuButtonHighligher>();
        mainMenuButton.SetActive(false);

        /*
        RadialMenu = GameObject.Find("Radial Menu");
        RadialMenuHighlighter = RadialMenu.GetComponent<MenuButtonHighligher>();
        RadialMenu.SetActive(false);
        */

        gameOverHeart = transform.Find("GameOverHeart").gameObject;
        gameOverHeartSpriteRen = gameOverHeart.GetComponent<SpriteRenderer>();
        gameOverHeartParts = transform.Find("GameOverParts").GetComponent<ParticleSystem>();
        gameOverHeart.SetActive(false);

        StartFight();
    }

    void Update()
    {
        //if (!isEnabled) return; //<- Not sure what its doing
        inputManager();
        buttonManager();
        /*
        if (OVRInput.GetDown(OVRInput.RawButton.X))
        {
            RadialMenu.SetActive(true);
            //RadialMenuSpawner.ins.SpawnMenu(button);
        }
        else if (OVRInput.GetUp(OVRInput.RawButton.X))
        {
            RadialMenu.SetActive(false);
            //Destroy(RadialMenu);
        }
        */
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

    private void buttonManager()
    {
        if (domPressedThisFrame)
        {
            if (mainMenuButtonHighlighter.IsHovered())
            {
                mainMenuButton.SetActive(false);
                mainMenuButtonHighlighter.ForceDeselect();
                SceneManager.LoadScene(0);
            }
            /*
            else if (RadialMenuHighlighter.IsHovered())
            {
                RadialMenu.SetActive(false);
                RadialMenuHighlighter.ForceDeselect();
                //Switch Magic option here
                Debug.Log("Selected Magic");
            }
            */
        }
    }

    public static bool GetDominantTriggerDown()
    {
        return OVRInput.Get(OVRInput.RawButton.RIndexTrigger);
    }

    public void StartFight() {
        GameObject n = (GameObject)Instantiate(Resources.Load("NewBoss"));
        n.GetComponent<AILibrary>().RegisterAttackButtons(fightButton, mercyButton);
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
        activePlayerPointer = Instantiate(playerPointer, attachShield.position, attachShield.rotation, attachShield);
        isEnabled = true;
    }
    private void playMusic(AudioClip clip)
    {
        playerController.PlayMusic(clip);
    }

}
